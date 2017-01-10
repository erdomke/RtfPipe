// -- FILE ------------------------------------------------------------------
// name       : ILoggerMonitor.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	/// <summary>
	/// Allows monitoring <code>ILogger</code> instances for events they generate.
	/// </summary>
	/// <remarks>
	/// A <code>ILoggerListener</code> may register itself for multiple contexts.
	/// When doing so it will need to unregister itself for each context for which
	/// it had registered itself previously.
	/// </remarks>
	public interface ILoggerMonitor
	{

		// ----------------------------------------------------------------------
		/// <summary>
		/// Registers a listener to handle events occurring in a given context.
		/// </summary>
		/// <param name="loggerListener">the listener to register</param>
		/// <param name="context">the context for which to register the listener</param>
		/// <exception cref="System.ArgumentNullException">when given a null argument</exception>
		/// <exception cref="System.ArgumentException">when given an empty context</exception>
		void Register( ILoggerListener loggerListener, string context );

		// ----------------------------------------------------------------------
		/// <summary>
		/// Unregisters a listener from handling events occurring in a given context.
		/// </summary>
		/// <param name="loggerListener">the listener to unregister</param>
		/// <param name="context">the context for which to unregister the listener</param>
		/// <exception cref="System.ArgumentNullException">when given a null argument</exception>
		/// <exception cref="System.ArgumentException">when given an empty context</exception>
		void Unregister( ILoggerListener loggerListener, string context );

	} // interface ILoggerMonitor

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
