using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class Tab : ControlTag
  {
    public override string Name => "tab";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class DefaultTabWidth : ControlWord<UnitValue>
  {
    public override string Name => "deftab";
    public override TokenType Type => TokenType.HeaderTag;

    public DefaultTabWidth(UnitValue value) : base(value) { }
  }

  public class TabPosition : ControlWord<UnitValue>
  {
    public override string Name => "tx";
    public override TokenType Type => TokenType.ParagraphFormat;

    public TabPosition(UnitValue value) : base(value) { }
  }

  public class TabAlignment : ControlWord<TextAlignment>
  {
    public override string Name => "tq" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.ParagraphFormat;

    public TabAlignment(TextAlignment value) : base(value) { }
  }
}
