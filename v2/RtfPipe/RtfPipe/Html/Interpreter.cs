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

    public Interpreter(XmlWriter writer)
    {
      _xml = writer;
    }

    public void ToHtml(Document doc)
    {
      _html = doc.HasHtml ? (IHtmlWriter)new DeencapsulationWriter(_xml) : new HtmlWriter(_xml);

      var body = new Group();
      foreach (var token in doc.Contents)
      {
        if (token is DefaultFontRef defaultFont)
        {
          _html.DefaultFont = doc.FontTable[defaultFont.Value];
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
      _html.Close();
    }

    private void ToHtmlGroup(Document doc, Group group, bool processRtf)
    {
      if (group.Contents.Count > 1
        && group.Contents[0] is IgnoreUnrecognized
        && (group.Contents[1].GetType().Name == "GenericTag" || group.Contents[1].GetType().Name == "GenericWord"))
      {
        return;
      }

      if (_inputStyle.Count > 0)
        _inputStyle.Push(_inputStyle.Peek().Clone());
      else
        _inputStyle.Push(new FormatContext());
      var currStyle = _inputStyle.Peek();

      foreach (var token in group.Contents)
      {
        if (token is HtmlRtf htmlRtf)
        {
          processRtf = !htmlRtf.Value;
        }
        else if (processRtf)
        {
          if (token.Type == TokenType.CharacterFormat
            || token.Type == TokenType.ParagraphFormat
            || token.Type == TokenType.SectionFormat
            || token.Type == TokenType.HtmlFormat)
          {
            currStyle.Add(token);
          }
          else if (token is Group childGroup)
          {
            var dest = childGroup.Destination;
            if (dest is NumberingTextFallback || dest is ListTextFallback)
            {
              // skip
            }
            else if (childGroup.Contents.OfType<ParagraphNumbering>().Any())
            {
              foreach (var child in childGroup.Contents.Where(t => t.Type == TokenType.ParagraphFormat))
              {
                currStyle.Add(child);
              }
            }
            else
            {
              ToHtmlGroup(doc, childGroup, processRtf);
            }
          }
          else if (token is TextToken text)
          {
            _html.AddText(FixStyles(doc, currStyle), text.Value);
          }
          else if (token is ParagraphBreak
            || token is SectionBreak
            || token is LineBreak)
          {
            _html.AddBreak(FixStyles(doc, currStyle), token);
          }
        }
        else if (token is Group childGroup2)
        {
          ToHtmlGroup(doc, childGroup2, processRtf);
        }
      }

      _inputStyle.Pop();
    }

    private static FormatContext FixStyles(Document doc, FormatContext style)
    {
      var styleId = style.RemoveFirstOfType<ListStyleId>();
      if (styleId != null && doc.ListStyles.TryGetValue(styleId.Value, out var listStyle))
      {
        var level = style.RemoveFirstOfType<ListLevelNumber>()?.Value ?? 0;
        style.AddRange(listStyle.Style.Levels[level].Where(t => t.Type == TokenType.ParagraphFormat));
      }
      return style;
    }
  }
}
