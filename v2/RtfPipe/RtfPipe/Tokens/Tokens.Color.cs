namespace RtfPipe.Tokens
{
  public class ColorTable : ControlTag
  {
    public override string Name => "colortbl";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Red : ControlWord<byte>
  {
    public override string Name => "red";
    public override TokenType Type => TokenType.HeaderTag;

    public Red(byte value) : base(value) { }
  }

  public class Green : ControlWord<byte>
  {
    public override string Name => "green";
    public override TokenType Type => TokenType.HeaderTag;

    public Green(byte value) : base(value) { }
  }

  public class Blue : ControlWord<byte>
  {
    public override string Name => "blue";
    public override TokenType Type => TokenType.HeaderTag;

    public Blue(byte value) : base(value) { }
  }
}
