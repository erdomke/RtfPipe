using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class ControlCharacter : Node
  {
    public ControlCharacterType Type { get; }

    public ControlCharacter(ControlCharacterType type)
    {
      Type = type;
    }

    public override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
