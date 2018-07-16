using System;

namespace RtfPipe.Converter.Text
{

	public class RtfTextConvertSettings
	{

		public const string SeparatorCr = "\r";
		public const string SeparatorLf = "\n";
		public const string SeparatorCrLf = "\r\n";
		public const string SeparatorLfCr = "\n\r";

		public RtfTextConvertSettings() :
			this( SeparatorCrLf )
		{
		}

		public RtfTextConvertSettings( string breakText )
		{
			SetBreakText( breakText );
		}

		public bool IsShowHiddenText { get; set; }

		public string TabulatorText
		{
			get { return tabulatorText; }
			set { tabulatorText = value; }
		}

		public string NonBreakingSpaceText
		{
			get { return nonBreakingSpaceText; }
			set { nonBreakingSpaceText = value; }
		}

		public string EmSpaceText
		{
			get { return emSpaceText; }
			set { emSpaceText = value; }
		}

		public string EnSpaceText
		{
			get { return enSpaceText; }
			set { enSpaceText = value; }
		}

		public string QmSpaceText
		{
			get { return qmSpaceText; }
			set { qmSpaceText = value; }
		}

		public string EmDashText
		{
			get { return emDashText; }
			set { emDashText = value; }
		}

		public string EnDashText
		{
			get { return enDashText; }
			set { enDashText = value; }
		}

		public string OptionalHyphenText
		{
			get { return optionalHyphenText; }
			set { optionalHyphenText = value; }
		}

		public string NonBreakingHyphenText
		{
			get { return nonBreakingHyphenText; }
			set { nonBreakingHyphenText = value; }
		}

		public string BulletText
		{
			get { return bulletText; }
			set { bulletText = value; }
		}

		public string LeftSingleQuoteText
		{
			get { return leftSingleQuoteText; }
			set { leftSingleQuoteText = value; }
		}

		public string RightSingleQuoteText
		{
			get { return rightSingleQuoteText; }
			set { rightSingleQuoteText = value; }
		}

		public string LeftDoubleQuoteText
		{
			get { return leftDoubleQuoteText; }
			set { leftDoubleQuoteText = value; }
		}

		public string RightDoubleQuoteText
		{
			get { return rightDoubleQuoteText; }
			set { rightDoubleQuoteText = value; }
		}

		public string UnknownSpecialCharText { get; set; }

		public string LineBreakText
		{
			get { return lineBreakText; }
			set { lineBreakText = value; }
		}

		public string PageBreakText
		{
			get { return pageBreakText; }
			set { pageBreakText = value; }
		}

		public string ParagraphBreakText
		{
			get { return paragraphBreakText; }
			set { paragraphBreakText = value; }
		}

		public string SectionBreakText
		{
			get { return sectionBreakText; }
			set { sectionBreakText = value; }
		}

		public string UnknownBreakText
		{
			get { return unknownBreakText; }
			set { unknownBreakText = value; }
		}

		public string ImageFormatText
		{
			get { return imageFormatText; }
			set { imageFormatText = value; }
		}

		public void SetBreakText( string breakText )
		{
			if ( breakText == null )
			{
				throw new ArgumentNullException( "breakText" );
			}

			lineBreakText = breakText;
			pageBreakText = breakText + breakText;
			paragraphBreakText = breakText;
			sectionBreakText = breakText + breakText;
			unknownBreakText = breakText;
		}

		// members: hidden text

		// members: special chars
		private string tabulatorText = "\t";
		private string nonBreakingSpaceText = " ";
		private string emSpaceText = " ";
		private string enSpaceText = " ";
		private string qmSpaceText = " ";
		private string emDashText = "-";
		private string enDashText = "-";
		private string optionalHyphenText = "-";
		private string nonBreakingHyphenText = "-";
		private string bulletText = "°";
		private string leftSingleQuoteText = "`";
		private string rightSingleQuoteText = "´";
		private string leftDoubleQuoteText = "``";
		private string rightDoubleQuoteText = "´´";

		// members: breaks
		private string lineBreakText;
		private string pageBreakText;
		private string paragraphBreakText;
		private string sectionBreakText;
		private string unknownBreakText;

		// members: image
		private string imageFormatText = Strings.ImageFormatText;

	}

}

