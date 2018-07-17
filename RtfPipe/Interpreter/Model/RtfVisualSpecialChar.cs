using RtfPipe.Sys;
using System;

namespace RtfPipe.Model
{
  public sealed class RtfVisualSpecialChar : RtfVisual, IRtfVisualSpecialChar
  {
    public RtfVisualSpecialChar(RtfVisualSpecialCharKind charKind, Style format) :
      base(RtfVisualKind.Special)
    {
      this.CharKind = charKind;
      this._format = format;
    }

    protected override void DoVisit(IRtfVisualVisitor visitor)
    {
      visitor.VisitSpecial(this);
    }

    public RtfVisualSpecialCharKind CharKind { get; private set; }

    public Style Format
    {
      get { return _format; }
      set
      {
        if (_format == null)
        {
          throw new ArgumentNullException("value");
        }
        _format = value;
      }
    }

    protected override bool IsEqual(object obj)
    {
      var compare = obj as RtfVisualSpecialChar; // guaranteed to be non-null
      return compare != null
        && base.IsEqual(compare)
        && CharKind == compare.CharKind;
    }

    protected override int ComputeHashCode()
    {
      return HashTool.AddHashCode(base.ComputeHashCode(), CharKind);
    }

    public override string ToString()
    {
      return CharKind.ToString();
    }

    private Style _format;
  }

}
