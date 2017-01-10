// -- FILE ------------------------------------------------------------------
// name       : IRtfHtmlStyleConverter.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.07.13
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace RtfPipe.Converter.Html
{

	// ------------------------------------------------------------------------
	public interface IRtfHtmlStyleConverter
	{

		// ----------------------------------------------------------------------
		IRtfHtmlStyle TextToHtml( IRtfVisualText visualText );

	} // interface IRtfHtmlStyleConverter

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
