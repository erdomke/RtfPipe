using System;
using System.Diagnostics;

namespace RtfPipe
{
  [DebuggerDisplay("{Value} {Unit}")]
  public struct UnitValue : IEquatable<UnitValue>, IComparable<UnitValue>
  {
    public bool HasValue { get { return Unit != UnitType.Empty; } }
    public double Value { get; set; }
    public UnitType Unit { get; set; }

    public UnitValue(double value, UnitType unit)
    {
      this.Value = value;
      this.Unit = unit;
    }

    public double ToPx()
    {
      switch (Unit)
      {
        // Absolute units
        case UnitType.Centimeter:
          return this.Value * 96.0 / 2.54;
        case UnitType.Inch:
          return this.Value * 96.0;
        case UnitType.Millimeter:
          return this.Value * 96.0 / 25.4;
        case UnitType.Pica:
          return this.Value * 96.0 / 6;
        case UnitType.Pixel:
          return this.Value;
        case UnitType.Point:
          return this.Value * 96.0 / 72;
        case UnitType.Twip:
          return this.Value * 96.0 / 1440;
        case UnitType.Empty:
          return 0;
        default:
          throw new InvalidOperationException();
      }
    }

    public double ToPt()
    {
      return ToPx() * 72.0 / 96;
    }

    public override bool Equals(object obj)
    {
      if (obj is UnitValue unitValue)
        return Equals(unitValue);
      return false;
    }

    public bool Equals(UnitValue other)
    {
      return this.Unit == other.Unit
        && this.Value == other.Value;
    }

    public static bool operator ==(UnitValue a, UnitValue b)
    {
      return a.Equals(b);
    }

    public static bool operator !=(UnitValue a, UnitValue b)
    {
      return !a.Equals(b);
    }

    public static bool operator <(UnitValue a, UnitValue b)
    {
      return a.CompareTo(b) < 0;
    }

    public static bool operator >(UnitValue a, UnitValue b)
    {
      return a.CompareTo(b) > 0;
    }

    public static bool operator <=(UnitValue a, UnitValue b)
    {
      return a.CompareTo(b) <= 0;
    }

    public static bool operator >=(UnitValue a, UnitValue b)
    {
      return a.CompareTo(b) >= 0;
    }

    public static UnitValue operator +(UnitValue a, UnitValue b)
    {
      if (a.Unit == b.Unit)
        return new UnitValue(a.Value + b.Value, a.Unit);
      return new UnitValue(a.ToPx() + b.ToPx(), UnitType.Pixel);
    }

    public static UnitValue operator -(UnitValue a, UnitValue b)
    {
      if (a.Unit == b.Unit)
        return new UnitValue(a.Value - b.Value, a.Unit);
      return new UnitValue(a.ToPx() - b.ToPx(), UnitType.Pixel);
    }

    public static UnitValue operator *(UnitValue a, double b)
    {
      return new UnitValue(a.Value * b, a.Unit);
    }

    public static UnitValue operator /(UnitValue a, double b)
    {
      return new UnitValue(a.Value / b, a.Unit);
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode()
        .AddHashCode(Unit)
        .AddHashCode(Value);
    }

    public int CompareTo(UnitValue other)
    {
      return this.ToPx().CompareTo(other.ToPx());
    }

    public static UnitValue FromHalfPoint(int value)
    {
      return new UnitValue(value * 10, UnitType.Twip);
    }

    public static UnitValue FromQuarterPoint(int value)
    {
      return new UnitValue(value * 5, UnitType.Twip);
    }

    public static UnitValue Empty { get; } = new UnitValue();
  }
}
