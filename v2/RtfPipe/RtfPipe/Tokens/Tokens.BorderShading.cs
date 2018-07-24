using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class BorderStyleTag : ControlWord<BorderStyle>
  {
    public override string Name => "brdr" + GetName();

    public BorderStyleTag(BorderStyle value) : base(value) { }

    private string GetName()
    {
      switch (Value)
      {
        case BorderStyle.SingleThick: return "s";
        case BorderStyle.DoubleThick: return "th";
        case BorderStyle.Shadowed: return "sh";
        case BorderStyle.Double: return "db";
        case BorderStyle.Dotted: return "dot";
        case BorderStyle.Dashed: return "dash";
        case BorderStyle.Hairline: return "hair";
        case BorderStyle.DashedSmall: return "dashsm";
        case BorderStyle.DotDashed: return "dashd";
        case BorderStyle.DotDotDashed: return "dashdd";
        case BorderStyle.Inset: return "inset";
        case BorderStyle.Outset: return "outset";
        case BorderStyle.Triple: return "triple";
        case BorderStyle.ThickThinSmall: return "tnthsg";
        case BorderStyle.ThinThickSmall: return "thtnsg";
        case BorderStyle.ThinThickThinSmall: return "tnthtnsg";
        case BorderStyle.ThickThinMedium: return "tnthmg";
        case BorderStyle.ThinThickMedium: return "thtnmg";
        case BorderStyle.ThinThickThinMedium: return "tnthtnmg";
        case BorderStyle.ThickThinLarge: return "tnthlg";
        case BorderStyle.ThinThickLarge: return "thtnlg";
        case BorderStyle.ThinThickThinLarge: return "tnthtnlg";
        case BorderStyle.Wavy: return "wavy";
        case BorderStyle.DoubleWavy: return "wavydb";
        case BorderStyle.Striped: return "dashdotstr";
        case BorderStyle.Embossed: return "emboss";
        case BorderStyle.Engraved: return "engrave";
        case BorderStyle.Frame: return "frame";
        default: return "none";
      }
    }
  }

  public class BorderWidth : ControlWord<UnitValue>
  {
    public override string Name => "brdrw";

    public BorderWidth(UnitValue value) : base(value) { }
  }

  public class BorderColor : ControlWord<ColorValue>
  {
    public override string Name => "brdrcf";

    public BorderColor(ColorValue value) : base(value) { }
  }

  public class BorderSide : ControlWord<BorderPosition>
  {
    public override string Name => "brdr" + Value.ToString().ToLowerInvariant()[0];

    public BorderSide(BorderPosition value) : base(value) { }
  }
}
