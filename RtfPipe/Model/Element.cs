using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RtfPipe.Model
{
  internal class Element : Node
  {
    private Node _content;
    private IEnumerable<IToken> _styles;

    internal int TableLevel => Styles.OfType<NestingLevel>().FirstOrDefault()?.Value ?? 0;
    internal int ListLevel => Styles.OfType<ListLevelNumber>().FirstOrDefault()?.Value ?? 0;

    public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public IEnumerable<IToken> Styles => _styles ?? Enumerable.Empty<IToken>();
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
      {
        _content = null;
      }
      else
      {
        previous.Next = node.Next;
        if (_content == node)
          _content = previous;
      }
    }

    internal void SetStyles(IEnumerable<IToken> styles)
    {
      _styles = styles
        .Where(t => (t.Type & TokenType.Format) > 0)
        .ToList();
    }

    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
