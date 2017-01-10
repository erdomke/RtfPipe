// -- FILE ------------------------------------------------------------------
// name       : Strings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2009.02.19
// language   : c#
// environment: .NET 3.5
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Resources;
using Itenso.Sys;
using System.Text;
using System.Globalization;

namespace Itenso.Rtf
{

  // ------------------------------------------------------------------------
  /// <summary>Provides strongly typed resource access for this namespace.</summary>
  internal sealed class Strings : StringsBase
  {
    // ----------------------------------------------------------------------
    public static string InvalidFirstHexDigit(char hexDigit)
    {
      return Format(inst.GetString("InvalidFirstHexDigit"), hexDigit);
    } // InvalidFirstHexDigit

    // ----------------------------------------------------------------------
    public static string InvalidSecondHexDigit(char hexDigit)
    {
      return Format(inst.GetString("InvalidSecondHexDigit"), hexDigit);
    } // InvalidSecondHexDigit

    // ----------------------------------------------------------------------
    public static string ToManyBraces
    {
      get { return inst.GetString("ToManyBraces"); }
    } // ToManyBraces

    // ----------------------------------------------------------------------
    public static string ToFewBraces
    {
      get { return inst.GetString("ToFewBraces"); }
    } // ToFewBraces

    // ----------------------------------------------------------------------
    public static string NoRtfContent
    {
      get { return inst.GetString("NoRtfContent"); }
    } // NoRtfContent

    // ----------------------------------------------------------------------
    public static string TagOnRootLevel(string tagName)
    {
      return Format(inst.GetString("TagOnRootLevel"), tagName);
    } // TagOnRootLevel

    // ----------------------------------------------------------------------
    public static string InvalidUnicodeSkipCount(string tagName)
    {
      return Format(inst.GetString("InvalidUnicodeSkipCount"), tagName);
    } // InvalidUnicodeSkipCount

    // ----------------------------------------------------------------------
    public static string UnexpectedEndOfFile
    {
      get { return inst.GetString("UnexpectedEndOfFile"); }
    } // UnexpectedEndOfFile

    // ----------------------------------------------------------------------
    public static string InvalidMultiByteEncoding(byte[] buffer, int index, Encoding encoding)
    {
      var buf = new StringBuilder();
      for (int i = 0; i < index; i++)
      {
        buf.Append(string.Format(
          CultureInfo.InvariantCulture,
          "{0:X}",
          buffer[i]));
      }
      return Format(inst.GetString("InvalidMultiByteEncoding"),
        buf.ToString(), encoding.EncodingName, encoding.CodePage);
    } // InvalidMultiByteEncoding

    // ----------------------------------------------------------------------
    public static string EndOfFileInvalidCharacter
    {
      get { return inst.GetString("EndOfFileInvalidCharacter"); }
    } // EndOfFileInvalidCharacter

    // ----------------------------------------------------------------------
    public static string TextOnRootLevel(string text)
    {
      return Format(inst.GetString("TextOnRootLevel"), text);
    } // TextOnRootLevel

    // ----------------------------------------------------------------------
    public static string MissingGroupForNewTag
    {
      get { return inst.GetString("MissingGroupForNewTag"); }
    } // MissingGroupForNewTag

    // ----------------------------------------------------------------------
    public static string MissingGroupForNewText
    {
      get { return inst.GetString("MissingGroupForNewText"); }
    } // MissingGroupForNewText

    // ----------------------------------------------------------------------
    public static string MultipleRootLevelGroups
    {
      get { return inst.GetString("MultipleRootLevelGroups"); }
    } // MultipleRootLevelGroups

    // ----------------------------------------------------------------------
    public static string UnclosedGroups
    {
      get { return inst.GetString("UnclosedGroups"); }
    } // UnclosedGroups

    // ----------------------------------------------------------------------
    public static string LogGroupBegin
    {
      get { return inst.GetString("LogGroupBegin"); }
    } // LogGroupBegin

    // ----------------------------------------------------------------------
    public static string LogGroupEnd
    {
      get { return inst.GetString("LogGroupEnd"); }
    } // LogGroupEnd

