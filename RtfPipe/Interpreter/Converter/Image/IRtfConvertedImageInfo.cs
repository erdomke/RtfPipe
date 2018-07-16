namespace RtfPipe.Converter.Image
{


  public interface IRtfConvertedImageInfo
  {

    string FileName { get; }

#if DRAWING
    System.Drawing.Imaging.ImageFormat Format { get; }

    System.Drawing.Size Size { get; }
#endif

  }

}

