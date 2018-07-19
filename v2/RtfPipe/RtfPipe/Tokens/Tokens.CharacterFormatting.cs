namespace RtfPipe.Tokens
{
  public class BackgroundColorRef : ControlWord<int>
  {
    public override string Name => "chcbpat";

    public BackgroundColorRef(int value) : base(value) { }
  }

  public class BoldToken : ControlWord<bool>
  {
    public override string Name => "b";

    public BoldToken(bool value) : base(value) { }
  }

  public class CapitalToken : ControlWord<bool>
  {
    public override string Name => "caps";

    public CapitalToken(bool value) : base(value) { }
  }

  public class ForegroundColorRef : ControlWord<int>
  {
    public override string Name => "cf";

    public ForegroundColorRef(int value) : base(value) { }
  }

  public class ItalicToken : ControlWord<bool>
  {
    public override string Name => "i";

    public ItalicToken(bool value) : base(value) { }
  }

  public class HiddenToken : ControlWord<bool>
  {
    public override string Name => "v";

    public HiddenToken(bool value) : base(value) { }
  }

  public class OffsetToken : ControlWord<UnitValue>
  {
    public override string Name => Value.Value > 0 ? "up" : "dn";

    public OffsetToken(UnitValue value) : base(value) { }
  }

  public class NoSuperSubToken : ControlTag
  {
    public override string Name => "nosupersub";
  }

  public class StrikeDoubleToken : ControlWord<bool>
  {
    public override string Name => "striked";

    public StrikeDoubleToken(bool value) : base(value) { }
  }

  public class StrikeToken : ControlWord<bool>
  {
    public override string Name => "strike";

    public StrikeToken(bool value) : base(value) { }
  }

  public class SubStartToken : ControlTag
  {
    public override string Name => "sub";
  }

  public class SuperStartToken : ControlTag
  {
    public override string Name => "super";
  }

  public class UnderlineToken : ControlWord<bool>
  {
    public override string Name => "ul";

    public UnderlineToken(bool value) : base(value) { }
  }
}
