using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

	public interface IRtfHtmlCssStyleCollection : IEnumerable<IRtfHtmlCssStyle>
	{

		int Count { get; }

		IRtfHtmlCssStyle this[ int index ] { get; }

		bool Contains( string selectorName );

		void CopyTo( IRtfHtmlCssStyle[] array, int index );

	}

}

