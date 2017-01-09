// -- FILE ------------------------------------------------------------------
// name       : LoggerEvent.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	public sealed class LoggerEvent : ILoggerEvent
	{

		// ----------------------------------------------------------------------
		public LoggerEvent( LoggerLevel level, string source, string context, string message, Exception caughtException )
		{
			int levelValue = (int)level;
			this.level = levelValue < 0 ? LoggerLevel.Fatal : ( levelValue > 4 ? LoggerLevel.Fatal : level );
			this.source = ArgumentCheck.NonemptyTrimmedString( source, "source" );
			this.context = context ?? string.Empty;
			this.message = message ?? string.Empty;
			this.caughtException = caughtException;
		} // LoggerEvent

		// ----------------------------------------------------------------------
		public LoggerLevel Level
		{
			get { return level; }
		} // Level

		// ----------------------------------------------------------------------
		public string Source
		{
			get { return source; }
		} // Source

		// ----------------------------------------------------------------------
		public string Context
		{
			get { return context; }
		} // Context

		// ----------------------------------------------------------------------
		public string Message
		{
			get { return message; }
		} // Message

		// ----------------------------------------------------------------------
		public Exception CaughtException
		{
			get { return caughtException; }
		} // CaughtException

		// ----------------------------------------------------------------------
		// members
		private readonly LoggerLevel level;
		private readonly string source;
		private readonly string context;
		private readonly string message;
		private readonly Exception caughtException;

	} // class LoggerEvent

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
