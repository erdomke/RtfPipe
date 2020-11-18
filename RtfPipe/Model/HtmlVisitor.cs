using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace RtfPipe.Model
{
  internal class HtmlVisitor : INodeVisitor
  {
    private UnitValue _defaultTabWidth = new UnitValue(0.5, UnitType.Inch);
    private HtmlTag _lastTag;
    private HashSet<string> _renderedBookmarks = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private Stack<StyleList> _stack = new Stack<StyleList>();
    private IEnumerable<IToken> _stylesheet = new IToken[]
    {
      new ForegroundColor(new ColorValue(0, 0, 0)),
      new BackgroundColor(new ColorValue(255, 255, 255))
    };
    private readonly XmlWriter _writer;

    public UnitValue DefaultTabWidth
    {
      get { return _defaultTabWidth; }
      set { _defaultTabWidth = value.HasValue ? value : new UnitValue(0.5, UnitType.Inch); }
    }

    public RtfHtmlSettings Settings { get; set; }

    public HtmlVisitor(TextWriter writer)
    {
      _writer = new HtmlTextWriter(writer);
    }

    public HtmlVisitor(XmlWriter writer)
    {
      _writer = writer;
    }

    private HtmlTag GetElementTag(ElementType primary, ElementType? secondary, HtmlTag defaultValue)
    {
      var tags = Settings?.ElementTags ?? RtfHtmlSettings.DefaultTags;
      if (tags.TryGetValue(primary, out var result)
        || (secondary.HasValue && tags.TryGetValue(secondary.Value, out result)))
        return result;
      return defaultValue;
    }
    
    public void Visit(Element element)
    {
      if (element.Type == ElementType.Document && element.Nodes().Count() == 1)
      {
        element.Nodes().First().Visit(this);
        return;
      }

      var tag = GetElementTag(element.Type, ElementType.Paragraph, HtmlTag.Div);
      _writer.WriteStartElement(tag.Name);

      var elementStyles = (IEnumerable<IToken>)element.Styles;
      if (element.Type == ElementType.TableCell || element.Type == ElementType.TableHeaderCell)
        elementStyles = elementStyles.Concat(elementStyles.OfType<CellToken>().SelectMany(c => c.Styles));
      else if (element.Type == ElementType.Section || element.Type == ElementType.Document)
        elementStyles = elementStyles.Where(t => t.Type != TokenType.ParagraphFormat && t.Type != TokenType.RowFormat && t.Type != TokenType.CellFormat);
      var styleList = GetNewStyles(elementStyles, tag)
        .Where(t => !IsSpanElement(t))
        .ToList();
      if (element.Type != ElementType.TableCell && element.Type != ElementType.TableHeaderCell && element.Type != ElementType.TableRow && element.Type != ElementType.Table)
        styleList.RemoveWhere(t => t.Type == TokenType.CellFormat || t.Type == TokenType.RowFormat);

      if (element.Type == ElementType.OrderedList)
      {
        var numType = element.Styles.OfType<ListLevelType>().FirstOrDefault()?.Value
            ?? element.Styles.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
            ?? NumberingType.Numbers;
        switch (numType)
        {
          case NumberingType.LowerLetter:
            _writer.WriteAttributeString("type", "a");
            break;
          case NumberingType.LowerRoman:
            _writer.WriteAttributeString("type", "i");
            break;
          case NumberingType.UpperLetter:
            _writer.WriteAttributeString("type", "A");
            break;
          case NumberingType.UpperRoman:
            _writer.WriteAttributeString("type", "I");
            break;
        }

        var startAt = element.Styles.OfType<NumberingStart>().FirstOrDefault()?.Value ?? 1;
        if (startAt > 1)
          _writer.WriteAttributeString("start", startAt.ToString());
      }
      else if (element.Type == ElementType.TableCell || element.Type == ElementType.TableHeaderCell)
      {
        var colspan = element.Styles.OfType<CellToken>().FirstOrDefault()?.ColSpan ?? 1;
        if (colspan > 1)
          _writer.WriteAttributeString("colspan", colspan.ToString());
      }

      ProcessLeadingTabs(element, styleList);
      if (element.Type == ElementType.Section 
        && element.Parent != null
        && element.Parent.Elements().First() != element)
      {
        styleList.Add(new PageBreak());
      }

      if (styleList.Count > 0)
      {
        var css = new CssString(styleList, element.Type);
        if (css.Length > 0)
          _writer.WriteAttributeString("style", css.ToString());
      }

      if (element.Type == ElementType.Table)
        ProcessColumns(element);

      _stack.Push(new StyleList(tag.Styles.Where(t => t.Type == TokenType.CharacterFormat))
        .MergeRange(element.Styles));
      var anyNodes = false;
      foreach (var node in element.Nodes())
      {
        node.Visit(this);
        anyNodes = true;
      }
      if (!anyNodes)
      {
        if (element.Type == ElementType.Paragraph)
          new Run() { Value = "\n" }.Visit(this);
        else if (element.Type == ElementType.TableCell || element.Type == ElementType.TableHeaderCell)
          new Run() { Value = "\u00a0" }.Visit(this);
      }
      _stack.Pop();

      _writer.WriteEndElement();
    }

    private void ProcessLeadingTabs(Element element, List<IToken> styles)
    {
      if (element.Nodes().FirstOrDefault() is Run run && run.Value.StartsWith("\t"))
      {
        var firstLineIndent = styles.OfType<FirstLineIndent>().FirstOrDefault()?.Value ?? UnitValue.Empty;

        var tabCount = 0;
        while (tabCount < run.Value.Length && run.Value[tabCount] == '\t')
          tabCount++;

        var newFirstLineIndent = IndentSize(styles, true, tabCount);
        if (newFirstLineIndent > firstLineIndent)
        {
          styles.RemoveWhere(t => t is FirstLineIndent);
          styles.Add(new FirstLineIndent(newFirstLineIndent));
        }
      }
    }

    private void ProcessColumns(Element table)
    {
      var boundaries = table.Elements()
        .SelectMany(e => e.Styles.OfType<CellToken>())
        .Select(c => c.RightBoundary)
        .Distinct()
        .OrderBy(v => v)
        .Select(v => new CellIndex() { RightBoundary = v })
        .ToList();

      if (boundaries.Count < 1)
        return;

      var widths = new List<UnitValue>()
      {
        boundaries[0].RightBoundary
      };

      var cellIdx = 0;
      for (var i = 1; i < boundaries.Count; i++)
      {
        var width = boundaries[i].RightBoundary - boundaries[i - 1].RightBoundary;
        if (width.ToPx() > 0.25)
        {
          cellIdx++;
          widths.Add(width);
        }
        boundaries[i].Index = cellIdx;
      }
      var indexDict = boundaries.ToDictionary(b => b.RightBoundary, b => b.Index);

      var rows = table.Elements().ToList();
      if (!rows.Any(e => e.Type == ElementType.TableRow))
        rows = rows.SelectMany(e => e.Elements()).ToList();
      foreach (var row in rows)
      {
        var cells = row.Elements().ToList();
        var startIndex = 0;
        for (var i = 0; i < cells.Count; i++)
        {
          var token = cells[i].Styles.OfType<CellToken>().Single();
          var lastIndex = indexDict[token.RightBoundary];
          token.Index = startIndex;
          token.ColSpan = lastIndex - startIndex + 1;

          // Fix widths to be the widths instead of the right boundary when there is a discrepancy
          if (startIndex == lastIndex && token.WidthUnit == CellWidthUnit.Twip)
            widths[startIndex] = new UnitValue(token.Width, UnitType.Twip);

          startIndex = lastIndex + 1;
        }
      }

      _writer.WriteStartElement("colgroup");
      foreach (var width in widths)
      {
        _writer.WriteStartElement("col");
        _writer.WriteAttributeString("style", new CssString().Append("width", width).ToString());
        _writer.WriteEndElement();
      }
      _writer.WriteEndElement();
    }

    public void Flush()
    {
      _writer.Flush();
    }

    private class CellIndex
    {
      public UnitValue RightBoundary { get; set; }
      public int Index { get; set; }
    }

    private IEnumerable<IToken> GetNewStyles(IEnumerable<IToken> styles, HtmlTag tag)
    {
      var existing = new StyleList(_stylesheet);
      if (_stack.Count > 0)
        existing.MergeRange(_stack.Peek());
      existing.MergeRange(tag.Styles);
      var requested = styles.ToList();
      var intersection = existing.Intersect(requested).ToList();

      var newStyles = requested.Where(t => !intersection.Contains(t)).ToList();
      var toNegate = existing.Where(t => !intersection.Contains(t)).ToList();
      foreach (var styleToNegate in toNegate)
      {
        if (styleToNegate is SpaceAfter && !newStyles.OfType<SpaceAfter>().Any())
          newStyles.Add(new SpaceAfter(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is SpaceBefore && !newStyles.OfType<SpaceBefore>().Any())
          newStyles.Add(new SpaceBefore(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is CellVerticalAlign && !newStyles.OfType<CellVerticalAlign>().Any())
          newStyles.Add(new CellVerticalAlign(VerticalAlignment.Top));
        else if (styleToNegate is BottomCellSpacing && !newStyles.OfType<BottomCellSpacing>().Any())
          newStyles.Add(new BottomCellSpacing(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is TopCellSpacing && !newStyles.OfType<TopCellSpacing>().Any())
          newStyles.Add(new TopCellSpacing(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is LeftCellSpacing && !newStyles.OfType<LeftCellSpacing>().Any())
          newStyles.Add(new LeftCellSpacing(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is RightCellSpacing && !newStyles.OfType<RightCellSpacing>().Any())
          newStyles.Add(new RightCellSpacing(new UnitValue(0, UnitType.Pixel)));
        else if (styleToNegate is TextAlign && tag.Styles.OfType<TextAlign>().Any())
          newStyles.Add(new TextAlign(TextAlignment.Left));
        else if (styleToNegate is ControlWord<bool> boolControl)
          newStyles.Add(ControlTag.Negate(boolControl));
      }

      return newStyles;
    }

    public void Visit(Run run)
    {
      var hyperlink = run.Styles.OfType<HyperlinkToken>().FirstOrDefault();
      var elementType = hyperlink == null ? ElementType.Span : ElementType.Hyperlink;
      var tag = GetElementTag(elementType, null, hyperlink == null ? HtmlTag.Span : HtmlTag.A);
      var styleList = new StyleList(GetNewStyles(run.Styles, tag)
        .Where(t => t.Type == TokenType.CharacterFormat));
      var stylesWritten = false;

      var endTags = 0;
      if (styleList.TryRemoveFirstTrue(out BoldToken boldToken))
      {
        _writer.WriteStartElement(GetElementTag(ElementType.Strong, null, HtmlTag.Strong).Name);
        endTags++;
      }
      if (styleList.TryRemoveFirstTrue(out ItalicToken italicToken))
      {
        _writer.WriteStartElement(GetElementTag(ElementType.Emphasis, null, HtmlTag.Em).Name);
        endTags++;
      }
      if (hyperlink == null && styleList.TryRemoveMany(StyleList.IsUnderline, out var underlineStyles))
      {
        _writer.WriteStartElement(GetElementTag(ElementType.Underline, null, HtmlTag.U).Name);
        var underlineCss = new CssString(underlineStyles.Where(t => !(t is UnderlineToken)), ElementType.Underline);
        if (underlineCss.Length > 0)
        {
          _writer.WriteAttributeString("style", underlineCss.ToString());
          stylesWritten = true;
        }
        endTags++;
      }
      if (styleList.TryRemoveFirstTrue(out StrikeToken strikeToken)
        || styleList.OfType<StrikeDoubleToken>().FirstOrDefault()?.Value == true)
      {
        _writer.WriteStartElement("s");
        endTags++;
      }
      if (styleList.TryRemoveFirst(out SubStartToken subToken))
      {
        _writer.WriteStartElement("sub");
        endTags++;
      }
      if (styleList.TryRemoveFirst(out SuperStartToken superToken))
      {
        _writer.WriteStartElement("sup");
        endTags++;
      }

      var css = new CssString(styleList, elementType);
      if (hyperlink != null)
      {
        _writer.WriteStartElement(tag.Name);
        if (css.Length > 0)
          _writer.WriteAttributeString("style", css.ToString());
        if (!string.IsNullOrEmpty(hyperlink.Url))
          _writer.WriteAttributeString("href", hyperlink.Url);
        if (!string.IsNullOrEmpty(hyperlink.Target))
          _writer.WriteAttributeString("target", hyperlink.Target);
        if (!string.IsNullOrEmpty(hyperlink.Title))
          _writer.WriteAttributeString("title", hyperlink.Title);
        endTags++;
      }
      else if (css.Length > 0 && endTags > 0 && !stylesWritten)
      {
        _writer.WriteAttributeString("style", css.ToString());
      }
      else if (css.Length > 0)
      {
        _writer.WriteStartElement(tag.Name);
        _writer.WriteAttributeString("style", css.ToString());
        endTags++;
      }

      var bookmark = styleList.OfType<BookmarkToken>().FirstOrDefault();
      if (bookmark != null && !_renderedBookmarks.Contains(bookmark.Id))
      {
        _renderedBookmarks.Add(bookmark.Id);
        _writer.WriteStartElement("a");
        _writer.WriteAttributeString("id", bookmark.Id);
        _writer.WriteEndElement();
      }

      WriteRunText(run);
      
      for (var j = 0; j < endTags; j++)
        _writer.WriteEndElement();
    }
    
    private void WriteRunText(Run run)
    {
      var i = 0;
      var charBuffer = run.Value.ToCharArray();
      var eastAsian = run.Styles.OfType<Font>().Any(f => TextEncoding.IsEastAsian(f.Encoding));

      if (run.Parent?.Nodes().First() == run)
      {
        while (i < charBuffer.Length && charBuffer[i] == '\t')
          i++;
      }

      var start = i;
      var inTabList = false;
      while (i < charBuffer.Length)
      {
        if (charBuffer[i] == '\t')
        {
          if (!inTabList)
          {
            if (start < i)
              _writer.WriteChars(charBuffer, start, i - start);
            start = i;
            inTabList = true;
          }
        }
        else if (charBuffer[i] == '\n')
        {
          if (start < i && !inTabList)
            _writer.WriteChars(charBuffer, start, i - start);
          inTabList = false;
          start = i + 1;
          _writer.WriteStartElement("br");
          _writer.WriteEndElement();
        }
        else
        {
          if (inTabList)
          {
            WriteTabs(run.Parent?.Styles, start == 0 || charBuffer[start - 1] == '\n', i - start);
            inTabList = false;
            start = i;
          }
          else if (i > 0 && charBuffer[i] == ' '
            && (charBuffer[i - 1] == ' ' || charBuffer[i - 1] == '\u00a0' || charBuffer[i - 1] == '\u2007'))
          {
            charBuffer[i] = eastAsian ? '\u2007' : '\u00a0';
          }
        }
        i++;
      }

      if (start < charBuffer.Length)
      {
        if (inTabList)
          WriteTabs(run.Parent?.Styles, start == 0 || charBuffer[start - 1] == '\n', charBuffer.Length - start);
        else
          _writer.WriteChars(charBuffer, start, charBuffer.Length - start);
      }

      if (charBuffer.Length > 0 && charBuffer[charBuffer.Length - 1] == '\n'
        && run.Parent != null && run.Parent.Nodes().Last() == run)
        _writer.WriteValue("\u00a0");
    }

    private UnitValue IndentSize(IEnumerable<IToken> parentStyles, bool newLine, int tabCount)
    {
      var tabPositions = (parentStyles ?? Enumerable.Empty<IToken>()).OfType<TabPosition>().ToList();
      if (!newLine || tabPositions.Count < 1)
        return DefaultTabWidth * tabCount;
      else if (tabCount > tabPositions.Count)
        return tabPositions.Last().Value + DefaultTabWidth * (tabCount - tabPositions.Count);
      else
        return tabPositions[tabCount - 1].Value;
    }

    private void WriteTabs(IEnumerable<IToken> parentStyles, bool newLine, int tabCount)
    {
      var size = IndentSize(parentStyles, newLine, tabCount);
      _writer.WriteStartElement("span");
      _writer.WriteAttributeString("style", $"display:inline-block;width:{size.ToPx()}px");
      _writer.WriteEndElement();
    }

    internal static bool IsSpanElement(IToken token)
    {
      return token is BoldToken
        || token is ItalicToken
        || StyleList.IsUnderline(token)
        || token is StrikeToken
        || token is StrikeDoubleToken
        || token is SubStartToken
        || token is SuperStartToken
        || token is HyperlinkToken
        || token is BookmarkToken;
    }

    public void Visit(Picture image)
    {
      var uri = Settings?.ImageUriGetter(image);
      if (!string.IsNullOrEmpty(uri))
      {
        _writer.WriteStartElement("img");

        if (image.Width.HasValue)
          _writer.WriteAttributeString("width", image.Width.ToPx().ToString("0"));

        if (image.Height.HasValue)
          _writer.WriteAttributeString("height", image.Height.ToPx().ToString("0"));

        _writer.WriteAttributeString("src", uri);
        _writer.WriteEndElement();
      }
    }

    public void Visit(Attachment attachment)
    {
      Settings?.AttachmentRenderer(attachment.Index, _writer);
    }

    public void Visit(HorizontalRule horizontalRule)
    {
      _writer.WriteStartElement("hr");
      _writer.WriteAttributeString("style", "width:2in;border:0.5px solid black;margin-left:0");
      _writer.WriteEndElement();
    }
  }
}
