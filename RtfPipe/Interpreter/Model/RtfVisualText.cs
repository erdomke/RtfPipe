using System;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

  public sealed class RtfVisualText : RtfVisual, IRtfVisualText
  {

    public RtfVisualText(string text, Style format) :
      base(RtfVisualKind.Text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }
      if (format == null)
      {
        throw new ArgumentNullException("format");
      }
      this.text = text;
      this.format = format;
    }

    protected override void DoVisit(IRtfVisualVisitor visitor)
    {
      visitor.VisitText(this);
    }

    public string Text
    {
      get { return text; }
    }

    public Style Format
    {
      get { return format; }
      set
      {
        if (format == null)
        {
          throw new ArgumentNullException("value");
        }
        format = value;
      }
    }

    protected override bool IsEqual(object obj)
    {
      RtfVisualText compare = obj as RtfVisualText; // guaranteed to be non-null
      return
        compare != null &&
        base.IsEqual(compare) &&
        text.Equals(compare.text) &&
        format.Equals(compare.format);
    }

    protected override int ComputeHashCode()
    {
      int hash = base.ComputeHashCode();
      hash = HashTool.AddHashCode(hash, text);
      hash = HashTool.AddHashCode(hash, format);
      return hash;
    }

    public override string ToString()
    {
      return "'" + text + "'";
    }

    private readonly string text;
    private Style format;

  }

}
