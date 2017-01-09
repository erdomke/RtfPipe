// -- FILE ------------------------------------------------------------------
// name       : RtfParserListenerLogger.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.19
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Text;
using System.Globalization;
using Itenso.Sys.Logging;

namespace Itenso.Rtf.Parser
{

	// ------------------------------------------------------------------------
	public class RtfParserListenerLogger : RtfParserListenerBase
	{

		// ----------------------------------------------------------------------
		public RtfParserListenerLogger() :
			this( new RtfParserLoggerSettings(), systemLogger )
		{
		} // RtfParserListenerLogger

		// ----------------------------------------------------------------------
		public RtfParserListenerLogger( RtfParserLoggerSettings settings ) :
			this( settings, systemLogger )
		{
		} // RtfParserListenerLogger

		// ----------------------------------------------------------------------
		public RtfParserListenerLogger( ILogger logger ) :
			this( new RtfParserLoggerSettings(), logger )
		{
		} // RtfParserListenerLogger

		// ----------------------------------------------------------------------
		public RtfParserListenerLogger( RtfParserLoggerSettings settings, ILogger logger )
		{
			if ( settings == null )
			{
				throw new ArgumentNullException( "settings" );
			}
			if ( logger == null )
			{
				throw new ArgumentNullException( "logger" );
			}

			this.settings = settings;
			this.logger = logger;
		} // RtfParserListenerLogger

		// ----------------------------------------------------------------------
		public RtfParserLoggerSettings Settings
		{
			get { return settings; }
		} // Settings

		// ----------------------------------------------------------------------
		public ILogger Logger
		{
			get { return logger; }
		} // Logger

		// ----------------------------------------------------------------------
		protected override void DoParseBegin()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseBeginText ) )
			{
				Log( settings.ParseBeginText );
			}
		} // DoParseBegin

		// ----------------------------------------------------------------------
		protected override void DoGroupBegin()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseGroupBeginText ) )
			{
				Log( settings.ParseGroupBeginText );
			}
		} // DoGroupBegin

		// ----------------------------------------------------------------------
		protected override void DoTagFound( IRtfTag tag )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseTagText ) )
			{
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.ParseTagText,
					tag ) );
			}
		} // DoTagFound

		// ----------------------------------------------------------------------
		protected override void DoTextFound( IRtfText text )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseTextText ) )
			{
				string msg = text.Text;
				if ( msg.Length > settings.TextMaxLength && !string.IsNullOrEmpty( settings.TextOverflowText ) )
				{
					msg = msg.Substring( 0, msg.Length - settings.TextOverflowText.Length ) + settings.TextOverflowText;
				}
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.ParseTextText,
					msg ) );
			}
		} // DoTextFound

		// ----------------------------------------------------------------------
		protected override void DoGroupEnd()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseGroupEndText ) )
			{
				Log( settings.ParseGroupEndText );
			}
		} // DoGroupEnd

		// ----------------------------------------------------------------------
		protected override void DoParseSuccess()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseSuccessText ) )
			{
				Log( settings.ParseSuccessText );
			}
		} // DoParseSuccess

		// ----------------------------------------------------------------------
		protected override void DoParseFail( RtfException reason )
		{
			if ( settings.Enabled && logger.IsInfoEnabled )
			{
				if ( reason != null )
				{
					if ( !string.IsNullOrEmpty( settings.ParseFailKnownReasonText ) )
					{
						Log( string.Format(
							CultureInfo.InvariantCulture,
							settings.ParseFailKnownReasonText,
							reason.Message ) );
					}
				}
				else
				{
					if ( !string.IsNullOrEmpty( settings.ParseFailUnknownReasonText ) )
					{
						Log( settings.ParseFailUnknownReasonText );
					}
				}
			}
		} // DoParseFail

		// ----------------------------------------------------------------------
		protected override void DoParseEnd()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseEndText ) )
			{
				Log( settings.ParseEndText );
			}
		} // DoParseEnd

		// ----------------------------------------------------------------------
		private void Log( params string[] msg )
		{
			string logText = Indent( msg );

			systemLogger.Info( logText );

			if ( logger != null )
			{
				logger.Info( logText );
			}
		} // Log

		// ----------------------------------------------------------------------
		private string Indent( params string[] msg )
		{
			StringBuilder buf = new StringBuilder();
			if ( msg != null )
			{
				for ( int i = 0; i < Level; i++ )
				{
					buf.Append( " " );
				}
				foreach ( string m in msg )
				{
					buf.Append( m );
				}
			}
			return buf.ToString();
		} // Indent

		// ----------------------------------------------------------------------
		// members
		private readonly RtfParserLoggerSettings settings;
		private readonly ILogger logger;

		private static readonly ILogger systemLogger = Sys.Logging.Logger.GetLogger( typeof( RtfParserListenerLogger ) );

	} // class RtfParserListenerLogger

} // namespace Itenso.Rtf.Parser
// -- EOF -------------------------------------------------------------------
