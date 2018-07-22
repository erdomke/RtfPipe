using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class StyleSheetTag : ControlTag
  {
    public override string Name => "stylesheet";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class StyleRef : ControlWord<int>
  {
    public override string Name => "s";
    public override TokenType Type => TokenType.ParagraphFormat;

    public StyleRef(int value) : base(value) { }
  }
}
