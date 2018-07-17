using System;
using System.Collections.Generic;

namespace RtfPipe.Model
{
  public sealed class RtfDocument : IRtfDocument
  {
    public RtfDocument(IRtfInterpreterContext context, IList<IRtfVisual> visualContent) :
      this(context.RtfVersion,
        context.DefaultFont,
        context.FontTable,
        context.ColorTable,
        context.Generator,
        context.UniqueTextFormats,
        context.DocumentInfo,
        context.UserProperties,
        visualContent
      )
    {
    }

    public RtfDocument(
      int rtfVersion,
      Font defaultFont,
      RtfFontCollection fontTable,
      IList<ColorValue> colorTable,
      string generator,
      IList<Style> uniqueTextFormats,
      IRtfDocumentInfo documentInfo,
      IList<IRtfDocumentProperty> userProperties,
      IList<IRtfVisual> visualContent
    )
    {
      if (rtfVersion != RtfSpec.RtfVersion1)
        throw new RtfUnsupportedStructureException(Strings.UnsupportedRtfVersion(rtfVersion));
      if (defaultFont == null)
        throw new ArgumentNullException(nameof(defaultFont));
      if (fontTable == null)
        throw new ArgumentNullException(nameof(fontTable));
      if (colorTable == null)
        throw new ArgumentNullException(nameof(colorTable));
      if (uniqueTextFormats == null)
        throw new ArgumentNullException(nameof(uniqueTextFormats));
      if (documentInfo == null)
        throw new ArgumentNullException(nameof(documentInfo));
      if (userProperties == null)
        throw new ArgumentNullException(nameof(userProperties));
      if (visualContent == null)
        throw new ArgumentNullException(nameof(visualContent));

      this.RtfVersion = rtfVersion;
      this.DefaultFont = defaultFont;
      DefaultTextFormat = new Style(defaultFont, RtfSpec.DefaultFontSize);
      this.FontTable = fontTable;
      this.ColorTable = colorTable;
      this.Generator = generator;
      this.UniqueTextFormats = uniqueTextFormats;
      this.DocumentInfo = documentInfo;
      this.UserProperties = userProperties;
      this.VisualContent = visualContent;
    }

    public int RtfVersion { get; }

    public Font DefaultFont { get; }

    public Style DefaultTextFormat { get; }

    public RtfFontCollection FontTable { get; }

    public IList<ColorValue> ColorTable { get; }

    public string Generator { get; }

    public IList<Style> UniqueTextFormats { get; }

    public IRtfDocumentInfo DocumentInfo { get; }

    public IList<IRtfDocumentProperty> UserProperties { get; }

    public IList<IRtfVisual> VisualContent { get; }
  }

}

