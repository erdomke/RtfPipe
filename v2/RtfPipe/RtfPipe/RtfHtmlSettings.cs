using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class RtfHtmlSettings : HtmlWriterSettings
  {
    public Func<Picture, string> ImageUriGetter { get; set; }

    public RtfHtmlSettings()
    {
      ImageUriGetter = DataUri;
    }

    private string DataUri(Picture picture)
    {
      return "data:" + picture.MimeType() + ";base64," + Convert.ToBase64String(picture.Bytes);
    }
  }
}
