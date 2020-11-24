using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RtfPipe.Model
{
  [DebuggerTypeProxy(typeof(ElementDebugView))]
  internal class Element : Node
  {
    private Node _content;
    
    internal int TableLevel => Styles.OfType<NestingLevel>().FirstOrDefault()?.Value 
      ?? (Styles.OfType<InTable>().Any() ? 1 : 0);
    internal int ListLevel => Styles.OfType<ListLevelNumber>().FirstOrDefault()?.Value ?? 0;

    public StyleList Styles { get; } = new StyleList();
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
        node.NextNode = node;
      }
      else
      {
        node.NextNode = _content.NextNode;
        _content.NextNode = node;
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
        curr = curr.NextNode;
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
        node.NextNode = after.NextNode;
        after.NextNode = node;
      }
    }

    internal void RemoveNode(Node node)
    {
      node.Parent = null;
      var previous = Nodes().First(n => n.NextNode == node);
      if (previous == node)
      {
        _content = null;
      }
      else
      {
        previous.NextNode = node.NextNode;
        if (_content == node)
          _content = previous;
      }
    }

    internal void SetStyles(IEnumerable<IToken> styles)
    {
      Styles.Set(styles
        .Where(t => (t.Type & TokenType.Format) > 0)
        .ToList());
      if (Type == ElementType.Paragraph 
        && Styles.OfType<ParagraphNumbering>().Any() 
        && !Styles.OfType<NumberingLevelContinue>().Any())
        Type = ElementType.ListItem;
    }

    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }

    private class ElementDebugView
    {
      private Element _elem;

      public IEnumerable<Element> Elements => _elem.Elements().ToArray();
      internal int ListLevel => _elem.ListLevel;
      public IEnumerable<Node> Nodes => _elem.Nodes().ToArray();
      public StyleList Styles => _elem.Styles;
      internal int TableLevel => _elem.TableLevel;
      public ElementType Type => _elem.Type;

      public ElementDebugView(Element elem)
      {
        _elem = elem;
      }
    }
  }
}
