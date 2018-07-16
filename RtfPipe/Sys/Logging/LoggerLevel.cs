// project    : System Framelet
using RtfPipe.Sys.Collection;

namespace RtfPipe.Sys.Logging
{

	public enum LoggerLevel
	{
		Fatal = 0,
		Error = 1,
		Warn = 2,
		Info = 3,
		Debug = 4
	}

	public static class LoggerLevelEnumHelper
	{

		public static LoggerLevel Parse( string value )
		{
			return (LoggerLevel)CollectionTool.ParseEnumValue( typeof( LoggerLevel ), value, true );
		}

		public static string Format( LoggerLevel value )
		{
			return value.ToString();
		}

		public static string PossibleValues()
		{
			return "[]";
		}

	}

}

