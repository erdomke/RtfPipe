using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class GroupToken : IToken
  {
    public bool Start { get; set; }

    public override string ToString()
    {
      return Start ? "{" : "}";
    }
  }
}
