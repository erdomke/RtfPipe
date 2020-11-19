namespace RtfPipe.Tokens
{
  public class BackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "chcbpat";
    public override TokenType Type => TokenType.CharacterFormat;

    public BackgroundColor(ColorValue value) : base(value) { }
  }

  public class ParagraphBackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "cbpat";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ParagraphBackgroundColor(ColorValue value) : base(value) { }
  }

  public class IsBold : ControlWord<bool>
  {
    public override string Name => "b";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsBold(bool value) : base(value) { }
  }

  public class IsAllCaps : ControlWord<bool>
  {
    public override string Name => "caps";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsAllCaps(bool value) : base(value) { }
  }

  public class ForegroundColor : ControlWord<ColorValue>
  {
    public override string Name => "cf";
    public override TokenType Type => TokenType.CharacterFormat;

    public ForegroundColor(ColorValue value) : base(value) { }
  }

  public class IsEmbossed : ControlWord<bool>
  {
    public override string Name => "embo";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsEmbossed(bool value) : base(value) { }
  }

  public class IsEngraved : ControlWord<bool>
  {
    public override string Name => "impr";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsEngraved(bool value) : base(value) { }
  }

  public class IsItalic : ControlWord<bool>
  {
    public override string Name => "i";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsItalic(bool value) : base(value) { }
  }

  public class IsHidden : ControlWord<bool>
  {
    public override string Name => "v";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsHidden(bool value) : base(value) { }
  }

  public class PositionOffset : ControlWord<UnitValue>
  {
    public override string Name => Value.Value > 0 ? "dn" : "up";
    public override TokenType Type => TokenType.CharacterFormat;

    public PositionOffset(UnitValue value) : base(value) { }
  }

  public class IsOutlined : ControlWord<bool>
  {
    public override string Name => "outl";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsOutlined(bool value) : base(value) { }
  }

  public class SuperSubEnd : ControlTag
  {
    public override string Name => "nosupersub";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class IsShadow : ControlWord<bool>
  {
    public override string Name => "shad";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsShadow(bool value) : base(value) { }
  }

  public class IsSmallCaps : ControlWord<bool>
  {
    public override string Name => "scaps";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsSmallCaps(bool value) : base(value) { }
  }

  public class IsDoubleStrike : ControlWord<bool>
  {
    public override string Name => "striked";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsDoubleStrike(bool value) : base(value) { }
  }

  public class IsStrikethrough : ControlWord<bool>
  {
    public override string Name => "strike";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsStrikethrough(bool value) : base(value) { }
  }

  public class SubscriptStart : ControlTag
  {
    public override string Name => "sub";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class SuperscriptStart : ControlTag
  {
    public override string Name => "super";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class IsUnderline : ControlWord<bool>
  {
    public override string Name => "ul";
    public override TokenType Type => TokenType.CharacterFormat;

    public IsUnderline(bool value) : base(value) { }
  }

  public class UnderlineColor : ControlWord<ColorValue>
  {
    public override string Name => "ulc";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineColor(ColorValue value) : base(value) { }
  }

  public class UnderlineDotted : ControlTag
  {
    public override string Name => "uld";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineDashed : ControlTag
  {
    public override string Name => "uldash";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineDashDot : ControlTag
  {
    public override string Name => "uldashd";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineDashDotDot : ControlTag
  {
    public override string Name => "uldashdd";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineDouble : ControlTag
  {
    public override string Name => "uldb";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineHeavyWave : ControlTag
  {
    public override string Name => "ulhwave";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineLongDash : ControlTag
  {
    public override string Name => "ulldash";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThick : ControlTag
  {
    public override string Name => "ulth";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThickDotted : ControlTag
  {
    public override string Name => "ulthd";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThickDash : ControlTag
  {
    public override string Name => "ulthdash";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThickDashDot : ControlTag
  {
    public override string Name => "ulthdashd";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThickDashDotDot : ControlTag
  {
    public override string Name => "ulthdashdd";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineThickLongDash : ControlTag
  {
    public override string Name => "ulthldash";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineDoubleWave : ControlTag
  {
    public override string Name => "ululdbwave";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineWord : ControlTag
  {
    public override string Name => "ulw";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineWave : ControlTag
  {
    public override string Name => "ulwave";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class PlainStyle : ControlTag
  {
    public override string Name => "plain";
    public override TokenType Type => TokenType.CharacterFormat;
  }
}
