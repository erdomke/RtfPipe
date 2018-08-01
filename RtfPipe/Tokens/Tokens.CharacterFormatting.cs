namespace RtfPipe.Tokens
{
  public class BackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "chcbpat";
    public override TokenType Type => TokenType.CharacterFormat;

    public BackgroundColor(ColorValue value) : base(value) { }
  }

  public class ParaBackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "cbpat";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ParaBackgroundColor(ColorValue value) : base(value) { }
  }

  public class BoldToken : ControlWord<bool>
  {
    public override string Name => "b";
    public override TokenType Type => TokenType.CharacterFormat;

    public BoldToken(bool value) : base(value) { }
  }

  public class CapitalToken : ControlWord<bool>
  {
    public override string Name => "caps";
    public override TokenType Type => TokenType.CharacterFormat;

    public CapitalToken(bool value) : base(value) { }
  }

  public class ForegroundColor : ControlWord<ColorValue>
  {
    public override string Name => "cf";
    public override TokenType Type => TokenType.CharacterFormat;

    public ForegroundColor(ColorValue value) : base(value) { }
  }

  public class EmbossText : ControlWord<bool>
  {
    public override string Name => "embo";
    public override TokenType Type => TokenType.CharacterFormat;

    public EmbossText(bool value) : base(value) { }
  }

  public class EngraveText : ControlWord<bool>
  {
    public override string Name => "impr";
    public override TokenType Type => TokenType.CharacterFormat;

    public EngraveText(bool value) : base(value) { }
  }

  public class ItalicToken : ControlWord<bool>
  {
    public override string Name => "i";
    public override TokenType Type => TokenType.CharacterFormat;

    public ItalicToken(bool value) : base(value) { }
  }

  public class HiddenToken : ControlWord<bool>
  {
    public override string Name => "v";
    public override TokenType Type => TokenType.CharacterFormat;

    public HiddenToken(bool value) : base(value) { }
  }

  public class OffsetToken : ControlWord<UnitValue>
  {
    public override string Name => Value.Value > 0 ? "dn" : "up";
    public override TokenType Type => TokenType.CharacterFormat;

    public OffsetToken(UnitValue value) : base(value) { }
  }

  public class OutlineText : ControlWord<bool>
  {
    public override string Name => "outl";
    public override TokenType Type => TokenType.CharacterFormat;

    public OutlineText(bool value) : base(value) { }
  }

  public class NoSuperSubToken : ControlTag
  {
    public override string Name => "nosupersub";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class ShadowText : ControlWord<bool>
  {
    public override string Name => "shad";
    public override TokenType Type => TokenType.CharacterFormat;

    public ShadowText(bool value) : base(value) { }
  }

  public class SmallCapitalToken : ControlWord<bool>
  {
    public override string Name => "scaps";
    public override TokenType Type => TokenType.CharacterFormat;

    public SmallCapitalToken(bool value) : base(value) { }
  }

  public class StrikeDoubleToken : ControlWord<bool>
  {
    public override string Name => "striked";
    public override TokenType Type => TokenType.CharacterFormat;

    public StrikeDoubleToken(bool value) : base(value) { }
  }

  public class StrikeToken : ControlWord<bool>
  {
    public override string Name => "strike";
    public override TokenType Type => TokenType.CharacterFormat;

    public StrikeToken(bool value) : base(value) { }
  }

  public class SubStartToken : ControlTag
  {
    public override string Name => "sub";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class SuperStartToken : ControlTag
  {
    public override string Name => "super";
    public override TokenType Type => TokenType.CharacterFormat;
  }

  public class UnderlineToken : ControlWord<bool>
  {
    public override string Name => "ul";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineToken(bool value) : base(value) { }
  }

  public class UnderlineColor : ControlWord<ColorValue>
  {
    public override string Name => "ulc";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineColor(ColorValue value) : base(value) { }
  }

  public class UnderlineDotted : ControlWord<bool>
  {
    public override string Name => "uld";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDotted(bool value) : base(value) { }
  }

  public class UnderlineDashed : ControlWord<bool>
  {
    public override string Name => "uldash";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDashed(bool value) : base(value) { }
  }

  public class UnderlineDashDot : ControlWord<bool>
  {
    public override string Name => "uldashd";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDashDot(bool value) : base(value) { }
  }

  public class UnderlineDashDotDot : ControlWord<bool>
  {
    public override string Name => "uldashdd";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDashDotDot(bool value) : base(value) { }
  }

  public class UnderlineDouble : ControlWord<bool>
  {
    public override string Name => "uldb";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDouble(bool value) : base(value) { }
  }

  public class UnderlineHeavyWave : ControlWord<bool>
  {
    public override string Name => "ulhwave";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineHeavyWave(bool value) : base(value) { }
  }

  public class UnderlineLongDash : ControlWord<bool>
  {
    public override string Name => "ulldash";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineLongDash(bool value) : base(value) { }
  }

  public class UnderlineThick : ControlWord<bool>
  {
    public override string Name => "ulth";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThick(bool value) : base(value) { }
  }

  public class UnderlineThickDotted : ControlWord<bool>
  {
    public override string Name => "ulthd";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThickDotted(bool value) : base(value) { }
  }

  public class UnderlineThickDash : ControlWord<bool>
  {
    public override string Name => "ulthdash";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThickDash(bool value) : base(value) { }
  }

  public class UnderlineThickDashDot : ControlWord<bool>
  {
    public override string Name => "ulthdashd";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThickDashDot(bool value) : base(value) { }
  }

  public class UnderlineThickDashDotDot : ControlWord<bool>
  {
    public override string Name => "ulthdashdd";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThickDashDotDot(bool value) : base(value) { }
  }

  public class UnderlineThickLongDash : ControlWord<bool>
  {
    public override string Name => "ulthldash";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineThickLongDash(bool value) : base(value) { }
  }

  public class UnderlineDoubleWave : ControlWord<bool>
  {
    public override string Name => "ululdbwave";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineDoubleWave(bool value) : base(value) { }
  }

  public class UnderlineWord : ControlWord<bool>
  {
    public override string Name => "ulw";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineWord(bool value) : base(value) { }
  }

  public class UnderlineWave : ControlWord<bool>
  {
    public override string Name => "ulwave";
    public override TokenType Type => TokenType.CharacterFormat;

    public UnderlineWave(bool value) : base(value) { }
  }

  public class PlainToken : ControlTag
  {
    public override string Name => "plain";
    public override TokenType Type => TokenType.CharacterFormat;
  }
}
