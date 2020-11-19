using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal class HorizontalRule : Node
  {
    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
