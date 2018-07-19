using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class TextToken : IToken
  {
    public string Value { get; set; }

    public override string ToString()
    {
      return Value;
    }
  }
}
