using System;
using System.Collections.Generic;
using System.Text;
using RtfPipe;

namespace RtfPipe
{
  public class DataUriImageVisitor : IImageVisitor
  {
    private const int twipsPerInch = 1440;
    private const double DefaultDpi = 96.0;

    public int CalcImageHeight(IRtfVisualImage image)
    {
      float imgScaleX = image.ScaleWidthPercent / 100.0f;
      return (int)Math.Round((double)image.DesiredWidth * imgScaleX / twipsPerInch * DefaultDpi);
    }

    public int CalcImageWidth(IRtfVisualImage image)
    {
      float imgScaleY = image.ScaleHeightPercent / 100.0f;
      return (int)Math.Round((double)image.DesiredHeight * imgScaleY / twipsPerInch * DefaultDpi);
    }

    public string GetUri(IRtfVisualImage image)
    {
      var result = "data:";
      switch (image.Format)
      {
        case RtfVisualImageFormat.Bmp:
          result += "image/bmp;base64,";
          break;
        case RtfVisualImageFormat.Jpg:
          result += "image/jpg;base64,";
          break;
        case RtfVisualImageFormat.Wmf:
          result += "windows/metafile;base64,";
          break;
        default:
          result += "image/png;base64,";
          break;
      }
      return result + Convert.ToBase64String(image.ImageDataBinary);
    }
  }
}
