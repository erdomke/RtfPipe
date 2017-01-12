// -- FILE ------------------------------------------------------------------
// name       : RtfVisualSpecialCharKind.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.22
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace RtfPipe
{

  // ------------------------------------------------------------------------
  public enum RtfVisualSpecialCharKind
  {
    Tabulator,
    ParagraphNumberBegin,
    ParagraphNumberEnd,
    NonBreakingSpace,
    EmDash,
    EnDash,
    EmSpace,
    EnSpace,
    QmSpace,
    Bullet,
    LeftSingleQuote,
    RightSingleQuote,
    LeftDoubleQuote,
    RightDoubleQuote,
    OptionalHyphen,
    NonBreakingHyphen,
    ObjectAttachPoint // Used in Outlook RTF
  } // enum RtfVisualSpecialCharKind

} // namespace RtfPipe
// -- EOF -------------------------------------------------------------------
