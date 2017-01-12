using RtfPipe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RtfPipe
{
  public interface IObjectVisitor
  {
    void RenderObject(int index, XmlWriter writer);
    string GetUri(IRtfVisualImage image);
    int CalcImageWidth(IRtfVisualImage image);
    int CalcImageHeight(IRtfVisualImage image);
  }
}
