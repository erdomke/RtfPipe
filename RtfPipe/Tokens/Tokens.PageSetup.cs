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

  public class HeaderLeft : ControlTag
  {
    public override string Name => "headerl";
  }

  public class HeaderRight : ControlTag
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

  public class FooterLeft : ControlTag
  {
    public override string Name => "footerl";
  }

  public class FooterRight : ControlTag
  {
    public override string Name => "footerr";
  }

  public class FooterFirst : ControlTag
  {
    public override string Name => "footerf";
  }
}
