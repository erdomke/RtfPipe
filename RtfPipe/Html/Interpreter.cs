using System.Collections.Generic;
using System.Text;
using System.Xml;
using RtfPipe.Tokens;
using System.Linq;

namespace RtfPipe
{
  internal class Interpreter
  {
    private readonly Stack<FormatContext> _inputStyle = new Stack<FormatContext>();
    private IHtmlWriter _html;
    private readonly XmlWriter _xml;
    private readonly List<Group> _footnotes = new List<Group>();
    private int _footnoteIdx;

    public Interpreter(XmlWriter writer)
    {
      _xml = writer;
    }

    public void ToHtml(Document doc, RtfHtmlSettings settings)
    {
      _footnotes.Clear();
      _footnoteIdx = 0;
      _html = doc.HasHtml ? (IHtmlWriter)new DeencapsulationWriter(_xml) : new HtmlWriter(_xml, settings);

      var body = new Group();
      foreach (var token in doc.Contents)
      {
        if (token is DefaultFontRef defaultFont)
        {
          _html.DefaultFont = doc.FontTable.TryGetValue(defaultFont.Value, out var font) ? font : doc.FontTable.FirstOrDefault().Value;
        }
        else if (token is DefaultTabWidth tabWidth)
        {
          _html.DefaultTabWidth = tabWidth.Value;
        }
        else if (token is Group group)
        {
          if (group.Destination?.Type != TokenType.HeaderTag)
          {
            body.Contents.Add(token);
          }
        }
        else if (token.Type != TokenType.HeaderTag)
        {
          body.Contents.Add(token);
        }
      }

      ToHtmlGroup(doc, body, true);
      RenderFootnotes(doc);
      _html.Close();
    }

    private FormatContext ToHtmlGroup(Document doc, Group group, bool processRtf)
    {
      var tabCount = 0;

      if (group.Contents.Count > 1
        && group.Contents[0] is IgnoreUnrecognized
        && (group.Contents[1].GetType().Name == "GenericTag" || group.Contents[1].GetType().Name == "GenericWord"))
      {
        return null;
      }

      if (_inputStyle.Count > 0)
        _inputStyle.Push(_inputStyle.Peek().Clone());
      else
        _inputStyle.Push(new FormatContext());
      var currStyle = _inputStyle.Peek();
      if (group is Row)
      {
        if (group.Contents.OfType<RightCellBoundary>().Any())
          currStyle.RemoveWhere(t => t is CellToken_Orig);
        currStyle.RemoveWhere(t => t is NestingLevel);
      }

      for (var i = 0; i < group.Contents.Count; i++)
      {
        var token = group.Contents[i];
        if (token is HtmlRtf htmlRtf)
        {
          processRtf = !htmlRtf.Value;
        }
        else if (processRtf)
        {
          if (token is RowDefaults && !(group is Row))
          {
            var table = Table.Create(group.Contents, ref i);
            var prevStyle = default(FormatContext);
            foreach (var child in table.Contents.OfType<Group>())
            {
              var usePrevious = prevStyle != null && !child.Contents.OfType<RowDefaults>().Any();
              if (usePrevious)
                _inputStyle.Push(prevStyle);
              prevStyle = ToHtmlGroup(doc, child, processRtf);
              if (usePrevious)
                _inputStyle.Pop();
            }
          }
          else if (token.Type == TokenType.CellFormat)
          {
            var start = i;
            while (i < group.Contents.Count && !(group.Contents[i] is RightCellBoundary))
              i++;
            var cell = new CellToken_Orig(group.Contents.Skip(start).Take(i - start + 1)
              , group as Row
              , currStyle.OfType<CellToken_Orig>().LastOrDefault());
            currStyle.Add(cell);
          }
          else if (token is ControlWord<BorderPosition> borderSide)
          {
            var border = new BorderToken(borderSide);
            i++;
            while (i < group.Contents.Count && border.Add(group.Contents[i]))
              i++;
            i--;
            currStyle.Add(border);
          }
          else if ((token.Type & TokenType.Format) == TokenType.Format)
          {
            currStyle.Add(token);
          }
          else if (token is Group childGroup)
          {
            var dest = childGroup.Destination;
            if (dest is NumberingTextFallback
              || dest is ListTextFallback
              || dest?.Type == TokenType.HeaderTag
              || dest is NoNestedTables)
            {
              // skip
            }
            else if (dest is Header
              || dest is HeaderEven
              || dest is HeaderFirst
              || dest is HeaderOdd
              || dest is Footer
              || dest is FooterFirst
              || dest is FooterOdd)
            {
              // skip for now
            }
            else if (dest is FieldInstructions)
            {
              var instructions = childGroup.Contents
                .OfType<Group>().LastOrDefault(g => g.Destination == null && g.Contents.OfType<TextToken>().Any())
                ?.Contents.OfType<TextToken>().FirstOrDefault()?.Value?.Trim();
              if (string.IsNullOrEmpty(instructions)
                && !childGroup.Contents.OfType<Group>().Any()
                && childGroup.Contents.Count == 3)
              {
                instructions = (childGroup.Contents[2] as TextToken)?.Value;
              }

              if (!string.IsNullOrEmpty(instructions))
              {
                var args = instructions.Split(' ');
                if (args[0] == "HYPERLINK")
                  currStyle.Add(new HyperlinkToken(args));
              }
            }
            else if (dest is BookmarkStart)
            {
              currStyle.Add(new BookmarkToken()
              {
                Start = true,
                Id = childGroup.Contents.OfType<TextToken>().FirstOrDefault()?.Value
              });
            }
            else if (dest is BookmarkEnd)
            {
              currStyle.Add(new BookmarkToken()
              {
                Start = false,
                Id = childGroup.Contents.OfType<TextToken>().FirstOrDefault()?.Value
              });
            }
            else if (dest is PictureTag)
            {
              var pict = new Picture_Orig(childGroup);
              var style = FixStyles(doc, currStyle);
              if (tabCount > 0)
                _html.AddBreak(style, new Tab(), tabCount);
              _html.AddPicture(style, pict);
              tabCount = 0;
            }
            else if (dest is Footnote)
            {
              _footnotes.Add(childGroup);
              var linkGroup = new Group();
              _footnoteIdx++;
              var title = string.Concat(childGroup.Contents
                .Select(t =>
                {
                  if (t is TextToken txt)
                    return txt.Value;
                  if (t.Type == TokenType.BreakTag)
                    return " ";
                  return "";
                }).ToArray());
              linkGroup.Contents.Add(new HyperlinkToken()
              {
                Url = "#" + FootnoteId(_footnoteIdx),
                Title = title
              });
              linkGroup.Contents.Add(new SuperStartToken());
              linkGroup.Contents.Add(new TextToken() { Value = _footnoteIdx.ToString() });
              ToHtmlGroup(doc, linkGroup, true);
            }
            else if (childGroup.Contents.OfType<ParagraphNumbering>().Any())
            {
              foreach (var child in childGroup.Contents.Where(t => t.Type == TokenType.ParagraphFormat))
                currStyle.Add(child);
            }
            else
            {
              if (childGroup is Table)
                (_html as HtmlWriter)?.EnsureCell(currStyle);
              ToHtmlGroup(doc, childGroup, processRtf);
            }
          }
          else if (token is Tab)
          {
            tabCount++;
          }
          else if (token is TextToken text)
          {
            var style = FixStyles(doc, currStyle);
            if (tabCount > 0)
              _html.AddBreak(style, new Tab(), tabCount);
            _html.AddText(style, text.Value);
            tabCount = 0;
          }
          else if ((token.Type & TokenType.BreakTag) == TokenType.BreakTag)
          {
            AddBreak(doc, token, currStyle);
            tabCount = 0;
          }
        }
        else if (token is Group childGroup2)
        {
          ToHtmlGroup(doc, childGroup2, processRtf);
        }
      }

      return _inputStyle.Pop();
    }

