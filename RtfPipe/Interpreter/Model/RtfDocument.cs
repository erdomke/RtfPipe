// -- FILE ------------------------------------------------------------------
// name       : RtfDocument.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.21
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace RtfPipe.Model
{

  // ------------------------------------------------------------------------
  public sealed class RtfDocument : IRtfDocument
  {

    // ----------------------------------------------------------------------
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
    } // RtfDocument

    // ----------------------------------------------------------------------
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
    } // RtfDocument

    // ----------------------------------------------------------------------
    public int RtfVersion
    {
      get { return rtfVersion; }
    } // RtfVersion

    // ----------------------------------------------------------------------
    public IRtfFont DefaultFont
    {
      get { return defaultFont; }
    } // DefaultFont

    // ----------------------------------------------------------------------
    public IRtfTextFormat DefaultTextFormat
    {
      get { return defaultTextFormat; }
    } // DefaultTextFormat

    // ----------------------------------------------------------------------
    public RtfFontCollection FontTable
    {
      get { return fontTable; }
    } // FontTable

    // ----------------------------------------------------------------------
    public IList<IRtfColor> ColorTable
    {
      get { return colorTable; }
    } // ColorTable

    // ----------------------------------------------------------------------
    public string Generator
    {
      get { return generator; }
    } // Generator

    // ----------------------------------------------------------------------
    public IList<IRtfTextFormat> UniqueTextFormats
    {
      get { return uniqueTextFormats; }
    } // UniqueTextFormats

    // ----------------------------------------------------------------------
    public IRtfDocumentInfo DocumentInfo
    {
      get { return documentInfo; }
    } // DocumentInfo

    // ----------------------------------------------------------------------
    public IList<IRtfDocumentProperty> UserProperties
    {
      get { return userProperties; }
    } // UserProperties

    // ----------------------------------------------------------------------
    public IList<IRtfVisual> VisualContent
    {
      get { return visualContent; }
    } // VisualContent

    // ----------------------------------------------------------------------
    public override string ToString()
    {
      return "RTFv" + rtfVersion;
    } // ToString

    // ----------------------------------------------------------------------
    // members
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

  } // class RtfDocument

} // namespace RtfPipe.Model
// -- EOF -------------------------------------------------------------------
