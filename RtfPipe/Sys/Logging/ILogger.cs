// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{

	/// <summary>
	/// Public wrapper access to different logger implementations. All code in the
	/// framework should only use this interface. Instances can be gotten from the
	/// <see cref="Logger"/> class. This makes it easy to change the underlying
	/// logging facility without having to adapt all the using code (e.g. when
	/// switching from an own implementation to log4net or something the like).
	/// </summary>
	public interface ILogger
	{

		LoggerLevel Level { get; set; }

		bool IsDebugEnabled { get; }

		bool IsInfoEnabled { get; }

		bool IsWarnEnabled { get; }

		bool IsErrorEnabled { get; }

		bool IsFatalEnabled { get; }

		bool IsEnabledFor( LoggerLevel level );

		bool IsSupportedException( Exception exception );

		void Debug( object message );

		void Debug( object message, Exception exception );

		void DebugFormat( string format, params object[] args );

		void DebugFormat( IFormatProvider provider, string format, params object[] args );

		void Info( object message );

		void Info( object message, Exception exception );

		void InfoFormat( string format, params object[] args );

		void InfoFormat( IFormatProvider provider, string format, params object[] args );

		void Warn( object message );

		void Warn( object message, Exception exception );

		void WarnFormat( string format, params object[] args );

		void WarnFormat( IFormatProvider provider, string format, params object[] args );

		void Error( object message );

		void Error( object message, Exception exception );

		void ErrorFormat( string format, params object[] args );

		void ErrorFormat( IFormatProvider provider, string format, params object[] args );

		void Fatal( object message );

		void Fatal( object message, Exception exception );

		void FatalFormat( string format, params object[] args );

		void FatalFormat( IFormatProvider provider, string format, params object[] args );

		void Log( LoggerLevel level, object message );

		void Log( LoggerLevel level, object message, Exception exception );

		void LogFormat( LoggerLevel level, string format, params object[] args );

		void LogFormat( LoggerLevel level, IFormatProvider provider, string format, params object[] args );

		IDisposable PushContext( string context );

		int ContextDepth { get; }

		string Context { get; }

		void PopContext();

	}

}

