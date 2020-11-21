using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal class CssString
  {
    private StringBuilder _builder = new StringBuilder();

    public int Length => _builder.Length;

    public CssString() { }

    public CssString(IEnumerable<IToken> tokens, ElementType elementType, IEnumerable<IToken> allStyles)
    {
      var margins = new UnitValue[4];
      var borders = new BorderToken[4];
      var padding = new UnitValue[4];
      var cellSpacing = new UnitValue[4];

      const double DefaultBrowserLineHeight = 1.2;
      var allTokens = tokens.ToList();
      if (allTokens.OfType<CellPaddingUnit>().Any(u => u.Value == 3))
        allTokens.RemoveWhere(t => t is HalfCellPadding);
      else
        allTokens.RemoveWhere(t => t is CellPaddingBottom || t is CellPaddingLeft || t is CellPaddingRight || t is CellPaddingTop);

      foreach (var token in allTokens)
      {
        if (token is Font font)
          Append(font);
        else if (token is FontSize fontSize)
          Append("font-size", fontSize.Value.ToPt().ToString("0.#") + "pt");
        else if (token is BackgroundColor background)
          Append("background", "#" + background.Value);
        else if (token is ParagraphBackgroundColor backgroundPara)
          Append("background", "#" + backgroundPara.Value);
        else if (token is CellBackgroundColor backgroundCell)
          Append("background", "#" + backgroundCell.Value);
        else if (token is IsAllCaps capitalToken)
          Append("text-transform", capitalToken.Value ? "uppercase" : "none");
        else if (token is ForegroundColor color)
          Append("color", "#" + color.Value);
        else if (token is TextAlign align)
          Append("text-align", align.Value.ToString().ToLowerInvariant());
        else if (token is SpaceAfter spaceAfter)
          margins[2] = spaceAfter.Value;
        else if (token is SpaceBefore spaceBefore)
          margins[0] = spaceBefore.Value;
        else if (token is RowLeft rowLeft)
          margins[3] = rowLeft.Value;
        else if (token is IsUnderline underlineToken)
          Append("text-decoration", underlineToken.Value ? "underline" : "none");
        else if (token is IsBold boldToken)
          Append("font-weight", boldToken.Value ? "bold" : "normal");
        else if (token is PageBreak)
          Append("page-break-before", "always");
        else if (token is LeftIndent leftIndent && leftIndent.Value.Value != 0) //  || Name == "ul" || Name == "ol"
          margins[3] = leftIndent.Value;
        else if (token is NumberingIndent numIndent && numIndent.Value.Value != 0 && (elementType == ElementType.ListItem || elementType == ElementType.List || elementType == ElementType.OrderedList))
          margins[3] = numIndent.Value;
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
        else if (token is IsEmbossed)
          Append("text-shadow", "-1px -1px 1px #999, 1px 1px 1px #000");
        else if (token is IsEngraved)
          Append("text-shadow", "-1px -1px 1px #000, 1px 1px 1px #999");
        else if (token is IsOutlined)
          Append("text-shadow", "-1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000");
        else if (token is IsShadow)
          Append("text-shadow", "1px 1px 1px #CCC");
        else if (token is IsSmallCaps)
          Append("font-variant", "small-caps");
        else if (token is UnderlineDashed || token is UnderlineDashDot || token is UnderlineLongDash || token is UnderlineThickDash || token is UnderlineThickDashDot || token is UnderlineThickLongDash)
          Append("text-decoration-style", "dashed");
        else if (token is UnderlineDotted || token is UnderlineDashDotDot || token is UnderlineThickDotted || token is UnderlineThickDashDotDot)
          Append("text-decoration-style", "dotted");
        else if (token is UnderlineWave || token is UnderlineHeavyWave || token is UnderlineDoubleWave)
          Append("text-decoration-style", "wavy");
        else if (token is IsDoubleStrike || token is UnderlineDouble)
          Append("text-decoration-style", "double");
        else if (token is UnderlineColor underlineColor)
          Append("text-decoration-color", "#" + underlineColor.Value);
        else if (token is UnderlineWord)
          Append("text-decoration-skip", "spaces");
        else if (token is PositionOffset offset)
          Append("top", offset.Value).Append("position", "relative");
        else if (token is FirstLineIndent firstIndent && elementType != ElementType.ListItem && elementType != ElementType.List && elementType != ElementType.OrderedList && elementType != ElementType.TableCell && elementType != ElementType.TableHeaderCell)
          Append("text-indent", firstIndent.Value);
        else if (token is SpaceBetweenLines lineSpace && lineSpace.Value > int.MinValue && lineSpace.Value != 0)
        {
          if (tokens.OfType<LineSpacingMultiple>().Any(m => m.Value == 1))
          {
            if ((Math.Abs(lineSpace.Value) / 240.0).ToString("0.#") != "1")
              Append("line-height", (Math.Abs(lineSpace.Value) * DefaultBrowserLineHeight / 240.0).ToString("0.#"));
          }
          else if (lineSpace.Value < 0)
          {
            Append("line-height", new UnitValue(lineSpace.Value * -1, UnitType.Twip));
          }
          else
          {
            var fSize = allStyles.OfType<FontSize>().FirstOrDefault()?.Value ?? UnitValue.Empty;
            if (fSize.HasValue && lineSpace.Value > fSize.ToTwip() * DefaultBrowserLineHeight)
              Append("line-height", new UnitValue(lineSpace.Value, UnitType.Twip));
          }
        }
        else if (token is CellVerticalAlign cellVerticalAlign)
          Append("vertical-align", cellVerticalAlign.Value == VerticalAlignment.Center ? "middle" : cellVerticalAlign.Value.ToString().ToLowerInvariant());
        else if (token is TopCellSpacing topCellSpacing)
          cellSpacing[0] = topCellSpacing.Value;
        else if (token is RightCellSpacing rightCellSpacing)
          cellSpacing[1] = rightCellSpacing.Value;
        else if (token is BottomCellSpacing bottomCellSpacing)
          cellSpacing[2] = bottomCellSpacing.Value;
        else if (token is LeftCellSpacing leftCellSpacing)
          cellSpacing[3] = leftCellSpacing.Value;
        else if (token is HalfCellPadding halfCellPadding)
          padding[1] = padding[3] = halfCellPadding.Value;
        else if (token is CellPaddingTop cellPaddingTop)
          padding[0] = cellPaddingTop.Value;
        else if (token is CellPaddingRight cellPaddingRight)
          padding[1] = cellPaddingRight.Value;
        else if (token is CellPaddingBottom cellPaddingBottom)
          padding[2] = cellPaddingBottom.Value;
        else if (token is CellPaddingLeft cellPaddingLeft)
          padding[3] = cellPaddingLeft.Value;
        else if (token is RowHeight rowHeight)
        {
          if (rowHeight.Value.Value < 0)
            Append("height", new UnitValue(rowHeight.Value.Value * -1, rowHeight.Value.Unit));
          else
            Append("height", rowHeight.Value); // height works like min-height in tables
        }
        else if (token is CellWritingMode cellWritingMode && cellWritingMode.Value != WritingMode.HorizontalTb)
          Append("writing-mode", cellWritingMode.Value == WritingMode.VerticalLr ? "vertical-lr" : "vertical-rl");
      }

      if (cellSpacing.Any(v => v.HasValue) && elementType == ElementType.Table)
        Append("border-spacing", UnitValue.Average(new[] { cellSpacing[0] + cellSpacing[2], cellSpacing[1] + cellSpacing[3] }));
      
      if (tokens.OfType<IsOutlined>().Any() && !tokens.OfType<ForegroundColor>().Any())
        Append("color", "#fff");

      /*
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
      */

      if (elementType == ElementType.Table)
      {
        if (!tokens.OfType<FontSize>().Any())
          Append("font-size", "inherit");
        Append("box-sizing", "border-box");
      }
      else if (elementType == ElementType.TableCell || elementType == ElementType.TableHeaderCell)
      {
        for (var i = 0; i < 4; i++)
        {
          padding[i] += margins[i];
          margins[i] = UnitValue.Empty;
        }

        var indent = tokens.OfType<FirstLineIndent>().FirstOrDefault()?.Value ?? default;
        if (indent.ToPx() > 0)
        {
          if (padding[3].ToPx() < 0)
            indent += padding[3];
          Append("text-indent", indent);
        }
      }
      else if (elementType == ElementType.List || elementType == ElementType.OrderedList)
      {
        if (!tokens.OfType<LeftIndent>().Any() && !tokens.OfType<NumberingIndent>().Any())
          margins[3] = new UnitValue(720, UnitType.Twip);

        Append("padding-left", "0");
        // Try to approximate the correct list layout (without having to use complex ::marker CSS)
        // based on the first line indent, the left indent (margin) and the first tab
        // The first line indent is relative to the left indent.
        var firstLineIndent = tokens.OfType<FirstLineIndent>().FirstOrDefault()?.Value ?? new UnitValue(0, UnitType.Twip);
        var firstTab = tokens.OfType<TabPosition>().FirstOrDefault();
        if (firstLineIndent > new UnitValue(-0.125, UnitType.Inch))
        {
          Append("list-style-position", "inside");
          if (firstLineIndent.ToPx() != 0)
            Append("text-indent", firstLineIndent);
        }
        else if (firstTab?.Value > margins[3])
        {
          Append("text-indent", firstTab.Value - margins[3]);
        }

        if (elementType == ElementType.OrderedList)
        {
          var numType = tokens.OfType<ListLevelType>().FirstOrDefault()?.Value
              ?? tokens.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
              ?? NumberingType.Numbers;
          switch (numType)
          {
            case NumberingType.NoNumber:
              Append("list-style-type", "none");
              break;
            case NumberingType.ArabicAbjad:
            case NumberingType.ArabicAlif:
              Append("list-style-type", "arabic-indic");
              break;
            case NumberingType.Chinese1:
            case NumberingType.Chinese2:
            case NumberingType.Chinese3:
            case NumberingType.Chinese4:
            case NumberingType.ChineseDoubleByte1:
            case NumberingType.ChineseDoubleByte2:
            case NumberingType.ChineseDoubleByte3:
            case NumberingType.ChineseDoubleByte4:
            case NumberingType.TaiwaneseDoubleByte1:
            case NumberingType.TaiwaneseDoubleByte2:
            case NumberingType.TaiwaneseDoubleByte3:
            case NumberingType.TaiwaneseDoubleByte4:
              Append("list-style-type", "cjk-ideographic");
              break;
            case NumberingType.Hebrew:
            case NumberingType.HebrewBiblical:
              Append("list-style-type", "hebrew");
              break;
            case NumberingType.HindiNumbers:
              Append("list-style-type", "devanagari");
              break;
            case NumberingType.Katana1:
            case NumberingType.DoubleByteKatana1:
              Append("list-style-type", "katakana");
              break;
            case NumberingType.Katana2:
            case NumberingType.DoubleByteKatana2:
              Append("list-style-type", "katakana-iroha");
              break;
            case NumberingType.Kanji:
            case NumberingType.Kanji3:
            case NumberingType.Kanji4:
            case NumberingType.KanjiDigit:
              Append("list-style-type", "japanese-formal");
              break;
            case NumberingType.Korean1:
            case NumberingType.Korean2:
            case NumberingType.KoreanDoubleByte1:
            case NumberingType.KoreanDoubleByte2:
            case NumberingType.KoreanDoubleByte3:
            case NumberingType.KoreanDoubleByte4:
              Append("list-style-type", "korean-hanja-formal");
              break;
            case NumberingType.LeadingZeroArabic:
              Append("list-style-type", "decimal-leading-zero");
              break;
            case NumberingType.LowerGreekNumerals:
              Append("list-style-type", "lower-greek");
              break;
            case NumberingType.ThaiNumbers:
              Append("list-style-type", "thai");
              break;
            case NumberingType.UpperGreekNumerals:
              Append("list-style-type", "upper-greek");
              break;
            case NumberingType.Zodiac1:
            case NumberingType.Zodiac2:
            case NumberingType.Zodiac3:
              Append("list-style-type", "cjk-heavenly-stem");
              break;
          }
        }
      }

      if (borders.All(b => b != null) && borders.Skip(1).All(b => b.SameBorderStyle(borders[0])))
      {
        Append("border", borders[0]);
      }
      else
      {
        if (borders[0] != null)
          Append("border-top", borders[0]);
        if (borders[1] != null)
          Append("border-right", borders[1]);
        if (borders[2] != null)
          Append("border-bottom", borders[2]);
        if (borders[3] != null)
          Append("border-left", borders[3]);
      }

      /*
      if (Name == "td" && !this.Any(IsListMarker) && TryGetValue<FirstLineIndent>(out var firstLine))
      {
        var indent = firstLine.Value;
        if (padding[3].Value < 0)
          indent += padding[3];
        if (indent.Value > 0)
          WriteCss(builder, "text-indent", PxString(indent));
      }*/

      if (margins.Any(v => v.HasValue))
        Append("margin", margins);
      
      for (var i = 0; i < borders.Length; i++)
      {
        if (!padding[i].HasValue && borders[i] != null)
          padding[i] = borders[i].Padding;
        if (padding[i].Value < 0)
          padding[i] = UnitValue.Empty;
      }

      if (padding.Any(p => p.Value > 0) || elementType == ElementType.TableCell || elementType == ElementType.TableHeaderCell)
        Append("padding", padding);
    }

    public CssString Append(Font font)
    {
      if (string.IsNullOrEmpty(font?.Name))
        return this;

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
      return Append("font-family", name);
    }

    public CssString Append(string property, BorderToken border)
    {
      _builder.Append(property).Append(":");
      var width = GetThickness(border).ToPx();

      if (width <= 0 || border.Style == BorderStyle.None)
      {
        _builder.Append("none");
      }
      else
      {
        _builder.Append(width.ToString("0")).Append("px ");
        
        switch (border.Style)
        {
          case BorderStyle.Dashed:
          case BorderStyle.DashedSmall:
          case BorderStyle.DotDashed:
            _builder.Append("dashed");
            break;
          case BorderStyle.Dotted:
          case BorderStyle.DotDotDashed:
            _builder.Append("dotted");
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
            _builder.Append("double");
            break;
          case BorderStyle.Outset:
            _builder.Append("outset");
            break;
          case BorderStyle.Inset:
            _builder.Append("inset");
            break;
          case BorderStyle.Embossed:
          case BorderStyle.Frame:
            _builder.Append("ridge");
            break;
          case BorderStyle.Engraved:
            _builder.Append("groove");
            break;
          default:
            _builder.Append("solid");
            break;
        }

        if (border.Color != null)
          _builder.Append(" #").Append(border.Color);
      }

      _builder.Append(";");
      return this;
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

    public CssString Append(string property, params UnitValue[] values)
    {
      if (values.Length < 1)
        return this;

      _builder.Append(property).Append(":");
      if (values.Length > 1 && values.Select(v => v.ToPx()).Distinct().Count() == 1)
        values = new UnitValue[] { values[0] };
      else if (values.Length == 4 && values[0] == values[2] && values[1] == values[3])
        values = new UnitValue[] { values[0], values[1] };

      for (var i = 0; i < values.Length; i++)
      {
        if (i > 0)
          _builder.Append(' ');
        var px = values[i].ToPx().ToString("0.#");
        _builder.Append(px);
        if (px != "0")
          _builder.Append("px");
      }
      _builder.Append(";");
      return this;
    }
    
    public CssString Append(string property, string value)
    {
      _builder.Append(property)
        .Append(":")
        .Append(value)
        .Append(";");
      return this;
    }

    public override string ToString()
    {
      return _builder.ToString();
    }
  }
}
