using System;
using System.Text;
using System.Globalization;
using RtfPipe.Sys.Logging;

namespace RtfPipe.Parser
{

	public class RtfParserListenerLogger : RtfParserListenerBase
	{

		public RtfParserListenerLogger() :
			this( new RtfParserLoggerSettings(), systemLogger )
		{
		}

		public RtfParserListenerLogger( RtfParserLoggerSettings settings ) :
			this( settings, systemLogger )
		{
		}

		public RtfParserListenerLogger( ILogger logger ) :
			this( new RtfParserLoggerSettings(), logger )
		{
		}

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
		}

		public RtfParserLoggerSettings Settings
		{
			get { return settings; }
		}

		public ILogger Logger
		{
			get { return logger; }
		}

		protected override void DoParseBegin()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseBeginText ) )
			{
				Log( settings.ParseBeginText );
			}
		}

		protected override void DoGroupBegin()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseGroupBeginText ) )
			{
				Log( settings.ParseGroupBeginText );
			}
		}

		protected override void DoTagFound( IRtfTag tag )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseTagText ) )
			{
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.ParseTagText,
					tag ) );
			}
		}

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
		}

		protected override void DoGroupEnd()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseGroupEndText ) )
			{
				Log( settings.ParseGroupEndText );
			}
		}

		protected override void DoParseSuccess()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseSuccessText ) )
			{
				Log( settings.ParseSuccessText );
			}
		}

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
		}

		protected override void DoParseEnd()
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ParseEndText ) )
			{
				Log( settings.ParseEndText );
			}
		}

		private void Log( params string[] msg )
		{
			string logText = Indent( msg );

			systemLogger.Info( logText );

			if ( logger != null )
			{
				logger.Info( logText );
			}
		}

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
		}

		private readonly RtfParserLoggerSettings settings;
		private readonly ILogger logger;

		private static readonly ILogger systemLogger = Sys.Logging.Logger.GetLogger( typeof( RtfParserListenerLogger ) );

	}

}

