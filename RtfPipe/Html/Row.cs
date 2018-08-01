using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  internal class Row : Group
  {
    public ColumnBoundaries AllBoundaries
    {
      get { return Contents.OfType<ColumnBoundaries>().FirstOrDefault(); }
      set
      {
        Contents.RemoveWhere(t => t is ColumnBoundaries);
        if (value != null)
          Contents.Insert(0, value);
      }
    }

    private Row() { }
    private Row(IEnumerable<IToken> contents) : base(contents) { }

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
          return Process(CreateNestedRows(result));
        }
        else
        {
          result.Contents.Add(tokens[i]);
        }
      }

      idx = tokens.Count;
      return Process(CreateNestedRows(result));
    }

    private static Row Process(Row row)
    {
      var cellIndex = row.Contents.IndexOf(t => t.Type == TokenType.BreakTag
        || t is TextToken
        || t is Table
        || (t is Group group && group.Contents.Any(c => c.Type == TokenType.BreakTag || c is TextToken)));
      if (cellIndex < 0)
        return row;
      var postStyleIndex = row.Contents.Skip(cellIndex + 1).IndexOf(t => t is Group group && group.Contents.OfType<RowDefaults>().Any());
      if (postStyleIndex < 0)
        return row;

      postStyleIndex += cellIndex + 1;
      var postStyle = (Group)row.Contents[postStyleIndex];
      if (postStyle.Contents.OfType<RowBreak>().Any())
        row.Contents[postStyleIndex] = new RowBreak();
      else if (postStyle.Contents.OfType<NestRow>().Any())
        row.Contents[postStyleIndex] = new NestRow();
      else
        row.Contents.RemoveAt(postStyleIndex);

      row.Contents.InsertRange(cellIndex, postStyle.Contents
        .Where(t => !(t is RowBreak || t is NestRow || t is IgnoreUnrecognized || t is NestTableProperties)));
      row.Contents.RemoveWhere(t => t is Group group && group.Destination is NoNestedTables);
      return row;
    }

    private static Row CreateNestedRows(Row row)
    {
      if (!row.Contents.Any(t => t is NestCell || (t is Group group && group.Destination is NestTableProperties)))
        return row;

      var segments = new List<Segment>() { new Segment() { Start = 0 } };

      for (var i = 0; i < row.Contents.Count; i++)
      {
        var breakToken = IsNonNestedCellBreak(row.Contents[i]) ? row.Contents[i] : null;
        if (row.Contents[i] is Group group && !(group.Destination is NoNestedTables))
          breakToken = group.Contents.LastOrDefault(IsNonNestedCellBreak);

        if (breakToken != null)
        {
          var segment = segments.Last();
          segment.Length = i - segment.Start + 1;
          segment.BreakToken = breakToken;
          segments.Add(new Segment() { Start = i + 1 });
        }
        else if (row.Contents[i] is NestingLevel nestLevel)
        {
          segments.Last().Level = nestLevel.Value;
        }
      }

      if (segments.Last().Start >= row.Contents.Count)
      {
        segments.RemoveAt(segments.Count - 1);
      }
      else
      {
        var segment = segments.Last();
        segment.Length = row.Contents.Count - segment.Start;
      }

      for (var i = 0; i < segments.Count; i++)
      {
        if (segments[i].BreakToken is NestRow)
        {
          var start = i - 1;
          while (start >= 0
            && !(segments[start].BreakToken is NestRow && segments[start].Level == segments[i].Level)
            && segments[start].Level >= segments[i].Level)
          {
            start--;
          }
          start++;

          var newRow = default(Row);
          if (start < i)
          {
            newRow = new Row();
            var segment = new Segment()
            {
              Start = segments[start].Start,
              Level = segments[start].Level,
              BreakToken = segments[i].BreakToken
            };
            for (var j = start; j <= i; j++)
            {
              if (segments[start].Output is Table table)
                table.Process();

              if (segments[start].Output == null)
                newRow.Contents.AddRange(row.Contents.Skip(segments[start].Start).Take(segments[start].Length));
              else
                newRow.Contents.Add(segments[start].Output);
              segment.Length += segments[start].Length;
              segments.RemoveAt(start);
            }
            segments.Insert(start, segment);
            i = start;
            Process(newRow);
          }
          else
          {
            newRow = Process(new Row(row.Contents.Skip(segments[i].Start).Take(segments[i].Length)));
          }

          if (i - 1 >= 0 && segments[i - 1].Level == segments[i].Level && segments[i - 1].Output is Table tbl)
          {
            tbl.Contents.Add(newRow);
            segments[i - 1].Length += segments[i].Length;
            segments.RemoveAt(i);
            i--;
          }
          else
          {
            segments[i].Output = new Table(new[] { newRow });
          }
        }
      }

      var tokens = new List<IToken>();
      foreach (var segment in segments)
      {
        if (segment.Output is Table table)
          table.Process();

        if (segment.Output == null)
          tokens.AddRange(row.Contents.Skip(segment.Start).Take(segment.Length));
        else
          tokens.Add(segment.Output);
      }
      row.Contents.Clear();
      row.Contents.AddRange(tokens);
      return row;
    }

    [DebuggerDisplay("{Start}, \\itap{Level}, {BreakToken}")]
    private class Segment
    {
      public IToken BreakToken { get; set; }
      public int Start { get; set; }
      public int Length { get; set; }
      public int Level { get; set; } = 1;
      public IToken Output { get; set; }
    }

    private static bool IsNonNestedCellBreak(IToken token)
    {
      return token.Type == TokenType.BreakTag && !(token is NestCell) && !(token is LineBreak)
        || token is RightCellBoundary;
    }
  }
}
