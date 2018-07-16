using System;
using System.Collections.Specialized;
using RtfPipe;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

  public class RtfHtmlConvertSettings
  {

    public const string DefaultDocumentCharacterSet = "UTF-8";

    // regex souce: http://msdn.microsoft.com/en-us/library/aa159903.aspx
    public const string DefaultVisualHyperlinkPattern =
      @"[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*";

    public RtfHtmlConvertSettings() :
      this(new DataUriImageVisitor(), RtfHtmlConvertScope.All)
    {
    }

    public RtfHtmlConvertSettings(RtfHtmlConvertScope convertScope) :
      this(new DataUriImageVisitor(), convertScope)
    {
    }

    public RtfHtmlConvertSettings(IObjectVisitor objectVisitor) :
      this(objectVisitor, RtfHtmlConvertScope.All)
    {
    }

    public RtfHtmlConvertSettings(IObjectVisitor objectVisitor, RtfHtmlConvertScope convertScope)
    {
      this.objectVisitor = objectVisitor ?? new DataUriImageVisitor();
      ConvertScope = convertScope;
      VisualHyperlinkPattern = DefaultVisualHyperlinkPattern;
    }

    public IObjectVisitor ObjectVisitor
    {
      get { return objectVisitor; }
    }

    public RtfHtmlConvertScope ConvertScope { get; set; }

    public bool HasStyles
    {
      get { return styles != null && styles.Count > 0; }
    }

    public RtfHtmlCssStyleCollection Styles
    {
      get { return styles ?? (styles = new RtfHtmlCssStyleCollection()); }
    }

    public bool HasStyleSheetLinks
    {
      get { return styleSheetLinks != null && styleSheetLinks.Count > 0; }
    }

    public List<string> StyleSheetLinks
    {
      get { return styleSheetLinks ?? (styleSheetLinks = new List<string>()); }
    }

    public string Title { get; set; }

    public string CharacterSet
    {
      get { return characterSet; }
      set { characterSet = value; }
    }

    public string VisualHyperlinkPattern { get; set; }

    public bool IsShowHiddenText { get; set; }

    public bool ConvertVisualHyperlinks { get; set; }

    public bool UseNonBreakingSpaces { get; set; }

    private readonly IObjectVisitor objectVisitor;
    private RtfHtmlCssStyleCollection styles;
    private List<string> styleSheetLinks;
    private string characterSet = DefaultDocumentCharacterSet;
  }

}
