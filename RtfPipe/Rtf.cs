using System.IO;
using System.Xml;
using System.Linq;

namespace RtfPipe
{
  /// <summary>
  /// Convert a Rich Text Format (RTF) document to HTML
  /// </summary>
  public static class Rtf
  {
    /// <summary>
    /// Convert a Rich Text Format (RTF) document to an HTML string
    /// </summary>
    /// <param name="source">The source RTF document (either a <see cref="string"/>, <see cref="TextReader"/>, or <see cref="Stream"/>)</param>
    /// <param name="settings">The settings used in the HTML rendering</param>
    /// <returns>An HTML string that can be used to render the RTF</returns>
    public static string ToHtml(RtfSource source, RtfHtmlSettings settings = null)
    {
      using (var stringWriter = new StringWriter())
      {
        using (var writer = new HtmlTextWriter(stringWriter, settings))
        {
          ToHtml(source, writer, settings);
        }
        return stringWriter.ToString();
      }
    }

    /// <summary>
    /// Convert a Rich Text Format (RTF) document to HTML
    /// </summary>
    /// <param name="source">The source RTF document (either a <see cref="string"/>, <see cref="TextReader"/>, or <see cref="Stream"/>)</param>
    /// <param name="writer"><see cref="TextWriter"/> that the HTML will be written to</param>
    /// <param name="settings">The settings used in the HTML rendering</param>
    public static void ToHtml(RtfSource source, TextWriter writer, RtfHtmlSettings settings = null)
    {
      using (var xmlWriter = new HtmlTextWriter(writer, settings))
      {
        ToHtml(source, xmlWriter, settings);
      }
    }

    /// <summary>
    /// Convert a Rich Text Format (RTF) document to HTML
    /// </summary>
    /// <param name="source">The source RTF document (either a <see cref="string"/>, <see cref="TextReader"/>, or <see cref="Stream"/>)</param>
    /// <param name="writer"><see cref="XmlWriter"/> that the HTML will be written to</param>
    /// <param name="settings">The settings used in the HTML rendering</param>
    /// <example>
    /// This overload can be used for creating a document that can be further manipulated
    /// <code lang="csharp"><![CDATA[var doc = new XDocument();
    /// using (var writer = doc.CreateWriter())
    /// {
    ///   Rtf.ToHtml(rtf, writer);
    /// }]]>
    /// </code>
    /// </example>
    public static void ToHtml(RtfSource source, XmlWriter writer, RtfHtmlSettings settings = null)
    {
      var parser = new Parser(source.Reader);
      var doc = parser.Parse();
      if (doc.HasHtml)
      {
        new Model.RawBuilder().Build(doc, writer);
      }
      else
      {
        var html = new Model.Builder().Build(parser.Parse());
        var visitor = new Model.HtmlVisitor(writer)
        {
          DefaultTabWidth = html.DefaultTabWidth,
          Settings = settings ?? new RtfHtmlSettings()
        };
        html.Root.Visit(visitor);
      }
      writer.Flush();
    }
  }
}
