namespace RtfPipe.Model
{
  internal class Anchor : Node
  {
    public string Id { get; }
    public AnchorType Type { get; }

    public Anchor(AnchorType type, string id)
    {
      Type = type;
      Id = id;
    }

    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
