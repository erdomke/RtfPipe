using RtfPipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public interface IImageVisitor
  {
    string GetUri(IRtfVisualImage image);
    int CalcImageWidth(IRtfVisualImage image);
    int CalcImageHeight(IRtfVisualImage image);
  }
}
