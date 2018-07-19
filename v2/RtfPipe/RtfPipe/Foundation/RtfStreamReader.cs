using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RtfPipe
{
  internal class RtfStreamReader : TextReader, IRtfReader
  {
    private enum State
    {
      Start,
      Middle,
      End
    }

    private State _state = State.Start;
    private Encoding _encoding = TextEncoding.RtfDefault;
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private int _bufferStart;
    private int _bufferLength;
    private readonly char[] _charBuffer;
    private int _charPos;
    private int _charLength;

    public RtfStreamReader(Stream stream) : this(stream, 4096) { }
    public RtfStreamReader(Stream stream, int bufferSize)
    {
      _stream = stream;
      _buffer = new byte[bufferSize];
      _charBuffer = new char[bufferSize];
    }


    public Encoding Encoding
    {
      get { return _encoding; }
      set
      {
        if (_encoding != value)
          ChangeEncoding(_encoding, value);
        _encoding = value;
      }
    }

    public override int Peek()
    {
      FillBuffer();
      if (_state == State.End)
        return -1;
      return _charBuffer[_charPos];
    }

    public override int Read()
    {
      FillBuffer();
      if (_state == State.End)
        return -1;
      return _charBuffer[_charPos++];
    }

    private void FillBuffer()
    {
      if (_charPos >= _charLength)
      {
        if (_state == State.End)
          return;

        _bufferLength = _stream.Read(_buffer, 0, _buffer.Length);
        if (_bufferLength < 1)
        {
          _state = State.End;
          return;
        }

        // Skip past BOM marks to the actual file contents
        _bufferStart = 0;
        while (_state == State.Start && _bufferStart < _bufferLength && _buffer[_bufferStart] != '{')
          _bufferStart++;
        _state = State.Middle;

        _charLength = Encoding.GetChars(_buffer, _bufferStart, _bufferLength - _bufferStart, _charBuffer, 0);
        _charPos = 0;
        if (_charLength < 1)
          _state = State.End;
      }
    }

    private void ChangeEncoding(Encoding oldEncoding, Encoding newEncoding)
    {
      if (_state == State.Middle && _charPos < _charLength)
      {
        if (_charPos > 0)
          _bufferStart += oldEncoding.GetByteCount(_charBuffer, 0, _charPos);

        _charLength = newEncoding.GetChars(_buffer, _bufferStart, _bufferLength - _bufferStart, _charBuffer, 0);
        _charPos = 0;
        if (_charLength < 1)
          _state = State.End;
      }
    }
  }
}
