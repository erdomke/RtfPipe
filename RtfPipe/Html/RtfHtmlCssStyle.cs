using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace RtfPipe.Converter.Html
{

	public class RtfHtmlCssStyle : IRtfHtmlCssStyle
	{

		public RtfHtmlCssStyle( string selectorName )
		{
			if ( selectorName == null )
			{
				throw new ArgumentNullException( "selectorName" );
			}
			this.selectorName = selectorName;
		}

		public IDictionary<string, string> Properties
		{
			get { return properties; }
		}

		public string SelectorName
		{
			get { return selectorName; }
		}

		private readonly IDictionary<string, string> properties = new Dictionary<string, string>();
		private readonly string selectorName;

	}

}

