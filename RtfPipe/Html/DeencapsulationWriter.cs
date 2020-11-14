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
    public UnitValue DefaultTabWidth { get; set; }

    public DeencapsulationWriter(XmlWriter writer)
    {
      _writer = writer;
    }

    public void AddBreak(FormatContext format, IToken token, int count = 1)
    {
      if (!(token is Tab))
        _writer.WriteRaw("\r\n");
    }

    public void AddText(FormatContext format, string text)
    {
      if (format.OfType<HtmlTagToken>().Any())
        _writer.WriteRaw(text);
      else
        _writer.WriteValue(text);
    }

    public void Close()
    {

    }

    public void AddPicture(FormatContext format, Picture picture)
    {
      throw new NotSupportedException();
    }
  }
}
