using RtfPipe.Tokens;
using System;

namespace RtfPipe
{
  internal class BorderToken : IWord, IEquatable<BorderToken>
  {
    public string Name => "border" + Side;
    public TokenType Type => TokenType.ParagraphFormat;

    public ColorValue Color { get; set; }
    public UnitValue Padding { get; set; }
    public bool Shadow { get; set; }
    public BorderPosition Side { get; set; }
    public BorderStyle Style { get; set; }
    public UnitValue Width { get; set; }

    public BorderToken(ControlWord<BorderPosition> side)
    {
      Side = side.Value;
    }

    public bool Add(IToken token)
    {
      if (token is BorderSpacing spacing)
        Padding = spacing.Value;
      else if (token is BorderWidth width)
        Width = width.Value;
      else if (token is BorderStyleTag style)
        Style = style.Value;
      else if (token is BorderColor color)
        Color = color.Value;
      else if (token is BorderShadow)
        Shadow = true;
      else
        return false;
      return true;
    }

    public bool SameBorderStyle(BorderToken other)
    {
      return Width == other.Width
        && Style == other.Style
        && Color == other.Color
        && Shadow == other.Shadow;
    }

    public override bool Equals(object obj)
    {
      if (obj is BorderToken border)
        return Equals(border);
      return false;
    }

    public bool Equals(BorderToken other)
    {
      return Color == other.Color
        && Padding == other.Padding
        && Shadow == other.Shadow
        && Side == other.Side
        && Style == other.Style
        && Width == other.Width;
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode()
        .AddHashCode(Color)
        .AddHashCode(Padding)
        .AddHashCode(Shadow)
        .AddHashCode(Side)
        .AddHashCode(Style)
        .AddHashCode(Width);
    }
  }
}
