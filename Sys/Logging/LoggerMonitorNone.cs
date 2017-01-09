// -- FILE ------------------------------------------------------------------
// name       : LoggerMonitorNone.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerMonitorNone : ILoggerMonitor
	{

		// ----------------------------------------------------------------------
		public void Register( ILoggerListener loggerListener, string context )
		{
			logger.Warn( "ignoring registration of " + loggerListener + " for context " + context );
		} // Register

		// ----------------------------------------------------------------------
		public void Unregister( ILoggerListener loggerListener, string context )
		{
			logger.Warn( "ignoring unregistration of " + loggerListener + " for context " + context );
		} // Unregister

		// ----------------------------------------------------------------------
		// members
		private static readonly ILogger logger = Logger.GetLogger( typeof( LoggerMonitorNone ) );

	} // class LoggerMonitorNone

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
