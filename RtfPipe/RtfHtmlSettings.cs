using RtfPipe.Model;
using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace RtfPipe
{
  /// <summary>
  /// Settings used when converting RTF to HTML
  /// </summary>
  public class RtfHtmlSettings : HtmlWriterSettings
  {
    /// <summary>
    /// Callback used when building the HTML to render an e-mail attachment
    /// </summary>
    public Action<int, XmlWriter> AttachmentRenderer { get; set; }

    /// <summary>
    /// Mapping of HTML tags to use for various document element types
    /// </summary>
    public Dictionary<ElementType, HtmlTag> ElementTags { get; } = DefaultTags.ToDictionary(k => k.Key, k => k.Value);

    /// <summary>
    /// Callback used to get the URI for a picture stored in RTF. This could be 
    /// a data URI that contains the binary data of the picture, or a link to
    /// an external file.
    /// </summary>
    public Func<Picture, string> ImageUriGetter { get; set; }

    /// <summary>
    /// Create a new <see cref="RtfHtmlSettings"/> object
    /// </summary>
    public RtfHtmlSettings()
    {
      AttachmentRenderer = RenderAttachment;
      ImageUriGetter = DataUri;
    }

    public RtfHtmlSettings WithFullDocument()
    {
      ElementTags[ElementType.Meta] = HtmlTag.Meta;
      ElementTags[ElementType.Document] = HtmlTag.Body;
      ElementTags[ElementType.Footer] = HtmlTag.Header;
      ElementTags[ElementType.FooterFirst] = new HtmlTag("footer")
      {
        Attributes = { { "class", "footer-first" } }
      };
      ElementTags[ElementType.FooterLeft] = new HtmlTag("footer")
      {
        Attributes = { { "class", "footer-left" } }
      };
      ElementTags[ElementType.FooterRight] = new HtmlTag("footer")
      {
        Attributes = { { "class", "footer-right" } }
      };
      ElementTags[ElementType.Header] = HtmlTag.Header;
      ElementTags[ElementType.HeaderFirst] = new HtmlTag("header")
      {
        Attributes = { { "class", "header-first" } }
      };
      ElementTags[ElementType.HeaderLeft] = new HtmlTag("header")
      {
        Attributes = { { "class", "header-left" } }
      };
      ElementTags[ElementType.HeaderRight] = new HtmlTag("header")
      {
        Attributes = { { "class", "header-right" } }
      };
      return this;
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
      { ElementType.Container, HtmlTag.Div },
      { ElementType.Document, HtmlTag.Div },
      { ElementType.Emphasis, HtmlTag.Em },
      { ElementType.Heading1, HtmlTag.H1 },
      { ElementType.Heading2, HtmlTag.H2 },
      { ElementType.Heading3, HtmlTag.H3 },
      { ElementType.Heading4, HtmlTag.H4 },
      { ElementType.Heading5, HtmlTag.H5 },
      { ElementType.Heading6, HtmlTag.H6 },
      { ElementType.Hyperlink, HtmlTag.A },
      { ElementType.List, HtmlTag.Ul },
      { ElementType.ListItem, HtmlTag.Li },
      { ElementType.OrderedList, HtmlTag.Ol },
      { ElementType.Paragraph, HtmlTag.P },
      { ElementType.Section, HtmlTag.Div },
      { ElementType.Span, HtmlTag.Span },
      { ElementType.Strong, HtmlTag.Strong },
      { ElementType.Table, HtmlTag.Table },
      { ElementType.TableBody, HtmlTag.Tbody },
      { ElementType.TableCell, HtmlTag.Td },
      { ElementType.TableHeader, HtmlTag.Thead },
      { ElementType.TableHeaderCell, HtmlTag.Th },
      { ElementType.TableRow, HtmlTag.Tr },
      { ElementType.Underline, HtmlTag.U },
    };
  }
}
