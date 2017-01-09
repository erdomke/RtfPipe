// -- FILE ------------------------------------------------------------------
// name       : LoggerMonitorAppender.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using log4net.Core;
using log4net.Appender;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerMonitorAppender : AppenderSkeleton
	{

		// ----------------------------------------------------------------------
		protected override void Append( LoggingEvent loggingEvent )
		{
			LoggerMonitorLog4net monitor = Logger.Monitor as LoggerMonitorLog4net;
			if ( monitor != null )
			{
				LoggerLevel level = LoggerLevel.Fatal;
				int loggingEventLevelValue = loggingEvent.Level.Value;
				if ( loggingEventLevelValue <= Level.Debug.Value )
				{
					level = LoggerLevel.Debug;
				}
				else if ( loggingEventLevelValue <= Level.Info.Value )
				{
					level = LoggerLevel.Info;
				}
				else if ( loggingEventLevelValue <= Level.Warn.Value )
				{
					level = LoggerLevel.Warn;
				}
				else if ( loggingEventLevelValue <= Level.Error.Value )
				{
					level = LoggerLevel.Error;
				}

				string message = "" + loggingEvent.MessageObject;
				string source = loggingEvent.LoggerName;
				string context = Logger.GetLogger( source ).Context;
				Exception caughtException = loggingEvent.ExceptionObject;

				LoggerEvent loggerEvent = new LoggerEvent( level, source, context, message, caughtException );
				monitor.Handle( loggerEvent );
			}
		} // Append

	} // class LoggerMonitorAppender

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
