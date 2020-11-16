using RtfPipe.Tokens;
using System.Collections.Generic;
using System.Diagnostics;

namespace RtfPipe.Model
{
  [DebuggerDisplay("Cell {Index}, {Width} {WidthUnit}")]
  internal class CellToken : IWord
  {
    private List<IToken> _styles = new List<IToken>();

    public int ColSpan { get; set; } = 1;
    public int Index { get; set; }
    public UnitValue RightBoundary { get; }
    public IEnumerable<IToken> Styles => _styles;
    public int Width { get; }
    public CellWidthUnit WidthUnit { get; } = CellWidthUnit.Null;
    
    public TokenType Type => TokenType.CellFormat;
    public string Name => "CellToken" + Index;

    public CellToken(IEnumerable<IToken> tokens, CellToken previous)
    {
      foreach (var token in tokens)
      {
        if (token is CellWidth width)
        {
          Width = width.Value;
        }
        else if (token is CellWidthType widthType)
        {
          WidthUnit = widthType.Value;
        }
        else if (token is RightCellBoundary rightBoundary)
        {
          RightBoundary = rightBoundary.Value;
        }
        else
        {
          _styles.Add(token);
        }
      }

      if (WidthUnit == CellWidthUnit.Null)
      {
        WidthUnit = CellWidthUnit.Twip;
        if (previous == null)
          Width = RightBoundary.ToTwip();
        else
          Width = (RightBoundary - previous.RightBoundary).ToTwip();
      }
      Index = previous == null ? 0 : previous.Index + 1;
    }
  }
}
