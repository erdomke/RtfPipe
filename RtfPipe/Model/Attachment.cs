namespace RtfPipe.Model
{
  internal class Attachment : Node
  {
    public int Index { get; }

    public Attachment(int index)
    {
      Index = index;
    }

    internal override void Visit(INodeVisitor visitor)
    {
      visitor.Visit(this);
    }
  }
}
