using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{


  public sealed class RtfHtmlCssStyleCollection : IRtfHtmlCssStyleCollection
  {
    private List<IRtfHtmlCssStyle> _innerList = new List<IRtfHtmlCssStyle>();

    public int Count
    {
      get { return _innerList.Count; }
    }

    public IRtfHtmlCssStyle this[int index]
    {
      get { return _innerList[index] as RtfHtmlCssStyle; }
    }

    public bool Contains(string selectorName)
    {
      foreach (var cssStyle in _innerList)
      {
        if (cssStyle.SelectorName.Equals(selectorName))
        {
          return true;
        }
      }
      return false;
    }

    public void CopyTo(IRtfHtmlCssStyle[] array, int index)
    {
      _innerList.CopyTo(array, index);
    }

    public void Add(IRtfHtmlCssStyle item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("item");
      }
      _innerList.Add(item);
    }

    public void Clear()
    {
      _innerList.Clear();
    }

    public IEnumerator<IRtfHtmlCssStyle> GetEnumerator()
    {
      return _innerList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

}

