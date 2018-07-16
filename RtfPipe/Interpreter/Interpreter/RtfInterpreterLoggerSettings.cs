namespace RtfPipe.Interpreter
{

	public class RtfInterpreterLoggerSettings
	{

		public RtfInterpreterLoggerSettings() :
			this( true )
		{
		}

		public RtfInterpreterLoggerSettings( bool enabled )
		{
			Enabled = enabled;
		}

		public bool Enabled { get; set; }

		public string BeginDocumentText
		{
			get { return beginDocumentText; }
			set { beginDocumentText = value; }
		}

		public string EndDocumentText
		{
			get { return endDocumentText; }
			set { endDocumentText = value; }
		}

		public string TextFormatText
		{
			get { return textFormatText; }
			set { textFormatText = value; }
		}

		public string TextOverflowText
		{
			get { return textOverflowText; }
			set { textOverflowText = value; }
		}

		public string SpecialCharFormatText
		{
			get { return specialCharFormatText; }
			set { specialCharFormatText = value; }
		}

		public string BreakFormatText
		{
			get { return breakFormatText; }
			set { breakFormatText = value; }
		}

		public string ImageFormatText
		{
			get { return imageFormatText; }
			set { imageFormatText = value; }
		}

		public int TextMaxLength
		{
			get { return textMaxLength; }
			set { textMaxLength = value; }
		}

		private string beginDocumentText = Strings.LogBeginDocument;
		private string endDocumentText = Strings.LogEndDocument;
		private string textFormatText = Strings.LogInsertText;
		private string textOverflowText = Strings.LogOverflowText;
		private string specialCharFormatText = Strings.LogInsertChar;
		private string breakFormatText = Strings.LogInsertBreak;
		private string imageFormatText = Strings.LogInsertImage;

		private int textMaxLength = 80;

	}

}

