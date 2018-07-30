using System;
using System.Collections.Generic;
using System.Text;
using RtfPipe.Tokens;

namespace RtfPipe
{
  public class ListStyleReference
  {
    public int Id { get; }
    public ListStyle Style { get; }

    internal ListStyleReference(int id, ListStyle style)
    {
      Id = id;
      Style = style;
    }
  }
}
