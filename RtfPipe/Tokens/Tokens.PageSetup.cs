using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Tokens
{
  public class Header : ControlTag
  {
    public override string Name => "header";
  }

  public class HeaderEven : ControlTag
  {
    public override string Name => "headerl";
  }

  public class HeaderOdd : ControlTag
  {
    public override string Name => "headerr";
  }

  public class HeaderFirst : ControlTag
  {
    public override string Name => "headerf";
  }

  public class Footer : ControlTag
  {
    public override string Name => "footer";
  }

  public class FooterEven : ControlTag
  {
    public override string Name => "footerl";
  }

  public class FooterOdd : ControlTag
  {
    public override string Name => "footerr";
  }

  public class FooterFirst : ControlTag
  {
    public override string Name => "footerf";
  }
}
