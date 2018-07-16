namespace RtfPipe.Converter.Html
{

	public class RtfEmptyHtmlStyleConverter : IRtfHtmlStyleConverter
	{

		public virtual IRtfHtmlStyle TextToHtml( IRtfVisualText visualText )
		{
			return RtfHtmlStyle.Empty;
		}

	}

}

