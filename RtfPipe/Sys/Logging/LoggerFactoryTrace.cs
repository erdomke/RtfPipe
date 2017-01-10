// -- FILE ------------------------------------------------------------------
// name       : LoggerFactoryTrace.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Collections;

namespace RtfPipe.Sys.Logging
{

	// ------------------------------------------------------------------------
	internal sealed class LoggerFactoryTrace : LoggerFactory
	{

		// ----------------------------------------------------------------------
		public override ILogger GetLogger( string name )
		{
			ILogger logger = (ILogger)loggers[ name ];
			if ( logger == null )
			{
				lock ( this )
				{
					logger = (ILogger)loggers[ name ];
					if ( logger == null )
					{
						ILogger newLogger = new LoggerTrace( name );
						loggers.Add( name, newLogger );
						logger = newLogger;
					}
				}
			}
			return logger;
		} // GetLogger

		// ----------------------------------------------------------------------
		// members
		private static readonly Hashtable loggers = new Hashtable();

	} // class LoggerFactoryTrace

} // namespace RtfPipe.Sys.Logging
// -- EOF -------------------------------------------------------------------
