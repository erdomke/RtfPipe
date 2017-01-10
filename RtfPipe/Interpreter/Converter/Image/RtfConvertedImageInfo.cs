// -- FILE ------------------------------------------------------------------
// name       : RtfConvertedImageInfo.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.10.13
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
#if DRAWING
using System.Drawing;
using System.Drawing.Imaging;
#endif

namespace RtfPipe.Converter.Image
{

  // ------------------------------------------------------------------------
  public class RtfConvertedImageInfo : IRtfConvertedImageInfo
  {

#if DRAWING
    // ----------------------------------------------------------------------
    public RtfConvertedImageInfo( string fileName, ImageFormat format, Size size )
    {
      if ( fileName == null )
      {
        throw new ArgumentNullException( "fileName" );
      }

      this.fileName = fileName;
      this.format = format;
      this.size = size;
    } // RtfConvertedImageInfo
#endif

    // ----------------------------------------------------------------------
    public string FileName
    {
      get { return fileName; }
    } // FileName

    // ----------------------------------------------------------------------
    // members
    private readonly string fileName;

#if DRAWING
    // ----------------------------------------------------------------------
    public override string ToString()
    {
      return fileName + " " + format + " " + size.Width + "x" + size.Height;
    } // ToString

    // ----------------------------------------------------------------------
    public ImageFormat Format
    {
      get { return format; }
    } // Format

    // ----------------------------------------------------------------------
    public Size Size
    {
      get { return size; }
    } // Size
    private readonly ImageFormat format;
    private readonly Size size;
#endif

  } // class RtfConvertedImageInfo

} // namespace RtfPipe.Converter.Image
// -- EOF -------------------------------------------------------------------
