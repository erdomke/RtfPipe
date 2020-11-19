using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal interface INodeVisitor
  {
    void Visit(Anchor anchor);
    void Visit(Element element);
    void Visit(HorizontalRule horizontalRule);
    void Visit(Picture image);
    void Visit(Run run);
  }
}
