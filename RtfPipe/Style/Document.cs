using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class Document : Group
  {
    public List<ColorValue> ColorTable { get; } = new List<ColorValue>();
    public Dictionary<int, Font> FontTable { get; } = new Dictionary<int, Font>();
    public Dictionary<int, ListStyleReference> ListStyles { get; } = new Dictionary<int, ListStyleReference>();
    public Dictionary<IToken, object> Information { get; } = new Dictionary<IToken, object>();
    public bool HasHtml { get; set; }
  }
}
