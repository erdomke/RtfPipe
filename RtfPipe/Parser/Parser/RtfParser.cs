// -- FILE ------------------------------------------------------------------
// name       : RtfParser.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.20
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe.Parser
{

  // ------------------------------------------------------------------------
  public sealed class RtfParser : RtfParserBase
  {

    // ----------------------------------------------------------------------
    public RtfParser()
    {
    } // RtfParser

    // ----------------------------------------------------------------------
    public RtfParser( params IRtfParserListener[] listeners ) :
      base( listeners )
    {
    } // RtfParser

    // ----------------------------------------------------------------------
    protected override void DoParse( IRtfSource rtfTextSource )
    {
      NotifyParseBegin();
      try
      {
        ParseRtf( rtfTextSource.Reader );
        NotifyParseSuccess();
      }
      catch ( RtfException e )
      {
        NotifyParseFail( e );
        throw;
      }
      finally
      {
        NotifyParseEnd();
      }
    } // DoParse

    // ----------------------------------------------------------------------
    private void ParseRtf( TextReader reader )
    {
      curText = new StringBuilder();

      unicodeSkipCountStack.Clear();
      codePageStack.Clear();
      unicodeSkipCount = 1;
      level = 0;
      tagCountAtLastGroupStart = 0;
      tagCount = 0;
      fontTableStartLevel = -1;
      targetFont = null;
      expectingThemeFont = false;
      fontToCodePageMapping.Clear();
      hexDecodingBuffer.SetLength( 0 );
      UpdateEncoding( RtfSpec.AnsiCodePage );
      int groupCount = 0;
      const int eof = -1;
      int nextChar = PeekNextChar( reader, false );
      bool backslashAlreadyConsumed = false;
      while ( nextChar != eof )
      {
        int peekChar = 0;
        bool peekCharValid = false;
        switch ( nextChar )
        {
          case '\\':
            if ( !backslashAlreadyConsumed )
            {
              reader.Read(); // must still consume the 'peek'ed char
            }
            int secondChar = PeekNextChar( reader, true );
            switch ( secondChar )
            {
              case '\\':
              case '{':
              case '}':
                curText.Append( ReadOneChar( reader ) ); // must still consume the 'peek'ed char
                break;

              case '\n':
              case '\r':
                reader.Read(); // must still consume the 'peek'ed char
                // must be treated as a 'par' tag if preceded by a backslash
                // (see RTF spec page 144)
                HandleTag( reader, new RtfTag( RtfSpec.TagParagraph ) );
                break;

              case '\'':
                reader.Read(); // must still consume the 'peek'ed char
                char hex1 = (char)ReadOneByte( reader );
                char hex2 = (char)ReadOneByte( reader );
                if ( !IsHexDigit( hex1 ) )
                {
                  throw new RtfHexEncodingException( Strings.InvalidFirstHexDigit( hex1 ) );
                }
                if ( !IsHexDigit( hex2 ) )
                {
                  throw new RtfHexEncodingException( Strings.InvalidSecondHexDigit( hex2 ) );
                }
                int decodedByte = Int32.Parse( "" + hex1 + hex2, NumberStyles.HexNumber );
                hexDecodingBuffer.WriteByte( (byte)decodedByte );
                peekChar = PeekNextChar( reader, false );
                peekCharValid = true;
                bool mustFlushHexContent = true;
                if ( peekChar == '\\' )
                {
                  reader.Read();
                  backslashAlreadyConsumed = true;
                  int continuationChar = PeekNextChar( reader, false );
                  if ( continuationChar == '\'' )
                  {
                    mustFlushHexContent = false;
                  }
                }
                if ( mustFlushHexContent )
                {
                  // we may _NOT_ handle hex content in a character-by-character way as
                  // this results in invalid text for japanese/chinese content ...
                  // -> we wait until the following content is non-hex and then flush the
                  //    pending data. ugly but necessary with our decoding model.
                  DecodeCurrentHexBuffer();
                }
                break;

              case '|':
              case '~':
              case '-':
              case '_':
              case ':':
              case '*':
                HandleTag( reader, new RtfTag( "" + ReadOneChar( reader ) ) ); // must still consume the 'peek'ed char
                break;

              default:
                ParseTag( reader );
                break;
            }
            break;

          case '\n':
          case '\r':
            reader.Read(); // must still consume the 'peek'ed char
            break;

          case '\t':
            reader.Read(); // must still consume the 'peek'ed char
            // should be treated as a 'tab' tag (see RTF spec page 144)
            HandleTag( reader, new RtfTag( RtfSpec.TagTabulator ) );
            break;

          case '{':
            reader.Read(); // must still consume the 'peek'ed char
            FlushText();
            NotifyGroupBegin();
            tagCountAtLastGroupStart = tagCount;
            unicodeSkipCountStack.Push( unicodeSkipCount );
            codePageStack.Push( encoding == null ? 0 : _encodings[encoding.WebName] );
            level++;
            break;

          case '}':
            reader.Read(); // must still consume the 'peek'ed char
            FlushText();
            if ( level > 0 )
            {
              unicodeSkipCount = (int)unicodeSkipCountStack.Pop();
              if ( fontTableStartLevel == level )
              {
                fontTableStartLevel = -1;
                targetFont = null;
                expectingThemeFont = false;
              }
              UpdateEncoding( (int)codePageStack.Pop() );
              level--;
              NotifyGroupEnd();
              groupCount++;
            }
            else
            {
              throw new RtfBraceNestingException( Strings.ToManyBraces );
            }
            break;

          default:
            curText.Append( ReadOneChar( reader ) ); // must still consume the 'peek'ed char
            break;
        }
        if ( level == 0 && IgnoreContentAfterRootGroup )
        {
          break;
        }
        if ( peekCharValid )
        {
          nextChar = peekChar;
        }
        else
        {
          nextChar = PeekNextChar( reader, false );
          backslashAlreadyConsumed = false;
        }
      }
      FlushText();
#if READERCLOSE
      reader.Close();
#endif

      if ( level > 0 )
      {
        throw new RtfBraceNestingException( Strings.ToFewBraces );
      }
      if ( groupCount == 0 )
      {
        throw new RtfEmptyDocumentException( Strings.NoRtfContent );
      }
      curText = null;
    } // ParseRtf

    // ----------------------------------------------------------------------
    private void ParseTag( TextReader reader )
    {
      StringBuilder tagName = new StringBuilder();
      StringBuilder tagValue = null;
      bool readingName = true;
      bool delimReached = false;

      int nextChar = PeekNextChar( reader, true );
      while ( !delimReached )
      {
        if ( readingName && IsASCIILetter( nextChar ) )
        {
          tagName.Append( ReadOneChar( reader ) ); // must still consume the 'peek'ed char
        }
        else if ( IsDigit( nextChar ) || (nextChar == '-' && tagValue == null) )
        {
          readingName = false;
          if ( tagValue == null )
          {
            tagValue = new StringBuilder();
          }
          tagValue.Append( ReadOneChar( reader ) ); // must still consume the 'peek'ed char
        }
        else
        {
          delimReached = true;
          IRtfTag newTag;
          if ( tagValue != null && tagValue.Length > 0 )
          {
            newTag = new RtfTag( tagName.ToString(), tagValue.ToString() );
          }
          else
          {
            newTag = new RtfTag( tagName.ToString() );
          }
          bool skippedContent = HandleTag( reader, newTag );
          if ( nextChar == ' ' && !skippedContent )
          {
            reader.Read(); // must still consume the 'peek'ed char
          }
        }
        if ( !delimReached )
        {
          nextChar = PeekNextChar( reader, true );
        }
      }
    } // ParseTag

    // ----------------------------------------------------------------------
    private bool HandleTag( TextReader reader, IRtfTag tag )
    {
      if ( level == 0 )
      {
        throw new RtfStructureException( Strings.TagOnRootLevel( tag.ToString() ) );
      }

      if ( tagCount < 4 )
      {
        // this only handles the initial encoding tag in the header section
        UpdateEncoding( tag );
      }

      string tagName = tag.Name;
      // enable the font name detection in case the last tag was introducing
      // a theme font
      bool detectFontName = expectingThemeFont;
      if ( tagCountAtLastGroupStart == tagCount )
      {
        // first tag in a group
        switch ( tagName )
        {
          case RtfSpec.TagThemeFontLoMajor:
          case RtfSpec.TagThemeFontHiMajor:
          case RtfSpec.TagThemeFontDbMajor:
          case RtfSpec.TagThemeFontBiMajor:
          case RtfSpec.TagThemeFontLoMinor:
          case RtfSpec.TagThemeFontHiMinor:
          case RtfSpec.TagThemeFontDbMinor:
          case RtfSpec.TagThemeFontBiMinor:
            // these introduce a new font, but the actual font tag will be
            // the second tag in the group, so we must remember this condition ...
            expectingThemeFont = true;
            break;
        }
        // always enable the font name detection also for the first tag in a group
        detectFontName = true;
      }
      if ( detectFontName )
      {
        // first tag in a group:
        switch ( tagName )
        {
          case RtfSpec.TagFont:
            if ( fontTableStartLevel > 0 )
            {
              // in the font-table definition:
              // -> remember the target font for charset mapping
              targetFont = tag.FullName;
              expectingThemeFont = false; // reset that state now
            }
            break;
          case RtfSpec.TagFontTable:
            // -> remember we're in the font-table definition
            fontTableStartLevel = level;
            break;
        }
      }
      if ( targetFont != null )
      {
        // within a font-tables font definition: perform charset mapping
        if ( RtfSpec.TagFontCharset.Equals( tagName ) )
        {
          int charSet = tag.ValueAsNumber;
          int codePage = RtfSpec.GetCodePage( charSet );
          fontToCodePageMapping[ targetFont ] = codePage;
          UpdateEncoding( codePage );
        }
      }
      if ( fontToCodePageMapping.Count > 0 && RtfSpec.TagFont.Equals( tagName ) )
      {
        int codePage;
        if (fontToCodePageMapping.TryGetValue(tag.FullName, out codePage))
        {
          UpdateEncoding( codePage );
        }
      }

      bool skippedContent = false;
      switch ( tagName )
      {
        case RtfSpec.TagUnicodeCode:
          int unicodeValue = tag.ValueAsNumber;
          char unicodeChar = (char)unicodeValue;
          curText.Append( unicodeChar );
          // skip over the indicated number of 'alternative representation' text
          for ( int i = 0; i < unicodeSkipCount; i++ )
          {
            int nextChar = PeekNextChar( reader, true );
            switch ( nextChar )
            {
              case ' ':
              case '\r':
              case '\n':
                reader.Read(); // consume peeked char
                skippedContent = true;
                if ( i == 0 )
                {
                  // the first whitespace after the tag
                  // -> only a delimiter, doesn't count for skipping ...
                  i--;
                }
                break;
              case '\\':
                reader.Read(); // consume peeked char
                skippedContent = true;
                int secondChar = ReadOneByte( reader ); // mandatory
                switch ( secondChar )
                {
                  case '\'':
                    // ok, this is a hex-encoded 'byte' -> need to consume both
                    // hex digits too
                    ReadOneByte( reader ); // high nibble
                    ReadOneByte( reader ); // low nibble
                    break;
                }
                break;
              case '{':
              case '}':
                // don't consume peeked char and abort skipping
                i = unicodeSkipCount;
                break;
              default:
                reader.Read(); // consume peeked char
                skippedContent = true;
                break;
            }
          }
          break;

        case RtfSpec.TagUnicodeSkipCount:
          int newSkipCount = tag.ValueAsNumber;
          if ( newSkipCount < 0 || newSkipCount > 10 )
          {
            throw new RtfUnicodeEncodingException( Strings.InvalidUnicodeSkipCount( tag.ToString() ) );
          }
          unicodeSkipCount = newSkipCount;
          break;

        default:
          FlushText();
          NotifyTagFound( tag );
          break;
      }

      tagCount++;

      return skippedContent;
    } // HandleTag

    // ----------------------------------------------------------------------
    private void UpdateEncoding( IRtfTag tag )
    {
      switch ( tag.Name )
      {
        case RtfSpec.TagEncodingAnsi:
          UpdateEncoding( RtfSpec.AnsiCodePage );
          break;
        case RtfSpec.TagEncodingMac:
          UpdateEncoding( 10000 );
          break;
        case RtfSpec.TagEncodingPc:
          UpdateEncoding( 437 );
          break;
        case RtfSpec.TagEncodingPca:
          UpdateEncoding( 850 );
          break;
        case RtfSpec.TagEncodingAnsiCodePage:
          UpdateEncoding( tag.ValueAsNumber );
          break;
      }
    } // UpdateEncoding

    // ----------------------------------------------------------------------
    private void UpdateEncoding( int codePage )
    {
      if ( encoding == null || codePage != _encodings[encoding.WebName] )
      {
        switch ( codePage )
        {
          case RtfSpec.AnsiCodePage:
          case RtfSpec.SymbolFakeCodePage: // hack to handle a windows legacy ...
            encoding = RtfSpec.AnsiEncoding;
            break;
          default:
            encoding = Encoding.GetEncoding( _codePages[codePage] );
            break;
        }
        byteToCharDecoder = null;
      }
      if ( byteToCharDecoder == null )
      {
        byteToCharDecoder = encoding.GetDecoder();
      }
    } // UpdateEncoding

    // ----------------------------------------------------------------------
    private static bool IsASCIILetter( int character )
    {
      return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
    } // IsASCIILetter

    // ----------------------------------------------------------------------
    private static bool IsHexDigit( int character )
    {
      return (character >= '0' && character <= '9') ||
             (character >= 'a' && character <= 'f') ||
             (character >= 'A' && character <= 'F');
    } // IsHexDigit

    // ----------------------------------------------------------------------
    private static bool IsDigit( int character )
    {
      return character >= '0' && character <= '9';
    } // IsDigit

    // ----------------------------------------------------------------------
    private static int ReadOneByte( TextReader reader )
    {
      int byteValue = reader.Read();
      if ( byteValue == -1 )
      {
        throw new RtfUnicodeEncodingException( Strings.UnexpectedEndOfFile );
      }
      return byteValue;
    } // ReadOneByte

    // ----------------------------------------------------------------------
    private char ReadOneChar( TextReader reader )
    {
      // NOTE: the handling of multi-byte encodings is probably not the most
      // efficient here ...

      bool completed = false;
      int byteIndex = 0;
      while ( !completed )
      {
        byteDecodingBuffer[ byteIndex ] = (byte)ReadOneByte( reader );
        byteIndex++;
        int usedBytes;
        int usedChars;
        byteToCharDecoder.Convert(
          byteDecodingBuffer, 0, byteIndex,
          charDecodingBuffer, 0, 1,
          true,
          out usedBytes,
          out usedChars,
          out completed );
        if ( completed && ( usedBytes != byteIndex || usedChars != 1 ) )
        {
          throw new RtfMultiByteEncodingException( Strings.InvalidMultiByteEncoding( 
          byteDecodingBuffer, byteIndex, encoding ) );
        }
      }
      char character = charDecodingBuffer[ 0 ];
      return character;
    } // ReadOneChar

    // ----------------------------------------------------------------------
    private void DecodeCurrentHexBuffer()
    {
      long pendingByteCount = hexDecodingBuffer.Length;
      if ( pendingByteCount > 0 )
      {
        byte[] pendingBytes = hexDecodingBuffer.ToArray();
        char[] convertedChars = new char[ pendingByteCount ]; // should be enough

        int startIndex = 0;
        bool completed = false;
        while ( !completed && startIndex < pendingBytes.Length )
        {
          int usedBytes;
          int usedChars;
          byteToCharDecoder.Convert(
            pendingBytes, startIndex, pendingBytes.Length - startIndex,
            convertedChars, 0, convertedChars.Length,
            true,
            out usedBytes,
            out usedChars,
            out completed );
          curText.Append( convertedChars, 0, usedChars );
          startIndex += usedChars;
        }

        hexDecodingBuffer.SetLength( 0 );
      }
    } // DecodeCurrentHexBuffer

    // ----------------------------------------------------------------------
// ReSharper disable UnusedParameter.Local
    private static int PeekNextChar( TextReader reader, bool mandatory )
// ReSharper restore UnusedParameter.Local
    {
      int character = reader.Peek();
      if ( mandatory && character == -1 )
      {
        throw new RtfMultiByteEncodingException( Strings.EndOfFileInvalidCharacter );
      }
      return character;
    } // PeekNextChar

    // ----------------------------------------------------------------------
    private void FlushText()
    {
      if ( curText.Length > 0 )
      {
        if ( level == 0 )
        {
          throw new RtfStructureException( Strings.TextOnRootLevel( curText.ToString() ) );
        }
        NotifyTextFound( new RtfText( curText.ToString() ) );
        curText.Remove( 0, curText.Length );
      }
    } // FlushText

    // ----------------------------------------------------------------------
    // members
    private StringBuilder curText;
    private readonly Stack<int> unicodeSkipCountStack = new Stack<int>();
    private int unicodeSkipCount;
    private readonly Stack<int> codePageStack = new Stack<int>();
    private int level;
    private int tagCountAtLastGroupStart;
    private int tagCount;
    private int fontTableStartLevel;
    private string targetFont;
    private bool expectingThemeFont;
    private readonly Dictionary<string, int> fontToCodePageMapping = new Dictionary<string, int>();
    private Encoding encoding;
    private Decoder byteToCharDecoder;
    private readonly MemoryStream hexDecodingBuffer = new MemoryStream();
    private readonly byte[] byteDecodingBuffer = new byte[ 8 ]; // >0 for multi-byte encodings
    private readonly char[] charDecodingBuffer = new char[ 1 ];


    internal static readonly Dictionary<int, string> _codePages = new Dictionary<int, string>()
    {
      {37, "IBM037"},
      {437, "IBM437"},
      {500, "IBM500"},
      {708, "ASMO-708"},
      {720, "DOS-720"},
      {737, "ibm737"},
      {775, "ibm775"},
      {850, "ibm850"},
      {852, "ibm852"},
      {855, "IBM855"},
      {857, "ibm857"},
      {858, "IBM00858"},
      {860, "IBM860"},
      {861, "ibm861"},
      {862, "DOS-862"},
      {863, "IBM863"},
      {864, "IBM864"},
      {865, "IBM865"},
      {866, "cp866"},
      {869, "ibm869"},
      {870, "IBM870"},
      {874, "windows-874"},
      {875, "cp875"},
      {932, "shift_jis"},
      {936, "gb2312"},
      {949, "ks_c_5601-1987"},
      {950, "big5"},
      {1026, "IBM1026"},
      {1047, "IBM01047"},
      {1140, "IBM01140"},
      {1141, "IBM01141"},
      {1142, "IBM01142"},
      {1143, "IBM01143"},
      {1144, "IBM01144"},
      {1145, "IBM01145"},
      {1146, "IBM01146"},
      {1147, "IBM01147"},
      {1148, "IBM01148"},
      {1149, "IBM01149"},
      {1200, "utf-16"},
      {1201, "utf-16BE"},
      {1250, "windows-1250"},
      {1251, "windows-1251"},
      {1252, "Windows-1252"},
      {1253, "windows-1253"},
      {1254, "windows-1254"},
      {1255, "windows-1255"},
      {1256, "windows-1256"},
      {1257, "windows-1257"},
      {1258, "windows-1258"},
      {1361, "Johab"},
      {10000, "macintosh"},
      {10001, "x-mac-japanese"},
      {10002, "x-mac-chinesetrad"},
      {10003, "x-mac-korean"},
      {10004, "x-mac-arabic"},
      {10005, "x-mac-hebrew"},
      {10006, "x-mac-greek"},
      {10007, "x-mac-cyrillic"},
      {10008, "x-mac-chinesesimp"},
      {10010, "x-mac-romanian"},
      {10017, "x-mac-ukrainian"},
      {10021, "x-mac-thai"},
      {10029, "x-mac-ce"},
      {10079, "x-mac-icelandic"},
      {10081, "x-mac-turkish"},
      {10082, "x-mac-croatian"},
      {12000, "utf-32"},
      {12001, "utf-32BE"},
      {20000, "x-Chinese-CNS"},
      {20001, "x-cp20001"},
      {20002, "x-Chinese-Eten"},
      {20003, "x-cp20003"},
      {20004, "x-cp20004"},
      {20005, "x-cp20005"},
      {20105, "x-IA5"},
      {20106, "x-IA5-German"},
      {20107, "x-IA5-Swedish"},
      {20108, "x-IA5-Norwegian"},
      {20127, "us-ascii"},
      {20261, "x-cp20261"},
      {20269, "x-cp20269"},
      {20273, "IBM273"},
      {20277, "IBM277"},
      {20278, "IBM278"},
      {20280, "IBM280"},
      {20284, "IBM284"},
      {20285, "IBM285"},
      {20290, "IBM290"},
      {20297, "IBM297"},
      {20420, "IBM420"},
      {20423, "IBM423"},
      {20424, "IBM424"},
      {20833, "x-EBCDIC-KoreanExtended"},
      {20838, "IBM-Thai"},
      {20866, "koi8-r"},
      {20871, "IBM871"},
      {20880, "IBM880"},
      {20905, "IBM905"},
      {20924, "IBM00924"},
      {20932, "EUC-JP"},
      {20936, "x-cp20936"},
      {20949, "x-cp20949"},
      {21025, "cp1025"},
      {21866, "koi8-u"},
      {28591, "iso-8859-1"},
      {28592, "iso-8859-2"},
      {28593, "iso-8859-3"},
      {28594, "iso-8859-4"},
      {28595, "iso-8859-5"},
      {28596, "iso-8859-6"},
      {28597, "iso-8859-7"},
      {28598, "iso-8859-8"},
      {28599, "iso-8859-9"},
      {28603, "iso-8859-13"},
      {28605, "iso-8859-15"},
      {29001, "x-Europa"},
      {38598, "iso-8859-8-i"},
      {50220, "iso-2022-jp"},
      {50221, "csISO2022JP"},
      {50222, "iso-2022-jp"},
      {50225, "iso-2022-kr"},
      {50227, "x-cp50227"},
      {51932, "euc-jp"},
      {51936, "EUC-CN"},
      {51949, "euc-kr"},
      {52936, "hz-gb-2312"},
      {54936, "GB18030"},
      {57002, "x-iscii-de"},
      {57003, "x-iscii-be"},
      {57004, "x-iscii-ta"},
      {57005, "x-iscii-te"},
      {57006, "x-iscii-as"},
      {57007, "x-iscii-or"},
      {57008, "x-iscii-ka"},
      {57009, "x-iscii-ma"},
      {57010, "x-iscii-gu"},
      {57011, "x-iscii-pa"},
      {65000, "utf-7"},
      {65001, "utf-8"}
    };
    internal static readonly Dictionary<string, int> _encodings = new Dictionary<string, int>();

    static RtfParser()
    {
      foreach (var kvp in _codePages)
      {
        _encodings[kvp.Value] = kvp.Key;
      }
    }

  } // class RtfParser

} // namespace RtfPipe.Parser
// -- EOF -------------------------------------------------------------------
