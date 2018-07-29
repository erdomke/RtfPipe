using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal class FootnoteBreak : ControlTag
  {
    public override string Name => "FootnoteBreak";
    public override TokenType Type => TokenType.BreakTag;
  }
}
