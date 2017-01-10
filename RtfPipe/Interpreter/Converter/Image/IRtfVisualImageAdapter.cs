// -- FILE ------------------------------------------------------------------
// name       : IRtfVisualImageAdapter.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.05
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace RtfPipe.Converter.Image
{

  // ------------------------------------------------------------------------
  public interface IRtfVisualImageAdapter
  {
#if DRAWING
    // ----------------------------------------------------------------------
    System.Drawing.Imaging.ImageFormat TargetFormat { get; }
#endif

    // ----------------------------------------------------------------------
    string ResolveFileName( int index, RtfVisualImageFormat rtfVisualImageFormat );

    // ----------------------------------------------------------------------
    int CalcImageWidth( RtfVisualImageFormat format, int width,
      int desiredWidth, int scaleWidthPercent );

    // ----------------------------------------------------------------------
    int CalcImageHeight( RtfVisualImageFormat format, int height,
      int desiredHeight, int scaleHeightPercent );

  } // interface IRtfVisualImageAdapter

} // namespace RtfPipe.Converter.Image
// -- EOF -------------------------------------------------------------------
