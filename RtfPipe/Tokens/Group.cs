using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  public class Group : IToken
  {
    public List<IToken> Contents { get; }
    public TokenType Type => TokenType.Group;

    public IWord Destination
    {
      get
      {
        var i = 0;
        while (i < Contents.Count)
        {
          if (i == 0 && Contents[i] is IgnoreUnrecognized)
            i++;
          else
            return Contents[i] as IWord;
        }
        return null;
      }
    }

    public Group()
    {
      Contents = new List<IToken>();
    }

    public Group(IEnumerable<IToken> contents)
    {
      Contents = new List<IToken>(contents);
    }

    public override string ToString()
    {
      if (Contents.Count < 1)
        return "{";
      return "{" + string.Concat(Contents.Select(t => t.ToString())) + "}";
    }
  }
}
