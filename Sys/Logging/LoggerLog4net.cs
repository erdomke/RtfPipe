// -- FILE ------------------------------------------------------------------
// name       : LoggerLog4net.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Xml;
using log4net;
using log4net.Repository;
using log4net.Util;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
// ReSharper disable InconsistentNaming
	internal sealed class LoggerLog4net : LoggerBase, ILogger
// ReSharper restore InconsistentNaming
	{

		// ----------------------------------------------------------------------
		public LoggerLog4net( string name )
		{
			logger = LogManager.GetLogger( name );
		} // LoggerLog4net

		// ----------------------------------------------------------------------
		public LoggerLevel Level
		{
			get
			{
				LoggerLevel level = LoggerLevel.Fatal;
				if ( IsDebugEnabled )
				{
					level = LoggerLevel.Debug;
				}
				else if ( IsInfoEnabled )
				{
					level = LoggerLevel.Info;
				}
				else if ( IsWarnEnabled )
				{
					level = LoggerLevel.Warn;
				}
				else if ( IsErrorEnabled )
				{
					level = LoggerLevel.Error;
				}
				return level;
			}
			set
			{
				// log4net needs to re-load the config file ... so we build one :-)
				IXmlRepositoryConfigurator configurableRepository = logger.Logger.Repository as IXmlRepositoryConfigurator;
				if ( configurableRepository != null )
				{
					// the most minimal configuration document for defining the behavior of just our logger:
					/*
					<log4net update="Merge">
						<logger name="Itenso.Sys.Logging.LoggerLog4net">
							<level value="DEBUG" />
						</logger>
					</log4net>
					//*/
					try
					{
						XmlDocument configDoc = new XmlDocument();
						XmlElement configTag = configDoc.CreateElement( "log4net" );
						// the following is the default behavior, just here for documentation ...
						//configTag.SetAttribute( "update", "Merge" ); // merge with existing configuration
						XmlElement loggerTag = configDoc.CreateElement( "logger" );
						loggerTag.SetAttribute( "name", logger.Logger.Name );
						XmlElement levelTag = configDoc.CreateElement( "level" );
						levelTag.SetAttribute( "value", value.ToString() );
						loggerTag.AppendChild( levelTag );
						configTag.AppendChild( loggerTag );
						configDoc.AppendChild( configTag );
						configurableRepository.Configure( configTag );
					}
					catch ( XmlException e )
					{
						logger.Warn( "cannot set new logging-level due to an XmlException", e );
					}
				}
				else
				{
					logger.Warn( "cannot set new logging-level as the repository is not configurable" );
				}
			}
		} // Level

		// ----------------------------------------------------------------------
		public bool IsDebugEnabled
		{
			get { return logger.IsDebugEnabled; }
		} // IsDebugEnabled

		// ----------------------------------------------------------------------
		public bool IsInfoEnabled
		{
			get { return logger.IsInfoEnabled; }
		} // IsInfoEnabled

		// ----------------------------------------------------------------------
		public bool IsWarnEnabled
		{
			get { return logger.IsWarnEnabled; }
		} // IsWarnEnabled

		// ----------------------------------------------------------------------
		public bool IsErrorEnabled
		{
			get { return logger.IsErrorEnabled; }
		} // IsErrorEnabled

		// ----------------------------------------------------------------------
		public bool IsFatalEnabled
		{
			get { return logger.IsFatalEnabled; }
		} // IsFatalEnabled

		// ----------------------------------------------------------------------
		public bool IsEnabledFor( LoggerLevel level )
		{
			bool enabled = false;
			switch ( level )
			{
				case LoggerLevel.Debug:
					enabled = IsDebugEnabled;
					break;
				case LoggerLevel.Info:
					enabled = IsInfoEnabled;
					break;
				case LoggerLevel.Warn:
					enabled = IsWarnEnabled;
					break;
				case LoggerLevel.Error:
					enabled = IsErrorEnabled;
					break;
				case LoggerLevel.Fatal:
					enabled = IsFatalEnabled;
					break;
			}
			return enabled;
		} // IsEnabledFor

		// ----------------------------------------------------------------------
		public void Debug( object message )
		{
			logger.Debug( message );
		} // Debug

		// ----------------------------------------------------------------------
		public void Debug( object message, Exception exception )
		{
			if ( IsSupportedException( exception ) )
			{
				logger.Debug( message, exception );
			}
		} // Debug

		// ----------------------------------------------------------------------
		public void DebugFormat( string format, params object[] args )
		{
			logger.DebugFormat( format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void DebugFormat( IFormatProvider provider, string format, params object[] args )
		{
			logger.DebugFormat( provider, format, args );
		} // DebugFormat

		// ----------------------------------------------------------------------
		public void Info( object message )
		{
			logger.Info( message );
		} // Info

		// ----------------------------------------------------------------------
		public void Info( object message, Exception exception )
		{
			if ( IsSupportedException( exception ) )
			{
				logger.Info( message, exception );
			}
		} // Info

		// ----------------------------------------------------------------------
		public void InfoFormat( string format, params object[] args )
		{
			logger.InfoFormat( format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void InfoFormat( IFormatProvider provider, string format, params object[] args )
		{
			logger.InfoFormat( provider, format, args );
		} // InfoFormat

		// ----------------------------------------------------------------------
		public void Warn( object message )
		{
			logger.Warn( message );
		} // Warn

		// ----------------------------------------------------------------------
		public void Warn( object message, Exception exception )
		{
			if ( IsSupportedException( exception ) )
			{
				logger.Warn( message, exception );
			}
		} // Warn

		// ----------------------------------------------------------------------
		public void WarnFormat( string format, params object[] args )
		{
			logger.WarnFormat( format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void WarnFormat( IFormatProvider provider, string format, params object[] args )
		{
			logger.WarnFormat( provider, format, args );
		} // WarnFormat

		// ----------------------------------------------------------------------
		public void Error( object message )
		{
			logger.Error( message );
		} // Error

		// ----------------------------------------------------------------------
		public void Error( object message, Exception exception )
		{
			if ( IsSupportedException( exception ) )
			{
				logger.Error( message, exception );
			}
		} // Error

		// ----------------------------------------------------------------------
		public void ErrorFormat( string format, params object[] args )
		{
			logger.ErrorFormat( format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void ErrorFormat( IFormatProvider provider, string format, params object[] args )
		{
			logger.ErrorFormat( provider, format, args );
		} // ErrorFormat

		// ----------------------------------------------------------------------
		public void Fatal( object message )
		{
			logger.Fatal( message );
		} // Fatal

		// ----------------------------------------------------------------------
		public void Fatal( object message, Exception exception )
		{
			if ( IsSupportedException( exception ) )
			{
				logger.Fatal( message, exception );
			}
		} // Fatal

		// ----------------------------------------------------------------------
		public void FatalFormat( string format, params object[] args )
		{
			logger.FatalFormat( format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void FatalFormat( IFormatProvider provider, string format, params object[] args )
		{
			logger.FatalFormat( provider, format, args );
		} // FatalFormat

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel level, object message )
		{
			switch ( level )
			{
				case LoggerLevel.Debug:
					logger.Debug( message );
					break;
				case LoggerLevel.Info:
					logger.Info( message );
					break;
				case LoggerLevel.Warn:
					logger.Warn( message );
					break;
				case LoggerLevel.Error:
					logger.Error( message );
					break;
				case LoggerLevel.Fatal:
					logger.Fatal( message );
					break;
			}
		} // Log

		// ----------------------------------------------------------------------
		public void Log( LoggerLevel level, object message, Exception exception )
		{
			if ( !IsSupportedException( exception ) )
			{
				return;
			}

			switch ( level )
			{
				case LoggerLevel.Debug:
					logger.Debug( message, exception );
					break;
				case LoggerLevel.Info:
					logger.Info( message, exception );
					break;
				case LoggerLevel.Warn:
					logger.Warn( message, exception );
					break;
				case LoggerLevel.Error:
					logger.Error( message, exception );
					break;
				case LoggerLevel.Fatal:
					logger.Fatal( message, exception );
					break;
			}
		} // Log

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel level, string format, params object[] args )
		{
			switch ( level )
			{
				case LoggerLevel.Debug:
					logger.DebugFormat( format, args );
					break;
				case LoggerLevel.Info:
					logger.InfoFormat( format, args );
					break;
				case LoggerLevel.Warn:
					logger.WarnFormat( format, args );
					break;
				case LoggerLevel.Error:
					logger.ErrorFormat( format, args );
					break;
				case LoggerLevel.Fatal:
					logger.FatalFormat( format, args );
					break;
			}
		} // LogFormat

		// ----------------------------------------------------------------------
		public void LogFormat( LoggerLevel level, IFormatProvider provider, string format, params object[] args )
		{
			switch ( level )
			{
				case LoggerLevel.Debug:
					logger.DebugFormat( provider, format, args );
					break;
				case LoggerLevel.Info:
					logger.InfoFormat( provider, format, args );
					break;
				case LoggerLevel.Warn:
					logger.WarnFormat( provider, format, args );
					break;
				case LoggerLevel.Error:
					logger.ErrorFormat( provider, format, args );
					break;
				case LoggerLevel.Fatal:
					logger.FatalFormat( provider, format, args );
					break;
			}
		} // LogFormat

		// ----------------------------------------------------------------------
		public override IDisposable PushContext( string context )
		{
			IDisposable stackCleaner = base.PushContext( context );
			ThreadContext.Stacks[ "NDC" ].Push( context );
			return stackCleaner;
		} // PushContext

		// ----------------------------------------------------------------------
		public override int ContextDepth
		{
			get { return ThreadContext.Stacks[ "NDC" ].Count; }
		} // ContextDepth

		// ----------------------------------------------------------------------
		public override string Context
		{
			get
			{
				ThreadContextStack nestedDiagnosticContext = ThreadContext.Stacks[ "NDC" ];
				string context = nestedDiagnosticContext != null ? nestedDiagnosticContext.ToString() : null;
				context = context != null ? context.Trim() : null;
				context = context ?? "(null)";
				return context;
			}
		} // Context

		// ----------------------------------------------------------------------
		public override void PopContext()
		{
			base.PopContext();
			ThreadContext.Stacks[ "NDC" ].Pop();
		} // PopContext

		// ----------------------------------------------------------------------
		protected override ILogger Logger
		{
			get { return this; }
		} // Logger

		// ----------------------------------------------------------------------
		// members
		private readonly ILog logger;

	} // class LoggerLog4net

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
