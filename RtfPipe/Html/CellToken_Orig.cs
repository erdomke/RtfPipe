using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RtfPipe.Tokens;

namespace RtfPipe
{
  [DebuggerDisplay("Cell {Index}, {Width} {WidthUnit}")]
  internal class CellToken_Orig : IWord
  {
    public ColorValue BackgroundColor { get; }
    public BorderToken[] Borders { get; } = new BorderToken[4];
    public UnitValue BoundaryDiff { get; }
    public int ColSpan { get; } = 1;
    public int Index { get; }
    public UnitValue RightBoundary { get; }
    public int Width { get; }
    public CellWidthUnit WidthUnit { get; }
    public VerticalAlignment VerticalAlignment { get; }

    public TokenType Type => TokenType.CellFormat;
    public string Name => "CellToken" + Index;

    public CellToken_Orig(IEnumerable<IToken> tokens, Row row, CellToken_Orig previous)
    {
      Index = (previous?.Index ?? -1) + 1;
      var arr = tokens.ToList();
      for (var i = 0; i < arr.Count; i++)
      {
        if (arr[i] is ControlWord<BorderPosition> side)
        {
          var border = new BorderToken(side);
          i++;
          while (i < arr.Count && border.Add(arr[i]))
            i++;
          i--;
          Borders[(int)border.Side] = border;
        }
        else if (arr[i] is CellVerticalAlign vertAlign)
        {
          VerticalAlignment = vertAlign.Value;
        }
        else if (arr[i] is CellWidth width)
        {
          Width = width.Value;
        }
        else if (arr[i] is CellWidthType widthType)
        {
          WidthUnit = widthType.Value;
        }
        else if (arr[i] is RightCellBoundary rightBoundary)
        {
          RightBoundary = rightBoundary.Value;
        }
        else if (arr[i] is CellBackgroundColor backgroundColor)
        {
          BackgroundColor = backgroundColor.Value;
        }
      }

      BoundaryDiff = RightBoundary - (previous?.RightBoundary ?? new UnitValue(0, UnitType.Twip));
      if (row != null && RightBoundary.Value > 0)
      {
        var idx = row.AllBoundaries[RightBoundary];
        var prevIdx = -1;
        if (previous != null)
          prevIdx = row.AllBoundaries[previous.RightBoundary];
        ColSpan = idx - prevIdx;
      }
    }
  }
}
