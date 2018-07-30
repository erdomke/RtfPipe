using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal static class Utils
  {
    public static T SafePeek<T>(this Stack<T> stack)
    {
      if (stack.Count < 1)
        return default;
      return stack.Peek();
    }

    public static IEnumerable<IToken> Flatten(this Group group)
    {
      foreach (var token in group.Contents)
      {
        if (token is Group child)
        {
          foreach (var childToken in child.Flatten())
            yield return childToken;
        }
        else
        {
          yield return token;
        }
      }
    }
  }
}
