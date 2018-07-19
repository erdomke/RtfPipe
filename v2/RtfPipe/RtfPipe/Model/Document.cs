using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class Document
  {
    public ReferenceTable<ColorValue> ColorTable { get; } = new ReferenceTable<ColorValue>();
    public FontTable FontTable { get; } = new FontTable();
  }
}
