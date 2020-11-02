using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class Element : Node
  {
    private Node _content;

    internal int Level { get; set; }

    public ElementType Type { get; set; }

    public Element(ElementType type, params Node[] nodes)
    {
      Type = type;
      foreach (var node in nodes)
        Add(node);
    }

    public void Add(Node node)
    {
      if (node == null)
        return;

      node.Parent = this;
      if (_content == null)
      {
        node.Next = node;
      }
      else
      {
        node.Next = _content.Next;
        _content.Next = node;
      }
      _content = node;
    }

    public IEnumerable<Element> Descendants()
    {
      foreach (var child in Elements())
      {
        yield return child;
        foreach (var descendant in child.Descendants())
          yield return descendant;
      }
    }

    public IEnumerable<Element> Elements()
    {
      return Nodes().OfType<Element>();
    }

    public IEnumerable<Node> Nodes()
    {
      var curr = _content;
      if (curr == null)
        yield break;

      do
      {
        curr = curr.Next;
        yield return curr;
      }
      while (curr != _content);
    }

    internal void InsertAfter(Node after, Node node)
    {
      if (after == _content)
      {
        Add(node);
      }
      else
      {
        node.Next = after.Next;
        after.Next = node;
      }
    }

    internal void RemoveNode(Node node)
    {
      node.Parent = null;
      var previous = Nodes().First(n => n.Next == node);
      if (previous == node)
        _content = null;
      else
        previous.Next = node.Next;
    }

    public override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
