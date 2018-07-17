using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{


  public interface IRtfInterpreterContext
  {

    RtfInterpreterState State { get; }

    int RtfVersion { get; }

    string DefaultFontId { get; }

    Font DefaultFont { get; }

    RtfFontCollection FontTable { get; }

    IList<ColorValue> ColorTable { get; }

    string Generator { get; }

    IList<Style> UniqueTextFormats { get; }

    Style CurrentTextFormat { get; }

    Style GetSafeCurrentTextFormat();

    Style GetUniqueTextFormatInstance( Style templateFormat );

    IRtfDocumentInfo DocumentInfo { get; }

    IList<IRtfDocumentProperty> UserProperties { get; }

  }

}

