using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Model
{
  public class RtfFontCollection : IList<IRtfFont>
  {
    private List<IRtfFont> _list = new List<IRtfFont>();
    private Dictionary<string, IRtfFont> _dict = new Dictionary<string, IRtfFont>();

    public IRtfFont this[string id]
    {
      get
      {
        IRtfFont result;
        if (_dict.TryGetValue(id, out result))
          return result;
        return default(IRtfFont);
      }
    }

    public IRtfFont this[int index]
    {
      get { return _list[index]; }
      set
      {
        _dict.Remove(_list[index].Id);
        _dict.Add(value.Id, value);
        _list[index] = value;
      }
    }

    public int Count { get { return _list.Count; } }
    public bool IsReadOnly { get { return false; } }

    public void Add(IRtfFont item)
    {
      _dict.Add(item.Id, item);
      _list.Add(item);
    }

    public void Clear()
    {
      _dict.Clear();
      _list.Clear();
    }

    public bool ContainsKey(string id)
    {
      return _dict.ContainsKey(id);
    }
    public bool Contains(IRtfFont item)
    {
      return _dict.ContainsKey(item.Id);
    }

    public void CopyTo(IRtfFont[] array, int arrayIndex)
    {
      _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<IRtfFont> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public int IndexOf(IRtfFont item)
    {
      return _list.IndexOf(item);
    }

    public void Insert(int index, IRtfFont item)
    {
      _dict.Add(item.Id, item);
      _list.Insert(index, item);
    }

    public bool Remove(IRtfFont item)
    {
      return _dict.Remove(item.Id) && _list.Remove(item);
    }

    public void RemoveAt(int index)
    {
      _dict.Remove(_list[index].Id);
      _list.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
