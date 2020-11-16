using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RtfPipe.Model
{
  internal class RawBuilder
  {
    public void Build(Document document, XmlWriter writer)
    {
      var body = new List<IToken>();
      var defaultStyles = new List<IToken>
      {
        new FontSize(UnitValue.FromHalfPoint(24))
      };
      if (document.ColorTable.Any())
        defaultStyles.Add(new ForegroundColor(document.ColorTable.First()));
      else
        defaultStyles.Add(new ForegroundColor(new ColorValue(0, 0, 0)));

      foreach (var token in document.Contents)
      {
        if (token is DefaultFontRef || token is DefaultTabWidth)
        {
          // Do nothing
        }
        else if (token is Group group)
        {
          if (group.Destination?.Type != TokenType.HeaderTag)
          {
            body.Add(token);
          }
        }
        else if (token.Type != TokenType.HeaderTag)
        {
          body.Add(token);
        }
      }

      var groups = new Stack<TokenState>();
      groups.Push(new TokenState(body));

      while (groups.Count > 0)
      {
        while (groups.Peek().Tokens.MoveNext())
        {
          var token = groups.Peek().Tokens.Current;
          if (token is Group childGroup)
          {
            var dest = childGroup.Destination;
            if (childGroup.Contents.Count > 1
              && childGroup.Contents[0] is IgnoreUnrecognized
              && (childGroup.Contents[1].GetType().Name == "GenericTag" || childGroup.Contents[1].GetType().Name == "GenericWord"))
            {
              // Ignore groups with the "skip if unrecognized" flag
            }
            else if (dest is HtmlTagToken)
            {
              groups.Push(new TokenState(childGroup.Contents)
              {
                RawHtml = true,
                ProcessRtf = groups.Peek().ProcessRtf
              });
            }
            else
            {
              groups.Push(new TokenState(childGroup.Contents)
              {
                RawHtml = groups.Peek().RawHtml,
                ProcessRtf = groups.Peek().ProcessRtf
              });
            }
          }
          else if (token is HtmlRtf htmlRtf)
          {
            groups.Peek().ProcessRtf = htmlRtf.Value;
          }
          else if (!groups.Peek().ProcessRtf)
          {
            if (token is TextToken text)
            {
              if (groups.Peek().RawHtml)
                writer.WriteRaw(text.Value);
              else
                writer.WriteValue(text.Value);
            }
            else if (token is ParagraphBreak || token is LineBreak)
            {
              if (groups.Peek().RawHtml)
              {
                writer.WriteRaw(Environment.NewLine);
              }
              else
              {
                writer.WriteStartElement("br");
                writer.WriteEndElement();
              }
            }
            else if (token is Tab)
            {
              writer.WriteRaw("\t");
            }
          }
        }

        groups.Pop();
      }
    }

    private class TokenState
    {
      public IEnumerator<IToken> Tokens { get; }
      public bool RawHtml { get; set; }
      public bool ProcessRtf { get; set; }
      
      public TokenState(IEnumerable<IToken> tokens)
      {
        Tokens = tokens.GetEnumerator();
      }
    }
  }
}
