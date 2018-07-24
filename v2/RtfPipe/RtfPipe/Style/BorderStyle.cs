using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public enum BorderStyle
  {
    ///<summary>No border.</summary>
    None,
    ///<summary>Single-thickness border.</summary>
    SingleThick,
    ///<summary>Double-thickness border.</summary>
    DoubleThick,
    ///<summary>Shadowed border.</summary>
    Shadowed,
    ///<summary>Double border.</summary>
    Double,
    ///<summary>Dotted border.</summary>
    Dotted,
    ///<summary>Dashed border.</summary>
    Dashed,
    ///<summary>Hairline border.</summary>
    Hairline,
    ///<summary>Dashed border (small).</summary>
    DashedSmall,
    ///<summary>Dot-dashed border.</summary>
    DotDashed,
    ///<summary>Dot-dot-dashed border.</summary>
    DotDotDashed,
    ///<summary>Inset border.</summary>
    Inset,
    ///<summary>Outset border.</summary>
    Outset,
    ///<summary>Triple border.</summary>
    Triple,
    ///<summary>Thick-thin border (small).</summary>
    ThickThinSmall,
    ///<summary>Thin-thick border (small).</summary>
    ThinThickSmall,
    ///<summary>Thin-thick thin border (small).</summary>
    ThinThickThinSmall,
    ///<summary>Thick-thin border (medium).</summary>
    ThickThinMedium,
    ///<summary>Thin-thick border (medium).</summary>
    ThinThickMedium,
    ///<summary>Thin-thick thin border (medium).</summary>
    ThinThickThinMedium,
    ///<summary>Thick-thin border (large).</summary>
    ThickThinLarge,
    ///<summary>Thin-thick border (large).</summary>
    ThinThickLarge,
    ///<summary>Thin-thick-thin border (large).</summary>
    ThinThickThinLarge,
    ///<summary>Wavy border.</summary>
    Wavy,
    ///<summary>Double wavy border.</summary>
    DoubleWavy,
    ///<summary>Striped border.</summary>
    Striped,
    ///<summary>Embossed border.</summary>
    Embossed,
    ///<summary>Engraved border.</summary>
    Engraved,
    ///<summary>Border resembles a “Frame.”</summary>
    Frame,
  }
}
