using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  class CssString
  {
    private StringBuilder _builder = new StringBuilder();

    public int Length => _builder.Length;

    public CssString(IEnumerable<IToken> tokens)
    {
      var margins = new UnitValue[4];
      var borders = new BorderToken[4];
      var padding = new UnitValue[4];
      var underline = false;
      const double DefaultBrowserLineHeight = 1.2;
      
      foreach (var token in tokens)
      {
        if (token is Font font)
          Append(font);
        else if (token is FontSize fontSize)
          Append("font-size", fontSize.Value.ToPt().ToString("0.#") + "pt");
        else if (token is BackgroundColor background)
          Append("background", "#" + background.Value);
        else if (token is ParaBackgroundColor backgroundPara)
          Append("background", "#" + backgroundPara.Value);
        else if (token is CapitalToken)
          Append("text-transform", "uppercase");
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
        else if (token is UnderlineToken underlineToken)
          underline = underlineToken.Value;
        else if (token is PageBreak)
          Append("page-break-before", "always");
        else if (token is LeftIndent leftIndent && leftIndent.Value.Value != 0) //  || Name == "ul" || Name == "ol"
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
          Append("text-shadow", "-1px -1px 1px #999, 1px 1px 1px #000");
        else if (token is EngraveText)
          Append("text-shadow", "-1px -1px 1px #000, 1px 1px 1px #999");
        else if (token is OutlineText)
          Append("text-shadow", "-1px -1px 0 #000, 1px -1px 0 #000, -1px 1px 0 #000, 1px 1px 0 #000");
        else if (token is ShadowText)
          Append("text-shadow", "1px 1px 1px #CCC");
        else if (token is SmallCapitalToken)
          Append("font-variant", "small-caps");
        else if (token is UnderlineDashed || token is UnderlineDashDot || token is UnderlineLongDash || token is UnderlineThickDash || token is UnderlineThickDashDot || token is UnderlineThickLongDash)
          Append("text-decoration-style", "dashed");
        else if (token is UnderlineDotted || token is UnderlineDashDotDot || token is UnderlineThickDotted || token is UnderlineThickDashDotDot)
          Append("text-decoration-style", "dotted");
        else if (token is UnderlineWave || token is UnderlineHeavyWave || token is UnderlineDoubleWave)
          Append("text-decoration-style", "wavy");
        else if (token is StrikeDoubleToken || token is UnderlineDouble)
          Append("text-decoration-style", "double");
        else if (token is UnderlineColor underlineColor)
          Append("text-decoration-color", "#" + underlineColor.Value);
        else if (token is UnderlineWord)
          Append("text-decoration-skip", "spaces");
        else if (token is OffsetToken offset)
          Append("top", offset.Value).Append("position", "relative");
        else if (token is FirstLineIndent firstIndent) //Name == "p"
          Append("text-indent", firstIndent.Value);
        else if (token is SpaceBetweenLines lineSpace && lineSpace.Value > int.MinValue && lineSpace.Value != 0 && (Math.Abs(lineSpace.Value) / 240.0).ToString("0.#") != "1")
          Append("line-height", (Math.Abs(lineSpace.Value) * DefaultBrowserLineHeight / 240.0).ToString("0.#"));
      }

      if (tokens.OfType<OutlineText>().Any() && !tokens.OfType<ForegroundColor>().Any())
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
      else if (Name == "a")
      {
        WriteCss(builder, "text-decoration", underline ? "underline" : "none");
        if (!this.OfType<ForegroundColor>().Any() && _all.TryGetValue<ForegroundColor>(out var foreColor))
          WriteCss(builder, "color", "#" + foreColor.Value);
      }*/

      /*
      if (borders.All(b => b != null) && borders.Skip(1).All(b => b.SameBorderStyle(borders[0])))
      {
        Append("border", BorderString(borders[0]));
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
      }*/

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

      /*
      if (padding.Any(p => p.Value > 0) || Name == "td")
        WriteCss(builder, "padding", WriteBoxShort(padding));
        */
    }

    public CssString Append(Font font)
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
      return Append("font-family", name);
    }

    private CssString Append(string property, params UnitValue[] values)
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
