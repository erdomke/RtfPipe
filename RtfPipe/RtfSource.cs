using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RtfPipe
{
  /// <summary>
  /// Represents a source of RTF content. It auto-converts from either a 
  /// <see cref="string"/>, <see cref="TextReader"/>, or <see cref="Stream" />.
  /// Using a <see cref="Stream"/> is preferred as an RTF file can switch binary
  /// encodings in the middle of a file
  /// </summary>
  public class RtfSource
  {
    /// <summary>
    /// A reader used to read text from the source
    /// </summary>
    public TextReader Reader { get; }

    /// <summary>
    /// Create an RTF source from a reader
    /// </summary>
    /// <param name="reader">The <see cref="TextReader"/> to use</param>
    public RtfSource(TextReader reader)
    {
      Reader = reader;
    }

    /// <summary>
    /// Implicitly convert strings containing RTF content to an <see cref="RtfSource"/>
    /// </summary>
    /// <param name="value">RTF content</param>
    public static implicit operator RtfSource(string value)
    {
      return new RtfSource(new StringReader(value));
    }

    /// <summary>
    /// Implicitly convert a <see cref="TextReader"/> containing RTF content to an <see cref="RtfSource"/>
    /// </summary>
    /// <param name="value">RTF content</param>
    public static implicit operator RtfSource(TextReader value)
    {
      return new RtfSource(value);
    }

    /// <summary>
    /// Implicitly convert a <see cref="Stream"/> containing RTF content to an <see cref="RtfSource"/>
    /// </summary>
    /// <param name="value">RTF content</param>
    public static implicit operator RtfSource(Stream value)
    {
      return new RtfSource(new RtfStreamReader(value));
    }
  }
}
