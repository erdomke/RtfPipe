using RtfPipe.Sys;
using System;

namespace RtfPipe.Model
{


  public sealed class RtfVisualSpecialChar : RtfVisual, IRtfVisualSpecialChar
  {

    public RtfVisualSpecialChar(RtfVisualSpecialCharKind charKind, IRtfTextFormat format) :
      base(RtfVisualKind.Special)
    {
      this.charKind = charKind;
      this.format = format;
    }

    protected override void DoVisit(IRtfVisualVisitor visitor)
    {
      visitor.VisitSpecial(this);
    }

    public RtfVisualSpecialCharKind CharKind
    {
      get { return charKind; }
    }

    public IRtfTextFormat Format
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
      RtfVisualSpecialChar compare = obj as RtfVisualSpecialChar; // guaranteed to be non-null
      return
        compare != null &&
        base.IsEqual(compare) &&
        charKind == compare.charKind;
    }

    protected override int ComputeHashCode()
    {
      return HashTool.AddHashCode(base.ComputeHashCode(), charKind);
    }

    public override string ToString()
    {
      return charKind.ToString();
    }

    private readonly RtfVisualSpecialCharKind charKind;
    private IRtfTextFormat format;

  }

}
