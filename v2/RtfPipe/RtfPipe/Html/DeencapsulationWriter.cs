using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace RtfPipe
{
  internal class DeencapsulationWriter : IHtmlWriter
  {
    private readonly XmlWriter _writer;

    public Font DefaultFont { get; set; }

    public DeencapsulationWriter(XmlWriter writer)
    {
      _writer = writer;
    }

    public void AddBreak(FormatContext format, IToken token)
    {
      _writer.WriteRaw("\r\n");
    }

    public void AddText(FormatContext format, string text)
    {
      if (format.OfType<HtmlTag>().Any())
        _writer.WriteRaw(text);
      else
        _writer.WriteValue(text);
    }

    public void Close()
    {

    }
  }
}
