using System;
#if DRAWING
using System.Drawing;
using System.Drawing.Imaging;
#endif

namespace RtfPipe.Converter.Image
{


  public class RtfConvertedImageInfo : IRtfConvertedImageInfo
  {

#if DRAWING
    public RtfConvertedImageInfo( string fileName, ImageFormat format, Size size )
    {
      if ( fileName == null )
      {
        throw new ArgumentNullException( "fileName" );
      }

      this.fileName = fileName;
      this.format = format;
      this.size = size;
    }
#endif

    public string FileName
    {
      get { return fileName; }
    }

    private readonly string fileName;

#if DRAWING
    public override string ToString()
    {
      return fileName + " " + format + " " + size.Width + "x" + size.Height;
    }

    public ImageFormat Format
    {
      get { return format; }
    }

    public Size Size
    {
      get { return size; }
    }
    private readonly ImageFormat format;
    private readonly Size size;
#endif

  }

}

