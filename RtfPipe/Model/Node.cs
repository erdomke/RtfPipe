using System;
using System.Collections.Generic;
using System.IO;

namespace RtfPipe.Model
{
  /// <summary>
  /// A node in the HTML representation of the document
  /// </summary>
  public abstract class Node
  {
    internal Node NextNode { get; set; }
    internal Element Parent { get; set; }

    internal IEnumerable<Element> Parents()
    {
      var parent = Parent;
      while (parent != null)
      {
        yield return parent;
        parent = parent.Parent;
      }
    }

    internal void Remove()
    {
      if (Parent == null)
        throw new InvalidOperationException("This node does not have a parent");
      Parent.RemoveNode(this);
    }

    internal abstract void Visit(INodeVisitor visitor);

    /// <summary>
    /// Render the node as a string
    /// </summary>
    public override string ToString()
    {
      using (var writer = new StringWriter())
      {
        var visitor = new HtmlVisitor(writer);
        Visit(visitor);
        visitor.Flush();
        return writer.ToString();
      }
    }
  }
}
