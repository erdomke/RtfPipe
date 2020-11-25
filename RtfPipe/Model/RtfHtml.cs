using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  internal class RtfHtml
  {
    public UnitValue DefaultTabWidth { get; set; }
    public Dictionary<IToken, object> Metadata { get; set; }
    public Element Root { get; set; }
  }
}