    private void AddBreak(Document doc, IToken token, FormatContext currStyle)
    {
      if (token is PageBreak || token is SectionBreak)
        RenderFootnotes(doc);
      _html.AddBreak(FixStyles(doc, currStyle), token);
      if (token is RowBreak)
      {
        foreach (var style in _inputStyle)
          style.InTable = false;
      }
    }

    private string FootnoteId(int id)
    {
      return "footnote" + id.ToString("d2");
    }

    private void RenderFootnotes(Document doc)
    {
      if (_footnotes.Count > 0)
      {
        var style = new FormatContext();
        _html.AddBreak(style, new FootnoteBreak());
        var start = _footnoteIdx - _footnotes.Count + 1;
        for (var i = 0; i < _footnotes.Count; i++)
        {
          if (i > 0)
            _html.AddBreak(style, new ParagraphBreak());
          var group = new Group();
          var id = FootnoteId(start + i);
          group.Contents.Add(new BookmarkToken() { Id = id, Start = true });
          group.Contents.Add(new SuperStartToken());
          group.Contents.Add(new TextToken() { Value = (start + i) + " " });
          group.Contents.Add(new NoSuperSubToken());
          group.Contents.Add(new BookmarkToken() { Id = id, Start = false });
          group.Contents.AddRange(_footnotes[i].Contents.Where(t => !(t is Footnote)));
          ToHtmlGroup(doc, group, true);
        }

        _footnotes.Clear();
      }
    }

    private static FormatContext FixStyles(Document doc, FormatContext style)
    {
      style.ReplaceToken<ListStyleId>(styleId =>
      {
        var list = new List<IToken>();
        if (doc.ListStyles.TryGetValue(styleId.Value, out var listStyle))
        {
          var levelNum = style.RemoveFirstOfType<ListLevelNumber>() ?? new ListLevelNumber(0);
          var level = levelNum.Value;
          list.AddRange(listStyle.Style.Levels[level]
            .Where(t =>
            {
              // This is a bit of a hack, but not sure how MS Word is interpreting this
              if (t is FirstLineIndent firstLine)
                return firstLine.Value > new UnitValue(-1, UnitType.Inch);
              return t.Type == TokenType.ParagraphFormat;
            }));
          list.Add(styleId);
          list.Add(levelNum);
        }
        return list;
      });
      return style;
    }
  }
}
