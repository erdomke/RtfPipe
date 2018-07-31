using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  internal class Table : Group
  {

    private Table() { }

    public Table(IEnumerable<IToken> contents) : base(contents) { }

    public void Process()
    {
      var boundaries = this.Flatten()
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

      foreach (var row in Contents.OfType<Row>())
      {
        row.AllBoundaries = allBoundaries;
      }
    }

    public static Table Create(IList<IToken> tokens, ref int idx)
    {
      var result = new Table();
      while (idx < tokens.Count && IsTableRow(tokens, idx))
      {
        result.Contents.Add(Row.Create(tokens, ref idx));
      }

      result.Process();
      return result;
    }

    private static bool IsTableRow(IList<IToken> tokens, int idx)
    {
      for (var i = idx; i < Math.Min(tokens.Count, idx + 10); i++)
      {
        if (tokens[i] is RowDefaults || tokens[i] is InTable)
          return true;
        else if (tokens[i] is TextToken)
          return false;
      }
      return false;
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
