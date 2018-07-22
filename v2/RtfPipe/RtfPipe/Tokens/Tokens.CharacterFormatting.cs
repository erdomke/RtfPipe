namespace RtfPipe.Tokens
{
  public class BackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "chcbpat";
    public override TokenType Type => TokenType.CharacterFormat;

    public BackgroundColor(ColorValue value) : base(value) { }
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
    public override string Name => Value.Value > 0 ? "up" : "dn";
    public override TokenType Type => TokenType.CharacterFormat;

    public OffsetToken(UnitValue value) : base(value) { }
  }

  public class NoSuperSubToken : ControlTag
  {
    public override string Name => "nosupersub";
    public override TokenType Type => TokenType.CharacterFormat;
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

  public class PlainToken : ControlTag
  {
    public override string Name => "plain";
    public override TokenType Type => TokenType.CharacterFormat;
  }
}
