using System.Collections.Generic;
using System.Collections.Specialized;

namespace RtfPipe.Converter.Html
{

	public interface IRtfHtmlCssStyle
	{

		IDictionary<string, string> Properties { get; }

		string SelectorName { get; }

	}

}

