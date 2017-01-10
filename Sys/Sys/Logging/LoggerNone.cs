// -- FILE ------------------------------------------------------------------
// name       : LoggerNone.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerNone : LoggerBase, ILogger
	{

		// ----------------------------------------------------------------------
		public LoggerLevel Level
		{
			get { return LoggerLevel.Fatal; }
			set {}
		} // Level

		// ----------------------------------------------------------------------
		public bool IsDebugEnabled
		{
			get { return false; }
		} // IsDebugEnabled

		// ----------------------------------------------------------------------
		public bool IsInfoEnabled
		{
			get { return false; }
		} // IsInfoEnabled

		// ----------------------------------------------------------------------
		public bool IsWarnEnabled
		{
			get { return false; }
		} // IsWarnEnabled

		// ----------------------------------------------------------------------
		public bool IsErrorEnabled
		{
			get { return false; }
		} // IsErrorEnabled

		// ----------------------------------------------------------------------
		public bool IsFatalEnabled
		{
			get { return false; }
		} // IsFatalEnabled

		// ----------------------------------------------------------------------
		public bool IsEnabledFor( LoggerLevel level )
		{
			return false;
		} // IsEnabledFor

		// ----------------------------------------------------------------------
		public void Debug( object message )
		{
		} // Debug

		// ----------------------------------------------------------------------
		public void Debug( object message, Exception exception )
		{
		} // Debug

		// ----------------------------------------------------------------------
		public void DebugFormat( string format, params object[] args )
		{
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void DebugFormat( IFormatProvider provider, string format, params object[] args )
		{
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void Info( object message )
		{
		} // Info

		// ----------------------------------------------------------------------
		public void Info( object message, Exception exception )
		{
		} // Info

		// ----------------------------------------------------------------------
		public void InfoFormat( string format, params object[] args )
		{
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void InfoFormat( IFormatProvider provider, string format, params object[] args )
		{
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void Warn( object message )
		{
		} // Warn

		// ----------------------------------------------------------------------
		public void Warn( object message, Exception exception )
		{
		} // Warn

		// ----------------------------------------------------------------------
		public void WarnFormat( string format, params object[] args )
		{
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void WarnFormat( IFormatProvider provider, string format, params object[] args )
		{
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void Error( object message )
		{
		} // Error

		// ----------------------------------------------------------------------
		public void Error( object message, Exception exception )
		{
		} // Error

		// ----------------------------------------------------------------------
		public void ErrorFormat( string format, params object[] args )
		{
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void ErrorFormat( IFormatProvider provider, string format, params object[] args )
		{
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void Fatal( object message )
		{
		} // Fatal

		// ----------------------------------------------------------------------
		public void Fatal( object message, Exception exception )
		{
		} // Fatal

		// ----------------------------------------------------------------------
		public void FatalFormat( string format, params object[] args )
		{
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void FatalFormat( IFormatProvider provider, string format, params object[] args )
		{
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel level, object message )
		{
		} // Log

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel level, object message, Exception exception )
		{
		} // Log

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel level, string format, params object[] args )
		{
		} // LogFormat

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel level, IFormatProvider provider, string format, params object[] args )
		{
		} // LogFormat

		// ----------------------------------------------------------------------
		protected override ILogger Logger
		{
			get { return this; }
		} // Logger

	} // class LoggerNone

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
