using System;
using System.IO;
#if DRAWING
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endif
using RtfPipe.Model;
using RtfPipe.Interpreter;
using System.Collections.Generic;

namespace RtfPipe.Converter.Image
{


  public class RtfImageConverter : RtfInterpreterListenerBase
  {

    public RtfImageConverter() :
      this( new RtfImageConvertSettings() )
    {
    }

    public RtfImageConverter( RtfImageConvertSettings settings )
    {
      if ( settings == null )
      {
        throw new ArgumentNullException( "settings" );
      }

      this.settings = settings;
    }

    public RtfImageConvertSettings Settings
    {
      get { return settings; }
    }

    public IList<IRtfConvertedImageInfo> ConvertedImages
    {
      get { return convertedImages; }
    }

    protected override void DoBeginDocument( IRtfInterpreterContext context )
    {
      base.DoBeginDocument( context );

      convertedImages.Clear();
    }

    protected override void DoInsertImage( IRtfInterpreterContext context,
      RtfVisualImageFormat format,
      int width, int height, 
      int desiredWidth, int desiredHeight,
      int scaleWidthPercent, int scaleHeightPercent,
      string imageDataHex
    )
    {
#if DRAWING && FILEIO
      int imageIndex = convertedImages.Count + 1;
      string fileName = settings.GetImageFileName( imageIndex, format );
      EnsureImagesPath( fileName );

      byte[] imageBuffer = RtfVisualImage.ToBinary( imageDataHex );
      Size imageSize;
      ImageFormat imageFormat;
      if ( settings.ImageAdapter.TargetFormat == null )
      {
        using ( System.Drawing.Image image = System.Drawing.Image.FromStream( new MemoryStream( imageBuffer ) ) )
        {
          imageFormat = image.RawFormat;
          imageSize = image.Size;
        }
        using ( BinaryWriter binaryWriter = new BinaryWriter( File.Open( fileName, FileMode.Create ) ) )
        {
          binaryWriter.Write( imageBuffer );
        }
      }
      else
      {
        imageFormat = settings.ImageAdapter.TargetFormat;
        if ( settings.ScaleImage )
        {
          imageSize = new Size(
           settings.ImageAdapter.CalcImageWidth( format, width, desiredWidth, scaleWidthPercent ),
           settings.ImageAdapter.CalcImageHeight( format, height, desiredHeight, scaleHeightPercent ) );
        }
        else
        {
          imageSize = new Size( width, height );
        }

        SaveImage( imageBuffer, format, fileName, imageSize );
      }
      convertedImages.Add( new RtfConvertedImageInfo( fileName, imageFormat, imageSize ) );
#endif
    }

#if DRAWING && FILEIO
    protected virtual void SaveImage( byte[] imageBuffer, RtfVisualImageFormat format, string fileName, Size size )
    {
      ImageFormat targetFormat = settings.ImageAdapter.TargetFormat;

      float scaleOffset = settings.ScaleOffset;
      float scaleExtension = settings.ScaleExtension;
      using ( System.Drawing.Image image = System.Drawing.Image.FromStream(
        new MemoryStream( imageBuffer, 0, imageBuffer.Length ) ) )
      {
        Bitmap convertedImage = new Bitmap( new Bitmap( size.Width, size.Height, image.PixelFormat ) );
        Graphics graphic = Graphics.FromImage( convertedImage );
        graphic.CompositingQuality = CompositingQuality.HighQuality;
        graphic.SmoothingMode = SmoothingMode.HighQuality;
        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
        RectangleF rectangle = new RectangleF( 
          scaleOffset, 
          scaleOffset, 
          size.Width + scaleExtension, 
          size.Height + scaleExtension );

        if ( settings.BackgroundColor.HasValue )
        {
          graphic.Clear( settings.BackgroundColor.Value );
        }

        graphic.DrawImage( image, rectangle );
        convertedImage.Save( fileName, targetFormat );
      }
    }

    protected virtual void EnsureImagesPath( string imageFileName )
    {
      FileInfo fi = new FileInfo( imageFileName );
      if ( !string.IsNullOrEmpty( fi.DirectoryName ) && !Directory.Exists( fi.DirectoryName ) )
      {
        Directory.CreateDirectory( fi.DirectoryName );
      }
    }
#endif

    private readonly IList<IRtfConvertedImageInfo> convertedImages = new List<IRtfConvertedImageInfo>();
    private readonly RtfImageConvertSettings settings;

  }

}

