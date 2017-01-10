// -- FILE ------------------------------------------------------------------
// name       : IRtfHtmlCssStyleCollection.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.08
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Collections;

namespace RtfPipe.Converter.Html
{

	// ------------------------------------------------------------------------
	public interface IRtfHtmlCssStyleCollection : IEnumerable
	{

		// ----------------------------------------------------------------------
		int Count { get; }

		// ----------------------------------------------------------------------
		IRtfHtmlCssStyle this[ int index ] { get; }

		// ----------------------------------------------------------------------
		bool Contains( string selectorName );

		// ----------------------------------------------------------------------
		void CopyTo( IRtfHtmlCssStyle[] array, int index );

	} // interface IRtfHtmlCssStyleCollection

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
