using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal interface INodeVisitor
  {
    void Visit(Attachment attachment);
    void Visit(Element element);
    void Visit(Run run);
    void Visit(Picture image);
  }
}
