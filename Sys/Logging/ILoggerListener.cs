// -- FILE ------------------------------------------------------------------
// name       : ILoggerListener.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	public interface ILoggerListener
	{

		// ----------------------------------------------------------------------
		/// <summary>
		/// Called by an <code>ILoggerMonitor</code> to handle an event.
		/// </summary>
		/// <param name="loggerEvent">the event to handle</param>
		void Handle( ILoggerEvent loggerEvent );

	} // interface ILoggerListener

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
