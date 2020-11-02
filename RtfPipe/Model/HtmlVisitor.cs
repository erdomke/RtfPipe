using System.IO;
using System.Xml;

namespace RtfPipe.Model
{
  public class HtmlVisitor : INodeVisitor
  {
    private readonly XmlWriter _writer;

    public HtmlVisitor(TextWriter writer)
    {
      _writer = new HtmlTextWriter(writer);
    }

    public HtmlVisitor(XmlWriter writer)
    {
      _writer = writer;
    }

    public string DocumentElement { get; set; } = "main";
    public string SectionElement { get; set; } = "section";
    public string ParagraphElement { get; set; } = "p";

    public void Visit(Element element)
    {
      switch (element.Type)
      {
        case ElementType.Cell:
          _writer.WriteStartElement("td");
          break;
        case ElementType.Document:
          _writer.WriteStartElement(DocumentElement);
          break;
        case ElementType.Row:
          _writer.WriteStartElement("tr");
          break;
        case ElementType.Section:
          _writer.WriteStartElement(SectionElement);
          break;
        case ElementType.Table:
          _writer.WriteStartElement("table");
          break;
        case ElementType.ListItem:
          _writer.WriteStartElement("li");
          break;
        case ElementType.List:
          _writer.WriteStartElement("ul");
          break;
        default: // paragraph
          _writer.WriteStartElement("paragraph");
          break;
      }
      foreach (var node in element.Nodes())
        node.Visit(this);
      _writer.WriteEndElement();
    }

    public void Visit(Run run)
    {
      _writer.WriteValue(run.Value);
    }

    public void Visit(ControlCharacter controlCharacter)
    {
      switch (controlCharacter.Type)
      {
        case ControlCharacterType.LineBreak:
          _writer.WriteStartElement("br");
          _writer.WriteEndElement();
          break;
        case ControlCharacterType.Tab:
          _writer.WriteStartElement("tab");
          _writer.WriteEndElement();
          break;
      }
    }
  }
}
