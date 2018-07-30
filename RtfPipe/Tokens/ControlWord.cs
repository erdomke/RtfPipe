using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RtfPipe
{
  public abstract class ControlWord<T> : IEquatable<ControlWord<T>>, IWord
  {
    public abstract string Name { get; }
    public virtual TokenType Type => TokenType.Word;
    public T Value { get; }

    protected ControlWord(T value)
    {
      Value = value;
    }

    public override bool Equals(object obj)
    {
      if (obj is ControlWord<T> word)
        return Equals(word);
      return false;
    }

    public bool Equals(ControlWord<T> other)
    {
      if (other == null)
        return false;
      return this.Name == other.Name
        && this.Value?.Equals(other.Value) == true;
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode()
        .AddHashCode(Name)
        .AddHashCode(Value);
    }

    public override string ToString()
    {
      if (Value is bool b)
      {
        return "\\" + Name + (b ? "" : "0");
      }
      else if (Value is UnitValue unit)
      {
        return "\\" + Name + (int)unit.Value;
      }
      else if (Value is Enum e)
      {
        return "\\" + Name + (int)((object)e);
      }
      else
      {
        return "\\" + Name + Value;
      }
    }
  }
}
