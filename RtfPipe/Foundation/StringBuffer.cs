using System;
using System.IO;
using System.Text;

namespace RtfPipe
{
  internal class StringBuffer : IValueBuffer
  {
    const int SizeIncrement = 256;

    private byte[] _buffer = new byte[SizeIncrement];
    private int _position;
    private readonly StringBuilder _chars = new StringBuilder();

    public Encoding Encoding { get; set; } = TextEncoding.RtfDefault;
    public int Length { get { return _chars.Length + _position; } }

    public IValueBuffer Append(byte value)
    {
      if (_position >= _buffer.Length)
        ResizeBuffer(_buffer.Length + SizeIncrement);
      _buffer[_position++] = value;
      return this;
    }

    public IValueBuffer Append(int ch)
    {
      return Append((char)ch);
    }

    public IValueBuffer Append(char ch)
    {
      FlushBuffer();
      _chars.Append(ch);
      return this;
    }

    public IValueBuffer Append(string value)
    {
      FlushBuffer();
      _chars.Append(value);
      return this;
    }

    private void FlushBuffer()
    {
      if (_position > 0)
      {
        _chars.Append(Encoding.GetString(_buffer, 0, _position));
        _position = 0;
      }
    }

    private void ResizeBuffer(int size)
    {
      var newBuffer = new byte[size];
      Array.Copy(_buffer, 0, newBuffer, 0, Math.Min(_buffer.Length, size));
      _buffer = newBuffer;
    }

    public void Clear()
    {
      _position = 0;
      _chars.Length = 0;
    }

    public override string ToString()
    {
      FlushBuffer();
      return _chars.ToString();
    }
  }
}
