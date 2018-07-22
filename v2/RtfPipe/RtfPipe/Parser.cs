using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  public class Parser
  {
    private readonly TextReader _reader;
    private readonly StringBuilder _controlBuffer = new StringBuilder();
    private readonly StringBuffer _textBuffer = new StringBuffer();
    private readonly Stack<EncodingContext> _context = new Stack<EncodingContext>();
    private int _ignoreDepth = int.MaxValue;
    private readonly Document _document = new Document();

    private int Depth { get { return _context.Count; } }

    public Parser(TextReader reader)
    {
      _reader = reader;
    }

    public Document Parse()
    {
      var groups = new Stack<Group>();
      var infoGroup = default(Group);

      foreach (var token in Tokens())
      {
        if (token is Group group)
        {
          if (groups.Count < 1)
          {
            groups.Push(_document);
          }
          else
          {
            groups.Peek().Contents.Add(group);
            groups.Push(group);
          }
        }
        else if (token is GroupEnd)
        {
          groups.Pop();
        }
        else
        {
          groups.Peek().Contents.Add(token);
          if (token is Info)
            infoGroup = groups.Peek();
        }
      }

      if (infoGroup != null)
        ParseInfo(_document, infoGroup);

      return _document;
    }

    private void ParseInfo(Document document, Group info)
    {
      foreach (var item in info.Contents.Skip(1))
      {
        if (item is Group group)
        {
          if (group.Contents.Count == 2
            && group.Contents[1] is TextToken txt)
          {
            document.Information[group.Contents[0]] = txt.Value;
          }
          else if (group.Contents.Count > 1
            && group.Contents[1] is Year yr)
          {
            var date = new DateTime(
              yr.Value,
              group.Contents.OfType<Month>().FirstOrDefault()?.Value ?? 1,
              group.Contents.OfType<Day>().FirstOrDefault()?.Value ?? 1,
              group.Contents.OfType<Hour>().FirstOrDefault()?.Value ?? 0,
              group.Contents.OfType<Minute>().FirstOrDefault()?.Value ?? 0,
              group.Contents.OfType<Second>().FirstOrDefault()?.Value ?? 0);
            document.Information[group.Contents[0]] = date;
          }
          else if (group.Contents.Count == 1
            && group.Contents[0] is ControlWord<int> intWord)
          {
            document.Information[group.Contents[0]] = intWord.Value;
          }
        }
        else if (item is ControlWord<int> intWord2)
        {
          document.Information[item] = intWord2.Value;
        }
      }
    }

    public IEnumerable<IToken> Tokens()
    {
      var curr = -1;
      while ((curr = _reader.Read()) > 0)
      {
        if (Depth >= _ignoreDepth)
        {
          switch ((char)curr)
          {
            case '\\':
              _reader.Read();
              break;
            case '{':
              _context.Push(new EncodingContext());
              break;
            case '}':
              _context.Pop();
              if (Depth < _ignoreDepth)
                _ignoreDepth = int.MaxValue;
              break;
          }
        }
        else
        {
          switch ((char)curr)
          {
            case '\\':
              switch (_reader.Peek())
              {
                case '\\':
                case '{':
                case '}':
                  _textBuffer.Append(_reader.Read());
                  break;

                case '\n':
                case '\r':
                  _reader.Read();
                  yield return ConsumeToken(GetControlWord("para"));
                  break;

                case '\'':
                  _reader.Read();
                  var hex = byte.Parse(((char)_reader.Read()).ToString() + (char)_reader.Read(), NumberStyles.HexNumber);
                  _textBuffer.Append(hex);
                  break;

                case '|':
                case '~':
                case '-':
                case '_':
                case ':':
                case '*':
                  var singleToken = GetControlWord(((char)_reader.Read()).ToString());
                  if (singleToken != null)
                  {
                    if (_textBuffer.Length > 0)
                      yield return ConsumeTextBuffer();
                    yield return ConsumeToken(singleToken);
                  }
                  break;
                default:
                  if (!IsLetter((char)_reader.Peek()))
                    throw new NotSupportedException();
                  var token = ReadControlWord();
                  if (token != null)
                  {
                    if (_textBuffer.Length > 0)
                      yield return ConsumeTextBuffer();
                    yield return ConsumeToken(token);
                  }
                  break;
              }
              break;
            case '{':
              if (_context.Count > 0)
                _context.Push(_context.Peek().Clone());
              else
                _context.Push(new EncodingContext());

              if (Depth < _ignoreDepth)
              {
                if (_textBuffer.Length > 0)
                  yield return ConsumeTextBuffer();
                yield return new Group();
              }
              break;
            case '}':
              if (_textBuffer.Length > 0)
                yield return ConsumeTextBuffer();
              yield return new GroupEnd();

              if (_context.Count > 0)
                _context.Pop();
              if (_context.Count > 0)
                UpdateEncoding(_context.Peek().Encoding);
              break;
            case '\n':
            case '\r':
              // must still consume the 'peek'ed char
              break;
            case '\t':
              if (_textBuffer.Length > 0)
                yield return ConsumeTextBuffer();
              yield return ConsumeToken(GetControlWord("tab"));
              break;
            default:
              _textBuffer.Append(curr);
              break;
          }
        }
      }

      if (_textBuffer.Length > 0)
        yield return ConsumeTextBuffer();
    }

    private TextToken ConsumeTextBuffer()
    {
      var result = new TextToken() { Value = _textBuffer.ToString() };
      _textBuffer.Clear();
      ConsumeToken(result);
      return result;
    }

    private IToken ReadControlWord()
    {
      var curr = -1;
      while (IsLetter((char)_reader.Peek()))
        _controlBuffer.Append((char)_reader.Read());
      var name = _controlBuffer.ToString();
      _controlBuffer.Length = 0;

      if (_reader.Peek() == '-')
        _controlBuffer.Append((char)_reader.Read());
      while (IsDigit((char)_reader.Peek()))
        _controlBuffer.Append((char)_reader.Read());

      var number = int.MinValue;
      if (_controlBuffer.Length > 0)
        number = int.Parse(_controlBuffer.ToString());
      _controlBuffer.Length = 0;

      if (name == "u")
      {
        var skip = _context.Count < 1 ? 1 : _context.Peek().AsciiFallbackChars;
        for (var i = 0; i < skip; i++)
          _reader.Read();
      }

      if (_reader.Peek() == ' ')
        _reader.Read();

      return GetControlWord(name, number);
    }

    private IToken ConsumeToken(IToken token)
    {
      if (token is ControlWord<Encoding> ctrlEncode && !(ctrlEncode is FontCharSet))
        UpdateEncoding(ctrlEncode.Value);
      else if (token is Font font && font.Encoding != null)
        UpdateEncoding(font.Encoding);
      else if (token is FromHtml || token.Type == TokenType.HtmlFormat)
        _document.HasHtml = true;

      var destination = _context.FirstOrDefault(c => c.Destination != null)?.Destination;

      if (destination is FontTableTag)
      {
        if (token is FontRef fontRef)
        {
          _document.FontTable[fontRef.Value] = new Font(fontRef.Value);
          _context.Peek().Buffer.Add(_document.FontTable[fontRef.Value]);
          if (_context.Peek().Destination == null)
            _context.Peek().Destination = new FontTableTag();
        }
        else if (_context.Peek().Buffer.Count > 0)
        {
          ((Font)_context.Peek().Buffer.Last()).Add(token);
        }
      }
      else if (destination is ColorTable)
      {
        if (token is TextToken)
        {
          if (_context.Peek().Buffer.Count != 3)
          {
            _document.ColorTable.Add(new ColorValue(0, 0, 0));
          }
          else
          {
            _document.ColorTable.Add(new ColorValue(
              _context.Peek().Buffer.OfType<Red>().Single().Value
              , _context.Peek().Buffer.OfType<Green>().Single().Value
              , _context.Peek().Buffer.OfType<Blue>().Single().Value));
          }
          _context.Peek().Buffer.Clear();
        }
        else
        {
          _context.Peek().Buffer.Add(token);
        }
      }
      else if (IsWord(token) && _context.Count > 0 && _context.Peek().Destination == null)
      {
        _context.Peek().Destination = token;
      }

      return token;
    }

    private bool IsWord(IToken token)
    {
      return ((token?.Type ?? TokenType.None) & TokenType.Word) == TokenType.Word;
    }

    private IToken GetControlWord(string name, int number = int.MinValue)
    {
      switch (name)
      {
        // Characters
        case "emdash":
          _textBuffer.Append('\u2014');
          return null;
        case "endash":
          _textBuffer.Append('\u2013');
          return null;
        case "emspace":
          _textBuffer.Append('\u2003');
          return null;
        case "enspace":
          _textBuffer.Append('\u2002');
          return null;
        case "lquote":
          _textBuffer.Append('\u2018');
          return null;
        case "rquote":
          _textBuffer.Append('\u2019');
          return null;
        case "ldblquote":
          _textBuffer.Append('\u201C');
          return null;
        case "rdblquote":
          _textBuffer.Append('\u201D');
          return null;
        case "-":
          _textBuffer.Append('\u00AD'); // soft hyphen
          return null;
        case "~":
          _textBuffer.Append('\u00A0'); // non-breaking space
          return null;
        case "_":
          _textBuffer.Append('\u2011'); // non-breaking hyphen
          return null;
        case "u":
          if (number < 0 && number != int.MinValue)
            number += 65536;
          _textBuffer.Append(number);
          return null;
        case "upr":
          _ignoreDepth = Depth + 1;
          return null;
        case "ud":
          return new UnicodeTextTag();

        // General
        case "*":
          return new IgnoreUnrecognized();
        case "fromhtml":
          return new FromHtml(number != 0);
        case "htmltag":
          return new HtmlTag((HtmlEncapsulation)number);
        case "htmlrtf":
          return new HtmlRtf(number != 0);

        // Info
        case "info":
          return new Info();
        case "title":
          return new Title();
        case "subject":
          return new Subject();
        case "author":
          return new Author();
        case "manager":
          return new Manager();
        case "company":
          return new Company();
        case "operator":
          return new Operator();
        case "category":
          return new Category();
        case "keywords":
          return new Keywords();
        case "comment":
          return new Comment();
        case "doccomm":
          return new DocComment();
        case "hlinkbase":
          return new HyperlinkBase();
        case "creatim":
          return new CreateTime();
        case "revtim":
          return new RevisionTime();
        case "printim":
          return new PrintTime();
        case "buptim":
          return new BackupTime();
        case "yr":
          return new Year(number);
        case "mo":
          return new Month(number);
        case "dy":
          return new Day(number);
        case "hr":
          return new Hour(number);
        case "min":
          return new Minute(number);
        case "sec":
          return new Second(number);
        case "version":
          return new Tokens.Version(number);
        case "vern":
          return new InternalVersion(number);
        case "edmins":
          return new EditingTime(TimeSpan.FromMinutes(number));
        case "nofpages":
          return new NumPages(number);
        case "nofwords":
          return new NumWords(number);
        case "nofchars":
          return new NumChars(number);
        case "nofcharsws":
          return new NumCharsWs(number);


        // Font Tags
        case "f":
          if (_document.FontTable.TryGetValue(number, out var font))
            return font;
          return new FontRef(number);
        case "fnil":
          return new FontCategory(FontFamilyCategory.Nil);
        case "froman":
          return new FontCategory(FontFamilyCategory.Roman);
        case "fswiss":
          return new FontCategory(FontFamilyCategory.Swiss);
        case "fmodern":
          return new FontCategory(FontFamilyCategory.Modern);
        case "fscript":
          return new FontCategory(FontFamilyCategory.Script);
        case "fdecor":
          return new FontCategory(FontFamilyCategory.Decor);
        case "ftech":
          return new FontCategory(FontFamilyCategory.Tech);
        case "fbidi":
          return new FontCategory(FontFamilyCategory.Bidi);
        case "fcharset":
          return new FontCharSet(TextEncoding.EncodingFromCharSet(number));
        case "cpg":
          return new CodePage(TextEncoding.EncodingFromCodePage(number));
        case "ansicpg":
          return new DefaultCodePage(TextEncoding.EncodingFromCodePage(number));
        case "ansi":
          return new DefaultNamedCodePage(NamedCodePage.Ansi);
        case "mac":
          return new DefaultNamedCodePage(NamedCodePage.Mac);
        case "pc":
          return new DefaultNamedCodePage(NamedCodePage.Pc);
        case "pca":
          return new DefaultNamedCodePage(NamedCodePage.Pca);
        case "fprq":
          return new FontPitchToken((FontPitch)number);
        case "fs":
          return new FontSize(UnitValue.FromHalfPoint(number));
        case "deff":
          return new DefaultFontRef(number);
        case "uc":
          if (_context.Count > 0)
            _context.Peek().AsciiFallbackChars = number;
          return new GenericWord(name, number);
        case "fonttbl":
          return new FontTableTag();

        // Style Sheet
        case "stylesheet":
          return new StyleSheetTag();
        case "s":
          return new StyleRef(number);
        case "generator":
          return new GeneratorTag();

        // Color
        case "colortbl":
          return new ColorTable();
        case "red":
          return new Red((byte)number);
        case "green":
          return new Green((byte)number);
        case "blue":
          return new Blue((byte)number);

        // Paragraph tags
        case "par":
          return new ParagraphBreak();
        case "pard":
          return new ParagraphDefault();
        case "line":
          return new LineBreak();
        case "qc":
          return new TextAlign(TextAlignment.Center);
        case "qj":
          return new TextAlign(TextAlignment.Justify);
        case "ql":
          return new TextAlign(TextAlignment.Left);
        case "qr":
          return new TextAlign(TextAlignment.Right);

        // Section tags
        case "sect":
          return new SectionBreak();
        case "sectd":
          return new SectionDefault();

        // Character Tags
        case "b":
          return new BoldToken(number != 0);
        case "caps":
          return new CapitalToken(number != 0);
        case "cb":
        case "chcbpat":
        case "highlight":
          return new BackgroundColor(_document.ColorTable[number]);
        case "cf":
          return new ForegroundColor(_document.ColorTable[number]);
        case "dn":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(-1 * number));
          return new OffsetToken(UnitValue.FromHalfPoint(-6));
        case "i":
          return new ItalicToken(number != 0);
        case "nosupersub":
          return new NoSuperSubToken();
        case "plain":
          return new PlainToken();
        case "up":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(number));
          return new OffsetToken(UnitValue.FromHalfPoint(6));
        case "strike":
          return new StrikeToken(number != 0);
        case "sub":
          return new SubStartToken();
        case "super":
          return new SuperStartToken();
        case "ul":
          return new UnderlineToken(number != 0);
        case "ulnone":
          return new UnderlineToken(false);
        case "v":
          return new HiddenToken(number != 0);

        default:
          if (number == int.MinValue)
            return new GenericTag(name);
          return new GenericWord(name, number);
      }
    }

    private static bool IsLetter(char ch)
    {
      return (ch >= 'a' && ch <= 'z')
        || (ch >= 'A' && ch <= 'Z');
    }

    private static bool IsDigit(char ch)
    {
      return ch >= '0' && ch <= '9';
    }

    private void UpdateEncoding(Encoding encoding)
    {
      if (_context.Count > 0)
        _context.Peek().Encoding = encoding;
      if (_reader is RtfStreamReader stream)
        stream.Encoding = encoding;
      _textBuffer.Encoding = encoding;
    }

    private class EncodingContext
    {
      public Encoding Encoding { get; set; } = TextEncoding.RtfDefault;
      public int AsciiFallbackChars { get; set; } = 1;
      public IToken Destination { get; set; }
      public List<IToken> Buffer { get; } = new List<IToken>();

      public EncodingContext Clone()
      {
        return new EncodingContext()
        {
          Encoding = Encoding,
          AsciiFallbackChars = AsciiFallbackChars
        };
      }
    }
  }
}
