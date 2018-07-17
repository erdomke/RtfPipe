using System;
using System.Collections.Generic;
using System.Text;
using RtfPipe.Sys;

namespace RtfPipe
{
  public class Style : IEquatable<Style>
  {
    public TextAlignment Alignment { get; set; }
    public IBackground Background { get; set; }
    public ColorValue Color { get; set; }
    public Font Font { get; set; }
    public UnitValue FontSize { get; set; }
    public bool IsBold { get; set; }
    public bool IsHidden { get; set; }
    public bool IsItalic { get; set; }
    public bool IsSubscript { get; set; }
    public bool IsSuperscript { get; set; }
    public Strikethrough Strikethrough { get; set; }
    public TextAlignment TextAlignment { get; set; }
    public Underline Underline { get; set; }
    public UnitValue Offset { get; set; }

    private Style() { }

    public Style(Font font, UnitValue fontSize)
    {
      Font = font;
      FontSize = fontSize;
    }

    public Style Clone(Action<Style> updates)
    {
      var result = new Style()
      {
        Alignment = Alignment,
        Background = Background?.Clone(),
        Color = Color?.Clone(),
        Font = Font,
        FontSize = FontSize,
        IsBold = IsBold,
        IsHidden = IsHidden,
        IsItalic = IsItalic,
        IsSubscript = IsSubscript,
        IsSuperscript = IsSuperscript,
        Strikethrough = Strikethrough,
        TextAlignment = TextAlignment,
        Underline = Underline,
        Offset = Offset
      };
      updates?.Invoke(result);
      return result;
    }

    public Style DeriveNormal()
    {
      return new Style()
      {
        Alignment = Alignment,
        Font = Font,
        FontSize = RtfSpec.DefaultFontSize
      };
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode()
        .AddHashCode(Alignment)
        .AddHashCode(Background)
        .AddHashCode(Color)
        .AddHashCode(Font)
        .AddHashCode(FontSize)
        .AddHashCode(IsBold)
        .AddHashCode(IsHidden)
        .AddHashCode(IsItalic)
        .AddHashCode(IsSubscript)
        .AddHashCode(IsSuperscript)
        .AddHashCode(Strikethrough)
        .AddHashCode(TextAlignment)
        .AddHashCode(Underline)
        .AddHashCode(Offset);
    }

    public override bool Equals(object obj)
    {
      if (obj is Style style)
        return Equals(style);
      return false;
    }

    public bool Equals(Style other)
    {
      if (other == null)
        return false;
      return Alignment == other.Alignment
        && Background == other.Background
        && Color == other.Color
        && Font == other.Font
        && FontSize == other.FontSize
        && IsBold == other.IsBold
        && IsHidden == other.IsHidden
        && IsItalic == other.IsItalic
        && IsSubscript == other.IsSubscript
        && IsSuperscript == other.IsSuperscript
        && Strikethrough == other.Strikethrough
        && TextAlignment == other.TextAlignment
        && Underline == other.Underline
        && Offset == other.Offset;
    }
  }
}
