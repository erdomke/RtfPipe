// project    : System Framelet
namespace RtfPipe.Sys.Logging
{

	public interface ILoggerListener
	{

		/// <summary>
		/// Called by an <code>ILoggerMonitor</code> to handle an event.
		/// </summary>
		/// <param name="loggerEvent">the event to handle</param>
		void Handle( ILoggerEvent loggerEvent );

	}

}

