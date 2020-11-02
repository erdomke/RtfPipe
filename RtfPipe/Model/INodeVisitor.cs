using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public interface INodeVisitor
  {
    void Visit(Element element);
    void Visit(Run run);
    void Visit(ControlCharacter controlCharacter);
  }
}
