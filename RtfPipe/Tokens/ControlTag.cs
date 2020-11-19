using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    private static Dictionary<Type, Func<bool, ControlWord<bool>>> _factory = new Dictionary<Type, Func<bool, ControlWord<bool>>>();

    public static ControlWord<bool> Negate(ControlWord<bool> word)
    {
      var type = word.GetType();
      if (!_factory.TryGetValue(type, out var factory))
      {
        var ctor = type.GetConstructor(new[] { typeof(bool) });
        var boolParam = Expression.Parameter(typeof(bool), "value");
        factory = (Func<bool, ControlWord<bool>>)Expression.Lambda(Expression.Convert(Expression.New(ctor, boolParam), typeof(ControlWord<bool>)), boolParam).Compile();
        _factory[type] = factory;
      }
      return factory(!word.Value);
    }
  }
}
