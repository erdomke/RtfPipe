using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RtfPipe
{
  public class RtfHtmlSettings : HtmlWriterSettings
  {
    public Action<int, XmlWriter> AttachmentRenderer { get; set; }
    public Func<Picture, string> ImageUriGetter { get; set; }

    public RtfHtmlSettings()
    {
      AttachmentRenderer = RenderAttachment;
      ImageUriGetter = DataUri;
    }

    private static string DataUri(Picture picture)
    {
      return "data:" + picture.MimeType() + ";base64," + Convert.ToBase64String(picture.Bytes);
    }

    private static void RenderAttachment(int index, XmlWriter writer)
    {
      writer.WriteStartElement("span");
      writer.WriteAttributeString("data-index", index.ToString());
      writer.WriteEndElement();
    }
  }
}
