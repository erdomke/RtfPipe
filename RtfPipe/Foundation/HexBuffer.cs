using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal class HexBuffer : IValueBuffer
  {
    private readonly List<byte> _bytes = new List<byte>();
    private bool _firstNibble = true;
    private int _nibble;

    public Encoding Encoding { get; set; } = TextEncoding.RtfDefault;
    public int Length => _bytes.Count;

    public IValueBuffer Append(int ch)
    {
      if (Encoding is BinaryEncoding)
        _bytes.Add((byte)ch);
      else if (ch >= '0' && ch <= '9')
        AppendNibble(ch - '0');
      else if (ch >= 'a' && ch <= 'f')
        AppendNibble(ch - 'a' + 10);
      else if (ch >= 'A' && ch <= 'Z')
        AppendNibble(ch - 'A' + 10);
      else if (!(_bytes.Count == 0 && char.IsWhiteSpace((char)ch)))
        throw new NotSupportedException();

      return this;
    }

    public IValueBuffer Append(byte value)
    {
      return Append((int)value);
    }

    public IValueBuffer Append(char ch)
    {
      return Append((int)ch);
    }

    private void AppendNibble(int value)
    {
      if (_firstNibble)
        _nibble = value << 4;
      else
        _bytes.Add((byte)(_nibble + value));

      _firstNibble = !_firstNibble;
    }

    public void Clear()
    {
      _bytes.Clear();
    }

    public byte[] ToArray()
    {
      return _bytes.ToArray();
    }
  }
}
