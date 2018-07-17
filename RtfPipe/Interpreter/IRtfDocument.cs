using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{


  public interface IRtfDocument
  {

    int RtfVersion { get; }

    Font DefaultFont { get; }

    Style DefaultTextFormat { get; }

    RtfFontCollection FontTable { get; }

    IList<ColorValue> ColorTable { get; }

    string Generator { get; }

    IList<Style> UniqueTextFormats { get; }

    IRtfDocumentInfo DocumentInfo { get; }

    IList<IRtfDocumentProperty> UserProperties { get; }

    IList<IRtfVisual> VisualContent { get; }

  }

}

