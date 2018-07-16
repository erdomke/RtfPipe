// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{

	internal sealed class LoggerContextDisposable : IDisposable
	{

		public LoggerContextDisposable( ILogger logger )
		{
			this.logger = logger;
		}

		void IDisposable.Dispose()
		{
			if ( logger != null )
			{
				logger.PopContext();
			}
		}

		private readonly ILogger logger;

	}

}

