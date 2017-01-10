// -- FILE ------------------------------------------------------------------
// name       : RtfHtmlCssStyle.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.08
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace RtfPipe.Converter.Html
{

	// ------------------------------------------------------------------------
	public class RtfHtmlCssStyle : IRtfHtmlCssStyle
	{

		// ----------------------------------------------------------------------
		public RtfHtmlCssStyle( string selectorName )
		{
			if ( selectorName == null )
			{
				throw new ArgumentNullException( "selectorName" );
			}
			this.selectorName = selectorName;
		} // RtfHtmlCssStyle

		// ----------------------------------------------------------------------
		public IDictionary<string, string> Properties
		{
			get { return properties; }
		} // Properties

		// ----------------------------------------------------------------------
		public string SelectorName
		{
			get { return selectorName; }
		} // SelectorName

		// ----------------------------------------------------------------------
		// members
		private readonly IDictionary<string, string> properties = new Dictionary<string, string>();
		private readonly string selectorName;

	} // class RtfHtmlCssStyle

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
