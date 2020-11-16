using System.IO;
using System.Xml;
using System.Linq;

namespace RtfPipe
{
  public static class Rtf
  {
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

    public static void ToHtml(RtfSource source, TextWriter writer, RtfHtmlSettings settings = null)
    {
      using (var xmlWriter = new HtmlTextWriter(writer, settings))
      {
        ToHtml(source, xmlWriter, settings);
      }
    }

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
      
      //var interpreter = new Interpreter(writer);
      //interpreter.ToHtml(doc, settings);
    }
  }
}
