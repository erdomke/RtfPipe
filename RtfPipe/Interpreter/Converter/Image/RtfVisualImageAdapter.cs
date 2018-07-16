using System;
using System.Globalization;
#if DRAWING
using System.Drawing.Imaging;
#endif

namespace RtfPipe.Converter.Image
{
	public class RtfVisualImageAdapter : IRtfVisualImageAdapter
	{

		public const double DefaultDpi = 96.0;

		public RtfVisualImageAdapter() :
			this( defaultFileNamePattern )
		{
		}

		public RtfVisualImageAdapter( string fileNamePattern )
		{
      if (fileNamePattern == null)
      {
        throw new ArgumentNullException("fileNamePattern");
      }

      this.fileNamePattern = fileNamePattern;
      this.dpiX = DefaultDpi;
      this.dpiY = DefaultDpi;
    }

#if DRAWING
    public RtfVisualImageAdapter( ImageFormat targetFormat ) :
			this( defaultFileNamePattern, targetFormat )
		{
		}

		public RtfVisualImageAdapter( string fileNamePattern, ImageFormat targetFormat ) :
			this( fileNamePattern, targetFormat, DefaultDpi, DefaultDpi )
		{
		}

		public RtfVisualImageAdapter( string fileNamePattern, ImageFormat targetFormat, double dpiX, double dpiY )
		{
			if ( fileNamePattern == null )
			{
				throw new ArgumentNullException( "fileNamePattern" );
			}

			this.fileNamePattern = fileNamePattern;
			this.targetFormat = targetFormat;
			this.dpiX = dpiX;
			this.dpiY = dpiY;
		}
#endif

		public string FileNamePattern
		{
			get { return fileNamePattern; }
		}

#if DRAWING
		public ImageFormat TargetFormat
		{
			get { return targetFormat; }
		}
#endif

		public double DpiX
		{
			get { return dpiX; }
		}

		public double DpiY
		{
			get { return dpiY; }
		}

#if DRAWING
    public ImageFormat GetImageFormat( RtfVisualImageFormat rtfVisualImageFormat )
		{
			ImageFormat imageFormat = null;

			switch ( rtfVisualImageFormat )
			{
				case RtfVisualImageFormat.Emf:
					imageFormat = ImageFormat.Emf;
					break;
				case RtfVisualImageFormat.Png:
					imageFormat = ImageFormat.Png;
					break;
				case RtfVisualImageFormat.Jpg:
					imageFormat = ImageFormat.Jpeg;
					break;
				case RtfVisualImageFormat.Wmf:
					imageFormat = ImageFormat.Wmf;
					break;
				case RtfVisualImageFormat.Bmp:
					imageFormat = ImageFormat.Bmp;
					break;
			}

			return imageFormat;
		}
#endif

		public string ResolveFileName( int index, RtfVisualImageFormat rtfVisualImageFormat )
		{
#if DRAWING
      ImageFormat imageFormat = targetFormat ?? GetImageFormat( rtfVisualImageFormat );

			return string.Format(
				CultureInfo.InvariantCulture,
				fileNamePattern,
				index,
				GetFileImageExtension( imageFormat ) );
#else
      return string.Format(
        CultureInfo.InvariantCulture,
        fileNamePattern,
        index,
        rtfVisualImageFormat);
#endif
    }

		public int CalcImageWidth( RtfVisualImageFormat format, int width,
			int desiredWidth, int scaleWidthPercent )
		{
			float imgScaleX = scaleWidthPercent / 100.0f;
			return (int)Math.Round( (double)desiredWidth * imgScaleX / twipsPerInch * dpiX );
		}

		public int CalcImageHeight( RtfVisualImageFormat format, int height,
			int desiredHeight, int scaleHeightPercent )
		{
			float imgScaleY = scaleHeightPercent / 100.0f;
			return (int)Math.Round( (double)desiredHeight * imgScaleY / twipsPerInch * dpiY );
		}

#if DRAWING
    private static string GetFileImageExtension( ImageFormat imageFormat )
		{
			string imageExtension = null;

			if ( imageFormat == ImageFormat.Bmp )
			{
				imageExtension = ".bmp";
			}
			else if ( imageFormat == ImageFormat.Emf )
			{
				imageExtension = ".emf";
			}
			else if ( imageFormat == ImageFormat.Exif )
			{
				imageExtension = ".exif";
			}
			else if ( imageFormat == ImageFormat.Gif )
			{
				imageExtension = ".gif";
			}
			else if ( imageFormat == ImageFormat.Icon )
			{
				imageExtension = ".ico";
			}
			else if ( imageFormat == ImageFormat.Jpeg )
			{
				imageExtension = ".jpg";
			}
			else if ( imageFormat == ImageFormat.Png )
			{
				imageExtension = ".png";
			}
			else if ( imageFormat == ImageFormat.Tiff )
			{
				imageExtension = ".tiff";
			}
			else if ( imageFormat == ImageFormat.Wmf )
			{
				imageExtension = ".wmf";
			}

			return imageExtension;
		}
		private readonly ImageFormat targetFormat;
#endif

    private readonly string fileNamePattern;
		private readonly double dpiX;
		private readonly double dpiY;

		private const string defaultFileNamePattern = "{0}{1}";
		private const int twipsPerInch = 1440;

	}

}

