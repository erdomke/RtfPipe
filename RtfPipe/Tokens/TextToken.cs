using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class TextToken : IToken
  {
    public string Value { get; set; }
    public TokenType Type => TokenType.Text;

    public override string ToString()
    {
      return Value;
    }
  }
}
