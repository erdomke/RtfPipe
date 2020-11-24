using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class ShapeTag : ControlTag
  {
    public override string Name => "shp";
  }

  public class ShapeInstructions : ControlTag
  {
    public override string Name => "shpinst";
  }

  public class ShapeProperty : ControlTag
  {
    public override string Name => "sp";
  }

  public class ShapePropertyName : ControlTag
  {
    public override string Name => "sn";
  }

  public class ShapePropertyValue : ControlTag
  {
    public override string Name => "sv";
  }
}
