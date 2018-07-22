using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class GeneratorTag : ControlTag
  {
    public override string Name => "generator";
    public override TokenType Type => TokenType.HeaderTag;
  }
}
