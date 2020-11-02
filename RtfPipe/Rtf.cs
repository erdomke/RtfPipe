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

    public static void ToHtml(RtfSource source, XmlWriter writer, RtfHtmlSettings settings = null)
    {
      var parser = new Parser(source.Reader);
      var doc = parser.Parse();
      var builder = new Model.Builder();
      var node = builder.Build(doc);
      //var interpreter = new Interpreter(writer);
      //interpreter.ToHtml(doc, settings);
    }
  }
}
