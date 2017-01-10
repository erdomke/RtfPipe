// -- FILE ------------------------------------------------------------------
// name       : IRtfConvertedImageInfo.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.10.13
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace RtfPipe.Converter.Image
{

  // ------------------------------------------------------------------------
  public interface IRtfConvertedImageInfo
  {

    // ----------------------------------------------------------------------
    string FileName { get; }

#if DRAWING
    // ----------------------------------------------------------------------
    System.Drawing.Imaging.ImageFormat Format { get; }

    // ----------------------------------------------------------------------
    System.Drawing.Size Size { get; }
#endif

  } // interface IRtfConvertedImageInfo

} // namespace RtfPipe.Converter.Image
// -- EOF -------------------------------------------------------------------
