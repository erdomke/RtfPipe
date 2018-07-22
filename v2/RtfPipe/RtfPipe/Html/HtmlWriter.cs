using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RtfPipe.Tokens;

namespace RtfPipe
{
  internal class HtmlWriter : IHtmlWriter
  {
    private readonly XmlWriter _writer;
    private readonly Stack<TagContext> _tags = new Stack<TagContext>();

    public Font DefaultFont { get; set; }
    public FontSize DefaultFontSize { get; } = new FontSize(UnitValue.FromHalfPoint(24));

    public HtmlWriter(XmlWriter xmlWriter)
    {
      _writer = xmlWriter;
    }

    public void AddText(FormatContext format, string text)
    {
      if (format.OfType<HiddenToken>().Any())
        return;
      EnsureSpans(format);
      _writer.WriteValue(text);
    }

    public void AddBreak(FormatContext format, IToken token)
    {
      if (token is ParagraphBreak)
      {
        EnsureParagraph(format);
        while (_tags.Peek().Name != "p")
          EndTag();
        EndTag();
      }
      else if (token is SectionBreak)
      {
        EnsureSection(format);
        while (_tags.Peek().Name != "div")
          EndTag();
        EndTag();
      }
      else if (token is LineBreak)
      {
        _writer.WriteStartElement("br");
        _writer.WriteEndElement();
      }
    }

    public void Close()
    {
      while (_tags.Count > 0)
        EndTag();
    }

    private void EnsureSection(FormatContext format)
    {
      if (_tags.Any(t => t.Name == "div"))
        return;

      var tag = new TagContext("div", _tags.SafePeek());
      tag.AddRange(format.Where(t => t.Type == TokenType.SectionFormat));
      tag.Add(DefaultFontSize);
      if (DefaultFont != null) tag.Add(DefaultFont);
      WriteTag(tag);
    }

