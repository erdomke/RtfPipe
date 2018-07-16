using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{


  public interface IRtfDocument
  {

    int RtfVersion { get; }

    IRtfFont DefaultFont { get; }

    IRtfTextFormat DefaultTextFormat { get; }

    RtfFontCollection FontTable { get; }

    IList<IRtfColor> ColorTable { get; }

    string Generator { get; }

    IList<IRtfTextFormat> UniqueTextFormats { get; }

    IRtfDocumentInfo DocumentInfo { get; }

    IList<IRtfDocumentProperty> UserProperties { get; }

    IList<IRtfVisual> VisualContent { get; }

  }

}

