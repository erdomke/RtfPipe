using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class GenericTag : ControlTag
  {
    public override string Name { get; }

    public GenericTag(string name)
    {
      Name = name;
    }
  }
}
