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
          if (group.Contents
            .SkipWhile(t => t is IgnoreUnrecognized)
            .FirstOrDefault()?.Type != TokenType.HeaderTag)
          {
            body.Contents.Add(token);
          }
        }
        else if (token.Type != TokenType.HeaderTag)
        {
          body.Contents.Add(token);
        }
      }

      ToHtmlGroup(body);
      _html.Close();
    }

    private void ToHtmlGroup(Group group)
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
      var addStyles = true;

      foreach (var token in group.Contents)
      {
        if (token is HtmlRtf htmlRtf)
        {
          addStyles = !htmlRtf.Value;
        }
        else if (addStyles && (token.Type == TokenType.CharacterFormat
          || token.Type == TokenType.ParagraphFormat
          || token.Type == TokenType.SectionFormat
          || token.Type == TokenType.HtmlFormat))
        {
          currStyle.Add(token);
        }
        else if (token is Group childGroup)
        {
          ToHtmlGroup(childGroup);
        }
        else if (token is TextToken text)
        {
          _html.AddText(currStyle, text.Value);
        }
        else if (token is ParagraphBreak
          || token is SectionBreak
          || token is LineBreak)
        {
          _html.AddBreak(currStyle, token);
        }
      }

      _inputStyle.Pop();
    }
  }
}
