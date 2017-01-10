// -- FILE ------------------------------------------------------------------
// name       : RtfImageConvertSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.05
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace RtfPipe.Converter.Image
{

  // ------------------------------------------------------------------------
  public class RtfImageConvertSettings
  {

    // ----------------------------------------------------------------------
    public RtfImageConvertSettings() :
      this( new RtfVisualImageAdapter() )
    {
    } // RtfImageConvertSettings

    // ----------------------------------------------------------------------
    public RtfImageConvertSettings( IRtfVisualImageAdapter imageAdapter )
    {
      if ( imageAdapter == null )
      {
        throw new ArgumentNullException( "imageAdapter" );
      }

      this.imageAdapter = imageAdapter;
    } // RtfImageConvertSettings

    // ----------------------------------------------------------------------
    public IRtfVisualImageAdapter ImageAdapter
    {
      get { return imageAdapter; }
    } // ImageAdapter

    // ----------------------------------------------------------------------
    public IRtfColor BackgroundColor { get; set; }

    // ----------------------------------------------------------------------
    public string ImagesPath
    {
      get { return imagesPath; }
      set { imagesPath = value; }
    } // ImagesPath

    // ----------------------------------------------------------------------
    public bool ScaleImage
    {
      get { return scaleImage; }
      set { scaleImage = value; }
    } // ScaleImage

    // ----------------------------------------------------------------------
    public float ScaleOffset { get; set; }

    // ----------------------------------------------------------------------
    public float ScaleExtension { get; set; }

    // ----------------------------------------------------------------------
    public string GetImageFileName( int index, RtfVisualImageFormat rtfVisualImageFormat )
    {
      string imageFileName = imageAdapter.ResolveFileName( index, rtfVisualImageFormat );
      if ( !string.IsNullOrEmpty( imagesPath ) )
      {
#if FILEIO
        imageFileName = Path.Combine( imagesPath, imageFileName );
#else
        imageFileName = imagesPath.TrimEnd('\\') + "\\" + imageFileName.TrimStart('\\');
#endif
      }
      return imageFileName;
    } // GetImageFileName

    // ----------------------------------------------------------------------
    private readonly IRtfVisualImageAdapter imageAdapter;
    private string imagesPath;
    private bool scaleImage = true;
  } // class RtfImageConvertSettings

} // namespace RtfPipe.Converter.Image
// -- EOF -------------------------------------------------------------------
