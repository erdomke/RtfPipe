namespace RtfPipe.Converter.Image
{


  public interface IRtfVisualImageAdapter
  {
#if DRAWING
    System.Drawing.Imaging.ImageFormat TargetFormat { get; }
#endif

    string ResolveFileName(int index, RtfVisualImageFormat rtfVisualImageFormat);

    int CalcImageWidth(RtfVisualImageFormat format, int width,
      int desiredWidth, int scaleWidthPercent);

    int CalcImageHeight(RtfVisualImageFormat format, int height,
      int desiredHeight, int scaleHeightPercent);

  }

}

