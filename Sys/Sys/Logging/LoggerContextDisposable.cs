// -- FILE ------------------------------------------------------------------
// name       : LoggerContextDisposable.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.11
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerContextDisposable : IDisposable
	{

		// ----------------------------------------------------------------------
		public LoggerContextDisposable( ILogger logger )
		{
			this.logger = logger;
		} // LoggerContextDisposable

		// ----------------------------------------------------------------------
		void IDisposable.Dispose()
		{
			if ( logger != null )
			{
				logger.PopContext();
			}
		} // Dispose

		// ----------------------------------------------------------------------
		// members
		private readonly ILogger logger;

	} // class LoggerContextDisposable

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
