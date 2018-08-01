using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  [DebuggerDisplay("Columns: {Count}")]
  internal class ColumnBoundaries : Dictionary<UnitValue, int>, IWord
  {
    public TokenType Type => TokenType.ParagraphFormat;
    public string Name => "ColumnBoundaries";
  }
}
