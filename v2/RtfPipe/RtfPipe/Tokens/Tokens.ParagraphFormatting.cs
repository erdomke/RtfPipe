using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class ParagraphBreak : ControlTag
  {
    public override string Name => "par";
  }

  public class LineBreak : ControlTag
  {
    public override string Name => "line";
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
}