    // ----------------------------------------------------------------------
    public static string LogOverflowText
    {
      get { return inst.GetString("LogOverflowText"); }
    } // LogOverflowText

    // ----------------------------------------------------------------------
    public static string LogParseBegin
    {
      get { return inst.GetString("LogParseBegin"); }
    } // LogParseBegin

    // ----------------------------------------------------------------------
    public static string LogParseEnd
    {
      get { return inst.GetString("LogParseEnd"); }
    } // LogParseEnd

    // ----------------------------------------------------------------------
    public static string LogParseFail
    {
      get { return inst.GetString("LogParseFail"); }
    } // LogParseFail

    // ----------------------------------------------------------------------
    public static string LogParseFailUnknown
    {
      get { return inst.GetString("LogParseFailUnknown"); }
    } // LogParseFailUnknown

    // ----------------------------------------------------------------------
    public static string LogParseSuccess
    {
      get { return inst.GetString("LogParseSuccess"); }
    } // LogParseSuccess

    // ----------------------------------------------------------------------
    public static string LogTag
    {
      get { return inst.GetString("LogTag"); }
    } // LogTag

    // ----------------------------------------------------------------------
    public static string LogText
    {
      get { return inst.GetString("LogText"); }
    } // LogText

      // ----------------------------------------------------------------------
    public static string ColorTableUnsupportedText( string text )
    {
      return Format( inst.GetString( "ColorTableUnsupportedText" ), text );
    } // ColorTableUnsupportedText

    // ----------------------------------------------------------------------
    public static string DuplicateFont( string fontId )
    {
      return Format( inst.GetString( "DuplicateFont" ), fontId );
    } // DuplicateFont

    // ----------------------------------------------------------------------
    public static string EmptyDocument
    {
      get { return inst.GetString( "EmptyDocument" ); }
    } // EmptyDocument

    // ----------------------------------------------------------------------
    public static string MissingDocumentStartTag
    {
      get { return inst.GetString( "MissingDocumentStartTag" ); }
    } // MissingDocumentStartTag

    // ----------------------------------------------------------------------
    public static string InvalidDocumentStartTag( string expectedTag )
    {
      return Format( inst.GetString( "InvalidDocumentStartTag" ), expectedTag );
    } // InvalidDocumentStartTag

    // ----------------------------------------------------------------------
    public static string MissingRtfVersion
    {
      get { return inst.GetString( "MissingRtfVersion" ); }
    } // MissingRtfVersion

    // ----------------------------------------------------------------------
    public static string InvalidInitTagState( string tag )
    {
      return Format( inst.GetString( "InvalidInitTagState" ), tag );
    } // InvalidInitTagState

    // ----------------------------------------------------------------------
    public static string UndefinedFont( string fontId )
    {
      return Format( inst.GetString( "UndefinedFont" ), fontId );
    } // UndefinedFont

    // ----------------------------------------------------------------------
    public static string InvalidFontSize( int fontSize )
    {
      return Format( inst.GetString( "InvalidFontSize" ), fontSize );
    } // InvalidFontSize

    // ----------------------------------------------------------------------
    public static string UndefinedColor( int colorIndex )
    {
      return Format( inst.GetString( "UndefinedColor" ), colorIndex );
    } // UndefinedColor

    // ----------------------------------------------------------------------
    public static string InvalidInitGroupState( string group )
    {
      return Format( inst.GetString( "InvalidInitGroupState" ), group );
    } // InvalidInitGroupState

    // ----------------------------------------------------------------------
    public static string InvalidGeneratorGroup( string group )
    {
      return Format( inst.GetString( "InvalidGeneratorGroup" ), group );
    } // InvalidGeneratorGroup

    // ----------------------------------------------------------------------
    public static string InvalidInitTextState( string text )
    {
      return Format( inst.GetString( "InvalidInitTextState" ), text );
    } // InvalidInitTextState	

    // ----------------------------------------------------------------------
    public static string InvalidDefaultFont( string fontId, string allowedFontIds )
    {
      return Format( inst.GetString( "InvalidDefaultFont" ), fontId, allowedFontIds );
    } // InvalidDefaultFont

