using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class ParagraphBreak : ControlTag
  {
    public override string Name => "par";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class LineBreak : ControlTag
  {
    public override string Name => "line";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class ParagraphDefault : ControlTag
  {
    public override string Name => "pard";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class TextAlign : ControlWord<TextAlignment>
  {
    public override string Name => "q" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.ParagraphFormat;

    public TextAlign(TextAlignment value) : base(value) { }

    public override string ToString() => Name;
  }

  public class FirstLineIndent : ControlWord<UnitValue>
  {
    public override string Name => "fi";
    public override TokenType Type => TokenType.ParagraphFormat;

    public FirstLineIndent(UnitValue value) : base(value) { }
  }

  public class LeftIndent : ControlWord<UnitValue>
  {
    public override string Name => "li";
    public override TokenType Type => TokenType.ParagraphFormat;

    public LeftIndent(UnitValue value) : base(value) { }
  }

  public class SpaceAfter : ControlWord<UnitValue>
  {
    public override string Name => "sa";
    public override TokenType Type => TokenType.ParagraphFormat;

    public SpaceAfter(UnitValue value) : base(value) { }
  }

  public class SpaceBetweenLines : ControlWord<int>
  {
    public override string Name => "sl";
    public override TokenType Type => TokenType.ParagraphFormat;

    public SpaceBetweenLines(int value) : base(value) { }
  }

  public class LineSpacingMultiple : ControlWord<int>
  {
    public override string Name => "slmult";
    public override TokenType Type => TokenType.ParagraphFormat;

    public LineSpacingMultiple(int value) : base(value) { }
  }

  public class OutlineLevel : ControlWord<int>
  {
    public override string Name => "outlinelevel";
    public override TokenType Type => TokenType.ParagraphFormat;

    public OutlineLevel(int value) : base(value) { }
  }
}
