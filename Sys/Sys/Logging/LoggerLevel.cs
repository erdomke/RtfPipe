// -- FILE ------------------------------------------------------------------
// name       : LoggerLevel.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.05
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using Itenso.Sys.Collection;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	public enum LoggerLevel
	{
		Fatal = 0,
		Error = 1,
		Warn = 2,
		Info = 3,
		Debug = 4
	} // enum LoggerLevel

	// ------------------------------------------------------------------------
	public static class LoggerLevelEnumHelper
	{

		// ----------------------------------------------------------------------
		public static LoggerLevel Parse( string value )
		{
			return (LoggerLevel)CollectionTool.ParseEnumValue( typeof( LoggerLevel ), value, true );
		} // Parse

		// ----------------------------------------------------------------------
		public static string Format( LoggerLevel value )
		{
			return value.ToString();
		} // Format

		// ----------------------------------------------------------------------
		public static string PossibleValues()
		{
			return CollectionTool.EnumValuesToString( typeof( LoggerLevel ) );
		} // PossibleValues

	} // class LoggerLevelEnumHelper

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
