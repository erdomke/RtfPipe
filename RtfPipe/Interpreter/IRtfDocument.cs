// -- FILE ------------------------------------------------------------------
// name       : IRtfDocument.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.21
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{

  // ------------------------------------------------------------------------
  public interface IRtfDocument
  {

    // ----------------------------------------------------------------------
    int RtfVersion { get; }

    // ----------------------------------------------------------------------
    IRtfFont DefaultFont { get; }

    // ----------------------------------------------------------------------
    IRtfTextFormat DefaultTextFormat { get; }

    // ----------------------------------------------------------------------
    RtfFontCollection FontTable { get; }

    // ----------------------------------------------------------------------
    IList<IRtfColor> ColorTable { get; }

    // ----------------------------------------------------------------------
    string Generator { get; }

    // ----------------------------------------------------------------------
    IList<IRtfTextFormat> UniqueTextFormats { get; }

    // ----------------------------------------------------------------------
    IRtfDocumentInfo DocumentInfo { get; }

    // ----------------------------------------------------------------------
    IList<IRtfDocumentProperty> UserProperties { get; }

    // ----------------------------------------------------------------------
    IList<IRtfVisual> VisualContent { get; }

  } // interface IRtfDocument

} // namespace RtfPipe
// -- EOF -------------------------------------------------------------------
