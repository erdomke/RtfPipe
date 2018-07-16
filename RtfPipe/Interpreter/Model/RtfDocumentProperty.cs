using System;
using System.Text;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

  public sealed class RtfDocumentProperty : IRtfDocumentProperty
  {

    public RtfDocumentProperty(int propertyKindCode, string name, string staticValue) :
      this(propertyKindCode, name, staticValue, null)
    {
    }

    public RtfDocumentProperty(int propertyKindCode, string name, string staticValue, string linkValue)
    {
      if (name == null)
        throw new ArgumentNullException(nameof(name));
      if (staticValue == null)
        throw new ArgumentNullException(nameof(staticValue));

      this.PropertyKindCode = propertyKindCode;
      switch (propertyKindCode)
      {
        case RtfSpec.PropertyTypeInteger:
          PropertyKind = RtfPropertyKind.IntegerNumber;
          break;
        case RtfSpec.PropertyTypeRealNumber:
          PropertyKind = RtfPropertyKind.RealNumber;
          break;
        case RtfSpec.PropertyTypeDate:
          PropertyKind = RtfPropertyKind.Date;
          break;
        case RtfSpec.PropertyTypeBoolean:
          PropertyKind = RtfPropertyKind.Boolean;
          break;
        case RtfSpec.PropertyTypeText:
          PropertyKind = RtfPropertyKind.Text;
          break;
        default:
          PropertyKind = RtfPropertyKind.Unknown;
          break;
      }
      this.Name = name;
      this.StaticValue = staticValue;
      this.LinkValue = linkValue;
    }

    public int PropertyKindCode { get; }

    public RtfPropertyKind PropertyKind { get; }

    public string Name { get; }

    public string StaticValue { get; }

    public string LinkValue { get; }

    public override bool Equals(object obj)
    {
      if (obj == this)
      {
        return true;
      }

      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      return IsEqual(obj);
    }

    private bool IsEqual(object obj)
    {
      var compare = obj as RtfDocumentProperty; // guaranteed to be non-null
      return compare != null
        && PropertyKindCode == compare.PropertyKindCode
        && PropertyKind == compare.PropertyKind
        && Name.Equals(compare.Name)
        && CompareTool.AreEqual(StaticValue, compare.StaticValue)
        && CompareTool.AreEqual(LinkValue, compare.LinkValue);
    }

    public override int GetHashCode()
    {
      return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
    }

    private int ComputeHashCode()
    {
      var hash = PropertyKindCode;
      hash = HashTool.AddHashCode(hash, PropertyKind);
      hash = HashTool.AddHashCode(hash, Name);
      hash = HashTool.AddHashCode(hash, StaticValue);
      hash = HashTool.AddHashCode(hash, LinkValue);
      return hash;
    }

    public override string ToString()
    {
      var buf = new StringBuilder(Name);
      if (StaticValue != null)
      {
        buf.Append("=");
        buf.Append(StaticValue);
      }
      if (LinkValue != null)
      {
        buf.Append("@");
        buf.Append(LinkValue);
      }
      return buf.ToString();
    }
  }
}

