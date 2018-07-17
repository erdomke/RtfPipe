using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Model
{
  public class RtfFontCollection : IList<Font>
  {
    private List<Font> _list = new List<Font>();
    private Dictionary<string, Font> _dict = new Dictionary<string, Font>();

    public Font this[string id]
    {
      get
      {
        Font result;
        if (_dict.TryGetValue(id, out result))
          return result;
        return default(Font);
      }
    }

    public Font this[int index]
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

    public void Add(Font item)
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
    public bool Contains(Font item)
    {
      return _dict.ContainsKey(item.Id);
    }

    public void CopyTo(Font[] array, int arrayIndex)
    {
      _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Font> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public int IndexOf(Font item)
    {
      return _list.IndexOf(item);
    }

    public void Insert(int index, Font item)
    {
      _dict.Add(item.Id, item);
      _list.Insert(index, item);
    }

    public bool Remove(Font item)
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
