using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class ReferenceTable<T> : IReadOnlyList<T>, IEnumerable<T>
  {
    private readonly List<T> _values = new List<T>();

    public T this[int index] => _values[index];

    public int Count => _values.Count;

    internal void Add(T value)
    {
      _values.Add(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
