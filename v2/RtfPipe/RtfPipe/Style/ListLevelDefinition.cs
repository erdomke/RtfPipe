using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RtfPipe.Tokens;

namespace RtfPipe
{
  public class ListLevelDefinition : IEnumerable<IToken>
  {
    private readonly List<IToken> _tokens = new List<IToken>();

    internal ListLevelDefinition(Group group)
    {
      foreach (var token in group.Contents)
        Add(token);
    }

    public IEnumerator<IToken> GetEnumerator()
    {
      return _tokens.GetEnumerator();
    }

    internal void Add(IToken token)
    {
      _tokens.Add(token);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
