namespace RtfPipe.Parser
{

	public class RtfParserLoggerSettings
	{

		public RtfParserLoggerSettings() :
			this( true )
		{
		}

		public RtfParserLoggerSettings( bool enabled )
		{
			Enabled = enabled;
		}

		public bool Enabled { get; set; }

		public string ParseBeginText
		{
			get { return parseBeginText; }
			set { parseBeginText = value; }
		}

		public string ParseEndText
		{
			get { return parseEndText; }
			set { parseEndText = value; }
		}

		public string ParseGroupBeginText
		{
			get { return parseGroupBeginText; }
			set { parseGroupBeginText = value; }
		}

		public string ParseGroupEndText
		{
			get { return parseGroupEndText; }
			set { parseGroupEndText = value; }
		}

		public string ParseTagText
		{
			get { return parseTagText; }
			set { parseTagText = value; }
		}

		public string TextOverflowText
		{
			get { return textOverflowText; }
			set { textOverflowText = value; }
		}

		public string ParseTextText
		{
			get { return parseTextText; }
			set { parseTextText = value; }
		}

		public string ParseSuccessText
		{
			get { return parseSuccessText; }
			set { parseSuccessText = value; }
		}

		public string ParseFailKnownReasonText
		{
			get { return parseFailKnownReasonText; }
			set { parseFailKnownReasonText = value; }
		}

		public string ParseFailUnknownReasonText
		{
			get { return parseFailUnknownReasonText; }
			set { parseFailUnknownReasonText = value; }
		}

		public int TextMaxLength
		{
			get { return textMaxLength; }
			set { textMaxLength = value; }
		}

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

	}

}

