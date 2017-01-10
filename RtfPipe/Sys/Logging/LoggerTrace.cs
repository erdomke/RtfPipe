// -- FILE ------------------------------------------------------------------
// name       : LoggerTrace.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace RtfPipe.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerTrace : LoggerBase, ILogger
	{

		// ----------------------------------------------------------------------
		public LoggerTrace( string name )
		{
			this.name = ArgumentCheck.NonemptyTrimmedString( name, Strings.LoggerNameMayNotBeEmpty, "name" );
			level = new TraceSwitch( name, "trace switch for " + name + " from the application config file" );
		} // LoggerTrace

		// ----------------------------------------------------------------------
		public LoggerLevel Level
		{
			get { return TraceLevelToLoggerLevel( level.Level ); }
			set
			{
				lock ( level )
				{
					level.Level = LoggerLevelToTraceLevel( value );
				}
			}
		} // Level

		// ----------------------------------------------------------------------
		public bool IsDebugEnabled
		{
			get { return level.TraceVerbose; }
		} // IsDebugEnabled

		// ----------------------------------------------------------------------
		public bool IsInfoEnabled
		{
			get { return level.TraceInfo; }
		} // IsInfoEnabled

		// ----------------------------------------------------------------------
		public bool IsWarnEnabled
		{
			get { return level.TraceWarning; }
		} // IsWarnEnabled

		// ----------------------------------------------------------------------
		public bool IsErrorEnabled
		{
			get { return level.TraceError; }
		} // IsErrorEnabled

		// ----------------------------------------------------------------------
		public bool IsFatalEnabled
		{
			get { return level.Level != TraceLevel.Off; }
		} // IsFatalEnabled

		// ----------------------------------------------------------------------
		public bool IsEnabledFor( LoggerLevel loggerLevel )
		{
			bool enabled = false;
			switch ( loggerLevel )
			{
				case LoggerLevel.Debug:
					enabled = IsDebugEnabled;
					break;
				case LoggerLevel.Info:
					enabled = IsInfoEnabled;
					break;
				case LoggerLevel.Warn:
					enabled = IsWarnEnabled;
					break;
				case LoggerLevel.Error:
					enabled = IsErrorEnabled;
					break;
				case LoggerLevel.Fatal:
					enabled = IsFatalEnabled;
					break;
			}
			return enabled;
		} // IsEnabledFor

		// ----------------------------------------------------------------------
		public void Debug( object message )
		{
			Log( logLevelDebug, message, null );
		} // Debug

		// ----------------------------------------------------------------------
		public void Debug( object message, Exception exception )
		{
			Log( logLevelDebug, message, exception );
		} // Debug

		// ----------------------------------------------------------------------
		public void DebugFormat( string format, params object[] args )
		{
			LogFormat( logLevelDebug, null, format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void DebugFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( logLevelDebug, provider, format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void Info( object message )
		{
			Log( logLevelInfo, message, null );
		} // Info

		// ----------------------------------------------------------------------
		public void Info( object message, Exception exception )
		{
			Log( logLevelInfo, message, exception );
		} // Info

		// ----------------------------------------------------------------------
		public void InfoFormat( string format, params object[] args )
		{
			LogFormat( logLevelInfo, null, format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void InfoFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( logLevelInfo, provider, format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void Warn( object message )
		{
			Log( logLevelWarn, message, null );
		} // Warn

		// ----------------------------------------------------------------------
		public void Warn( object message, Exception exception )
		{
			Log( logLevelWarn, message, exception );
		} // Warn

		// ----------------------------------------------------------------------
		public void WarnFormat( string format, params object[] args )
		{
			LogFormat( logLevelWarn, null, format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void WarnFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( logLevelWarn, provider, format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void Error( object message )
		{
			Log( logLevelError, message, null );
		} // Error

		// ----------------------------------------------------------------------
		public void Error( object message, Exception exception )
		{
			Log( logLevelError, message, exception );
		} // Error

		// ----------------------------------------------------------------------
		public void ErrorFormat( string format, params object[] args )
		{
			LogFormat( logLevelError, null, format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void ErrorFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( logLevelError, provider, format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void Fatal( object message )
		{
			Log( logLevelFatal, message, null );
		} // Fatal

		// ----------------------------------------------------------------------
		public void Fatal( object message, Exception exception )
		{
			Log( logLevelFatal, message, exception );
		} // Fatal

		// ----------------------------------------------------------------------
		public void FatalFormat( string format, params object[] args )
		{
			LogFormat( logLevelFatal, null, format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void FatalFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( logLevelFatal, provider, format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel loggerLevel, object message )
		{
			Log( LoggerLevelToTraceLevel( loggerLevel ), message, null );
		} // Log

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel loggerLevel, object message, Exception exception )
		{
			Log( LoggerLevelToTraceLevel( loggerLevel ), message, exception );
		} // Log

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel loggerLevel, string format, params object[] args )
		{
			LogFormat( LoggerLevelToTraceLevel( loggerLevel ), null, format, args );
		} // LogFormat

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel loggerLevel, IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevelToTraceLevel( loggerLevel ), provider, format, args );
		} // LogFormat

		// ----------------------------------------------------------------------
		protected override ILogger Logger
		{
			get { return this; }
		} // Logger

		// ----------------------------------------------------------------------
		private void Log( TraceLevel logLevel, object message, Exception exception )
		{
			if ( !IsSupportedException( exception ) )
			{
				return;
			}

			if ( level.Level >= logLevel )
			{
				StringBuilder buf = new StringBuilder( name );
				buf.Append( ": " );
				switch ( logLevel )
				{
					case TraceLevel.Off:
						buf.Append( "Fatal" );
						break;
					case TraceLevel.Verbose:
						buf.Append( "Debug" );
						break;
					default:
						buf.Append( logLevel.ToString() );
						break;
				}
				buf.Append( ": " );
				if ( message != null )
				{
					buf.Append( message );
				}
				if ( exception != null )
				{
					buf.Append( ": " );
					buf.Append( exception.ToString() );
				}
				switch ( logLevel )
				{
					case TraceLevel.Off: // used by 'fatal'
					case TraceLevel.Error:
						Trace.TraceError( buf.ToString() );
						break;
					case TraceLevel.Warning:
						Trace.TraceWarning( buf.ToString() );
						break;
					case TraceLevel.Info:
						goto default;
					case TraceLevel.Verbose:
						goto default;
					default:
						Trace.TraceInformation( buf.ToString() );
						break;
				}
			}
		} // Log

		// ----------------------------------------------------------------------
		private void LogFormat( TraceLevel logLevel, IFormatProvider provider, string format, params object[] args )
		{
			if ( level.Level >= logLevel )
			{
				IFormatProvider formatter = provider ?? CultureInfo.InvariantCulture;
				try
				{
					string message = string.Format( formatter, format, args );
					Log( logLevel, message, null );
				}
				catch ( FormatException e )
				{
					Log( LoggerLevel.Fatal, "invalid log-message-format: " + e.Message, e );
					Log( LoggerLevel.Fatal, "original log-message was:" );
					Log( logLevel, format, null );
				}
			}
		} // LogFormat

		// ----------------------------------------------------------------------
		private static TraceLevel LoggerLevelToTraceLevel( LoggerLevel loggerLevel )
		{
			TraceLevel traceLevel = TraceLevel.Off;
			switch ( loggerLevel )
			{
				case LoggerLevel.Debug:
					traceLevel = TraceLevel.Verbose;
					break;
				case LoggerLevel.Info:
					traceLevel = TraceLevel.Info;
					break;
				case LoggerLevel.Warn:
					traceLevel = TraceLevel.Warning;
					break;
				case LoggerLevel.Error:
					traceLevel = TraceLevel.Error;
					break;
				case LoggerLevel.Fatal:
					traceLevel = TraceLevel.Off;
					break;
			}
			return traceLevel;
		} // LoggerLevelToTraceLevel

		// ----------------------------------------------------------------------
		private static LoggerLevel TraceLevelToLoggerLevel( TraceLevel traceLevel )
		{
			LoggerLevel loggerLevel = LoggerLevel.Fatal;
			switch ( traceLevel )
			{
				case TraceLevel.Verbose:
					loggerLevel = LoggerLevel.Debug;
					break;
				case TraceLevel.Info:
					loggerLevel = LoggerLevel.Info;
					break;
				case TraceLevel.Warning:
					loggerLevel = LoggerLevel.Warn;
					break;
				case TraceLevel.Error:
					loggerLevel = LoggerLevel.Error;
					break;
				case TraceLevel.Off:
					loggerLevel = LoggerLevel.Fatal;
					break;
			}
			return loggerLevel;
		} // TraceLevelToLoggerLevel

		// ----------------------------------------------------------------------
		// members
		private const TraceLevel logLevelDebug = TraceLevel.Verbose;
		private const TraceLevel logLevelInfo = TraceLevel.Info;
		private const TraceLevel logLevelWarn = TraceLevel.Warning;
		private const TraceLevel logLevelError = TraceLevel.Error;
		private const TraceLevel logLevelFatal = TraceLevel.Off; // yes, this is used as log-level ...

		private readonly string name;
		private readonly TraceSwitch level;

	} // class LoggerTrace

} // namespace RtfPipe.Sys.Logging
// -- EOF -------------------------------------------------------------------
