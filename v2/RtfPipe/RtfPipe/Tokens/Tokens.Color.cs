namespace RtfPipe.Tokens
{
  public class ColorTable : ControlTag
  {
    public override string Name => "colortbl";
  }

  public class Red : ControlWord<byte>
  {
    public override string Name => "red";

    public Red(byte value) : base(value) { }
  }

  public class Green : ControlWord<byte>
  {
    public override string Name => "green";

    public Green(byte value) : base(value) { }
  }

  public class Blue : ControlWord<byte>
  {
    public override string Name => "blue";

    public Blue(byte value) : base(value) { }
  }
}
