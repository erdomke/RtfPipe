// -- FILE ------------------------------------------------------------------
// name       : RtfHtmlConvertSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.02
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using RtfPipe.Converter.Image;
using RtfPipe;

namespace RtfPipe.Converter.Html
{

	// ------------------------------------------------------------------------
	public class RtfHtmlConvertSettings
	{

		// ----------------------------------------------------------------------
		public const string DefaultDocumentCharacterSet = "UTF-8";

		// regex souce: http://msdn.microsoft.com/en-us/library/aa159903.aspx
		public const string DefaultVisualHyperlinkPattern =
			@"[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*";

		// ----------------------------------------------------------------------
		public RtfHtmlConvertSettings() :
			this( new DataUriImageVisitor(), RtfHtmlConvertScope.All )
		{
		} // RtfHtmlConvertSettings

		// ----------------------------------------------------------------------
		public RtfHtmlConvertSettings( RtfHtmlConvertScope convertScope ) :
			this( new DataUriImageVisitor(), convertScope )
		{
		} // RtfHtmlConvertSettings

		// ----------------------------------------------------------------------
		public RtfHtmlConvertSettings( IImageVisitor imageVisitor ) :
			this( imageVisitor, RtfHtmlConvertScope.All )
		{
		} // RtfHtmlConvertSettings

		// ----------------------------------------------------------------------
		public RtfHtmlConvertSettings(IImageVisitor imageVisitor, RtfHtmlConvertScope convertScope )
		{
			this.imageVisitor = imageVisitor ?? new DataUriImageVisitor();
			ConvertScope = convertScope;
			VisualHyperlinkPattern = DefaultVisualHyperlinkPattern;
		} // RtfHtmlConvertSettings

		// ----------------------------------------------------------------------
		public IImageVisitor ImageVisitor
		{
			get { return imageVisitor; }
		} // ImageAdapter

		// ----------------------------------------------------------------------
		public RtfHtmlConvertScope ConvertScope { get; set; }

		// ----------------------------------------------------------------------
		public bool HasStyles
		{
			get { return styles != null && styles.Count > 0; }
		} // HasStyles

		// ----------------------------------------------------------------------
		public RtfHtmlCssStyleCollection Styles
		{
			get { return styles ?? ( styles = new RtfHtmlCssStyleCollection() ); }
		} // Styles

		// ----------------------------------------------------------------------
		public bool HasStyleSheetLinks
		{
			get { return styleSheetLinks != null && styleSheetLinks.Count > 0; }
		} // HasStyleSheetLinks

		// ----------------------------------------------------------------------
		public StringCollection StyleSheetLinks
		{
			get { return styleSheetLinks ?? ( styleSheetLinks = new StringCollection() ); }
		} // StyleSheetLinks

		// ----------------------------------------------------------------------
		public string Title { get; set; }

		// ----------------------------------------------------------------------
		public string CharacterSet
		{
			get { return characterSet; }
			set { characterSet = value; }
		} // CharacterSet

		// ----------------------------------------------------------------------
		public string VisualHyperlinkPattern { get; set; }

		// ----------------------------------------------------------------------
		public bool IsShowHiddenText { get; set; }

		// ----------------------------------------------------------------------
		public bool ConvertVisualHyperlinks { get; set; }

		// ----------------------------------------------------------------------
		public bool UseNonBreakingSpaces { get; set; }

		// ----------------------------------------------------------------------
		// members
		private readonly IImageVisitor imageVisitor;
		private RtfHtmlCssStyleCollection styles;
		private StringCollection styleSheetLinks;
		private string characterSet = DefaultDocumentCharacterSet;
	} // class RtfHtmlConvertSettings

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
