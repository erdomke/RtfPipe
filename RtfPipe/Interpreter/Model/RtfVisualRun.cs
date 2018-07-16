//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RtfPipe.Interpreter.Model
//{
//  public class RtfVisualRun : IRtfVisual
//  {
//    private List<IRtfVisual> _children = new List<IRtfVisual>();

//    public IList<IRtfVisual> Children { get { return _children; } }
//    public RtfVisualKind Kind => RtfVisualKind.Text;

//    public void Visit(IRtfVisualVisitor visitor)
//    {
//      visitor.VisitRun(this);
//    }
//  }
//}
