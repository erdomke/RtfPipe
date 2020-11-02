using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public abstract class Node
  {
    internal Node Next { get; set; }
    public Element Parent { get; internal set; }

    public void AddAfterSelf(Node node)
    {
      if (Parent == null)
        throw new InvalidOperationException("This node does not have a parent");
      Parent.InsertAfter(this, node);
    }

    public void Remove()
    {
      if (Parent == null)
        throw new InvalidOperationException("This node does not have a parent");
      Parent.RemoveNode(this);
    }

    public abstract void Visit(INodeVisitor visitor);

    public override string ToString()
    {
      using (var writer = new StringWriter())
      {
        Visit(new HtmlVisitor(writer));
        return writer.ToString();
      }
    }
  }
}
