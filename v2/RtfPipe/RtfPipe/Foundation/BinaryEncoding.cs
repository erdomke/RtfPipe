using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal class BinaryEncoding : Encoding
  {
    public override int GetByteCount(char[] chars, int index, int count)
    {
      throw new NotSupportedException();
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
      throw new NotSupportedException();
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return count;
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      var toCopy = Math.Min(byteCount, chars.Length - charIndex);
      for (var i = 0; i <= toCopy; i++)
      {
        chars[charIndex + i] = (char)bytes[byteIndex + i];
      }
      return toCopy;
    }

    public override int GetMaxByteCount(int charCount)
    {
      return charCount;
    }

    public override int GetMaxCharCount(int byteCount)
    {
      return byteCount;
    }
  }
}
