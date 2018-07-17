using System;

namespace RtfPipe.Converter.Image
{


  public class RtfImageConvertSettings
  {

    public RtfImageConvertSettings() :
      this(new RtfVisualImageAdapter())
    {
    }

    public RtfImageConvertSettings(IRtfVisualImageAdapter imageAdapter)
    {
      if (imageAdapter == null)
      {
        throw new ArgumentNullException("imageAdapter");
      }

      this.imageAdapter = imageAdapter;
    }

    public IRtfVisualImageAdapter ImageAdapter
    {
      get { return imageAdapter; }
    }

    public ColorValue BackgroundColor { get; set; }

    public string ImagesPath
    {
      get { return imagesPath; }
      set { imagesPath = value; }
    }

    public bool ScaleImage
    {
      get { return scaleImage; }
      set { scaleImage = value; }
    }

    public float ScaleOffset { get; set; }

    public float ScaleExtension { get; set; }

    public string GetImageFileName(int index, RtfVisualImageFormat rtfVisualImageFormat)
    {
      string imageFileName = imageAdapter.ResolveFileName(index, rtfVisualImageFormat);
      if (!string.IsNullOrEmpty(imagesPath))
      {
#if FILEIO
        imageFileName = Path.Combine( imagesPath, imageFileName );
#else
        imageFileName = imagesPath.TrimEnd('\\') + "\\" + imageFileName.TrimStart('\\');
#endif
      }
      return imageFileName;
    }

    private readonly IRtfVisualImageAdapter imageAdapter;
    private string imagesPath;
    private bool scaleImage = true;
  }

}

