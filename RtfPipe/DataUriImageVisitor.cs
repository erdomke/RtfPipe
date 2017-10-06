using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using RtfPipe;

namespace RtfPipe
{
  public class DataUriImageVisitor : IObjectVisitor
  {
    private const int twipsPerInch = 1440;
    private const double DefaultDpi = 96.0;

    public virtual int CalcImageWidth(IRtfVisualImage image)
    {
      float imgScaleX = image.ScaleWidthPercent / 100.0f;
      return (int)Math.Round((double)image.DesiredWidth * imgScaleX / twipsPerInch * DefaultDpi);
    }

    public virtual int CalcImageHeight(IRtfVisualImage image)
    {
      float imgScaleY = image.ScaleHeightPercent / 100.0f;
      return (int)Math.Round((double)image.DesiredHeight * imgScaleY / twipsPerInch * DefaultDpi);
    }

    public virtual string GetUri(IRtfVisualImage image)
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

    public virtual void RenderObject(int index, XmlWriter writer)
    {
      // Do nothing
    }
  }
}
