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
  }
}
