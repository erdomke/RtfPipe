using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RtfPipe
{
  public class RtfSource
  {
    public TextReader Reader { get; }

    public RtfSource(TextReader reader)
    {
      Reader = reader;
    }

    public static implicit operator RtfSource(string value)
    {
      return new RtfSource(new StringReader(value));
    }

    public static implicit operator RtfSource(TextReader value)
    {
      return new RtfSource(value);
    }

    public static implicit operator RtfSource(Stream value)
    {
      return new RtfSource(new RtfStreamReader(value));
    }
  }
}
