using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class SectionBreak : ControlTag
  {
    public override string Name => "sect";
  }

  public class SectionDefault : ControlTag
  {
    public override string Name => "sectd";
    public override TokenType Type => TokenType.SectionFormat;
  }

  public class HtmlTag : ControlWord<HtmlEncapsulation>
  {
    public override string Name => "htmltag";
    public override TokenType Type => TokenType.HtmlFormat;

    public HtmlTag(HtmlEncapsulation value) : base(value) { }
  }

  public class HtmlRtf : ControlWord<bool>
  {
    public override string Name => "htmlrtf";
    public override TokenType Type => TokenType.HtmlFormat;

    public HtmlRtf(bool value) : base(value) { }
  }

  public class FromHtml : ControlWord<bool>
  {
    public override string Name => "fromhtml";
    public override TokenType Type => TokenType.HeaderTag;

    public FromHtml(bool value) : base(value) { }
  }
}
