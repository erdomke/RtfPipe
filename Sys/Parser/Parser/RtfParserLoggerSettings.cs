// -- FILE ------------------------------------------------------------------
// name       : RtfParserLoggerSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.05.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace Itenso.Rtf.Parser
{

	// ------------------------------------------------------------------------
	public class RtfParserLoggerSettings
	{

		// ----------------------------------------------------------------------
		public RtfParserLoggerSettings() :
			this( true )
		{
		} // RtfParserLoggerSettings

		// ----------------------------------------------------------------------
		public RtfParserLoggerSettings( bool enabled )
		{
			Enabled = enabled;
		} // RtfParserLoggerSettings

		// ----------------------------------------------------------------------
		public bool Enabled { get; set; }

		// ----------------------------------------------------------------------
		public string ParseBeginText
		{
			get { return parseBeginText; }
			set { parseBeginText = value; }
		} // ParseBeginText

		// ----------------------------------------------------------------------
		public string ParseEndText
		{
			get { return parseEndText; }
			set { parseEndText = value; }
		} // ParseEndText

		// ----------------------------------------------------------------------
		public string ParseGroupBeginText
		{
			get { return parseGroupBeginText; }
			set { parseGroupBeginText = value; }
		} // ParseGroupBeginText

		// ----------------------------------------------------------------------
		public string ParseGroupEndText
		{
			get { return parseGroupEndText; }
			set { parseGroupEndText = value; }
		} // ParseGroupEndText

		// ----------------------------------------------------------------------
		public string ParseTagText
		{
			get { return parseTagText; }
			set { parseTagText = value; }
		} // ParseTagText

		// ----------------------------------------------------------------------
		public string TextOverflowText
		{
			get { return textOverflowText; }
			set { textOverflowText = value; }
		} // TextOverflowText

		// ----------------------------------------------------------------------
		public string ParseTextText
		{
			get { return parseTextText; }
			set { parseTextText = value; }
		} // ParseTextText

		// ----------------------------------------------------------------------
		public string ParseSuccessText
		{
			get { return parseSuccessText; }
			set { parseSuccessText = value; }
		} // ParseSuccessText

		// ----------------------------------------------------------------------
		public string ParseFailKnownReasonText
		{
			get { return parseFailKnownReasonText; }
			set { parseFailKnownReasonText = value; }
		} // ParseFailKnownReasonText

		// ----------------------------------------------------------------------
		public string ParseFailUnknownReasonText
		{
			get { return parseFailUnknownReasonText; }
			set { parseFailUnknownReasonText = value; }
		} // ParseFailUnknownReasonText

		// ----------------------------------------------------------------------
		public int TextMaxLength
		{
			get { return textMaxLength; }
			set { textMaxLength = value; }
		} // TextMaxLength

		// ----------------------------------------------------------------------
		// members
		private string parseBeginText = Strings.LogParseBegin;
		private string parseEndText = Strings.LogParseEnd;
		private string parseGroupBeginText = Strings.LogGroupBegin;
		private string parseGroupEndText = Strings.LogGroupEnd;
		private string parseTagText = Strings.LogTag;
		private string parseTextText = Strings.LogText;
		private string textOverflowText = Strings.LogOverflowText;
		private string parseSuccessText = Strings.LogParseSuccess;
		private string parseFailKnownReasonText = Strings.LogParseFail;
		private string parseFailUnknownReasonText = Strings.LogParseFailUnknown;

		private int textMaxLength = 80;

	} // class RtfParserLoggerSettings

} // namespace Itenso.Rtf.Parser
// -- EOF -------------------------------------------------------------------
