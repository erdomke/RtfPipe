// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{

	public abstract class LoggerImplBase : LoggerBase, ILogger
	{

		protected LoggerImplBase()
		{
			level = LoggerLevel.Warn;
		}

		protected LoggerImplBase( LoggerLevel level )
		{
			this.level = level;
		}

		public LoggerLevel Level
		{
			get { return level; }
			set { level = value; }
		}

		public bool IsDebugEnabled
		{
			get { return LoggerLevel.Debug.CompareTo( level ) <= 0; }
		}

		public bool IsInfoEnabled
		{
			get { return LoggerLevel.Info.CompareTo( level ) <= 0; }
		}

		public bool IsWarnEnabled
		{
			get { return LoggerLevel.Warn.CompareTo( level ) <= 0; }
		}

		public bool IsErrorEnabled
		{
			get { return LoggerLevel.Error.CompareTo( level ) <= 0; }
		}

		public bool IsFatalEnabled
		{
			get { return LoggerLevel.Fatal.CompareTo( level ) <= 0; }
		}

		public bool IsEnabledFor( LoggerLevel loggerLevel )
		{
			return loggerLevel.CompareTo( level ) <= 0;
		}

		public virtual void Debug( object message )
		{
			Log( LoggerLevel.Debug, message );
		}

		public virtual void Debug( object message, Exception exception )
		{
			Log( LoggerLevel.Debug, message, exception );
		}

		public virtual void DebugFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Debug, format, args );
		}

		public virtual void DebugFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Debug, provider, format, args );
		}

		public virtual void Info( object message )
		{
			Log( LoggerLevel.Info, message );
		}

		public virtual void Info( object message, Exception exception )
		{
			Log( LoggerLevel.Info, message, exception );
		}

		public virtual void InfoFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Info, format, args );
		}

		public virtual void InfoFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Info, provider, format, args );
		}

		public virtual void Warn( object message )
		{
			Log( LoggerLevel.Warn, message );
		}

		public virtual void Warn( object message, Exception exception )
		{
			Log( LoggerLevel.Warn, message, exception );
		}

		public virtual void WarnFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Warn, format, args );
		}

		public virtual void WarnFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Warn, provider, format, args );
		}

		public virtual void Error( object message )
		{
			Log( LoggerLevel.Error, message );
		}

		public virtual void Error( object message, Exception exception )
		{
			Log( LoggerLevel.Error, message, exception );
		}

		public virtual void ErrorFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Error, format, args );
		}

		public virtual void ErrorFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Error, provider, format, args );
		}

		public virtual void Fatal( object message )
		{
			Log( LoggerLevel.Fatal, message );
		}

		public virtual void Fatal( object message, Exception exception )
		{
			Log( LoggerLevel.Fatal, message, exception );
		}

		public virtual void FatalFormat( string format, params object[] args )
		{
			LogFormat( LoggerLevel.Fatal, format, args );
		}

		public virtual void FatalFormat( IFormatProvider provider, string format, params object[] args )
		{
			LogFormat( LoggerLevel.Fatal, provider, format, args );
		}

		public virtual void Log( LoggerLevel loggerLevel, object message )
		{
			Log( loggerLevel, message, null );
		}

		public virtual void Log( LoggerLevel loggerLevel, object message, Exception exception )
		{
			if ( IsEnabledFor( loggerLevel ) )
			{
				Output( loggerLevel, message, exception );
			}
		}

		public virtual void LogFormat( LoggerLevel loggerLevel, string format, params object[] args )
		{
			Log( loggerLevel, string.Format( format, args ) );
		}

		public virtual void LogFormat( LoggerLevel loggerLevel, IFormatProvider provider, string format, params object[] args )
		{
			Log( loggerLevel, string.Format( provider, format, args ) );
		}

		protected abstract void Output( LoggerLevel level, object message, Exception exception );

		protected sealed override ILogger Logger
		{
			get { return this; }
		}

		private LoggerLevel level;

	}

}

