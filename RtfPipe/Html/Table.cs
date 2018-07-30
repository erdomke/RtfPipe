using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  internal class Table : Group
  {
    public static Table Create(IList<IToken> tokens, ref int idx)
    {
      var result = new Table();
      while (idx < tokens.Count && tokens[idx] is RowDefaults)
      {
        result.Contents.Add(Row.Create(tokens, ref idx));
      }

      var boundaries = result.Flatten()
        .OfType<RightCellBoundary>()
        .Select(b => b.Value)
        .Distinct()
        .OrderBy(v => v)
        .Select((v, j) => new CellIndex(v, j))
        .ToList();

      var i = 1;
      var cellIdx = 0;
      while (i < boundaries.Count)
      {
        if ((boundaries[i].Value - boundaries[i - 1].Value).ToPx() > 0.25)
          cellIdx++;
        boundaries[i].Index = cellIdx;
        i++;
      }

      var allBoundaries = boundaries
        .ToDictionary(c => c.Value, c => c.Index);

      foreach (var row in result.Contents.OfType<Row>())
      {
        row.AllBoundaries = allBoundaries;
      }

      return result;
    }

    private class CellIndex
    {
      public UnitValue Value { get; }
      public int Index { get; set; }

      public CellIndex(UnitValue value, int index)
      {
        Value = value;
        Index = index;
      }
    }
  }
}
