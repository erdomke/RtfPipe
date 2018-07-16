// project    : System Framelet
namespace RtfPipe.Sys.Logging
{

	internal sealed class LoggerMonitorNone : ILoggerMonitor
	{

		public void Register( ILoggerListener loggerListener, string context )
		{
			logger.Warn( "ignoring registration of " + loggerListener + " for context " + context );
		}

		public void Unregister( ILoggerListener loggerListener, string context )
		{
			logger.Warn( "ignoring unregistration of " + loggerListener + " for context " + context );
		}

		private static readonly ILogger logger = Logger.GetLogger( typeof( LoggerMonitorNone ) );

	}

}

