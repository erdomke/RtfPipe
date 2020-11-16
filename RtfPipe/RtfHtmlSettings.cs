using RtfPipe.Model;
using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RtfPipe
{
  public class RtfHtmlSettings : HtmlWriterSettings
  {
    public Action<int, XmlWriter> AttachmentRenderer { get; set; }
    public Dictionary<ElementType, HtmlTag> ElementTags { get; } = DefaultTags.ToDictionary(k => k.Key, k => k.Value);
    public Func<Picture, string> ImageUriGetter { get; set; }

    public RtfHtmlSettings()
    {
      AttachmentRenderer = RenderAttachment;
      ImageUriGetter = DataUri;
    }

    private static string DataUri(Picture picture)
    {
#if NETFULL
      if (picture.Type is EmfBlip || picture.Type is WmMetafile)
      {
        using (var source = new MemoryStream(picture.Bytes))
        using (var dest = new MemoryStream())
        {
          var bmp = new System.Drawing.Bitmap(source);
          bmp.Save(dest, System.Drawing.Imaging.ImageFormat.Png);
          return "data:image/png;base64," + Convert.ToBase64String(dest.ToArray());
        }
      }
#endif
      return "data:" + picture.MimeType() + ";base64," + Convert.ToBase64String(picture.Bytes);
    }

    private static void RenderAttachment(int index, XmlWriter writer)
    {
      writer.WriteStartElement("span");
      writer.WriteAttributeString("data-index", index.ToString());
      writer.WriteEndElement();
    }

    internal static Dictionary<ElementType, HtmlTag> DefaultTags { get; } = new Dictionary<ElementType, HtmlTag>()
    {
      { ElementType.Document, HtmlTag.Div },
      { ElementType.Emphasis, HtmlTag.Em },
      { ElementType.Header1, HtmlTag.H1 },
      { ElementType.Header2, HtmlTag.H2 },
      { ElementType.Header3, HtmlTag.H3 },
      { ElementType.Header4, HtmlTag.H4 },
      { ElementType.Header5, HtmlTag.H5 },
      { ElementType.Header6, HtmlTag.H6 },
      { ElementType.Hyperlink, HtmlTag.A },
      { ElementType.List, HtmlTag.Ul },
      { ElementType.ListItem, HtmlTag.Li },
      { ElementType.OrderedList, HtmlTag.Ol },
      { ElementType.Paragraph, HtmlTag.P },
      { ElementType.Section, HtmlTag.Div },
      { ElementType.Span, HtmlTag.Span },
      { ElementType.Strong, HtmlTag.Strong },
      { ElementType.Table, HtmlTag.Table },
      { ElementType.TableCell, HtmlTag.Td },
      { ElementType.TableRow, HtmlTag.Tr },
      { ElementType.Underline, HtmlTag.U },
    };
  }
}
