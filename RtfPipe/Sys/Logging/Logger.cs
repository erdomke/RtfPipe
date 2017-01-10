// -- FILE ------------------------------------------------------------------
// name       : Logger.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.03
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace RtfPipe.Sys.Logging
{

	// ------------------------------------------------------------------------
	/// <summary>
	/// Provides public access to logger implementations.
	/// </summary>
	public static class Logger
	{

		// ----------------------------------------------------------------------
		/// <summary>
		/// Informational access to the active logger factory.
		/// </summary>
		/// <value>the type name of the active logger factory</value>
		public static string ActiveLoggerFactoryName
		{
			get { return LoggerFactory.Instance.GetType().FullName; }
		} // ActiveLoggerFactoryName

		// ----------------------------------------------------------------------
		/// <summary>
		/// Always returns a logger that will ignore all output directed to it.
		/// This can be used to pass a logger instance to code which requires one
		/// while not actually desiring the resulting output.
		/// </summary>
		/// <returns>a logger that ignores all output</returns>
		public static ILogger GetIgnoreAllLogger()
		{
			return LoggerFactory.Instance.GetLogger( "dummy" );
		} // GetIgnoreAllLogger

		// ----------------------------------------------------------------------
		/// <summary>
		/// Same as <c>GetLogger( type.FullName )</c>.
		/// </summary>
		/// <param name="type">the type for which a logger should be retrieved</param>
		/// <returns>a logger for the given type</returns>
		public static ILogger GetLogger( Type type )
		{
			if ( type == null )
			{
				throw new ArgumentNullException( "type" );
			}
			return GetLogger( type.FullName );
		} // GetLogger

		// ----------------------------------------------------------------------
		/// <summary>
		/// Gets the logger with the specified name. Multiple calls with the same name
		/// will return the same instance.
		/// </summary>
		/// <param name="name">the name of the logger to retrieve</param>
		/// <returns>the logger for the given name</returns>
		public static ILogger GetLogger( string name )
		{
			return LoggerFactory.Instance.GetLogger( name );
		} // GetLogger

		// ----------------------------------------------------------------------
		public static string CurrentContext
		{
			get
			{
				if ( logger == null )
				{
					lock ( mutex )
					{
						if ( logger == null )
						{
							logger = GetLogger( typeof( Logger ) );
						}
					}
				}
				return logger.Context;
			}
		} // CurrentContext

		// ----------------------------------------------------------------------
		/// <summary>
		/// Gets the monitor to register listeners for logging events.
		/// </summary>
		/// <returns>the monitor in use by the active logging factory</returns>
		public static ILoggerMonitor Monitor
		{
			get { return LoggerFactory.Instance.Monitor; }
		} // Monitor

		// ----------------------------------------------------------------------
		/// <summary>
		/// Attempts to define which logger factory to use.
		/// </summary>
		/// <param name="factoryName">the fully qualified class name of the logger factory</param>
		/// <returns>true if this factory is being used, false if another factory has already been defined</returns>
		public static bool InitializeLoggerFactory( string factoryName )
		{
			return LoggerFactory.InitializeLoggerFactory( factoryName );
		} // InitializeLoggerFactory

		// ----------------------------------------------------------------------
		/// <summary>
		/// Sets a new file appender to the root logger.
		/// </summary>
		/// <remarks>
		/// This can be used if an application should log to a configurable file. currently
		/// only the log4net logging mechanism supports  Use this feature by _not_ configuring
		/// a file appender in the app.config of the application and set an appender using this method.
		/// Like that logging will be effective only after this method has been called (except for
		/// other logger appenders of course).
		/// </remarks>
		/// <param name="absoluteLogFileName">the name of the log file</param>
		/// <param name="append">whether to append or create new content</param>
		/// <param name="messagePattern">the message pattern; may be empty or null in which case
		/// a default pattern is used</param>
		/// <exception cref="System.InvalidOperationException">in case the active logger factory
		/// doesn't support this</exception>
		/// <exception cref="System.ArgumentNullException">when given an empty or null log file name</exception>
		public static void SetLogFile( string absoluteLogFileName, bool append, string messagePattern )
		{
			LoggerFactory.Instance.SetLogFile( absoluteLogFileName, append, messagePattern );
		} // SetLogFile

		// ----------------------------------------------------------------------
		// Logger

		// ----------------------------------------------------------------------
		// members
		private static readonly object mutex = new object();
		private static volatile ILogger logger;

	} // class Logger

} // namespace RtfPipe.Sys.Logging
// -- EOF -------------------------------------------------------------------
