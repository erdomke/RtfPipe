using RtfPipe.Tokens;
using System;
using System.Diagnostics;

namespace RtfPipe
{
  [DebuggerDisplay("{Side} {Width} {Style} {Color}")]
  internal class BorderToken : IWord, IEquatable<BorderToken>
  {
    public string Name => "border" + Side;
    public TokenType Type => TokenType.ParagraphFormat;

    public ColorValue Color { get; private set; }
    public UnitValue Padding { get; private set; }
    public bool Shadow { get; private set; }
    public BorderPosition Side { get; }
    public BorderStyle Style { get; private set; }
    public UnitValue Width { get; private set; }

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
