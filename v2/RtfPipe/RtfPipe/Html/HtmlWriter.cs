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
    private readonly Dictionary<KeyValuePair<int, int>, int> _listPositions = new Dictionary<KeyValuePair<int, int>, int>();
    private KeyValuePair<int, int> _lastListLevel;
    private bool _needListEnd;

    public Font DefaultFont { get; set; }
    public UnitValue DefaultTabWidth { get; set; }

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

        if (_tags.Peek().Name == "td")
          WriteTag(ParagraphTag("p", format));
        else if (_tags.Peek().Name == "li")
          _needListEnd = true;
        else
          EndTag();
        _startOfLine = true;
      }
      else if (token is SectionBreak || token is PageBreak)
      {
        EnsureSection(format);
        while (_tags.Count > 1)
          EndTag();
        EndTag();
        _startOfLine = true;
        if (token is PageBreak)
        {
          format.Add(token);
          EnsureSection(format);
        }
      }
      else if (token is FootnoteBreak)
      {
        EnsureSection(format);
        while (_tags.Count > 1)
          EndTag();
        _writer.WriteStartElement("hr");
        _writer.WriteAttributeString("style", "width:2in;border:0.5px solid black;margin-left:0");
        _writer.WriteEndElement();
        _startOfLine = true;
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
      if (DefaultFont != null) tag.Add(DefaultFont);
      WriteTag(tag);
    }

    private void EnsureParagraph(FormatContext format)
    {
      var firstParaSection = _tags
        .SkipWhile(t => t.Type == TagType.Span)
        .FirstOrDefault();
      if (firstParaSection?.Type == TagType.Paragraph && !_needListEnd)
        return;

      EnsureSection(format);

      if (format.Any(t => t is ParagraphNumbering || t is ListLevelType))
      {
        var listLevel = new KeyValuePair<int, int>(
            format.OfType<ListStyleId>().FirstOrDefault()?.Value ?? 1
          , format.OfType<ListLevelNumber>().FirstOrDefault()?.Value ?? 0);

        if (listLevel.Key == _lastListLevel.Key)
        {
          while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "ul" || _tags.Peek().Name == "ol" || _tags.Peek().Name == "li"))
            EndTag();

          for (var i = 0; i < _lastListLevel.Value - listLevel.Value; i++)
          {
            if (_tags.Peek().Name == "li")
              EndTag();
            if (_tags.Peek().Name == "ol" || _tags.Peek().Name == "ul")
              EndTag();
          }

          if (_lastListLevel.Value >= listLevel.Value && _tags.Peek().Name == "li" && _needListEnd)
            EndTag();

          if (_lastListLevel.Value > listLevel.Value)
          {
            foreach (var keyToRemove in _listPositions.Keys
              .Where(k => k.Key == listLevel.Key && k.Value > listLevel.Value)
              .ToList())
            {
              _listPositions.Remove(keyToRemove);
            }
          }
        }
        else
        {
          while (_tags.Peek().Name != "div")
            EndTag();
        }
        _lastListLevel = listLevel;

        if (!_listPositions.TryGetValue(listLevel, out var startAt))
          startAt = format.OfType<NumberingStart>().FirstOrDefault()?.Value - 1 ?? 0;
        _listPositions[listLevel] = ++startAt;

        if (!(_tags.Peek().Name == "ol" || _tags.Peek().Name == "ul"))
        {
          var numType = format.OfType<ListLevelType>().FirstOrDefault()?.Value
            ?? (format.OfType<NumberLevelBullet>().Any() ? (NumberingType?)NumberingType.Bullet : null)
            ?? format.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
            ?? NumberingType.Bullet;

          var tag = new TagContext(numType == NumberingType.Bullet ? "ul" : "ol", _tags.SafePeek());
          tag.AddRange(format.Where(t => !IsSpanElement(t) && !(t is CapitalToken)
            && (t.Type == TokenType.ParagraphFormat
              || t.Type == TokenType.CharacterFormat)));
          WriteTag(tag);

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

          if (startAt > 1 && numType != NumberingType.Bullet)
            _writer.WriteAttributeString("start", startAt.ToString());
        }

        WriteTag(ParagraphTag("li", format));
      }
      else if (format.InTable)
      {
        _lastListLevel = default;
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "table" || _tags.Peek().Name == "tr"))
          EndTag();

        var tag = default(TagContext);
        if (!(_tags.Peek().Name == "table" || _tags.Peek().Name == "tr"))
        {
          tag = new TagContext("table", _tags.SafePeek());
          tag.AddRange(format.Where(t => t is CellSpacing || t is RowLeft));
          WriteTag(tag);
        }

        if (_tags.Peek().Name == "table")
        {
          tag = new TagContext("tr", _tags.SafePeek());
          WriteTag(tag);
        }

        var cellTag = ParagraphTag("td", format);
        WriteTag(cellTag);
        var cellToken = cellTag.CellToken();
        if (cellToken?.ColSpan > 1)
          _writer.WriteAttributeString("colspan", cellToken.ColSpan.ToString());
      }
      else if (format.TryGetValue<OutlineLevel>(out var outline) && outline.Value >= 0 && outline.Value < 6)
      {
        _lastListLevel = default;
        var tagName = "h" + (outline.Value + 1);
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == tagName))
          EndTag();

        WriteTag(ParagraphTag(tagName, format));
      }
      else
      {
        _lastListLevel = default;
        while (_tags.Peek().Name != "div")
          EndTag();

        var newBorders = format.OfType<BorderToken>().ToList();
        var existingBorders = _tags.Peek().OfType<BorderToken>().ToList();
        if (newBorders.Count != existingBorders.Count || newBorders.Except(existingBorders).Any())
        {
          if (existingBorders.Count > 0)
            EndTag();

          if (newBorders.Count > 0)
          {
            var tag = new TagContext("div", _tags.SafePeek());
            tag.AddRange(format.OfType<BorderToken>());
            WriteTag(tag);
          }
        }

        WriteTag(ParagraphTag("p", format));
      }

      _needListEnd = false;
    }

    private TagContext ParagraphTag(string name, FormatContext format)
    {
      var tag = new TagContext(name, _tags.SafePeek());
      tag.AddRange(format.Where(t => !IsSpanElement(t) && !(t is CapitalToken)
        && (t.Type == TokenType.ParagraphFormat
          || t.Type == TokenType.CharacterFormat
          || t.Type == TokenType.RowFormat)));
      return tag;
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

      var paragraphFormats = _tags.Peek().AllParagraph.ToList();
      existing = existing.Where(t => !intersection.Contains(t)).ToList();
      requested = requested.Where(t => !intersection.Contains(t)).ToList();

      if (_tags.Peek().Type != TagType.Paragraph && existing.Any(t => !paragraphFormats.Contains(t)))
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
      else if (TryGetUnderline(requested, out var underline))
      {
        WriteSpanElement(format, "u", underline, requested);
      }
      else if (TryGetValue<StrikeToken, IToken>(requested, out var strike))
      {
        WriteSpanElement(format, "s", strike, requested);
      }
      else if (TryGetValue<StrikeDoubleToken, IToken>(requested, out var strikeDouble))
      {
        WriteSpanElement(format, "s", strikeDouble, requested);
      }
      else if (TryGetValue<SubStartToken, IToken>(requested, out var sub))
      {
        WriteSpanElement(format, "sub", sub, requested);
      }
      else if (TryGetValue<SuperStartToken, IToken>(requested, out var super))
      {
        WriteSpanElement(format, "sup", super, requested);
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
        if (name == "u" && format.OfType<UnderlineColor>().Any())
          tag.AddRange(format.OfType<UnderlineColor>());
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

    private bool TryGetUnderline(IEnumerable<IToken> tokens, out IToken value)
    {
      value = tokens.FirstOrDefault(FormatContext.IsUnderline);
      return value != null;
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
      if (tag.Any())
        WriteTag(tag);
    }

    private List<IToken> CharacterFormats(FormatContext context)
    {
      var tokens = (IEnumerable<IToken>)context;
      if (context is TagContext tag)
        tokens = tag.AllInherited;
      return tokens
        .Where(t => t.Type == TokenType.CharacterFormat)
        .ToList();
    }

    private bool IsSpanElement(IToken token, string context = null)
    {
      return token is BoldToken
        || token is ItalicToken
        || (FormatContext.IsUnderline(token) && context != "a")
        || token is StrikeToken
        || token is StrikeDoubleToken
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
      private readonly FormatContext _allParagraph = new FormatContext();
      private readonly FormatContext _all = new FormatContext();

      private static readonly UnitValue _defaultFontSize = UnitValue.FromHalfPoint(24);
      private static readonly ColorValue _defaultColor = new ColorValue(0, 0, 0);

      public IEnumerable<IToken> AllInherited { get { return _all; } }
      public IEnumerable<IToken> AllParagraph { get { return _allParagraph; } }

      public string Name { get; }
      public TagContext Parent { get; }
      public TagType Type { get; }
      public int ChildCount { get; set; }
      public int CellIndex { get; set; }

      public TagContext(string name, TagContext parent)
      {
        Name = name;
        Parent = parent;

        if (name == "td")
          CellIndex = parent.CellIndex++;

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

        if (parent != null)
        {
          _allParagraph.AddRange(parent._allParagraph);
          _all.AddRange(parent._all);
        }
      }

      protected override void AddInternal(IToken token)
      {
        if (_all.Contains(token))
          return;

        // Don't need to set the default font-size
        if (token is FontSize size && size.Value == _defaultFontSize && !_all.OfType<FontSize>().Any())
          return;

        // Don't need to set the default color
        if (token is ForegroundColor foreColor && foreColor.Value == _defaultColor && !_all.OfType<ForegroundColor>().Any())
          return;

        base.AddInternal(token);
        _all.Add(token);
        if (Type != TagType.Span)
          _allParagraph.Add(token);
      }

      public CellToken CellToken()
      {
        return this.OfType<CellToken>().FirstOrDefault(t => t.Index == CellIndex);
      }

      public override string ToString()
      {
        var builder = new StringBuilder();
        var margins = new UnitValue[4];
        var borders = new BorderToken[4];
        var padding = new UnitValue[4];
        var underline = false;

        if (Parent == null && !this.OfType<FontSize>().Any())
          WriteCss(builder, "font-size", _defaultFontSize.ToPt().ToString("0.#") + "pt");

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
          else if (token is RowLeft rowLeft)
            margins[3] = rowLeft.Value;
          else if (token is UnderlineToken underlineToken)
            underline = underlineToken.Value;
          else if (token is PageBreak)
            WriteCss(builder, "page-break-before", "always");
          else if (token is LeftIndent leftIndent && (leftIndent.Value.ToPx() != 0 || Name == "ul" || Name == "ol"))
            margins[3] = leftIndent.Value;
          else if (token is BorderToken border)
            borders[(int)border.Side] = border;
          else if (token is TablePaddingTop paddingTop)
            padding[0] = paddingTop.Value;
          else if (token is TablePaddingRight paddingRight)
            padding[1] = paddingRight.Value;
          else if (token is TablePaddingBottom paddingBottom)
            padding[2] = paddingBottom.Value;
          else if (token is TablePaddingLeft paddingLeft)
            padding[3] = paddingLeft.Value;
          else if (token is EmbossText)
            WriteCss(builder, "text-shadow", "-1px -1px 1px #999, 1px 1px 1px #000");
          else if (token is EngraveText)
            WriteCss(builder, "text-shadow", "-1px -1px 1px #000, 1px 1px 1px #999");
          else if (token is OutlineText)
            WriteCss(builder, "text-shadow", "-1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000");
          else if (token is ShadowText)
            WriteCss(builder, "text-shadow", "1px 1px 1px #CCC");
          else if (token is SmallCapitalToken)
            WriteCss(builder, "font-variant", "small-caps");
          else if (token is UnderlineDashed || token is UnderlineDashDot || token is UnderlineLongDash || token is UnderlineThickDash || token is UnderlineThickDashDot || token is UnderlineThickLongDash)
            WriteCss(builder, "text-decoration-style", "dashed");
          else if (token is UnderlineDotted || token is UnderlineDashDotDot || token is UnderlineThickDotted || token is UnderlineThickDashDotDot)
            WriteCss(builder, "text-decoration-style", "dotted");
          else if (token is UnderlineWave || token is UnderlineHeavyWave || token is UnderlineDoubleWave)
            WriteCss(builder, "text-decoration-style", "wavy");
          else if (token is StrikeDoubleToken || token is UnderlineDouble)
            WriteCss(builder, "text-decoration-style", "double");
          else if (token is UnderlineColor underlineColor)
            WriteCss(builder, "text-decoration-color", "#" + underlineColor.Value);
          else if (token is UnderlineWord)
            WriteCss(builder, "text-decoration-skip", "spaces");
          else if (token is OffsetToken offset)
            WriteCss(builder, "top", PxString(offset.Value)).Append("position:relative;");
        }

        if (this.OfType<OutlineText>().Any() && !this.OfType<ForegroundColor>().Any())
          WriteCss(builder, "color", "#fff");

        var cell = CellToken();
        if (cell != null)
        {
          for (var i = 0; i < 4; i++)
          {
            borders[i] = cell.Borders[i] ?? borders[i];
            padding[i] += margins[i];
            margins[i] = UnitValue.Empty;
          }

          if (cell.VerticalAlignment != VerticalAlignment.Center)
            WriteCss(builder, "vertical-align"
              , cell.VerticalAlignment == VerticalAlignment.Center ? "middle" : cell.VerticalAlignment.ToString().ToLowerInvariant());

          if (cell.BackgroundColor != null && !this.OfType<BackgroundColor>().Any())
            WriteCss(builder, "background", "#" + cell.BackgroundColor);
        }

        if (Name == "table")
        {
          WriteCss(builder, "border-spacing", "0");
        }

        if (Name == "ul" || Name == "ol")
        {
          if (!this.OfType<LeftIndent>().Any())
            margins[3] = new UnitValue(720, UnitType.Twip);

          var lastMargin = Parents().FirstOrDefault(t => t.Name == "ol" || t.Name == "ul")
            ?.OfType<LeftIndent>().FirstOrDefault()?.Value
            ?? new UnitValue(0, UnitType.Twip);
          margins[3] -= lastMargin;

          var firstLineIndent = AllInherited.OfType<FirstLineIndent>().FirstOrDefault()?.Value ?? new UnitValue(0, UnitType.Twip);
          if (firstLineIndent > new UnitValue(-0.125, UnitType.Inch))
            margins[3] += new UnitValue(0.25, UnitType.Inch);
          WriteCss(builder, "margin", WriteBoxShort(margins));
          WriteCss(builder, "padding-left", "0");
        }
        else if (Name == "p" || !margins.All(v => v.ToPx() == 0))
        {
          WriteCss(builder, "margin", WriteBoxShort(margins));
        }
        else if (Name == "a")
        {
          WriteCss(builder, "text-decoration", underline ? "underline" : "none");
        }

        if (borders.All(b => b != null) && borders.Skip(1).All(b => b.SameBorderStyle(borders[0])))
        {
          WriteCss(builder, "border", BorderString(borders[0]));
        }
        else
        {
          if (borders[0] != null)
            WriteCss(builder, "border-top", BorderString(borders[0]));
          if (borders[1] != null)
            WriteCss(builder, "border-right", BorderString(borders[1]));
          if (borders[2] != null)
            WriteCss(builder, "border-bottom", BorderString(borders[2]));
          if (borders[3] != null)
            WriteCss(builder, "border-left", BorderString(borders[3]));
        }

        for (var i = 0; i < borders.Length; i++)
        {
          if (!padding[i].HasValue && borders[i] != null)
            padding[i] = borders[i].Padding;
        }

        if (padding.Any(p => p.Value > 0) || Name == "td")
          WriteCss(builder, "padding", WriteBoxShort(padding));

        if (cell != null)
        {
          var width = UnitValue.Empty;
          if (cell.Width > 0 && cell.WidthUnit == CellWidthUnit.Twip)
            width = new UnitValue(cell.Width, UnitType.Twip);
          else if (cell.BoundaryDiff.Value > 0)
            width = cell.BoundaryDiff;

          if (width.Value > 0)
          {
            if (borders[1] != null)
              width -= new UnitValue(Math.Round(borders[1].Width.ToPx()), UnitType.Pixel);
            if (borders[3] != null)
              width -= new UnitValue(Math.Round(borders[3].Width.ToPx()), UnitType.Pixel);
            width -= padding[1] + padding[3];

            WriteCss(builder, "width", PxString(width));
          }
        }

        return builder.ToString();
      }

      private IEnumerable<TagContext> Parents()
      {
        var parent = this.Parent;
        while (parent != null)
        {
          yield return parent;
          parent = parent.Parent;
        }
      }

      private string PxString(UnitValue value)
      {
        var px = value.ToPx().ToString("0.#");
        if (px == "0")
          return px;
        return px + "px";
      }

      private string BorderString(BorderToken border)
      {
        var width = border.Width.ToPx();
        if (width < 0.5 || border.Style == BorderStyle.Hairline)
          width = 1;
        else if (border.Style == BorderStyle.DoubleThick)
          width *= 2;
        else if (border.Style == BorderStyle.Triple)
          width *= 3;

        if (width <= 0 || border.Style == BorderStyle.None)
          return "none";

        var style = width.ToString("0") + "px ";

        switch (border.Style)
        {
          case BorderStyle.Dashed:
          case BorderStyle.DashedSmall:
          case BorderStyle.DotDashed:
            style += "dashed";
            break;
          case BorderStyle.Dotted:
          case BorderStyle.DotDotDashed:
            style += "dotted";
            break;
          case BorderStyle.Double:
          case BorderStyle.DoubleWavy:
          case BorderStyle.ThickThinLarge:
          case BorderStyle.ThickThinMedium:
          case BorderStyle.ThickThinSmall:
          case BorderStyle.ThinThickLarge:
          case BorderStyle.ThinThickMedium:
          case BorderStyle.ThinThickSmall:
          case BorderStyle.ThinThickThinLarge:
          case BorderStyle.ThinThickThinMedium:
          case BorderStyle.ThinThickThinSmall:
            style += "double";
            break;
          case BorderStyle.Outset:
            style += "outset";
            break;
          case BorderStyle.Inset:
            style += "inset";
            break;
          case BorderStyle.Embossed:
          case BorderStyle.Frame:
            style += "ridge";
            break;
          case BorderStyle.Engraved:
            style += "groove";
            break;
          default:
            style += "solid";
            break;
        }

        var color = border.Color ?? new ColorValue(0, 0, 0);
        style += " #" + color;

        return style;
      }

      private string WriteBoxShort(params UnitValue[] values)
      {
        if (values.Distinct().Count() == 1)
        {
          return PxString(values[0]);
        }

        var result = new StringBuilder();
        for (var i = 0; i < values.Length; i++)
        {
          if (i > 0)
            result.Append(' ');
          result.Append(PxString(values[i]));
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

      private StringBuilder WriteCss(StringBuilder builder, string property, string value)
      {
        builder.Append(property)
          .Append(":")
          .Append(value)
          .Append(";");
        return builder;
      }
    }
  }
}
