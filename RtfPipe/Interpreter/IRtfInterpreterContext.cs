// -- FILE ------------------------------------------------------------------
// name       : IRtfInterpreterContext.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.20
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{

  // ------------------------------------------------------------------------
  public interface IRtfInterpreterContext
  {

    // ----------------------------------------------------------------------
    RtfInterpreterState State { get; }

    // ----------------------------------------------------------------------
    int RtfVersion { get; }

    // ----------------------------------------------------------------------
    string DefaultFontId { get; }

    // ----------------------------------------------------------------------
    IRtfFont DefaultFont { get; }

    // ----------------------------------------------------------------------
    RtfFontCollection FontTable { get; }

    // ----------------------------------------------------------------------
    IList<IRtfColor> ColorTable { get; }

    // ----------------------------------------------------------------------
    string Generator { get; }

    // ----------------------------------------------------------------------
    IList<IRtfTextFormat> UniqueTextFormats { get; }

    // ----------------------------------------------------------------------
    IRtfTextFormat CurrentTextFormat { get; }

    // ----------------------------------------------------------------------
    IRtfTextFormat GetSafeCurrentTextFormat();

    // ----------------------------------------------------------------------
    IRtfTextFormat GetUniqueTextFormatInstance( IRtfTextFormat templateFormat );

    // ----------------------------------------------------------------------
    IRtfDocumentInfo DocumentInfo { get; }

    // ----------------------------------------------------------------------
    IList<IRtfDocumentProperty> UserProperties { get; }

  } // interface IRtfInterpreterContext

} // namespace RtfPipe
// -- EOF -------------------------------------------------------------------
