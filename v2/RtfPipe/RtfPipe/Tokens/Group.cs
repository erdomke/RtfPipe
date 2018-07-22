using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  public class Group : IToken
  {
    public List<IToken> Contents { get; } = new List<IToken>();
    public TokenType Type => TokenType.Group;

    public override string ToString()
    {
      if (Contents.Count < 1)
        return "{";
      return "{" + string.Join("", Contents.Select(t => t.ToString())) + "}";
    }
  }
}
