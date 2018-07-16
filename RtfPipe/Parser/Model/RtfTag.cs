using System;
using System.Globalization;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

  public sealed class RtfTag : RtfElement, IRtfTag
  {

    public RtfTag(string name) :
      base(RtfElementKind.Tag)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }
      fullName = name;
      this.name = name;
      valueAsText = null;
      valueAsNumber = -1;
    }

    public RtfTag(string name, string value) :
      base(RtfElementKind.Tag)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }
      if (value == null)
      {
        throw new ArgumentNullException("value");
      }
      fullName = name + value;
      this.name = name;
      valueAsText = value;
      int numericalValue;
      if (Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out numericalValue))
      {
        valueAsNumber = numericalValue;
      }
      else
      {
        valueAsNumber = -1;
      }
    }

    public string FullName
    {
      get { return fullName; }
    }

    public string Name
    {
      get { return name; }
    }

    public bool HasValue
    {
      get { return valueAsText != null; }
    }

    public string ValueAsText
    {
      get { return valueAsText; }
    }

    public int ValueAsNumber
    {
      get { return valueAsNumber; }
    }

    public override string ToString()
    {
      return "\\" + fullName;
    }

    protected override void DoVisit(IRtfElementVisitor visitor)
    {
      visitor.VisitTag(this);
    }

    protected override bool IsEqual(object obj)
    {
      RtfTag compare = obj as RtfTag; // guaranteed to be non-null
      return compare != null && base.IsEqual(obj) &&
        fullName.Equals(compare.fullName);
    }

    protected override int ComputeHashCode()
    {
      int hash = base.ComputeHashCode();
      hash = HashTool.AddHashCode(hash, fullName);
      return hash;
    }

    private readonly string fullName;
    private readonly string name;
    private readonly string valueAsText;
    private readonly int valueAsNumber;

  }

}
