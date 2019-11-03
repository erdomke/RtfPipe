using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private WriteState _state;
    private int _attachIndex;
    private Type _lastTokenType;

    private enum WriteState
    {
      Other,
      NeedListEnd,
      NeedParagraphStart
    }

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
      _writer.WriteValue(AddNonBreaking(text
        , format.OfType<Font>().Any(f => TextEncoding.IsEastAsian(f.Encoding))));
      _lastTokenType = typeof(TextToken);
    }

    private string AddNonBreaking(string text, bool eastAsian = false)
    {
      var output = text.ToCharArray();
      for (var i = 1; i < text.Length; i++)
      {
        if (text[i] == text[i - 1] && text[i] == ' ')
          output[i] = eastAsian ? '\u2007' : '\u00a0';
      }
      return new string(output);
    }

    public void AddPicture(FormatContext format, Picture picture)
    {
      _startOfLine = false;
      EnsureParagraph(format);
      var uri = _settings?.ImageUriGetter(picture);
      if (!string.IsNullOrEmpty(uri))
      {
        _writer.WriteStartElement("img");

        if (picture.Width.HasValue)
          _writer.WriteAttributeString("width", picture.Width.ToPx().ToString("0"));

        if (picture.Height.HasValue)
          _writer.WriteAttributeString("height", picture.Height.ToPx().ToString("0"));

        _writer.WriteAttributeString("src", uri);
        _writer.WriteEndElement();
        _tags.Peek().ChildCount++;
        _lastTokenType = typeof(Picture);
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
        {
          _state = WriteState.NeedParagraphStart;
          if (_lastTokenType == typeof(LineBreak))
            _writer.WriteValue("\u00a0");
        }
        else if (_tags.Peek().Name == "li")
        {
          _state = WriteState.NeedListEnd;
        }
        else
        {
          EndTag();
        }
        _startOfLine = true;
        format.RemoveFirstOfType<SingleLineIndent>();
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
        _state = WriteState.Other;
        format.RemoveFirstOfType<SingleLineIndent>();
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
        _state = WriteState.Other;
      }
      else if (token is CellBreak || token is NestCell)
      {
        if (_state == WriteState.NeedListEnd)
        {
          while (_tags.Peek().Name != "td")
            EndTag();
          WriteTag(ParagraphTag("p", format));
        }
        else
        {
          EnsureParagraph(format);
        }

        while (_tags.Peek().Name != "td")
          EndTag();
        EndTag();
        _startOfLine = true;
        _state = WriteState.Other;
        format.RemoveFirstOfType<SingleLineIndent>();
      }
      else if (token is RowBreak || token is NestRow)
      {
        while (_tags.Peek().Name != "tr")
          EndTag();
        EndTag();
        _startOfLine = true;
        _state = WriteState.Other;
        format.RemoveFirstOfType<SingleLineIndent>();
      }
      else if (token is LineBreak)
      {
        EnsureSection(format);
        _writer.WriteStartElement("br");
        _writer.WriteEndElement();
        _tags.Peek().ChildCount++;
        _startOfLine = true;
        format.RemoveFirstOfType<SingleLineIndent>();
      }
      else if (token is Tab)
      {
        if (_startOfLine)
        {
          if (_tags.PeekOrDefault()?.Type == TagType.Section)
          {
            var start = default(UnitValue);
            if (format.TryGetValue<FirstLineIndent>(out var firstIndent))
              start = firstIndent.Value;
            var tab = format.GetTab(count, DefaultTabWidth, start);
            format.Add(new SingleLineIndent(tab.Position));
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
      else if (token is ObjectAttachment)
      {
        EnsureParagraph(format);
        _settings.AttachmentRenderer(_attachIndex, _writer);
        _attachIndex++;
      }
      _lastTokenType = token.GetType();
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

      var tag = new TagContext("div", _tags.PeekOrDefault());
      tag.AddRange(format.Where(t => t.Type == TokenType.SectionFormat || t is PageBreak));
      if (DefaultFont != null) tag.Add(DefaultFont);
      WriteTag(tag);
    }

    public void EnsureCell(FormatContext format)
    {
      _state = WriteState.Other;
      EnsureParagraph(format);
    }

    private void EnsureParagraph(FormatContext format)
    {
      var firstParaSection = _tags
        .SkipWhile(t => t.Type == TagType.Span)
        .FirstOrDefault();

      var currLevel = _tags.Count(t => t.Name == "td");
      var reqLevel = format.OfType<NestingLevel>().FirstOrDefault()?.Value ?? (format.InTable ? 1 : 0);
      if (firstParaSection?.Type == TagType.Paragraph && _state == WriteState.Other && currLevel == reqLevel)
        return;

      EnsureSection(format);

      if (format.InTable)
      {
        var tableLevel = _tags.Count(t => t.Name == "table");
        while (currLevel < reqLevel)
        {
          while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "table" || _tags.Peek().Name == "tr"
            || _tags.Peek().Name == "tbody" || _tags.Peek().Name == "thead" || _tags.Peek().Name == "td"))
          {
            EndTag();
          }

          var tag = default(TagContext);
          if (tableLevel < reqLevel)
          {
            if (_tags.Peek().Name == "table" || _tags.Peek().Name == "tbody" || _tags.Peek().Name == "thead")
              WriteTag(new TagContext("tr", _tags.PeekOrDefault()));

            if (_tags.Peek().Name == "tr")
            {
              WriteTag(new TagContext("td", _tags.PeekOrDefault()));
              currLevel++;
            }
          }

          if (!(_tags.Peek().Name == "table" || _tags.Peek().Name == "tr" || _tags.Peek().Name == "tbody" || _tags.Peek().Name == "thead"))
          {
            tag = new TagContext("table", _tags.PeekOrDefault());
            tag.AddRange(format.Where(t => t is CellSpacing || t is RowLeft));
            WriteTag(tag);
            tableLevel++;
            var boundaries = format.OfType<ColumnBoundaries>().FirstOrDefault();
            if (boundaries != null)
            {
              _writer.WriteStartElement("colgroup");
              var rightBorders = boundaries
                .GroupBy(v => v.Value)
                .OrderBy(g => g.Key)
                .Select(g => UnitValue.Average(g.Select(k => k.Key)))
                .ToList();
              var widths = new UnitValue[rightBorders.Count];
              for (var i = 0; i < rightBorders.Count; i++)
              {
                widths[i] = i == 0
                  ? rightBorders[i]
                  : rightBorders[i] - rightBorders[i - 1];
              }

              var cells = format.OfType<CellToken>().ToList();
              var everyCell = cells.Count == widths.Length || cells.Count == (widths.Length * 2);
              for (var i = 0; i < widths.Length; i++)
              {
                if (everyCell && cells[i].WidthUnit == CellWidthUnit.Twip)
                  widths[i] = new UnitValue(cells[i].Width, UnitType.Twip);

                _writer.WriteStartElement("col");
                _writer.WriteAttributeString("style", "width:" + TagContext.PxString(widths[i]));
                _writer.WriteEndElement();

              }

              _writer.WriteEndElement();
            }
          }

          if (format.OfType<HeaderRow>().Any())
          {
            if (_tags.Peek().Name == "tbody")
              EndTag();

            if (_tags.Peek().Name == "table")
              WriteTag(new TagContext("thead", _tags.PeekOrDefault()));
          }
          else if (_tags.Peek().Name == "thead")
          {
            EndTag();
            WriteTag(new TagContext("tbody", _tags.PeekOrDefault()));
          }

          if (_tags.Peek().Name == "table" || _tags.Peek().Name == "tbody" || _tags.Peek().Name == "thead")
            WriteTag(new TagContext("tr", _tags.PeekOrDefault()));

          var cellTag = ParagraphTag("td", format);
          WriteTag(cellTag);
          var cellToken = cellTag.CellToken();
          if (cellToken?.ColSpan > 1)
            _writer.WriteAttributeString("colspan", cellToken.ColSpan.ToString());
          _state = WriteState.Other;

          currLevel++;
        }

        while (reqLevel < tableLevel)
        {
          while (_tags.Peek().Name != "table")
            EndTag();
          EndTag();
          tableLevel--;
        }
      }

      if (format.Any(IsListMarker))
      {
        var listLevel = new KeyValuePair<int, int>(
            format.OfType<ListStyleId>().FirstOrDefault()?.Value ?? 1
          , format.OfType<ListLevelNumber>().FirstOrDefault()?.Value ?? 0);

        if (listLevel.Key == _lastListLevel.Key)
        {
          while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "ul" || _tags.Peek().Name == "ol" || _tags.Peek().Name == "li" || _tags.Peek().Name == "td"))
            EndTag();

          for (var i = 0; i < _lastListLevel.Value - listLevel.Value; i++)
          {
            if (_tags.Peek().Name == "li")
              EndTag();
            if (_tags.Peek().Name == "ol" || _tags.Peek().Name == "ul")
              EndTag();
          }

          if (_lastListLevel.Value >= listLevel.Value && _tags.Peek().Name == "li" && _state == WriteState.NeedListEnd)
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
          while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == "td"))
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

          var tag = new TagContext(numType == NumberingType.Bullet ? "ul" : "ol", _tags.PeekOrDefault());
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
      else if (format.TryGetValue<OutlineLevel>(out var outline) && outline.Value >= 0 && outline.Value < 6)
      {
        _lastListLevel = default;
        var tagName = "h" + (outline.Value + 1);
        while (!(_tags.Peek().Name == "div" || _tags.Peek().Name == tagName || _tags.Peek().Name == "td"))
          EndTag();

        WriteTag(ParagraphTag(tagName, format));
      }
      else if (_tags.Peek().Name != "td")
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
            var tag = new TagContext("div", _tags.PeekOrDefault());
            tag.AddRange(format.Where(t => t is BorderToken
              || t is ParaBackgroundColor
              || t is LeftIndent
              || t is RightIndent));
            WriteTag(tag);
          }
        }

        WriteTag(ParagraphTag("p", format));
      }
      else if (_state == WriteState.NeedParagraphStart)
      {
        WriteTag(ParagraphTag("p", format));
      }

      _state = WriteState.Other;
    }

    private TagContext ParagraphTag(string name, FormatContext format)
    {
      var tag = new TagContext(name, _tags.PeekOrDefault());
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
          tag.AddRange(format.OfType<UnderlineColor>().OfType<IToken>());
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
      var tag = new TagContext("span", _tags.PeekOrDefault());
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

    private static bool IsListMarker(IToken token)
    {
      return token is ParagraphNumbering || token is ListLevelType;
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
      if (tag.Type == TagType.Paragraph)
      {
        if (tag.ChildCount == 0)
        {
          _writer.WriteStartElement("br");
          _writer.WriteEndElement();
        }
        else if (_lastTokenType == typeof(LineBreak))
        {
          _writer.WriteValue("\u00a0");
        }
      }
      _writer.WriteEndElement();
    }

    private enum TagType
    {
      Span,
      Paragraph,
      Section
    }

    [DebuggerDisplay("{Name}")]
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
            || Name == "tbody" || Name == "thead"
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
        const double DefaultBrowserLineHeight = 1.2;

        if (!this.OfType<FontSize>().Any())
        {
          if (Parent == null)
          {
            WriteCss(builder, "font-size", _defaultFontSize.ToPt().ToString("0.#") + "pt");
          }
          else if (Name == "table")
          {
            var fontSize = _all.OfType<FontSize>().FirstOrDefault()?.Value ?? _defaultFontSize;
            WriteCss(builder, "font-size", fontSize.ToPt().ToString("0.#") + "pt");
            WriteCss(builder, "box-sizing", "border-box");
            if (!this.OfType<TextAlign>().Any() && _all.TryGetValue<TextAlign>(out var align) && align.Value != TextAlignment.Left)
            {
              WriteCss(builder, "text-align", align.Value.ToString().ToLowerInvariant());
            }
          }
        }

        foreach (var token in this)
        {
          if (token is Font font)
            WriteCss(builder, font);
          else if (token is FontSize fontSize)
            WriteCss(builder, "font-size", fontSize.Value.ToPt().ToString("0.#") + "pt");
          else if (token is BackgroundColor background)
            WriteCss(builder, "background", "#" + background.Value);
          else if (token is ParaBackgroundColor backgroundPara)
            WriteCss(builder, "background", "#" + backgroundPara.Value);
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
          else if (token is LeftIndent leftIndent && (leftIndent.Value.Value != 0 || Name == "ul" || Name == "ol"))
            margins[3] = leftIndent.Value;
          else if (token is RightIndent rightIndent && rightIndent.Value.Value != 0)
            margins[1] = rightIndent.Value;
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
          else if (token is FirstLineIndent firstIndent && Name == "p")
            WriteCss(builder, "text-indent", PxString(firstIndent.Value));
          else if (token is SpaceBetweenLines lineSpace && lineSpace.Value > int.MinValue && lineSpace.Value != 0 && (Math.Abs(lineSpace.Value) / 240.0).ToString("0.#") != "1")
            WriteCss(builder, "line-height", (Math.Abs(lineSpace.Value) * DefaultBrowserLineHeight / 240.0).ToString("0.#"));
        }

        if (this.OfType<OutlineText>().Any() && !this.OfType<ForegroundColor>().Any())
          WriteCss(builder, "color", "#fff");

        var cell = CellToken();
        var isList = this.Any(t => t is ParagraphNumbering || t is ListLevelType);
        if (cell != null)
        {
          for (var i = 0; i < 4; i++)
          {
            borders[i] = cell.Borders[i] ?? borders[i];
            if (!isList)
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
          if (!this.OfType<ForegroundColor>().Any() && _all.TryGetValue<ForegroundColor>(out var foreColor))
            WriteCss(builder, "color", "#" + foreColor.Value);
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

        if (Name == "td" && !this.Any(IsListMarker) && TryGetValue<FirstLineIndent>(out var firstLine))
        {
          var indent = firstLine.Value;
          if (padding[3].Value < 0)
            indent += padding[3];
          if (indent.Value > 0)
            WriteCss(builder, "text-indent", PxString(indent));
        }

        for (var i = 0; i < borders.Length; i++)
        {
          if (!padding[i].HasValue && borders[i] != null)
            padding[i] = borders[i].Padding;
          if (padding[i].Value < 0)
            padding[i] = UnitValue.Empty;
        }

        if (padding.Any(p => p.Value > 0) || Name == "td")
          WriteCss(builder, "padding", WriteBoxShort(padding));

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

      internal static string PxString(UnitValue value)
      {
        var px = value.ToPx().ToString("0.#");
        if (px == "0")
          return px;
        return px + "px";
      }

      private string BorderString(BorderToken border)
      {
        var width = GetThickness(border).ToPx();

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
          case BorderStyle.Triple:
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

        if (border.Color != null)
          style += " #" + border.Color;

        return style;
      }

      private UnitValue GetThickness(BorderToken border)
      {
        if (border.Style == BorderStyle.None)
          return UnitValue.Empty;

        var width = border.Width.ToPx();
        if ((width > 0 && width < 0.5) || border.Style == BorderStyle.Hairline)
          width = 1;
        else if (border.Style == BorderStyle.DoubleThick)
          width *= 2;

        if (border.Width.Value > 0)
          return new UnitValue(Math.Round(width), UnitType.Pixel);

        switch (border.Style)
        {
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
          case BorderStyle.Triple:
          case BorderStyle.Outset:
          case BorderStyle.Inset:
          case BorderStyle.Embossed:
          case BorderStyle.Frame:
          case BorderStyle.Engraved:
            return new UnitValue(3, UnitType.Pixel);
          default:
            return new UnitValue(1, UnitType.Pixel);
        }
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
