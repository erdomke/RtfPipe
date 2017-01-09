// -- FILE ------------------------------------------------------------------
// name       : LoggerImplBase.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2008.05.14
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	public abstract class LoggerImplBase : LoggerBase, ILogger
	{

		// ----------------------------------------------------------------------
		protected LoggerImplBase()
		{
			level = LoggerLevel.Warn;
		} // LoggerImplBase

		// ----------------------------------------------------------------------
		protected LoggerImplBase( LoggerLevel level )
		{
			this.level = level;
		} // LoggerImplBase

		// ----------------------------------------------------------------------
		public LoggerLevel Level
		{
			get { return level; }
			set { level = value; }
		} // Level

		// ----------------------------------------------------------------------
		public bool IsDebugEnabled
		{
			get { return LoggerLevel.Debug.CompareTo( level ) <= 0; }
		} // IsDebugEnabled

		// ----------------------------------------------------------------------
		public bool IsInfoEnabled
		{
			get { return LoggerLevel.Info.CompareTo( level ) <= 0; }
		} // IsInfoEnabled

		// ----------------------------------------------------------------------
		public bool IsWarnEnabled
		{
			get { return LoggerLevel.Warn.CompareTo( level ) <= 0; }
		} // IsWarnEnabled

		// ----------------------------------------------------------------------
		public bool IsErrorEnabled
		{
			get { return LoggerLevel.Error.CompareTo( level ) <= 0; }
		} // IsErrorEnabled

		// ----------------------------------------------------------------------
		public bool IsFatalEnabled
		{
			get { return LoggerLevel.Fatal.CompareTo( level ) <= 0; }
		} // IsFatalEnabled

		// ----------------------------------------------------------------------
		public bool IsEnabledFor( LoggerLevel loggerLevel )
		{
			return loggerLevel.CompareTo( level ) <= 0;
		} // IsEnabledFor

		// ----------------------------------------------------------------------
		public virtual void Debug( object message )
		{
			Log( LoggerLevel.Debug, message );
		} // Debug

		// ----------------------------------------------------------------------
		public virtual void Debug( object message, Exception exception )
		{
			Log( LoggerLevel.Debug, message, exception );
		} // Debug

		// ----------------------------------------------------------------------
		public virtual void DebugFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Debug, format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public virtual void DebugFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Debug, provider, format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public virtual void Info( object message )
		{
			Log( LoggerLevel.Info, message );
		} // Info

		// ----------------------------------------------------------------------
		public virtual void Info( object message, Exception exception )
		{
			Log( LoggerLevel.Info, message, exception );
		} // Info

		// ----------------------------------------------------------------------
		public virtual void InfoFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Info, format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public virtual void InfoFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Info, provider, format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public virtual void Warn( object message )
		{
			Log( LoggerLevel.Warn, message );
		} // Warn

		// ----------------------------------------------------------------------
		public virtual void Warn( object message, Exception exception )
		{
			Log( LoggerLevel.Warn, message, exception );
		} // Warn

		// ----------------------------------------------------------------------
		public virtual void WarnFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Warn, format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public virtual void WarnFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Warn, provider, format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public virtual void Error( object message )
		{
			Log( LoggerLevel.Error, message );
		} // Error

		// ----------------------------------------------------------------------
		public virtual void Error( object message, Exception exception )
		{
			Log( LoggerLevel.Error, message, exception );
		} // Error

		// ----------------------------------------------------------------------
		public virtual void ErrorFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Error, format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public virtual void ErrorFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Error, provider, format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public virtual void Fatal( object message )
		{
			Log( LoggerLevel.Fatal, message );
		} // Fatal

		// ----------------------------------------------------------------------
		public virtual void Fatal( object message, Exception exception )
		{
			Log( LoggerLevel.Fatal, message, exception );
		} // Fatal

		// ----------------------------------------------------------------------
		public virtual void FatalFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Fatal, format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public virtual void FatalFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Fatal, provider, format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public virtual void Log( LoggerLevel loggerLevel, object message )
		{
			Log( loggerLevel, message, null );
		} // Log

		// ----------------------------------------------------------------------
		public virtual void Log( LoggerLevel loggerLevel, object message, Exception exception )
		{
			if ( IsEnabledFor( loggerLevel ) )
			{
				Output( loggerLevel, message, exception );
			}
		} // Log

		// ----------------------------------------------------------------------
		public virtual void LogFormat( LoggerLevel loggerLevel, string format, params object[] args )
		{
			Log( loggerLevel, string.Format( format, args ) );
		} // LogFormat

		// ----------------------------------------------------------------------
		public virtual void LogFormat( LoggerLevel loggerLevel, IFormatProvider provider, string format, params object[] args )
		{
			Log( loggerLevel, string.Format( provider, format, args ) );
		} // LogFormat

		// ----------------------------------------------------------------------
		protected abstract void Output( LoggerLevel level, object message, Exception exception );

		// ----------------------------------------------------------------------
		protected sealed override ILogger Logger
		{
			get { return this; }
		} // Logger

		// ----------------------------------------------------------------------
		// members
		private LoggerLevel level;

	} // class LoggerNone

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
