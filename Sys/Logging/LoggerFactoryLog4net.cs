// -- FILE ------------------------------------------------------------------
// name       : LoggerFactoryLog4net.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2005.05.04
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Itenso.Sys.Logging
{

	// ------------------------------------------------------------------------
// ReSharper disable InconsistentNaming
	internal sealed class LoggerFactoryLog4net : LoggerFactory
// ReSharper restore InconsistentNaming
	{

		// ----------------------------------------------------------------------
		public LoggerFactoryLog4net()
		{
			XmlConfigurator.Configure();
		} // LoggerFactoryLog4net

		// ----------------------------------------------------------------------
		public override ILogger GetLogger( string name )
		{
			return new LoggerLog4net( name );
		} // GetLogger

		// ----------------------------------------------------------------------
		protected override ILoggerMonitor CreateMonitor()
		{
			return new LoggerMonitorLog4net();
		} // CreateMonitor

		// ----------------------------------------------------------------------
		public override void SetLogFile( string absoluteLogFileName, bool append, string messagePattern )
		{
			if ( string.IsNullOrEmpty( absoluteLogFileName ) )
			{
				throw new ArgumentNullException( "absoluteLogFileName" );
			}

			string messagePatternToUse = !string.IsNullOrEmpty( messagePattern ) ? messagePattern :
				"%date{yyyyMMdd-HH:mm:ss.fff}-%-5level-%-25logger{1}: %message%newline";

			// log4net needs to re-load the config file ... so we build one :-)
			IXmlRepositoryConfigurator configurableRepository = LogManager.GetRepository( Assembly.GetCallingAssembly() ) as IXmlRepositoryConfigurator;
			if ( configurableRepository != null )
			{
				// the most minimal configuration document for defining the new appender:
				/*
				<root>
					<appender-ref ref="FILE-DYN" />
				</root>
				<appender name="FILE-DYN" type="log4net.Appender.FileAppender">
					<file value="path/to/some/log-file.log" />
					<appendToFile value="true" />
					<layout type="log4net.Layout.PatternLayout">
						<conversionPattern value="%date{yyyyMMdd-HH:mm:ss.fff}-%-5level-%-25logger{1}: %message%newline" />
					</layout>
				</appender>
				//*/
				try
				{
					XmlDocument doc = new XmlDocument();

// ReSharper disable InconsistentNaming
					XmlElement log4netTag = doc.CreateElement( "log4net" );
// ReSharper restore InconsistentNaming
					// the following is the default behavior, just here for documentation ...
					log4netTag.SetAttribute( "update", "Merge" ); // merge with existing configuration
					doc.AppendChild( log4netTag );

					XmlElement rootTag = doc.CreateElement( "root" );
					log4netTag.AppendChild( rootTag );

					//XmlElement levelTag = doc.CreateElement( "level" );
					//levelTag.SetAttribute( "value", "DEBUG" );
					//rootTag.AppendChild( levelTag );

					XmlElement consoleAppenderRefTag = doc.CreateElement( "appender-ref" );
					consoleAppenderRefTag.SetAttribute( "ref", "CONSOLE" );
					rootTag.AppendChild( consoleAppenderRefTag );

					XmlElement fileAppenderRefTag = doc.CreateElement( "appender-ref" );
					fileAppenderRefTag.SetAttribute( "ref", "DYN-FILE" );
					rootTag.AppendChild( fileAppenderRefTag );

					XmlElement consoleAppenderTag = doc.CreateElement( "appender" );
					consoleAppenderTag.SetAttribute( "name", "CONSOLE" );
					consoleAppenderTag.SetAttribute( "type", "log4net.Appender.ConsoleAppender" );
					log4netTag.AppendChild( consoleAppenderTag );

					XmlElement consoleLayoutTag = doc.CreateElement( "layout" );
					consoleLayoutTag.SetAttribute( "type", "log4net.Layout.PatternLayout" );
					consoleAppenderTag.AppendChild( consoleLayoutTag );

					XmlElement consoleConversionPatternTag = doc.CreateElement( "conversionPattern" );
					consoleConversionPatternTag.SetAttribute( "value", messagePatternToUse );
					consoleLayoutTag.AppendChild( consoleConversionPatternTag );

					XmlElement consoleThresholdTag = doc.CreateElement( "threshold" );
					consoleThresholdTag.SetAttribute( "value", "WARN" );
					consoleAppenderTag.AppendChild( consoleThresholdTag );

					XmlElement fileAppenderTag = doc.CreateElement( "appender" );
					fileAppenderTag.SetAttribute( "name", "DYN-FILE" );
					fileAppenderTag.SetAttribute( "type", "log4net.Appender.FileAppender" );
					log4netTag.AppendChild( fileAppenderTag );

					XmlElement fileTag = doc.CreateElement( "file" );
					fileTag.SetAttribute( "value", absoluteLogFileName );
					fileAppenderTag.AppendChild( fileTag );

					XmlElement appendToFileTag = doc.CreateElement( "appendToFile" );
					appendToFileTag.SetAttribute( "value", append ? "true" : "false" );
					fileAppenderTag.AppendChild( appendToFileTag );

					XmlElement layoutTag = doc.CreateElement( "layout" );
					layoutTag.SetAttribute( "type", "log4net.Layout.PatternLayout" );
					fileAppenderTag.AppendChild( layoutTag );

					XmlElement conversionPatternTag = doc.CreateElement( "conversionPattern" );
					conversionPatternTag.SetAttribute( "value", messagePatternToUse );
					layoutTag.AppendChild( conversionPatternTag );

					configurableRepository.Configure( log4netTag );
				}
				catch ( XmlException e )
				{
					throw new InvalidOperationException( Strings.LoggerLoggingLevelXmlError, e );
				}
			}
			else
			{
				throw new InvalidOperationException( Strings.LoggerLoggingLevelRepository );
			}
		} // SetLogFile

	} // class LoggerFactoryLog4net

} // namespace Itenso.Sys.Logging
// -- EOF -------------------------------------------------------------------
