using RtfPipe.Converter.Html;
using RtfPipe.Interpreter;
using RtfPipe.Parser;
using RtfPipe.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RtfPipe
{
  public static class Rtf
  {
    public static string ToHtml(RtfSource source, RtfHtmlWriterSettings settings = null)
    {
      using (var stringWriter = new StringWriter())
      {
        using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
        {
          ToHtml(source, writer, settings);
        }
        return stringWriter.ToString();
      }
    }
    public static void ToHtml(RtfSource source, XmlWriter writer, RtfHtmlWriterSettings settings = null)
    {
      settings = settings ?? new RtfHtmlWriterSettings();
      var content = ParseRtf(source);

      // Try to extract encoded html from within the rtf (Outlook likes to do this)
      if (!BuildHtmlContent(content, writer))
      {
        var intSettings = new RtfInterpreterSettings() { IgnoreDuplicatedFonts = true, IgnoreUnknownFonts = true };
        var rtfDocument = RtfInterpreterTool.BuildDoc(content, intSettings);
        var htmlConvertSettings = new RtfHtmlConvertSettings(settings.ObjectVisitor);
        htmlConvertSettings.IsShowHiddenText = false;
        htmlConvertSettings.UseNonBreakingSpaces = false;
        htmlConvertSettings.ConvertScope = RtfHtmlConvertScope.All;

        var htmlConverter = new RtfHtmlConverter(rtfDocument, htmlConvertSettings);
        htmlConverter.Convert(writer);
      }
    }

    private static bool BuildHtmlContent(IRtfGroup content, XmlWriter writer)
    {
      var nodesFound = false;
      bool doRender = false;
      foreach (IRtfElement elem in content.Contents)
      {
        switch (elem.Kind)
        {
          case RtfElementKind.Group:
            nodesFound = BuildHtmlContent((IRtfGroup)elem, writer) || nodesFound;
            break;
          case RtfElementKind.Text:
            if (doRender)
            {
              writer.WriteRaw(((IRtfText)elem).Text);
              nodesFound = true;
            }
            break;
          case RtfElementKind.Tag:
            switch (((IRtfTag)elem).Name)
            {
              case "htmltag":
              case "htmlrtf":
                doRender = !doRender;
                break;
              case "par":
                if (doRender)
                {
                  writer.WriteRaw("\r\n");
                  nodesFound = true;
                }
                break;
            }
            break;
        }
      }

      return nodesFound;
    }

    private static IRtfGroup ParseRtf(RtfSource source)
    {
      IRtfGroup rtfStructure;
      // parse the rtf structure
      RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
      RtfParser parser = new RtfParser(structureBuilder);
      parser.IgnoreContentAfterRootGroup = true; // support WordPad documents
      parser.Parse(source);
      rtfStructure = structureBuilder.StructureRoot;
      return rtfStructure;
    }
  }
}