    // ----------------------------------------------------------------------
    public static string InvalidTextContextState
    {
      get { return inst.GetString( "InvalidTextContextState" ); }
    } // InvalidTextContextState

    // ----------------------------------------------------------------------
    public static string UnsupportedRtfVersion( int version )
    {
      return Format( inst.GetString( "UnsupportedRtfVersion" ), version );
    } // UnsupportedRtfVersion

    // ----------------------------------------------------------------------
    public static string ImageFormatText
    {
      get { return inst.GetString( "ImageFormatText" ); }
    } // ImageFormatText

    // ----------------------------------------------------------------------
    public static string LogBeginDocument
    {
      get { return inst.GetString( "LogBeginDocument" ); }
    } // LogBeginDocument

    // ----------------------------------------------------------------------
    public static string LogEndDocument
    {
      get { return inst.GetString( "LogEndDocument" ); }
    } // LogEndDocument

    // ----------------------------------------------------------------------
    public static string LogInsertBreak
    {
      get { return inst.GetString( "LogInsertBreak" ); }
    } // LogInsertBreak
    
    // ----------------------------------------------------------------------
    public static string LogInsertChar
    {
      get { return inst.GetString( "LogInsertChar" ); }
    } // LogInsertChar
    
    // ----------------------------------------------------------------------
    public static string LogInsertImage
    {
      get { return inst.GetString( "LogInsertImage" ); }
    } // LogInsertImage
    
    // ----------------------------------------------------------------------
    public static string LogInsertText
    {
      get { return inst.GetString( "LogInsertText" ); }
    } // LogInsertText

    // ----------------------------------------------------------------------
    public static string InvalidColor( int color )
    {
      return Format( inst.GetString( "InvalidColor" ), color );
    } // InvalidColor

    // ----------------------------------------------------------------------
    public static string InvalidCharacterSet( int charSet )
    {
      return Format( inst.GetString( "InvalidCharacterSet" ), charSet );
    } // InvalidCharacterSet

    // ----------------------------------------------------------------------
    public static string InvalidCodePage( int codePage )
    {
      return Format( inst.GetString( "InvalidCodePage" ), codePage );
    } // InvalidCodePage

    // ----------------------------------------------------------------------
    public static string FontSizeOutOfRange( int fontSize )
    {
      return Format( inst.GetString( "FontSizeOutOfRange" ), fontSize );
    } // FontSizeOutOfRange

    // ----------------------------------------------------------------------
    public static string InvalidImageWidth( int width )
    {
      return Format( inst.GetString( "InvalidImageWidth" ), width );
    } // InvalidImageWidth

    // ----------------------------------------------------------------------
    public static string InvalidImageHeight( int height )
    {
      return Format( inst.GetString( "InvalidImageHeight" ), height );
    } // InvalidImageHeight

    // ----------------------------------------------------------------------
    public static string InvalidImageDesiredHeight( int width )
    {
      return Format( inst.GetString( "InvalidImageDesiredHeight" ), width );
    } // InvalidImageDesiredHeight

    // ----------------------------------------------------------------------
    public static string InvalidImageDesiredWidth( int height )
    {
      return Format( inst.GetString( "InvalidImageDesiredWidth" ), height );
    } // InvalidImageDesiredWidth

    // ----------------------------------------------------------------------
    public static string InvalidImageScaleWidth( int scaleWidth )
    {
      return Format( inst.GetString( "InvalidImageScaleWidth" ), scaleWidth );
    } // InvalidImageScaleWidth

    // ----------------------------------------------------------------------
    public static string InvalidImageScaleHeight( int scaleHeight )
    {
      return Format( inst.GetString( "InvalidImageScaleHeight" ), scaleHeight );
    } // InvalidImageScaleHeight

    // ----------------------------------------------------------------------
    // members
    private static readonly ResourceManager inst = NewInst( typeof( Strings ) );

  } // class Strings

} // namespace Itenso.Rtf
// -- EOF -------------------------------------------------------------------
