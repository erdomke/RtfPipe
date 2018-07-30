using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  internal class Row : Group
  {
    public Dictionary<UnitValue, int> AllBoundaries { get; set; }

    public static Row Create(IList<IToken> tokens, ref int idx)
    {
      var result = new Row();
      for (var i = idx; i < tokens.Count; i++)
      {
        if (tokens[i] is RowBreak
          || (tokens[i] is Group group && group.Contents.OfType<RowBreak>().Any()))
        {
          result.Contents.Add(tokens[i]);
          idx = i + 1;
          return result;
        }
        else
        {
          result.Contents.Add(tokens[i]);
        }
      }

      return result;
    }
  }
}
