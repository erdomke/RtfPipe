// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{

	public sealed class LoggerEvent : ILoggerEvent
	{

		public LoggerEvent( LoggerLevel level, string source, string context, string message, Exception caughtException )
		{
			int levelValue = (int)level;
			this.level = levelValue < 0 ? LoggerLevel.Fatal : ( levelValue > 4 ? LoggerLevel.Fatal : level );
			this.source = ArgumentCheck.NonemptyTrimmedString( source, "source" );
			this.context = context ?? string.Empty;
			this.message = message ?? string.Empty;
			this.caughtException = caughtException;
		}

		public LoggerLevel Level
		{
			get { return level; }
		}

		public string Source
		{
			get { return source; }
		}

		public string Context
		{
			get { return context; }
		}

		public string Message
		{
			get { return message; }
		}

		public Exception CaughtException
		{
			get { return caughtException; }
		}

		private readonly LoggerLevel level;
		private readonly string source;
		private readonly string context;
		private readonly string message;
		private readonly Exception caughtException;

	}

}

