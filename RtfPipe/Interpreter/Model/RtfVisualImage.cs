using System;
using System.Globalization;
using System.IO;
using System.Text;
using RtfPipe.Sys;

namespace RtfPipe.Model
{


  public sealed class RtfVisualImage : RtfVisual, IRtfVisualImage
  {

    public RtfVisualImage(
      RtfVisualImageFormat format,
      RtfTextAlignment alignment,
      int width,
      int height,
      int desiredWidth,
      int desiredHeight,
      int scaleWidthPercent,
      int scaleHeightPercent,
      string imageDataHex
    ) :
      base( RtfVisualKind.Image )
    {
      if ( width <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageWidth( width ) );
      }
      if ( height <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageHeight( height ) );
      }
      if ( desiredWidth <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageDesiredWidth( desiredWidth ) );
      }
      if ( desiredHeight <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageDesiredHeight( desiredHeight ) );
      }
      if ( scaleWidthPercent <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageScaleWidth( scaleWidthPercent ) );
      }
      if ( scaleHeightPercent <= 0 )
      {
        throw new ArgumentException( Strings.InvalidImageScaleHeight( scaleHeightPercent ) );
      }
      if ( imageDataHex == null )
      {
        throw new ArgumentNullException( "imageDataHex" );
      }
      this.format = format;
      this.alignment = alignment;
      this.width = width;
      this.height = height;
      this.desiredWidth = desiredWidth;
      this.desiredHeight = desiredHeight;
      this.scaleWidthPercent = scaleWidthPercent;
      this.scaleHeightPercent = scaleHeightPercent;
      this.imageDataHex = imageDataHex;
    }

    protected override void DoVisit( IRtfVisualVisitor visitor )
    {
      visitor.VisitImage( this );
    }

    public RtfVisualImageFormat Format
    {
      get { return format; }
    }

    public RtfTextAlignment Alignment
    {
      get { return alignment; }
      set { alignment = value; }
    }

    public int Width
    {
      get { return width; }
    }

    public int Height
    {
      get { return height; }
    }

    public int DesiredWidth
    {
      get { return desiredWidth; }
    }

    public int DesiredHeight
    {
      get { return desiredHeight; }
    }

    public int ScaleWidthPercent
    {
      get { return scaleWidthPercent; }
    }

    public int ScaleHeightPercent
    {
      get { return scaleHeightPercent; }
    }

    public string ImageDataHex
    {
      get { return imageDataHex; }
    }

    public byte[] ImageDataBinary
    {
      get { return imageDataBinary ?? ( imageDataBinary = ToBinary( imageDataHex ) ); }
    }

#if DRAWING
    public System.Drawing.Image ImageForDrawing
    {
      get
      {
        switch ( format )
        {
          case RtfVisualImageFormat.Bmp:
          case RtfVisualImageFormat.Jpg:
          case RtfVisualImageFormat.Png:
          case RtfVisualImageFormat.Emf:
          case RtfVisualImageFormat.Wmf:
            byte[] data = ImageDataBinary;
            return System.Drawing.Image.FromStream( new MemoryStream( data, 0, data.Length ) );
        }
        return null;
      }
    }
#endif

    public static byte[] ToBinary( string imageDataHex )
    {
      if ( imageDataHex == null )
      {
        throw new ArgumentNullException( "imageDataHex" );
      }

      int hexDigits = imageDataHex.Length;
      int dataSize = hexDigits / 2;
      byte[] imageDataBinary = new byte[ dataSize ];

      StringBuilder hex = new StringBuilder( 2 );

      int dataPos = 0;
      for ( int i = 0; i < hexDigits; i++ )
      {
        char c = imageDataHex[ i ];
        if ( char.IsWhiteSpace( c ) )
        {
          continue;
        }
        hex.Append( imageDataHex[ i ] );
        if ( hex.Length == 2 )
        {
          imageDataBinary[ dataPos ] = byte.Parse( hex.ToString(), NumberStyles.HexNumber );
          dataPos++;
          hex.Remove( 0, 2 );
        }
      }

      return imageDataBinary;
    }

    protected override bool IsEqual( object obj )
    {
      RtfVisualImage compare = obj as RtfVisualImage; // guaranteed to be non-null
      return
        compare != null &&
        base.IsEqual( compare ) &&
        format == compare.format &&
        alignment == compare.alignment &&
        width == compare.width &&
        height == compare.height &&
        desiredWidth == compare.desiredWidth &&
        desiredHeight == compare.desiredHeight &&
        scaleWidthPercent == compare.scaleWidthPercent &&
        scaleHeightPercent == compare.scaleHeightPercent &&
        imageDataHex.Equals( compare.imageDataHex );
      //imageDataBinary.Equals( compare.imageDataBinary ); // cached info only
    }

    protected override int ComputeHashCode()
    {
      int hash = base.ComputeHashCode();
      hash = HashTool.AddHashCode( hash, format );
      hash = HashTool.AddHashCode( hash, alignment );
      hash = HashTool.AddHashCode( hash, width );
      hash = HashTool.AddHashCode( hash, height );
      hash = HashTool.AddHashCode( hash, desiredWidth );
      hash = HashTool.AddHashCode( hash, desiredHeight );
      hash = HashTool.AddHashCode( hash, scaleWidthPercent );
      hash = HashTool.AddHashCode( hash, scaleHeightPercent );
      hash = HashTool.AddHashCode( hash, imageDataHex );
      //hash = HashTool.AddHashCode( hash, imageDataBinary ); // cached info only
      return hash;
    }

    public override string ToString()
    {
      return "[" + format + ": " + alignment + ", " +
        width + " x " + height + " " +
        "(" + desiredWidth + " x " + desiredHeight + ") " +
        "{" + scaleWidthPercent + "% x " + scaleHeightPercent + "%} " +
        ":" + ( imageDataHex.Length / 2 ) + " bytes]";
    }

    private readonly RtfVisualImageFormat format;
    private RtfTextAlignment alignment;
    private readonly int width;
    private readonly int height;
    private readonly int desiredWidth;
    private readonly int desiredHeight;
    private readonly int scaleWidthPercent;
    private readonly int scaleHeightPercent;
    private readonly string imageDataHex;
    private byte[] imageDataBinary; // cached info only

  }

}

