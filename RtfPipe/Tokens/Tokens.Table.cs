using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class RowDefaults : ControlTag
  {
    public override string Name => "trowd";
    public override TokenType Type => TokenType.RowFormat;
  }

  public class RowBreak : ControlTag
  {
    public override string Name => "row";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class CellDefaults : ControlTag
  {
    public override string Name => "tcelld";
    public override TokenType Type => TokenType.RowFormat;
  }

  public class CellBreak : ControlTag
  {
    public override string Name => "cell";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class HalfCellPadding : ControlWord<UnitValue>
  {
    public override string Name => "trgaph";
    public override TokenType Type => TokenType.RowFormat;

    public HalfCellPadding(UnitValue value) : base(value) { }
  }

  public class BottomCellSpacing : ControlWord<UnitValue>
  {
    public override string Name => "trspdb";
    public override TokenType Type => TokenType.CellFormat;

    public BottomCellSpacing(UnitValue value) : base(value) { }
  }

  public class LeftCellSpacing : ControlWord<UnitValue>
  {
    public override string Name => "trspdl";
    public override TokenType Type => TokenType.CellFormat;

    public LeftCellSpacing(UnitValue value) : base(value) { }
  }

  public class RightCellSpacing : ControlWord<UnitValue>
  {
    public override string Name => "trspdr";
    public override TokenType Type => TokenType.CellFormat;

    public RightCellSpacing(UnitValue value) : base(value) { }
  }

  public class TopCellSpacing : ControlWord<UnitValue>
  {
    public override string Name => "trspdt";
    public override TokenType Type => TokenType.CellFormat;

    public TopCellSpacing(UnitValue value) : base(value) { }
  }

  public class RightCellBoundary : ControlWord<UnitValue>
  {
    public override string Name => "cellx";
    public override TokenType Type => TokenType.CellFormat;

    public RightCellBoundary(UnitValue value) : base(value) { }
  }

  public class RowAutoFit : ControlWord<bool>
  {
    public override string Name => "trautofit";
    public override TokenType Type => TokenType.RowFormat;

    public RowAutoFit(bool value) : base(value) { }
  }

  public class RowLeft : ControlWord<UnitValue>
  {
    public override string Name => "trleft";
    public override TokenType Type => TokenType.RowFormat;

    public RowLeft(UnitValue value) : base(value) { }
  }

  public class RowHeight : ControlWord<UnitValue>
  {
    public override string Name => "trrh";
    public override TokenType Type => TokenType.RowFormat;

    public RowHeight(UnitValue value) : base(value) { }
  }

  public class CellWritingMode : ControlWord<WritingMode>
  {
    public override string Name
    {
      get
      {
        switch (Value)
        {
          case WritingMode.VerticalLr:
            return "cltxbtlr";
          case WritingMode.VerticalRl:
            return "cltxtbrl";
          default:
            return "cltxlrtb";
        }
      }
    }
    public override TokenType Type => TokenType.CellFormat;

    public CellWritingMode(WritingMode value) : base(value) { }

    public override string ToString() => "\\" + Name;
  }

  public class RowTextAlign : ControlWord<TextAlignment>
  {
    public override string Name => "trq" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.RowFormat;

    public RowTextAlign(TextAlignment value) : base(value) { }

    public override string ToString() => "\\" + Name;
  }

  public class TableBorderSide : ControlWord<BorderPosition>
  {
    public override string Name => "trbrdr" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.RowFormat;

    public TableBorderSide(BorderPosition value) : base(value) { }
  }

  public class ParagraphBorderSide : ControlWord<BorderPosition>
  {
    public override string Name => "brdr" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.ParagraphFormat;

    public ParagraphBorderSide(BorderPosition value) : base(value) { }
  }

  public class ParagraphBorderBetween : ControlTag
  {
    public override string Name => "brdrbtw";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class CellBorderSide : ControlWord<BorderPosition>
  {
    public override string Name => "clbrdr" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.CellFormat;

    public CellBorderSide(BorderPosition value) : base(value) { }
  }

  public class TablePaddingTop : ControlWord<UnitValue>
  {
    public override string Name => "trpaddt";
    public override TokenType Type => TokenType.RowFormat;

    public TablePaddingTop(UnitValue value) : base(value) { }
  }

  public class TablePaddingRight : ControlWord<UnitValue>
  {
    public override string Name => "trpaddr";
    public override TokenType Type => TokenType.RowFormat;

    public TablePaddingRight(UnitValue value) : base(value) { }
  }

  public class TablePaddingBottom : ControlWord<UnitValue>
  {
    public override string Name => "trpaddb";
    public override TokenType Type => TokenType.RowFormat;

    public TablePaddingBottom(UnitValue value) : base(value) { }
  }

  public class TablePaddingLeft : ControlWord<UnitValue>
  {
    public override string Name => "trpaddl";
    public override TokenType Type => TokenType.RowFormat;

    public TablePaddingLeft(UnitValue value) : base(value) { }
  }

  public class CellWidthType : ControlWord<CellWidthUnit>
  {
    public override string Name => "clftsWidth";
    public override TokenType Type => TokenType.CellFormat;

    public CellWidthType(CellWidthUnit value) : base(value) { }
  }

  public class CellWidth : ControlWord<int>
  {
    public override string Name => "clwWidth";
    public override TokenType Type => TokenType.CellFormat;

    public CellWidth(int value) : base(value) { }
  }

  public class CellMergePrevious : ControlTag
  {
    public override string Name => "clmrg";
    public override TokenType Type => TokenType.CellFormat;
  }

  public class CellVerticalAlign : ControlWord<VerticalAlignment>
  {
    public override string Name => "clvertal" + Value.ToString().ToLowerInvariant()[0];
    public override TokenType Type => TokenType.CellFormat;

    public CellVerticalAlign(VerticalAlignment value) : base(value) { }

    public override string ToString() => "\\" + Name;
  }

  public class CellPaddingLeft : ControlWord<UnitValue>
  {
    public override string Name => "clpadl";
    public override TokenType Type => TokenType.CellFormat;

    public CellPaddingLeft(UnitValue value) : base(value) { }
  }

  public class CellPaddingTop : ControlWord<UnitValue>
  {
    public override string Name => "clpadt";
    public override TokenType Type => TokenType.CellFormat;

    public CellPaddingTop(UnitValue value) : base(value) { }
  }

  public class CellPaddingBottom : ControlWord<UnitValue>
  {
    public override string Name => "clpadb";
    public override TokenType Type => TokenType.CellFormat;

    public CellPaddingBottom(UnitValue value) : base(value) { }
  }

  public class CellPaddingRight : ControlWord<UnitValue>
  {
    public override string Name => "clpadr";
    public override TokenType Type => TokenType.CellFormat;

    public CellPaddingRight(UnitValue value) : base(value) { }
  }

  public class CellPaddingUnit : ControlWord<int>
  {
    public override string Name => "clpadf" + Side;
    public override TokenType Type => TokenType.CellFormat;
    public char Side { get; }

    public CellPaddingUnit(int value, char side) : base(value)
    {
      Side = side;
    }
  }

  public class InTable : ControlTag
  {
    public override string Name => "intbl";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class CellBackgroundColor : ControlWord<ColorValue>
  {
    public override string Name => "clcbpat";
    public override TokenType Type => TokenType.CellFormat;

    public CellBackgroundColor(ColorValue value) : base(value) { }
  }

  public class HeaderRow : ControlTag
  {
    public override string Name => "trhdr";
    public override TokenType Type => TokenType.RowFormat;
  }

  public class NestedTableProperties : ControlTag
  {
    public override string Name => "nesttableprops";
  }

  public class NoNestedTables : ControlTag
  {
    public override string Name => "nonesttables";
  }

  public class NestedCellBreak : ControlTag
  {
    public override string Name => "nestcell";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class NestedRowBreak : ControlTag
  {
    public override string Name => "nestrow";
    public override TokenType Type => TokenType.BreakTag;
  }

  public class NestingLevel : ControlWord<int>
  {
    public override string Name => "itap";
    public override TokenType Type => TokenType.ParagraphFormat;

    public NestingLevel(int value) : base(value) { }
  }
}
