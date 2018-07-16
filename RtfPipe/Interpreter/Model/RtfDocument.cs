using System;
using System.Collections.Generic;

namespace RtfPipe.Model
{


  public sealed class RtfDocument : IRtfDocument
  {

    public RtfDocument( IRtfInterpreterContext context, IList<IRtfVisual> visualContent ) :
      this( context.RtfVersion,
        context.DefaultFont,
        context.FontTable,
        context.ColorTable,
        context.Generator,
        context.UniqueTextFormats,
        context.DocumentInfo,
        context.UserProperties,
        visualContent
      )
    {
    }

    public RtfDocument(
      int rtfVersion,
      IRtfFont defaultFont,
      RtfFontCollection fontTable,
      IList<IRtfColor> colorTable,
      string generator,
      IList<IRtfTextFormat> uniqueTextFormats,
      IRtfDocumentInfo documentInfo,
      IList<IRtfDocumentProperty> userProperties,
      IList<IRtfVisual> visualContent
    )
    {
      if ( rtfVersion != RtfSpec.RtfVersion1 )
      {
        throw new RtfUnsupportedStructureException( Strings.UnsupportedRtfVersion( rtfVersion ) );
      }
      if ( defaultFont == null )
      {
        throw new ArgumentNullException( "defaultFont" );
      }
      if ( fontTable == null )
      {
        throw new ArgumentNullException( "fontTable" );
      }
      if ( colorTable == null )
      {
        throw new ArgumentNullException( "colorTable" );
      }
      if ( uniqueTextFormats == null )
      {
        throw new ArgumentNullException( "uniqueTextFormats" );
      }
      if ( documentInfo == null )
      {
        throw new ArgumentNullException( "documentInfo" );
      }
      if ( userProperties == null )
      {
        throw new ArgumentNullException( "userProperties" );
      }
      if ( visualContent == null )
      {
        throw new ArgumentNullException( "visualContent" );
      }
      this.rtfVersion = rtfVersion;
      this.defaultFont = defaultFont;
      defaultTextFormat = new RtfTextFormat( defaultFont, RtfSpec.DefaultFontSize );
      this.fontTable = fontTable;
      this.colorTable = colorTable;
      this.generator = generator;
      this.uniqueTextFormats = uniqueTextFormats;
      this.documentInfo = documentInfo;
      this.userProperties = userProperties;
      this.visualContent = visualContent;
    }

    public int RtfVersion
    {
      get { return rtfVersion; }
    }

    public IRtfFont DefaultFont
    {
      get { return defaultFont; }
    }

    public IRtfTextFormat DefaultTextFormat
    {
      get { return defaultTextFormat; }
    }

    public RtfFontCollection FontTable
    {
      get { return fontTable; }
    }

    public IList<IRtfColor> ColorTable
    {
      get { return colorTable; }
    }

    public string Generator
    {
      get { return generator; }
    }

    public IList<IRtfTextFormat> UniqueTextFormats
    {
      get { return uniqueTextFormats; }
    }

    public IRtfDocumentInfo DocumentInfo
    {
      get { return documentInfo; }
    }

    public IList<IRtfDocumentProperty> UserProperties
    {
      get { return userProperties; }
    }

    public IList<IRtfVisual> VisualContent
    {
      get { return visualContent; }
    }

    public override string ToString()
    {
      return "RTFv" + rtfVersion;
    }

    private readonly int rtfVersion;
    private readonly IRtfFont defaultFont;
    private readonly IRtfTextFormat defaultTextFormat;
    private readonly RtfFontCollection fontTable;
    private readonly IList<IRtfColor> colorTable;
    private readonly string generator;
    private readonly IList<IRtfTextFormat> uniqueTextFormats;
    private readonly IRtfDocumentInfo documentInfo;
    private readonly IList<IRtfDocumentProperty> userProperties;
    private readonly IList<IRtfVisual> visualContent;

  }

}

