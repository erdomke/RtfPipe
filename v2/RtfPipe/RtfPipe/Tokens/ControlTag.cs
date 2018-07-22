using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public abstract class ControlTag : IEquatable<ControlTag>, IWord
  {
    public abstract string Name { get; }
    public virtual TokenType Type => TokenType.Word;

    public override bool Equals(object obj)
    {
      if (obj is ControlTag tag)
        return Equals(tag);
      return false;
    }

    public bool Equals(ControlTag other)
    {
      if (other == null)
        return false;
      return this.Name == other.Name;
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode()
        .AddHashCode(Name);
    }

    public override string ToString()
    {
      return "\\" + Name;
    }
  }
}