    private void EnsureParagraph(FormatContext format)
    {
      if (_tags.Any(t => t.Name == "p"))
        return;

      EnsureSection(format);

      var tag = new TagContext("p", _tags.SafePeek());
      tag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
        && !IsSpanElement(t)));
      WriteTag(tag);
    }

    private void EnsureSpans(FormatContext format)
    {
      EnsureParagraph(format);

      var existing = CharacterFormats(_tags.Peek());
      var requested = CharacterFormats(format);

      var intersection = existing.Intersect(requested).ToList();
      if (intersection.Count == existing.Count
        && intersection.Count == requested.Count)
      {
        return;
      }

      existing = existing.Where(t => !intersection.Contains(t) && IsSpanElement(t)).ToList();
      requested = requested.Where(t => !intersection.Contains(t)).ToList();

      if (existing.Count > 0 && _tags.Peek().Name != "p")
      {
        EndTag();
        EnsureSpans(format);
      }
      else if (TryGetValue<BoldToken, IToken>(requested, out var bold))
      {
        WriteSpanElement(format, "strong", bold, requested);
      }
      else if (TryGetValue<ItalicToken, IToken>(requested, out var italic))
      {
        WriteSpanElement(format, "em", italic, requested);
      }
      else if (TryGetValue<UnderlineToken, IToken>(requested, out var underline))
      {
        WriteSpanElement(format, "u", underline, requested);
      }
      else if (TryGetValue<StrikeToken, IToken>(requested, out var strike))
      {
        WriteSpanElement(format, "s", strike, requested);
      }
      else if (TryGetValue<SubStartToken, IToken>(requested, out var sub))
      {
        WriteSpanElement(format, "sub", sub, requested);
      }
      else if (TryGetValue<SuperStartToken, IToken>(requested, out var super))
      {
        WriteSpanElement(format, "super", super, requested);
      }
      else if (requested.Count > 0)
      {
        WriteSpan(format);
      }
    }

    private void WriteSpanElement(FormatContext format, string name, IToken spanToken, IEnumerable<IToken> requested)
    {
      var tag = new TagContext(name, _tags.Peek());
      if (requested.Any(t => IsSpanElement(t) && t != spanToken))
      {
        tag.Add(spanToken);
        WriteTag(tag);
        EnsureSpans(format);
      }
      else
      {
        tag.AddRange(requested);
        WriteTag(tag);
      }
    }

    private bool TryGetValue<T, TParent>(IEnumerable<TParent> list, out T value) where T : TParent
    {
      var found = list.OfType<T>().ToList();
      if (found.Count < 1)
      {
        value = default;
        return false;
      }
      else
      {
        value = found[0];
        return true;
      }
    }

    private void WriteSpan(FormatContext format)
    {
      var tag = new TagContext("span", _tags.SafePeek());
      tag.AddRange(CharacterFormats(format));
      WriteTag(tag);
    }

    private List<IToken> CharacterFormats(FormatContext context)
    {
      var tokens = (IEnumerable<IToken>)context;
      if (context is TagContext tag)
        tokens = tag.AllInherited();
      return tokens
        .Where(t => t.Type == TokenType.CharacterFormat)
        .ToList();
    }

    private bool IsSpanElement(IToken token)
    {
      return token is BoldToken
        || token is ItalicToken
        || token is UnderlineToken
        || token is StrikeToken
        || token is SubStartToken
        || token is SuperStartToken;
    }

    private void WriteTag(TagContext tag)
    {
      _writer.WriteStartElement(tag.Name);
      var style = tag.ToString();
      if (!string.IsNullOrEmpty(style))
        _writer.WriteAttributeString("style", style);
      _writer.WriteEndAttribute();
      _tags.Push(tag);
    }

    private void EndTag()
    {
      _tags.Pop();
      _writer.WriteEndElement();
    }

    private class TagContext : FormatContext
    {
      public string Name { get; }
      public TagContext Parent { get; }

      public TagContext(string name, TagContext parent)
      {
        Name = name;
        Parent = parent;
      }

      protected override void AddInternal(IToken token)
      {
        if (Parents().SelectMany(t => t).Contains(token))
          return;
        base.AddInternal(token);
      }

      public IEnumerable<TagContext> Parents()
      {
        var curr = this.Parent;
        while (curr != null)
        {
          yield return curr;
          curr = curr.Parent;
        }
      }

      public IEnumerable<TagContext> ParentsAndSelf()
      {
        var curr = this;
        while (curr != null)
        {
          yield return curr;
          curr = curr.Parent;
        }
      }

      public IEnumerable<IToken> AllInherited()
      {
        return ParentsAndSelf()
          .SelectMany(c => c);
      }

      public override string ToString()
      {
        var builder = new StringBuilder();
        foreach (var token in this)
        {
          if (token is Font font)
            WriteCss(builder, font);
          else if (token is FontSize fontSize)
            WriteCss(builder, "font-size", fontSize.Value.ToPt().ToString("0.#") + "pt");
          else if (token is BackgroundColor background)
            WriteCss(builder, "background", "#" + background.Value);
          else if (token is CapitalToken)
            WriteCss(builder, "text-transform", "uppercase");
          else if (token is ForegroundColor color)
            WriteCss(builder, "color", "#" + color.Value);
          else if (token is TextAlign align)
            WriteCss(builder, "text-align", align.Value.ToString().ToLowerInvariant());
        }
        return builder.ToString();
      }

      private void WriteCss(StringBuilder builder, Font font)
      {
        var name = font.Name.IndexOf(' ') > 0 ? "\"" + font.Name + "\"" : font.Name;
        switch (font.Category)
        {
          case FontFamilyCategory.Roman:
            name += ", serif";
            break;
          case FontFamilyCategory.Swiss:
            name += ", sans-serif";
            break;
          case FontFamilyCategory.Modern:
            name += ", monospace";
            break;
          case FontFamilyCategory.Script:
            name += ", cursive";
            break;
          case FontFamilyCategory.Decor:
            name += ", fantasy";
            break;
        }
        WriteCss(builder, "font-family", name);
      }

      private void WriteCss(StringBuilder builder, string property, string value)
      {
        builder.Append(property)
          .Append(":")
          .Append(value)
          .Append(";");
      }
    }
  }
}
