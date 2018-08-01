using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal static class Utils
  {
    public static T PeekOrDefault<T>(this Stack<T> stack)
    {
      if (stack.Count < 1)
        return default;
      return stack.Peek();
    }

    public static IEnumerable<IToken> Flatten(this Group group, Func<Group, bool> predicate)
    {
      foreach (var token in group.Contents)
      {
        if (token is Group child)
        {
          if (predicate(child))
          {
            foreach (var childToken in child.Flatten(predicate))
              yield return childToken;
          }
        }
        else
        {
          yield return token;
        }
      }
    }

    public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
      using (var enumerator = items.GetEnumerator())
      {
        var idx = 0;
        while (enumerator.MoveNext())
        {
          if (predicate(enumerator.Current))
            return idx;
          idx++;
        }
      }
      return -1;
    }

    public static void RemoveWhere<T>(this IList<T> items, Func<T, bool> predicate)
    {
      var i = 0;
      while (i < items.Count)
      {
        if (predicate(items[i]))
          items.RemoveAt(i);
        else
          i++;
      }
    }
  }
}
