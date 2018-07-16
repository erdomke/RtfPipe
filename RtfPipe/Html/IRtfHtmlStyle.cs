namespace RtfPipe.Converter.Html
{

	public interface IRtfHtmlStyle
	{

		string ForegroundColor { get; set; }

		string BackgroundColor { get; set; }

		string FontFamily { get; set; }

		string FontSize { get; set; }

		bool IsEmpty { get; }

	}

}

