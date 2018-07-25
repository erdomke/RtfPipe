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
    private readonly RtfHtmlSettings _settings;
    private bool _startOfLine = true;

    public Font DefaultFont { get; set; }
    public UnitValue DefaultTabWidth { get; set; }
    public FontSize DefaultFontSize { get; } = new FontSize(UnitValue.FromHalfPoint(24));

    public HtmlWriter(XmlWriter xmlWriter, RtfHtmlSettings settings)
    {
      _writer = xmlWriter;
      _settings = settings ?? new RtfHtmlSettings();
    }

    public void AddText(FormatContext format, string text)
    {
      if (format.OfType<HiddenToken>().Any())
        return;
      _startOfLine = false;
      EnsureSpans(format);
      _tags.Peek().ChildCount++;
      _writer.WriteValue(text);
    }

    public void AddPicture(FormatContext format, Picture picture)
    {
      _startOfLine = false;
      EnsureParagraph(format);
      var uri = _settings?.ImageUriGetter(picture);
      if (!string.IsNullOrEmpty(uri))
      {
        _writer.WriteStartElement("img");

        if (picture.WidthGoal.HasValue)
          _writer.WriteAttributeString("width", picture.WidthGoal.ToPx().ToString("0"));
        else if (picture.Width.HasValue)
          _writer.WriteAttributeString("width", picture.Width.ToPx().ToString("0"));

        if (picture.HeightGoal.HasValue)
          _writer.WriteAttributeString("height", picture.HeightGoal.ToPx().ToString("0"));
        else if (picture.Height.HasValue)
          _writer.WriteAttributeString("height", picture.Height.ToPx().ToString("0"));

        _writer.WriteAttributeString("src", uri);
        _writer.WriteEndElement();
        _tags.Peek().ChildCount++;
      }
    }

    public void AddBreak(FormatContext format, IToken token, int count = 1)
    {
      if (token is ParagraphBreak)
      {
        EnsureParagraph(format);
        while (_tags.Peek().Type != TagType.Paragraph)
          EndTag();
        EndTag();
        _startOfLine = true;
      }
      else if (token is SectionBreak || token is PageBreak)
      {
        EnsureSection(format);
        while (_tags.Peek().Name != "div")
          EndTag();
        EndTag();
        _startOfLine = true;
        if (token is PageBreak)
        {
          format.Add(token);
          EnsureSection(format);
        }
      }
      else if (token is CellBreak)
      {
        EnsureParagraph(format);
        while (_tags.Peek().Name != "td")
          EndTag();
        EndTag();
        _startOfLine = true;
      }
      else if (token is RowBreak)
      {
        while (_tags.Peek().Name != "tr")
          EndTag();
        EndTag();
        _startOfLine = true;
      }
      else if (token is LineBreak)
      {
        _writer.WriteStartElement("br");
        _writer.WriteEndElement();
        _tags.Peek().ChildCount++;
        _startOfLine = true;
      }
      else if (token is Tab)
      {
        if (_startOfLine)
        {
          if (_tags.SafePeek()?.Type == TagType.Section)
          {
            var start = default(UnitValue);
            if (format.TryGetValue<FirstLineIndent>(out var firstIndent))
              start = firstIndent.Value;
            var tab = format.GetTab(count, DefaultTabWidth, start);
            format.Add(new FirstLineIndent(tab.Position));
          }
          else
          {
            var tab = format.GetTab(count, DefaultTabWidth);
            _writer.WriteStartElement("span");
            _writer.WriteAttributeString("style", $"display:inline-block;width:{tab.Position.ToPx():0.#}px");
            _writer.WriteEndElement();
          }
        }
        else
        {
          var width = DefaultTabWidth * count;
          _writer.WriteStartElement("span");
          _writer.WriteAttributeString("style", $"display:inline-block;width:{width.ToPx():0.#}px");
          _writer.WriteEndElement();
        }
        _startOfLine = false;
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
      tag.AddRange(format.Where(t => t.Type == TokenType.SectionFormat || t is PageBreak));
      tag.Add(DefaultFontSize);
      if (DefaultFont != null) tag.Add(DefaultFont);
      WriteTag(tag);
    }

    private void EnsureParagraph(FormatContext format)
    {
      var firstParaSection = _tags
        .SkipWhile(t => t.Type == TagType.Span)
        .FirstOrDefault();
      if (firstParaSection?.Type == TagType.Paragraph)
        return;

      EnsureSection(format);

      if (format.Any(t => t is ParagraphNumbering || t is ListLevelType))
      {
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "ul" || _tags.Peek().Name == "ol"))
          EndTag();

        if (!(_tags.Peek().Name == "ol" || _tags.Peek().Name == "ul"))
        {
          var numType = format.OfType<ListLevelType>().FirstOrDefault()?.Value
            ?? (format.OfType<NumberLevelBullet>().Any() ? (NumberingType?)NumberingType.Bullet : null)
            ?? format.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
            ?? NumberingType.Bullet;

          var listTag = default(TagContext);
          if (numType == NumberingType.Bullet)
            listTag = new TagContext("ul", _tags.SafePeek());
          else
            listTag = new TagContext("ol", _tags.SafePeek());

          listTag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
            && !IsSpanElement(t)));
          WriteTag(listTag);

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
        }

        var tag = new TagContext("li", _tags.SafePeek());
        tag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
          && !IsSpanElement(t)));
        WriteTag(tag);
      }
      else if (format.InTable)
      {
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "table" || _tags.Peek().Name == "tr"))
          EndTag();

        var tag = default(TagContext);
        if (!(_tags.Peek().Name == "table" || _tags.Peek().Name == "tr"))
        {
          tag = new TagContext("table", _tags.SafePeek());
          WriteTag(tag);
        }

        if (_tags.Peek().Name == "table")
        {
          tag = new TagContext("tr", _tags.SafePeek());
          WriteTag(tag);
        }

        tag = new TagContext("td", _tags.SafePeek());
        tag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
          && !IsSpanElement(t)));
        WriteTag(tag);
      }
      else if (format.TryGetValue<OutlineLevel>(out var outline) && outline.Value >= 0 && outline.Value < 6)
      {
        var tagName = "h" + (outline.Value + 1);
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == tagName))
          EndTag();

        var tag = new TagContext(tagName, _tags.SafePeek());
        tag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
          && !IsSpanElement(t)));
        WriteTag(tag);
      }
      else
      {
        while (_tags.Peek().Name != "div")
          EndTag();

        var tag = new TagContext("p", _tags.SafePeek());
        tag.AddRange(format.Where(t => (t.Type == TokenType.ParagraphFormat || t.Type == TokenType.CharacterFormat)
          && !IsSpanElement(t)));
        WriteTag(tag);
      }
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

      var paragraphFormats = _tags.First(t => t.Type == TagType.Paragraph).ToList();
      existing = existing.Where(t => !intersection.Contains(t) && !paragraphFormats.Contains(t)).ToList();
      requested = requested.Where(t => !intersection.Contains(t)).ToList();

      if (existing.Count > 0 && _tags.Peek().Type != TagType.Paragraph)
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
      else if (TryGetValue<HyperlinkToken, IToken>(requested, out var hyperlink))
      {
        WriteSpanElement(format, "a", hyperlink, requested, w =>
        {
          if (!string.IsNullOrEmpty(hyperlink.Url))
            w.WriteAttributeString("href", hyperlink.Url);
          if (!string.IsNullOrEmpty(hyperlink.Target))
            w.WriteAttributeString("target", hyperlink.Target);
          if (!string.IsNullOrEmpty(hyperlink.Title))
            w.WriteAttributeString("title", hyperlink.Title);
        });
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
      else if (TryGetValue<BookmarkToken, IToken>(requested, out var bookmark))
      {
        WriteSpanElement(format, "a", bookmark, requested, w =>
        {
          if (!string.IsNullOrEmpty(bookmark.Id))
            w.WriteAttributeString("id", bookmark.Id);
        });
      }
      else if (requested.Count > 0)
      {
        WriteSpan(format);
      }
    }

    private void WriteSpanElement(FormatContext format, string name, IToken spanToken
      , IEnumerable<IToken> requested, Action<XmlWriter> attributes = null)
    {
      var tag = new TagContext(name, _tags.Peek());
      if (requested.Any(t => IsSpanElement(t, name) && t != spanToken))
      {
        tag.Add(spanToken);
        WriteTag(tag);
        attributes?.Invoke(_writer);
        EnsureSpans(format);
      }
      else
      {
        tag.AddRange(requested);
        WriteTag(tag);
        attributes?.Invoke(_writer);
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

    private bool IsSpanElement(IToken token, string context = null)
    {
      return token is BoldToken
        || token is ItalicToken
        || (token is UnderlineToken && context != "a")
        || token is StrikeToken
        || token is SubStartToken
        || token is SuperStartToken
        || token is HyperlinkToken
        || token is BookmarkToken;
    }

    private void WriteTag(TagContext tag)
    {
      _writer.WriteStartElement(tag.Name);
      var style = tag.ToString();
      if (!string.IsNullOrEmpty(style))
        _writer.WriteAttributeString("style", style);
      if (_tags.Count > 0)
        _tags.Peek().ChildCount++;
      _tags.Push(tag);
    }

    private void EndTag()
    {
      var tag = _tags.Pop();
      if (tag.Type == TagType.Paragraph && tag.ChildCount == 0)
      {
        _writer.WriteStartElement("br");
        _writer.WriteEndElement();
      }
      _writer.WriteEndElement();
    }

    private enum TagType
    {
      Span,
      Paragraph,
      Section
    }

    private class TagContext : FormatContext
    {
      public string Name { get; }
      public TagContext Parent { get; }
      public TagType Type { get; }
      public int ChildCount { get; set; }

      public TagContext(string name, TagContext parent)
      {
        Name = name;
        Parent = parent;

        if (name == "div" || name == "table" || name == "tr"
            || name == "ul" || name == "ol")
        {
          Type = TagType.Section;
        }
        else if (name == "p" || name == "li" || name == "td"
          || name == "h1" || name == "h2" || name == "h3"
          || name == "h4" || name == "h5" || name == "h6")
        {
          Type = TagType.Paragraph;
        }
        else
        {
          Type = TagType.Span;
        }
      }

      protected override void AddInternal(IToken token)
      {
        if (Parents().FirstOrDefault(t => t.Any(SameTokenPredicate(token)))
          ?.Contains(token) == true)
        {
          return;
        }
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
        var context = new FormatContext();
        context.AddRange(ParentsAndSelf()
          .Reverse()
          .SelectMany(c => c));
        return context;
      }

      public override string ToString()
      {
        var builder = new StringBuilder();
        var margins = new UnitValue[4];
        var underline = false;

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
          else if (token is SpaceAfter spaceAfter)
            margins[2] = spaceAfter.Value;
          else if (token is SpaceBefore spaceBefore)
            margins[0] = spaceBefore.Value;
          else if (token is UnderlineToken underlineToken)
            underline = underlineToken.Value;
          else if (token is PageBreak)
            WriteCss(builder, "page-break-before", "always");
        }

        if (Type == TagType.Paragraph)
          WriteCss(builder, "margin", WriteBoxShort(margins));
        else if (Name == "a")
          WriteCss(builder, "text-decoration", underline ? "underline" : "none");

        return builder.ToString();
      }

      private string WriteBoxShort(params UnitValue[] values)
      {
        var result = new StringBuilder();
        for (var i = 0; i < values.Length; i++)
        {
          if (i > 0)
            result.Append(' ');
          var value = values[i].ToPx().ToString("0.#");
          result.Append(value);
          if (value != "0")
            result.Append("px");
        }
        return result.ToString();
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
