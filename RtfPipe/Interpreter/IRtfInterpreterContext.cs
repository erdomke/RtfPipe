using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{


  public interface IRtfInterpreterContext
  {

    RtfInterpreterState State { get; }

    int RtfVersion { get; }

    string DefaultFontId { get; }

    IRtfFont DefaultFont { get; }

    RtfFontCollection FontTable { get; }

    IList<IRtfColor> ColorTable { get; }

    string Generator { get; }

    IList<IRtfTextFormat> UniqueTextFormats { get; }

    IRtfTextFormat CurrentTextFormat { get; }

    IRtfTextFormat GetSafeCurrentTextFormat();

    IRtfTextFormat GetUniqueTextFormatInstance( IRtfTextFormat templateFormat );

    IRtfDocumentInfo DocumentInfo { get; }

    IList<IRtfDocumentProperty> UserProperties { get; }

  }

}

