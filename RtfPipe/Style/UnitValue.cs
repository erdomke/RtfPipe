using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RtfPipe
{
  /// <summary>
  /// Represents a measurement with units
  /// </summary>
  [DebuggerDisplay("{Value} {Unit}")]
  public struct UnitValue : IEquatable<UnitValue>, IComparable<UnitValue>
  {
    /// <summary>
    /// Gets whether or not the unit has been initialied with a value
    /// </summary>
    public bool HasValue { get { return Unit != UnitType.Empty; } }

    /// <summary>
    /// Gets or sets the value of the measurement
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets the units of the measurement
    /// </summary>
    public UnitType Unit { get; set; }

    /// <summary>
    /// Create a new <see cref="UnitValue"/>
    /// </summary>
    /// <param name="value">The measurement value</param>
    /// <param name="unit">The measurement unit</param>
    public UnitValue(double value, UnitType unit)
    {
      this.Value = value;
      this.Unit = unit;
    }

    /// <summary>
    /// Convert the measurement value to pixels
    /// </summary>
    /// <returns>The measurement in the corresponding number of pixels</returns>
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

    /// <summary>
    /// Convert the measurement to points
    /// </summary>
    /// <returns>The measuremet value in points</returns>
    public double ToPt()
    {
      return ToPx() * 72.0 / 96;
    }

    /// <summary>
    /// Convert the measurement to twips
    /// </summary>
    /// <returns>The measurement in twips</returns>
    public int ToTwip()
    {
      return (int)(ToPx() * 1440.0 / 96);
    }

    /// <summary>
    /// Whether this measurement is exactly equal (in value and unit) to another measurement
    /// </summary>
    /// <param name="obj">Another measurement</param>
    /// <returns><c>true</c> if they are equal. <c>false</c> otherwise</returns>
    public override bool Equals(object obj)
    {
      if (obj is UnitValue unitValue)
        return Equals(unitValue);
      return false;
    }

    /// <summary>
    /// Whether this measurement is exactly equal (in value and unit) to another measurement
    /// </summary>
    /// <param name="other">Another measurement</param>
    /// <returns><c>true</c> if they are equal. <c>false</c> otherwise</returns>
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

    public static UnitValue Average(IEnumerable<UnitValue> values)
    {
      var result = UnitValue.Empty;
      var count = 0;

      foreach (var value in values)
      {
        if (value.HasValue)
        {
          if (!result.HasValue)
            result = value;
          else
            result += value;
          count++;
        }
      }

      return result / count;
    }
  }
}
