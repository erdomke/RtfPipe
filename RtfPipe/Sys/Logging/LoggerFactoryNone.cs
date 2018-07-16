// project    : System Framelet
namespace RtfPipe.Sys.Logging
{

	internal sealed class LoggerFactoryNone : LoggerFactory
	{

		public override ILogger GetLogger( string name )
		{
			if ( logger == null )
			{
				lock ( mutex )
				{
					if ( logger == null )
					{
						logger = new LoggerNone();
					}
				}
			}
			return logger;
		}

		private static readonly object mutex = new object();
		private static volatile LoggerNone logger;

	}

}

