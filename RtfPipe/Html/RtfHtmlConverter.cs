// -- FILE ------------------------------------------------------------------
// name       : RtfHtmlConverter.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.02
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;
using RtfPipe.Sys.Logging;
using RtfPipe.Support;
using RtfPipe.Converter.Image;
using System.Xml;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

  // ------------------------------------------------------------------------
  public class RtfHtmlConverter : RtfVisualVisitorBase
  {

    // ----------------------------------------------------------------------
    public const string DefaultHtmlFileExtension = ".html";

    // ----------------------------------------------------------------------
    public RtfHtmlConverter( IRtfDocument rtfDocument ) :
      this( rtfDocument, new RtfHtmlConvertSettings() )
    {
    } // RtfHtmlConverter

    // ----------------------------------------------------------------------
    public RtfHtmlConverter( IRtfDocument rtfDocument, RtfHtmlConvertSettings settings )
    {
      if ( rtfDocument == null )
      {
        throw new ArgumentNullException( "rtfDocument" );
      }
      if ( settings == null )
      {
        throw new ArgumentNullException( "settings" );
      }

      this._rtfDocument = rtfDocument;
      this._settings = settings;
    } // RtfHtmlConverter

    // ----------------------------------------------------------------------
    public IRtfDocument RtfDocument
    {
      get { return _rtfDocument; }
    } // RtfDocument

    // ----------------------------------------------------------------------
    public RtfHtmlConvertSettings Settings
    {
      get { return _settings; }
    } // Settings

    // ----------------------------------------------------------------------
    public IRtfHtmlStyleConverter StyleConverter
    {
      get { return _styleConverter; }
      set
      {
        if ( value == null )
        {
          throw new ArgumentNullException( "value" );
        }
        _styleConverter = value;
      }
    } // StyleConverter
    
    // ----------------------------------------------------------------------
    protected XmlWriter Writer
    {
      get { return _writer; }
    } // Writer

    // ----------------------------------------------------------------------
    protected RtfHtmlElementPath ElementPath
    {
      get { return _elementPath; }
    } // ElementPath

    // ----------------------------------------------------------------------
    protected bool IsInParagraph
    {
      get { return IsInElement( "p" ); }
    } // IsInParagraph

    // ----------------------------------------------------------------------
    protected bool IsInList
    {
      get { return IsInElement( "ul" ) || IsInElement( "ol" ); }
    } // IsInList

    // ----------------------------------------------------------------------
    protected bool IsInListItem
    {
      get { return IsInElement( "li" ); }
    } // IsInListItem

    // ----------------------------------------------------------------------
    public string Convert()
    {
      using (var stringWriter = new StringWriter() )
      {
        using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
        {
          Convert(writer);
        }
        return stringWriter.ToString();
      }
    } // Convert

    public void Convert(XmlWriter writer)
    {
      _writer = writer;
      RenderDocumentSection();
      RenderHtmlSection();
      if (_elementPath.Count != 0)
      {
        logger.Error("unbalanced element structure");
      }
    }

    // ----------------------------------------------------------------------
    protected bool IsCurrentElement( string tag )
    {
      return _elementPath.IsCurrent( tag );
    } // IsCurrentElement

    // ----------------------------------------------------------------------
    protected bool IsInElement(string tag )
    {
      return _elementPath.Contains( tag );
    } // IsInElement

    #region TagRendering

    // ----------------------------------------------------------------------
    protected void RenderBeginTag(string tag )
    {
      Writer.WriteStartElement(tag);
      _elementPath.Push( tag );
    } // RenderBeginTag
    
    // ----------------------------------------------------------------------
    protected virtual void RenderEndTag()
    {
      Writer.WriteEndElement();
      _elementPath.Pop();
    } // RenderEndTag

    // ----------------------------------------------------------------------
    protected virtual void RenderTitleTag()
    {
      RenderBeginTag( "title" );
    } // RenderTitleTag

    // ----------------------------------------------------------------------
    protected virtual void RenderMetaTag()
    {
      RenderBeginTag( "meta" );
    } // RenderMetaTag

    // ----------------------------------------------------------------------
    protected virtual void RenderHtmlTag()
    {
      RenderBeginTag( "html" );
    } // RenderHtmlTag

    // ----------------------------------------------------------------------
    protected virtual void RenderLinkTag()
    {
      RenderBeginTag( "link" );
    } // RenderLinkTag

    // ----------------------------------------------------------------------
    protected virtual void RenderHeadTag()
    {
      RenderBeginTag( "head" );
    } // RenderHeadTag

    // ----------------------------------------------------------------------
    protected virtual void RenderBodyTag()
    {
      RenderBeginTag( "body" );
    } // RenderBodyTag

    // ----------------------------------------------------------------------
    protected virtual void RenderLineBreak()
    {
      _writer.WriteStartElement("br");
      _writer.WriteEndElement();
    } // RenderLineBreak

    // ----------------------------------------------------------------------
    protected virtual void RenderATag()
    {
      RenderBeginTag( "a" );
    } // RenderATag

    // ----------------------------------------------------------------------
    protected virtual void RenderPTag()
    {
      RenderBeginTag( "p" );
    } // RenderPTag

    // ----------------------------------------------------------------------
    protected virtual void RenderBTag()
    {
      RenderBeginTag( "b" );
    } // RenderBTag

    // ----------------------------------------------------------------------
    protected virtual void RenderITag()
    {
      RenderBeginTag( "i" );
    } // RenderITag

    // ----------------------------------------------------------------------
    protected virtual void RenderUTag()
    {
      RenderBeginTag( "u" );
    } // RenderUTag

    // ----------------------------------------------------------------------
    protected virtual void RenderSTag()
    {
      RenderBeginTag( "s" );
    } // RenderSTag

    // ----------------------------------------------------------------------
    protected virtual void RenderSubTag()
    {
      RenderBeginTag( "sub" );
    } // RenderSubTag

    // ----------------------------------------------------------------------
    protected virtual void RenderSupTag()
    {
      RenderBeginTag( "sup" );
    } // RenderSupTag

    // ----------------------------------------------------------------------
    protected virtual void RenderSpanTag()
    {
      RenderBeginTag( "span" );
    } // RenderSpanTag

    // ----------------------------------------------------------------------
    protected virtual void RenderUlTag()
    {
      RenderBeginTag( "ul" );
    } // RenderUlTag

    // ----------------------------------------------------------------------
    protected virtual void RenderOlTag()
    {
      RenderBeginTag( "ol" );
    } // RenderOlTag

    // ----------------------------------------------------------------------
    protected virtual void RenderLiTag()
    {
      RenderBeginTag( "li" );
    } // RenderLiTag

    // ----------------------------------------------------------------------
    protected virtual void RenderImgTag()
    {
      RenderBeginTag( "img" );
    } // RenderImgTag

    // ----------------------------------------------------------------------
    protected virtual void RenderStyleTag()
    {
      RenderBeginTag( "style" );
    } // RenderStyleTag

    #endregion // TagRendering

    #region HtmlStructure

    // ----------------------------------------------------------------------
    protected virtual void RenderDocumentHeader()
    {
      Writer.WriteDocType("html", null, null, null);
    } // RenderDocumentHeader

    // ----------------------------------------------------------------------
    protected virtual void RenderMetaContentType()
    {
      RenderMetaTag();
      Writer.WriteAttributeString( "http-equiv", "content-type" );

      string content = "text/html";
      if ( !string.IsNullOrEmpty( _settings.CharacterSet ) )
      {
        content = string.Concat( content, "; charset=", _settings.CharacterSet );
      }
      Writer.WriteAttributeString( "content", content );
      RenderEndTag();
    } // RenderMetaContentType    

    // ----------------------------------------------------------------------
    protected virtual void RenderLinkStyleSheets()
    {
      if ( !_settings.HasStyleSheetLinks )
      {
        return;
      }

      foreach ( string styleSheetLink in _settings.StyleSheetLinks )
      {
        if ( string.IsNullOrEmpty( styleSheetLink ) )
        {
          continue;
        }

        RenderLinkTag();
        Writer.WriteAttributeString( "href", styleSheetLink );
        Writer.WriteAttributeString( "type", "text/css" );
        Writer.WriteAttributeString( "rel", "stylesheet" );
        RenderEndTag();
      }
    } // RenderLinkStyleSheets

    // ----------------------------------------------------------------------
    protected virtual void RenderHeadAttributes()
    {
      RenderMetaContentType();
      RenderLinkStyleSheets();
    } // RenderHeadAttributes

    // ----------------------------------------------------------------------
    protected virtual void RenderTitle()
    {
      if ( string.IsNullOrEmpty( _settings.Title ) )
      {
        return;
      }

      RenderTitleTag();
      Writer.WriteString( _settings.Title );
      RenderEndTag();
    } // RenderTitle

    // ----------------------------------------------------------------------
    protected virtual void RenderStyles()
    {
      if ( !_settings.HasStyles )
      {
        return;
      }
      
      RenderStyleTag();

      bool firstStyle = true;
      foreach ( IRtfHtmlCssStyle cssStyle in _settings.Styles )
      {
        if ( cssStyle.Properties.Count == 0 )
        {
          continue;
        }

        if ( !firstStyle )
        {
          Writer.WriteWhitespace("\r\n");
        }
        Writer.WriteString( cssStyle.SelectorName );
        Writer.WriteWhitespace("\r\n");
        Writer.WriteString( "{\r\n" );
        for ( int i = 0; i < cssStyle.Properties.Count; i++ )
        {
          Writer.WriteString( string.Format(
            CultureInfo.InvariantCulture,
            "  {0}: {1};",
            cssStyle.Properties.Keys[ i ],
            cssStyle.Properties[ i ] ) );
          Writer.WriteWhitespace("\r\n");
        }
        Writer.WriteString( "}" );
        firstStyle = false;
      }

      RenderEndTag();
    } // RenderStyles

    // ----------------------------------------------------------------------
    protected virtual void RenderRtfContent()
    {
      foreach ( IRtfVisual visual in _rtfDocument.VisualContent )
      {
        visual.Visit( this );
      }
      EnsureClosedList();
    } // RenderRtfContent

    // ----------------------------------------------------------------------
    protected virtual bool BeginParagraph()
    {
      if ( IsInParagraph )
      {
        return false;
      }
      RenderPTag();
      return true;
    } // BeginParagraph

    // ----------------------------------------------------------------------
    protected virtual void EndParagraph()
    {
      if ( !IsInParagraph )
      {
        return;
      }
      RenderEndTag();
    } // EndParagraph

    // ----------------------------------------------------------------------
    protected virtual bool OnEnterVisual( IRtfVisual rtfVisual )
    {
      return true;
    } // OnEnterVisual

    // ----------------------------------------------------------------------
    protected virtual void OnLeaveVisual( IRtfVisual rtfVisual )
    {
    } // OnLeaveVisual
    #endregion // HtmlStructure

    #region HtmlFormat

    // ----------------------------------------------------------------------
    protected virtual IRtfHtmlStyle GetHtmlStyle( IRtfVisual rtfVisual )
    {
      IRtfHtmlStyle htmlStyle = RtfHtmlStyle.Empty;

      switch ( rtfVisual.Kind )
      {
        case RtfVisualKind.Text:
          htmlStyle = _styleConverter.TextToHtml( rtfVisual as IRtfVisualText );
          break;
      }

      return htmlStyle;
    } // GetHtmlStyle

    #endregion // HtmlFormat

    #region RtfVisuals

    // ----------------------------------------------------------------------
    protected override void DoVisitText( IRtfVisualText visualText )
    {
      if ( !EnterVisual( visualText ) )
      {
        return;
      }

      // suppress hidden text
      if ( visualText.Format.IsHidden && _settings.IsShowHiddenText == false )
      {
        return;
      }

      IRtfTextFormat textFormat = visualText.Format;

      if ( !IsInListItem && BeginParagraph() )
      {
        switch (textFormat.Alignment)
        {
          case RtfTextAlignment.Left:
            //Writer.AddStyleAttribute( HtmlTextWriterStyle.TextAlign, "left" );
            break;
          case RtfTextAlignment.Center:
            Writer.WriteAttributeString("style", "text-align:center");
            break;
          case RtfTextAlignment.Right:
            Writer.WriteAttributeString("style", "text-align:right");
            break;
          case RtfTextAlignment.Justify:
            Writer.WriteAttributeString("style", "text-align:justify");
            break;
        }
      }


      // format tags
      if ( textFormat.IsBold )
      {
        RenderBTag();
      }
      if ( textFormat.IsItalic )
      {
        RenderITag();
      }
      if ( textFormat.IsUnderline )
      {
        RenderUTag();
      }
      if ( textFormat.IsStrikeThrough )
      {
        RenderSTag();
      }

      // span with style
      IRtfHtmlStyle htmlStyle = GetHtmlStyle( visualText );
      if ( !htmlStyle.IsEmpty )
      {
        RenderSpanTag();

        var styles = new Dictionary<string, string>();
        if ( !string.IsNullOrEmpty( htmlStyle.ForegroundColor ) )
        {
          styles["color"] = htmlStyle.ForegroundColor;
        }
        if ( !string.IsNullOrEmpty( htmlStyle.BackgroundColor ) )
        {
          styles["background-color"] = htmlStyle.BackgroundColor;
        }
        if ( !string.IsNullOrEmpty( htmlStyle.FontFamily ) )
        {
          styles["font-family"] = htmlStyle.FontFamily;
        }
        if ( !string.IsNullOrEmpty( htmlStyle.FontSize ) )
        {
          styles["font-size"] = htmlStyle.FontSize;
        }

        if (styles.Count > 0)
        {
          _writer.WriteStartAttribute("style");
          var first = true;
          foreach (var kvp in styles)
          {
            if (!first)
              _writer.WriteString(";");
            _writer.WriteString(kvp.Key);
            _writer.WriteString(":");
            _writer.WriteString(kvp.Value);
            first = false;
          }
          _writer.WriteEndAttribute();
        }
      }

      // visual hyperlink
      bool isHyperlink = false;
      if ( _settings.ConvertVisualHyperlinks )
      {
        string href = ConvertVisualHyperlink( visualText.Text );
        if ( !string.IsNullOrEmpty( href ) )
        {
          isHyperlink = true;
          RenderATag();
          Writer.WriteAttributeString( "href", href );
        }
      }

      // subscript and superscript
      if ( textFormat.SuperScript < 0 )
      {
        RenderSubTag();
      }
      else if ( textFormat.SuperScript > 0 )
      {
        RenderSupTag();
      }

      Writer.WriteString(visualText.Text);

      // subscript and superscript
      if ( textFormat.SuperScript < 0 )
      {
        RenderEndTag(); // sub
      }
      else if ( textFormat.SuperScript > 0 )
      {
        RenderEndTag(); // sup
      }

      // visual hyperlink
      if ( isHyperlink )
      {
        RenderEndTag(); // a
      }

      // span with style
      if ( !htmlStyle.IsEmpty )
      {
        RenderEndTag();
      }

      // format tags
      if ( textFormat.IsStrikeThrough )
      {
        RenderEndTag(); // s
      }
      if ( textFormat.IsUnderline )
      {
        RenderEndTag(); // u
      }
      if ( textFormat.IsItalic )
      {
        RenderEndTag(); // i
      }
      if ( textFormat.IsBold )
      {
        RenderEndTag(); // b
      }

      LeaveVisual( visualText );
    } // DoVisitText

    // ----------------------------------------------------------------------
    protected override void DoVisitImage( IRtfVisualImage visualImage )
    {
      if ( !EnterVisual( visualImage ) )
      {
        return;
      }

      if (BeginParagraph())
      {
        switch ( visualImage.Alignment )
        {
          case RtfTextAlignment.Left:
            //Writer.AddStyleAttribute( HtmlTextWriterStyle.TextAlign, "left" );
            break;
          case RtfTextAlignment.Center:
            Writer.WriteAttributeString("style", "text-align:center");
            break;
          case RtfTextAlignment.Right:
            Writer.WriteAttributeString("style", "text-align:right");
            break;
          case RtfTextAlignment.Justify:
            Writer.WriteAttributeString("style", "text-align:justify");
            break;
        }
      }

      string fileName = _settings.ImageVisitor.GetUri(visualImage);
      int width = _settings.ImageVisitor.CalcImageWidth(visualImage);
      int height = _settings.ImageVisitor.CalcImageHeight(visualImage);

      RenderImgTag();
      Writer.WriteAttributeString( "width", width.ToString( CultureInfo.InvariantCulture ) );
      Writer.WriteAttributeString( "height", height.ToString( CultureInfo.InvariantCulture ) );
      Writer.WriteAttributeString( "src", fileName);
      RenderEndTag();
      
      LeaveVisual( visualImage );
    } // DoVisitImage

    // ----------------------------------------------------------------------
    protected override void DoVisitSpecial( IRtfVisualSpecialChar visualSpecialChar )
    {
      if ( !EnterVisual( visualSpecialChar ) )
      {
        return;
      }

      if (!IsInListItem && BeginParagraph())
      {
        switch (visualSpecialChar.Format.Alignment)
        {
          case RtfTextAlignment.Left:
            //Writer.AddStyleAttribute( HtmlTextWriterStyle.TextAlign, "left" );
            break;
          case RtfTextAlignment.Center:
            Writer.WriteAttributeString("style", "text-align:center");
            break;
          case RtfTextAlignment.Right:
            Writer.WriteAttributeString("style", "text-align:right");
            break;
          case RtfTextAlignment.Justify:
            Writer.WriteAttributeString("style", "text-align:justify");
            break;
        }
      }
      
      switch ( visualSpecialChar.CharKind )
      {
        case RtfVisualSpecialCharKind.ParagraphNumberBegin:
          _isInParagraphNumber = true;
          break;
        case RtfVisualSpecialCharKind.ParagraphNumberEnd:
          _isInParagraphNumber = false;
          break;
        case RtfVisualSpecialCharKind.EmDash:
          Writer.WriteEntityRef("mdash");
          break;
        case RtfVisualSpecialCharKind.EmSpace:
          Writer.WriteEntityRef("emsp");
          break;
        case RtfVisualSpecialCharKind.EnDash:
          Writer.WriteEntityRef("ndash");
          break;
        case RtfVisualSpecialCharKind.EnSpace:
          Writer.WriteEntityRef("ensp");
          break;
        case RtfVisualSpecialCharKind.LeftDoubleQuote:
          Writer.WriteEntityRef("ldquo");
          break;
        case RtfVisualSpecialCharKind.LeftSingleQuote:
          Writer.WriteEntityRef("lsquo");
          break;
        case RtfVisualSpecialCharKind.NonBreakingHyphen:
          Writer.WriteEntityRef("#8209");
          break;
        case RtfVisualSpecialCharKind.NonBreakingSpace:
          Writer.WriteEntityRef("nbsp");
          break;
        case RtfVisualSpecialCharKind.RightDoubleQuote:
          Writer.WriteEntityRef("rdquo");
          break;
        case RtfVisualSpecialCharKind.RightSingleQuote:
          Writer.WriteEntityRef("rsquo");
          break;
      }
      
      LeaveVisual( visualSpecialChar );
    } // DoVisitSpecial

    // ----------------------------------------------------------------------
    protected override void DoVisitBreak( IRtfVisualBreak visualBreak )
    {
      if ( !EnterVisual( visualBreak ) )
      {
        return;
      }

      switch ( visualBreak.BreakKind )
      {
        case RtfVisualBreakKind.Line:
          RenderLineBreak();
          break;
        case RtfVisualBreakKind.Page:
          break;
        case RtfVisualBreakKind.Paragraph:
          if ( IsInParagraph )
          {
            EndParagraph(); // close paragraph
          }
          else if ( IsInListItem )
          {
            EndParagraph();
            RenderEndTag(); // close list item
          }
          else
          {
            BeginParagraph();
            Writer.WriteEntityRef("nbsp");
            EndParagraph();
          }
          break;
        case RtfVisualBreakKind.Section:
          break;
      }

      LeaveVisual( visualBreak );
    } // DoVisitBreak

    #endregion // RtfVisuals

    // ----------------------------------------------------------------------
    private string ConvertVisualHyperlink( string text )
    {
      if ( string.IsNullOrEmpty( text ) )
      {
        return null;
      }

      if ( _hyperlinkRegEx == null )
      {
        if ( string.IsNullOrEmpty( _settings.VisualHyperlinkPattern ) )
        {
          return null;
        }
        _hyperlinkRegEx = new Regex( _settings.VisualHyperlinkPattern );
      }

      return _hyperlinkRegEx.IsMatch( text ) ? text : null;
    } // ConvertVisualHyperlink

    // ----------------------------------------------------------------------
    private void RenderDocumentSection()
    {
      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Document ) != RtfHtmlConvertScope.Document )
      {
        return;
      }

      RenderDocumentHeader();
    } // RenderDocumentSection

    // ----------------------------------------------------------------------
    private void RenderHtmlSection()
    {
      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Html ) == RtfHtmlConvertScope.Html )
      {
        RenderHtmlTag();
      }

      RenderHeadSection();
      RenderBodySection();

      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Html ) == RtfHtmlConvertScope.Html )
      {
        RenderEndTag();
      }
    } // RenderHtmlSection

    // ----------------------------------------------------------------------
    private void RenderHeadSection()
    {
      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Head ) != RtfHtmlConvertScope.Head )
      {
        return;
      }

      RenderHeadTag();
      RenderHeadAttributes();
      RenderTitle();
      RenderStyles();
      RenderEndTag();
    } // RenderHeadSection

    // ----------------------------------------------------------------------
    private void RenderBodySection()
    {
      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Body ) == RtfHtmlConvertScope.Body )
      {
        RenderBodyTag();
      }

      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Content ) == RtfHtmlConvertScope.Content )
      {
        RenderRtfContent();
      }

      if ( ( _settings.ConvertScope & RtfHtmlConvertScope.Body ) == RtfHtmlConvertScope.Body )
      {
        RenderEndTag();
      }
    } // RenderBodySection

    // ----------------------------------------------------------------------
    private bool EnterVisual( IRtfVisual rtfVisual )
    {
      bool openList = EnsureOpenList( rtfVisual );
      if ( openList )
      {
        return false;
      }

      EnsureClosedList( rtfVisual );
      return OnEnterVisual( rtfVisual );
    } // EnterVisual

    // ----------------------------------------------------------------------
    private void LeaveVisual( IRtfVisual rtfVisual )
    {
      OnLeaveVisual( rtfVisual );
      _lastVisual = rtfVisual;
    } // LeaveVisual

    // ----------------------------------------------------------------------
    // returns true if visual is in list
    private bool EnsureOpenList( IRtfVisual rtfVisual )
    {
      IRtfVisualText visualText = rtfVisual as IRtfVisualText;
      if ( visualText == null || !_isInParagraphNumber )
      {
        return false;
      }

      if ( !IsInList )
      {
        var unsortedList = unsortedListValues.ContainsKey( visualText.Text );
        if ( unsortedList )
        {
          RenderUlTag(); // unsorted list
        }
        else
        {
          RenderOlTag(); // ordered list
        }
      }

      RenderLiTag();

      return true;
    } // EnsureOpenList

    // ----------------------------------------------------------------------
    private void EnsureClosedList()
    {
      if ( _lastVisual == null )
      {
        return;
      }
      EnsureClosedList( _lastVisual );
    } // EnsureClosedList

    // ----------------------------------------------------------------------
    private void EnsureClosedList( IRtfVisual rtfVisual )
    {
      if ( !IsInList )
      {
        return; // not in list
      }

      IRtfVisualBreak previousParagraph = _lastVisual as IRtfVisualBreak;
      if ( previousParagraph == null || previousParagraph.BreakKind != RtfVisualBreakKind.Paragraph )
      {
        return; // is not following to a pragraph
      }

      IRtfVisualSpecialChar specialChar = rtfVisual as IRtfVisualSpecialChar;
      if ( specialChar == null ||
        specialChar.CharKind != RtfVisualSpecialCharKind.ParagraphNumberBegin )
      {
        RenderEndTag(); // close ul/ol list
      }
    } // EnsureClosedList

    // ----------------------------------------------------------------------
    // members
    private readonly RtfHtmlElementPath _elementPath = new RtfHtmlElementPath();
    private readonly IRtfDocument _rtfDocument;
    private readonly RtfHtmlConvertSettings _settings;
    private IRtfHtmlStyleConverter _styleConverter = new RtfHtmlStyleConverter();
    private XmlWriter _writer;
    private IRtfVisual _lastVisual;
    private bool _isInParagraphNumber;

    private readonly Dictionary<string, bool> unsortedListValues = new Dictionary<string, bool>()
    {
      { "·", true },
      { "•", true }
    };

    private Regex _hyperlinkRegEx;

    private static readonly ILogger logger = Logger.GetLogger( typeof( RtfHtmlConverter ) );

  } // class RtfHtmlConverter

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
