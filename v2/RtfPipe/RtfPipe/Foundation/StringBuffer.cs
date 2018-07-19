using System;
using System.IO;
using System.Text;

namespace RtfPipe
{
  internal class StringBuffer
  {
    const int SizeIncrement = 256;

    private byte[] _buffer = new byte[SizeIncrement];
    private readonly char[] _charArray = new[] { '\0' };

    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public int Position { get; private set; }

    public StringBuffer Append(byte value)
    {
      if (Position >= _buffer.Length)
        ResizeBuffer(_buffer.Length + SizeIncrement);
      _buffer[Position++] = value;
      return this;
    }

    public StringBuffer Append(int ch)
    {
      return Append((char)ch);
    }

    public StringBuffer Append(char ch)
    {
      _charArray[0] = ch;
      var maxBytes = Encoding.GetMaxByteCount(1);
      if (Position + maxBytes > _buffer.Length)
        ResizeBuffer(_buffer.Length + Math.Max(SizeIncrement, maxBytes));
      Position += Encoding.GetBytes(_charArray, 0, 1, _buffer, Position);
      return this;
    }

    public StringBuffer Append(string value)
    {
      var maxBytes = Encoding.GetMaxByteCount(value.Length);
      if (Position + maxBytes > _buffer.Length)
        ResizeBuffer(_buffer.Length + Math.Max(SizeIncrement, maxBytes));
      Position += Encoding.GetBytes(value, 0, 1, _buffer, Position);
      return this;
    }

    private void ResizeBuffer(int size)
    {
      var newBuffer = new byte[size];
      Array.Copy(_buffer, 0, newBuffer, 0, Math.Min(_buffer.Length, size));
      _buffer = newBuffer;
    }

    public void Clear()
    {
      Position = 0;
    }

    public override string ToString()
    {
      if (Position <= 0)
        return string.Empty;
      return Encoding.GetString(_buffer, 0, Position);
    }
  }
}
