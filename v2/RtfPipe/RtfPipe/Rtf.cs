using System.IO;
using System.Xml;

namespace RtfPipe
{
  public static class Rtf
  {
    public static string ToHtml(RtfSource source, HtmlWriterSettings settings = null)
    {
      using (var stringWriter = new StringWriter())
      {
        using (var writer = new HtmlTextWriter(stringWriter, settings))
        {
          ToHtml(source, writer);
        }
        return stringWriter.ToString();
      }
    }

    public static void ToHtml(RtfSource source, XmlWriter writer)
    {
      var parser = new Parser(source.Reader);
      var interpreter = new Interpreter(writer);
      interpreter.ToHtml(parser.Parse());
    }
  }
}
