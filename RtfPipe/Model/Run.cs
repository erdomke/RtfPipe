using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class Run : Node
  {
    public RunFormatting Style { get; set; }
    public string Value { get; set; }

    public override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
