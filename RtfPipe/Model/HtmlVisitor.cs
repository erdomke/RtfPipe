using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace RtfPipe.Model
{
  public class HtmlVisitor : INodeVisitor
  {
    private readonly XmlWriter _writer;
    private IEnumerable<IToken> _stylesheet = new IToken[]
    {
      new ForegroundColor(new ColorValue(0, 0, 0))
    };
    private Stack<Element> _stack = new Stack<Element>();

    public RtfHtmlSettings Settings { get; set; }

    public HtmlVisitor(TextWriter writer)
    {
      _writer = new HtmlTextWriter(writer);
      /*_writer = XmlWriter.Create(writer, new XmlWriterSettings()
      {
        OmitXmlDeclaration = true,
        Indent = true,
        IndentChars = "  "
      });*/
    }

    public HtmlVisitor(XmlWriter writer)
    {
      _writer = writer;
    }
    
    public void Visit(Element element)
    {
      Settings = Settings ?? new RtfHtmlSettings();
      if (element.Type == ElementType.Document && element.Nodes().Count() == 1)
      {
        element.Nodes().First().Visit(this);
        return;
      }

      if (!Settings.ElementTags.TryGetValue(element.Type, out var tag)
        && !Settings.ElementTags.TryGetValue(ElementType.Paragraph, out tag))
        tag = HtmlTag.Div;

      _writer.WriteStartElement(tag.Name);

      var styleList = GetNewStyles(element.Styles, tag).Where(t => !IsSpanElement(t)).ToList();
      if (styleList.Count > 0)
      {
        var css = new CssString(styleList);
        if (css.Length > 0)
          _writer.WriteAttributeString("style", css.ToString());
      }

      _stack.Push(element);
      foreach (var node in element.Nodes())
        node.Visit(this);
      _stack.Pop();

      _writer.WriteEndElement();
    }

    private IEnumerable<IToken> GetNewStyles(IEnumerable<IToken> styles, HtmlTag tag)
    {
      var existing = new StyleSet((_stack.Count > 0 ? _stack.Peek().Styles : null) ?? _stylesheet);
      existing.AddRange(tag.Styles);
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
      }

      return newStyles;
    }

    public void Visit(Run run)
    {
      var styleList = GetNewStyles(run.Styles, HtmlTag.Span).ToList();

      var endTags = 0;
      if (TryRemoveFirst(styleList, out BoldToken boldToken) && boldToken.Value)
      {
        _writer.WriteStartElement("strong");
        endTags++;
      }
      if (TryRemoveFirst(styleList, out ItalicToken italicToken) && italicToken.Value)
      {
        _writer.WriteStartElement("em");
        endTags++;
      }
      if (TryRemoveMany(styleList, StyleSet.IsUnderline, out var underlineStyles))
      {
        _writer.WriteStartElement("u");
        var css = new CssString(underlineStyles);
        if (css.Length > 0)
          _writer.WriteAttributeString("style", css.ToString());
        endTags++;
      }
      if ((TryRemoveFirst(styleList, out StrikeToken strikeToken) && strikeToken.Value)
        || (TryRemoveFirst(styleList, out StrikeDoubleToken strikeDoubleToken) && strikeDoubleToken.Value))
      {
        _writer.WriteStartElement("s");
        endTags++;
      }
      if (TryRemoveFirst(styleList, out SubStartToken subToken))
      {
        _writer.WriteStartElement("sub");
        endTags++;
      }
      if (TryRemoveFirst(styleList, out SuperStartToken superToken))
      {
        _writer.WriteStartElement("super");
        endTags++;
      }
      if (styleList.Count > 0)
      {
        var css = new CssString(styleList);
        if (css.Length > 0)
        {
          _writer.WriteStartElement("span");
          _writer.WriteAttributeString("style", css.ToString());
          endTags++;
        }
      }
      _writer.WriteValue(run.Value);
      for (var i = 0; i < endTags; i++)
        _writer.WriteEndElement();
    }

    public void Flush()
    {
      _writer.Flush();
    }

    public void Visit(ControlCharacter controlCharacter)
    {
      switch (controlCharacter.Type)
      {
        case ControlCharacterType.LineBreak:
          _writer.WriteStartElement("br");
          _writer.WriteEndElement();
          break;
        case ControlCharacterType.Tab:
          _writer.WriteStartElement("tab");
          _writer.WriteEndElement();
          break;
      }
    }


    private bool TryRemoveFirst<T, S>(IList<T> items, out S result) where S : T
    {
      result = default(S);
      for (var i = 0; i < items.Count; i++)
      {
        if (items[i] is S)
        {
          result = (S)items[i];
          items.RemoveAt(i);
          return true;
        }
      }
      return false;
    }

    private bool TryRemoveMany<T>(IList<T> items, Func<T, bool> predicate, out IList<T> results)
    {
      var i = 0;
      results = new List<T>();
      while (i < items.Count)
      {
        if (predicate(items[i]))
        {
          results.Add(items[i]);
          items.RemoveAt(i);
        }
        else
        {
          i++;
        }
      }
      return results.Count > 0;
    }

    internal static bool IsSpanElement(IToken token)
    {
      return token is BoldToken
        || token is ItalicToken
        || StyleSet.IsUnderline(token)
        || token is StrikeToken
        || token is StrikeDoubleToken
        || token is SubStartToken
        || token is SuperStartToken
        || token is HyperlinkToken
        || token is BookmarkToken;
    }
  }
}
