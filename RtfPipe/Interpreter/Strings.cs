// environment: .NET 3.5
using System.Resources;
using RtfPipe.Sys;
using System.Text;
using System.Globalization;

namespace RtfPipe
{


  /// <summary>Provides strongly typed resource access for this namespace.</summary>
  internal sealed class Strings : StringsBase
  {
    public static string InvalidFirstHexDigit(char hexDigit)
    {
      return Format("invalid first hex digit after \\{0}", hexDigit);
    }

    public static string InvalidSecondHexDigit(char hexDigit)
    {
      return Format("invalid second hex digit after \\{0}", hexDigit);
    }

    public static string ToManyBraces
    {
      get { return "improper nesting of braces: too many"; }
    }

    public static string ToFewBraces
    {
      get { return "improper nesting of braces: too few"; }
    }

    public static string NoRtfContent
    {
      get { return "no rtf content"; }
    }

    public static string TagOnRootLevel(string tagName)
    {
      return Format("a tag cannot appear on root level, must be child of a group: {0}", tagName);
    }

    public static string InvalidUnicodeSkipCount(string tagName)
    {
      return Format("invalid unicode skip count: {0}", tagName);
    }

    public static string UnexpectedEndOfFile
    {
      get { return "unexpected end of file"; }
    }

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
      return Format("could not decode bytes 0x{0} to character with encoding {1} (from codepage {2})",
        buf.ToString(), encoding.WebName, Parser.RtfParser._encodings[encoding.WebName]);
    }

    public static string EndOfFileInvalidCharacter
    {
      get { return "unexpected end of file while examining next character"; }
    }

    public static string TextOnRootLevel(string text)
    {
      return Format("a text cannot appear on root level, must be child of a group: '{0}'", text);
    }

    public static string MissingGroupForNewTag
    {
      get { return "invalid state: no group available yet for adding a tag"; }
    }

    public static string MissingGroupForNewText
    {
      get { return "invalid state: no group available yet for adding a text"; }
    }

    public static string MultipleRootLevelGroups
    {
      get { return "invalid state: multiple root level groups"; }
    }

    public static string UnclosedGroups
    {
      get { return "invalid state: unclosed groups"; }
    }

    public static string LogGroupBegin
    {
      get { return "GroupBegin"; }
    }

    public static string LogGroupEnd
    {
      get { return "GroupEnd"; }
    }

    public static string LogOverflowText
    {
      get { return "..."; }
    }

    public static string LogParseBegin
    {
      get { return "ParseBegin"; }
    }

    public static string LogParseEnd
    {
      get { return "ParseEnd"; }
    }

    public static string LogParseFail
    {
      get { return "ParseFail: {0}"; }
    }

    public static string LogParseFailUnknown
    {
      get { return "ParseFail: unknown reason"; }
    }

    public static string LogParseSuccess
    {
      get { return "ParseSuccess"; }
    }

    public static string LogTag
    {
      get { return "Tag: {0}"; }
    }

    public static string LogText
    {
      get { return "Text: {0}"; }
    }

      public static string ColorTableUnsupportedText( string text )
    {
      return Format("unsupported text in color table: '{0}'", text );
    }

    public static string DuplicateFont( string fontId )
    {
      return Format("duplicate font id '{0}'", fontId );
    }

    public static string EmptyDocument
    {
      get { return "document has not contents"; }
    }

    public static string MissingDocumentStartTag
    {
      get { return "first element in document is not a tag"; }
    }

    public static string InvalidDocumentStartTag( string expectedTag )
    {
      return Format("first tag in document is not {0}", expectedTag );
    }

    public static string MissingRtfVersion
    {
      get { return "unspecified RTF version"; }
    }

    public static string InvalidInitTagState( string tag )
    {
      return Format("Init: illegal state for tag '{0}'", tag );
    }

    public static string UndefinedFont( string fontId )
    {
      return Format("undefined font: {0}", fontId );
    }

    public static string InvalidFontSize( int fontSize )
    {
      return Format("invalid font size", fontSize );
    }

    public static string UndefinedColor( int colorIndex )
    {
      return Format("undefined color index: {0}", colorIndex );
    }

    public static string InvalidInitGroupState( string group )
    {
      return Format("Init: illegal state for group: '{0}'", group );
    }

    public static string InvalidGeneratorGroup( string group )
    {
      return Format("invalid generator group: {0}", group );
    }

    public static string InvalidInitTextState( string text )
    {
      return Format("Init: illegal state for text: '{0}'", text );
    }	

    public static string InvalidDefaultFont( string fontId, string allowedFontIds )
    {
      return Format("invalid default font id '{0}', only the following fonts are available (yet): {1}", fontId, allowedFontIds );
    }

    public static string InvalidTextContextState
    {
      get { return "illegal state: cannot pop text format from empty stack"; }
    }

    public static string UnsupportedRtfVersion( int version )
    {
      return Format("unsupported RTF version: {0}", version );
    }

    public static string ImageFormatText
    {
      get { return "[{0}:{1} x {2}]"; }
    }

    public static string LogBeginDocument
    {
      get { return "BeginDocument"; }
    }

    public static string LogEndDocument
    {
      get { return "EndDocument"; }
    }

    public static string LogInsertBreak
    {
      get { return "InsertBreak: {0}"; }
    }
    
    public static string LogInsertChar
    {
      get { return "InsertChar: {0}"; }
    }
    
    public static string LogInsertImage
    {
      get { return "InsertImage: {0}: {1} x {2}, desired: {3} x {4}, scaled: {5}% x {6}%, {8} bytes"; }
    }
    
    public static string LogInsertText
    {
      get { return "InsertText: '{0}' with format [{1}]"; }
    }

    public static string InvalidColor( int color )
    {
      return Format("invalid color component, must be in the range [0..255], but is {0}", color );
    }

    public static string InvalidCharacterSet( int charSet )
    {
      return Format("charset may not be negative but is {0}", charSet );
    }

    public static string InvalidCodePage( int codePage )
    {
      return Format("code page may not be negative but is {0}", codePage );
    }

    public static string FontSizeOutOfRange( int fontSize )
    {
      return Format("invalid font size, must be in the range [1..0xFFFF], but is {0}", fontSize );
    }

    public static string InvalidImageWidth( int width )
    {
      return Format("image width must be > 0 but is {0}", width );
    }

    public static string InvalidImageHeight( int height )
    {
      return Format("image height must be > 0 but is {0}", height );
    }

    public static string InvalidImageDesiredHeight( int width )
    {
      return Format("desiredHeight must be > 0 but is {0}", width );
    }

    public static string InvalidImageDesiredWidth( int height )
    {
      return Format("image desiredWidth must be > 0 but is {0}", height );
    }

    public static string InvalidImageScaleWidth( int scaleWidth )
    {
      return Format("image scaleWidthPercent must be > 0 but is {0}", scaleWidth );
    }

    public static string InvalidImageScaleHeight( int scaleHeight )
    {
      return Format("scaleHeightPercent must be > 0 but is {0}", scaleHeight );
    }
    
  }

}

