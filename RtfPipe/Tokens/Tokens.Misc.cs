using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class IgnoreUnrecognized : ControlTag
  {
    public override string Name => "*";
  }

  public class GeneratorTag : ControlTag
  {
    public override string Name => "generator";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class ObjectAttachment : ControlTag
  {
    public override string Name => "objattph";
    public override TokenType Type => TokenType.BreakTag;
  }
}
