// -- FILE ------------------------------------------------------------------
// name       : RtfXmlConvertSettings.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.10
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace Itenso.Rtf.Converter.Xml
{

	// ------------------------------------------------------------------------
	public class RtfXmlConvertSettings
	{

		// ----------------------------------------------------------------------
		public RtfXmlConvertSettings() :
			this( null, null )
		{
		} // RtfXmlConvertSettings

		// ----------------------------------------------------------------------
		public RtfXmlConvertSettings( string ns ) :
			this( null, ns )
		{
		} // RtfXmlConvertSettings

		// ----------------------------------------------------------------------
		public RtfXmlConvertSettings( string prefix, string ns )
		{
			Prefix = prefix;
			Ns = ns;
		} // RtfXmlConvertSettings

		// ----------------------------------------------------------------------
		public string Prefix { get; set; }

		// ----------------------------------------------------------------------
		public string Ns { get; set; }

		// ----------------------------------------------------------------------
		public bool IsShowHiddenText { get; set; }

	} // class RtfXmlConvertSettings

} // namespace Itenso.Rtf.Converter.Xml
// -- EOF -------------------------------------------------------------------
