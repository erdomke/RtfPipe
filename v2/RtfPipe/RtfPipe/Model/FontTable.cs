using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class FontTable : IEnumerable<KeyValuePair<int, Font>>, IReadOnlyDictionary<int, Font>
  {
    private readonly Dictionary<int, Font> _fonts = new Dictionary<int, Font>();

    public Font this[int key]
    {
      get
      {
        if (_fonts.TryGetValue(key, out var font))
          return font;
        return null;
      }
      internal set { _fonts[key] = value; }
    }

    public IEnumerable<int> Keys => _fonts.Keys;

    public IEnumerable<Font> Values => _fonts.Values;

    public int Count => _fonts.Count;

    public bool ContainsKey(int key)
    {
      return _fonts.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<int, Font>> GetEnumerator()
    {
      return _fonts.GetEnumerator();
    }

    public bool TryGetValue(int key, out Font value)
    {
      return _fonts.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
