using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace RtfPipe
{
  /// <summary>
  /// Tokenize an RTF document and create a heirarchical document structure of the groups
  /// </summary>
  public partial class Parser
  {
    internal static Func<DateTime> Clock { get; set; } = () => DateTime.Now;

    private readonly TextReader _reader;
    private readonly StringBuilder _controlBuffer = new StringBuilder();
    private readonly StringBuffer _textBuffer = new StringBuffer();
    private readonly HexBuffer _hexBuffer = new HexBuffer();
    private readonly Stack<EncodingContext> _context = new Stack<EncodingContext>();
    private int _ignoreDepth = int.MaxValue;
    private readonly Document _document = new Document();

    private int Depth { get { return _context.Count; } }

    /// <summary>
    /// Parse RTF stored in a string
    /// </summary>
    /// <param name="rtf">RTF string</param>
    public Parser(string rtf)
    {
      _reader = new StringReader(rtf);
    }

    /// <summary>
    /// Parse RTF from a stream
    /// </summary>
    /// <param name="stream">Binary RTF data</param>
    public Parser(Stream stream)
    {
      _reader = new RtfStreamReader(stream);
    }

    /// <summary>
    /// Parse RTF from a text reader
    /// </summary>
    /// <param name="reader">RTF</param>
    public Parser(TextReader reader)
    {
      _reader = reader;
    }

    /// <summary>
    /// Build a document structure
    /// </summary>
    public Document Parse()
    {
      var groups = new Stack<Group>();
      var infoGroup = default(Group);
      var listStyles = new Dictionary<int, ListStyle>();

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
          var lastGroup = groups.Count > 0 ? groups.Pop() : default(Group);
          var dest = lastGroup?.Destination;
          if (dest is ListDefinition)
          {
            var style = new ListStyle(lastGroup);
            listStyles[style.Id] = style;
          }
          else if (dest is ListOverride)
          {
            var listId = lastGroup.Contents.OfType<ListId>().FirstOrDefault()?.Value;
            var refId = lastGroup.Contents.OfType<ListStyleId>().FirstOrDefault()?.Value;
            if (listId.HasValue && refId.HasValue && listStyles.TryGetValue(listId.Value, out var style))
            {
              _document.ListStyles[refId.Value] = new ListStyleReference(refId.Value, style);
            }
          }
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

    /// <summary>
    /// Get a flat list of all the tokens in the RTF document
    /// </summary>
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
              if (_context.Count > 0)
                _context.Push(_context.Peek().Clone(_textBuffer));
              else
                _context.Push(new EncodingContext() { ValueBuffer = _textBuffer });
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
                  _context.Peek().ValueBuffer.Append(_reader.Read());
                  break;

                case '\n':
                case '\r':
                  _reader.Read();
                  yield return ConsumeToken(GetControlWord("para"));
                  break;

                case '\'':
                  _reader.Read();
                  var hex = byte.Parse(((char)_reader.Read()).ToString() + (char)_reader.Read(), NumberStyles.HexNumber);
                  _context.Peek().ValueBuffer.Append(hex);
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
                    if (_context.Peek().ValueBuffer.Length > 0)
                      yield return ConsumeTextBuffer();
                    yield return ConsumeToken(singleToken);
                  }
                  break;
                default:
                  if (!IsLetter((char)_reader.Peek()))
                  {
                    _reader.Read();
                    break;
                  }

                  var token = ReadControlWord();
                  if (token != null)
                  {
                    if (_context.Peek().ValueBuffer.Length > 0)
                      yield return ConsumeTextBuffer();
                    yield return ConsumeToken(token);
                  }
                  break;
              }
              break;
            case '{':
              if (Depth < _ignoreDepth && _context.PeekOrDefault()?.ValueBuffer.Length > 0)
                yield return ConsumeTextBuffer();

              if (Depth < _ignoreDepth)
              {
                if (_context.Count > 0)
                  _context.Push(_context.Peek().Clone(_textBuffer));
                else
                  _context.Push(new EncodingContext() { ValueBuffer = _textBuffer });
                yield return new Group();
              }
              break;
            case '}':
              if (_context.Count > 0 && _context.Peek().ValueBuffer.Length > 0)
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
              if (_context.Peek().ValueBuffer.Length > 0)
                yield return ConsumeTextBuffer();
              yield return ConsumeToken(GetControlWord("tab"));
              break;
            default:
              if (_context.Count < 1)
              {
                if (!char.IsWhiteSpace((char)curr))
                  throw new NotSupportedException("Non-whitespace characters were found after the end of the file");
              }
              else if (_context.Peek().Destination is PictureBinaryLength binLength)
              {
                var i = 1;
                _context.Peek().ValueBuffer.Append(curr);
                while (i < binLength.Value && (curr = _reader.Read()) > 0)
                {
                  _context.Peek().ValueBuffer.Append(curr);
                  i++;
                }
              }
              else
              {
                _context.Peek().ValueBuffer.Append(curr);
              }
              break;
          }
        }
      }

      if (_context.PeekOrDefault()?.ValueBuffer.Length > 0)
        yield return ConsumeTextBuffer();
    }

    private IToken ConsumeTextBuffer()
    {
      var result = default(IToken);
      if (_context.Peek().ValueBuffer is HexBuffer hex)
        result = new BinaryToken() { Value = hex.ToArray() };
      else
        result = new TextToken() { Value = _context.Peek().ValueBuffer.ToString() };
      _context.Peek().ValueBuffer.Clear();
      ConsumeToken(result);
      return result;
    }

    private IToken ReadControlWord()
    {
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
        {
          var read = _reader.Read();
          if (read == ' ' && i == 0)
            read = _reader.Read();

          if (read == '\\' && _reader.Peek() == '\'')
          {
            _reader.Read(); // read single quote
            _reader.Read(); // read first nibble of byte
            _reader.Read(); // read second nibble of byte
          }
        }
      }
      else if (_reader.Peek() == ' ')
      {
        _reader.Read();
      }

      return GetControlWord(name, number);
    }

    private IToken ConsumeToken(IToken token)
    {
      if (token is ControlWord<Encoding> ctrlEncode)
        UpdateEncoding(ctrlEncode.Value);
      else if (token is Font font && font.Encoding != null)
        UpdateEncoding(font.Encoding);
      else if (token is FromHtml || token.Type == TokenType.HtmlFormat)
        _document.HasHtml = true;
      else if (token is PictureTag)
        _context.Peek().ValueBuffer = _hexBuffer;

      if (token is PictureBinaryLength)
      {
        UpdateEncoding(new BinaryEncoding());
        _context.Peek().Destination = token;
      }

      var destination = _context.FirstOrDefault(c => c.Destination != null)?.Destination;

      if (destination is FontTableTag)
      {
        if (token is FontRef fontRef)
        {
          _document.FontTable[fontRef.Value] = new Font(fontRef.Value);
          _context.Peek().TokenBuffer.Add(_document.FontTable[fontRef.Value]);
          if (_context.Peek().Destination == null)
            _context.Peek().Destination = new FontTableTag();
        }
        else if (_context.Peek().TokenBuffer.Count > 0)
        {
          ((Font)_context.Peek().TokenBuffer.Last()).Add(token);
        }
      }
      else if (destination is ColorTable)
      {
        if (token is TextToken)
        {
          if (_context.Peek().TokenBuffer.Count != 3)
          {
            _document.ColorTable.Add(new ColorValue(0, 0, 0));
          }
          else
          {
            _document.ColorTable.Add(new ColorValue(
              _context.Peek().TokenBuffer.OfType<Red>().Single().Value
              , _context.Peek().TokenBuffer.OfType<Green>().Single().Value
              , _context.Peek().TokenBuffer.OfType<Blue>().Single().Value));
          }
          _context.Peek().TokenBuffer.Clear();
        }
        else
        {
          _context.Peek().TokenBuffer.Add(token);
        }
      }
      else if (IsDestination(token) && _context.Count > 0 && _context.Peek().Destination == null)
      {
        _context.Peek().Destination = token;
      }

      return token;
    }

    private bool IsDestination(IToken token)
    {
      return ((token?.Type ?? TokenType.None) & TokenType.Word) == TokenType.Word
        && !(token is IgnoreUnrecognized);
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
      _context.Peek().ValueBuffer.Encoding = encoding;
    }

    private class EncodingContext
    {
      public Encoding Encoding { get; set; } = TextEncoding.RtfDefault;
      public int AsciiFallbackChars { get; set; } = 1;
      public IToken Destination { get; set; }
      public List<IToken> TokenBuffer { get; } = new List<IToken>();
      public IValueBuffer ValueBuffer { get; set; }

      public EncodingContext Clone(IValueBuffer valueBuffer)
      {
        return new EncodingContext()
        {
          Encoding = Encoding,
          AsciiFallbackChars = AsciiFallbackChars,
          ValueBuffer = valueBuffer ?? ValueBuffer
        };
      }
    }
  }
}
