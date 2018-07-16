using System;
using System.Globalization;
using RtfPipe.Sys.Logging;

namespace RtfPipe.Interpreter
{

	public class RtfInterpreterListenerLogger : RtfInterpreterListenerBase
	{

		public RtfInterpreterListenerLogger() :
			this( new RtfInterpreterLoggerSettings(), systemLogger )
		{
		}

		public RtfInterpreterListenerLogger( RtfInterpreterLoggerSettings settings ) :
			this( settings, systemLogger )
		{
		}

		public RtfInterpreterListenerLogger( ILogger logger ) :
			this( new RtfInterpreterLoggerSettings(), logger )
		{
		}

		public RtfInterpreterListenerLogger( RtfInterpreterLoggerSettings settings, ILogger logger )
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

		public RtfInterpreterLoggerSettings Settings
		{
			get { return settings; }
		}

		public ILogger Logger
		{
			get { return logger; }
		}

		protected override void DoBeginDocument( IRtfInterpreterContext context )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.BeginDocumentText ) )
			{
				Log( settings.BeginDocumentText );
			}
		}

		protected override void DoInsertText( IRtfInterpreterContext context, string text )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.TextFormatText ) )
			{
				string msg = text;
				if ( msg.Length > settings.TextMaxLength && !string.IsNullOrEmpty( settings.TextOverflowText ) )
				{
					msg = msg.Substring( 0, msg.Length - settings.TextOverflowText.Length ) + settings.TextOverflowText;
				}
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.TextFormatText,
					msg,
					context.GetSafeCurrentTextFormat() ) );
			}
		}

		protected override void DoInsertSpecialChar( IRtfInterpreterContext context, RtfVisualSpecialCharKind kind )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.SpecialCharFormatText ) )
			{
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.SpecialCharFormatText,
					kind ) );
			}
		}

		protected override void DoInsertBreak( IRtfInterpreterContext context, RtfVisualBreakKind kind )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.BreakFormatText ) )
			{
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.BreakFormatText,
					kind ) );
			}
		}

		protected override void DoInsertImage( IRtfInterpreterContext context,
			RtfVisualImageFormat format,
			int width, int height, int desiredWidth, int desiredHeight,
			int scaleWidthPercent, int scaleHeightPercent,
			string imageDataHex
		)
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.ImageFormatText ) )
			{
				Log( string.Format(
					CultureInfo.InvariantCulture,
					settings.ImageFormatText,
					format,
					width,
					height,
					desiredWidth,
					desiredHeight,
					scaleWidthPercent,
					scaleHeightPercent,
					imageDataHex,
					(imageDataHex.Length / 2) ) );
			}
		}

		protected override void DoEndDocument( IRtfInterpreterContext context )
		{
			if ( settings.Enabled && logger.IsInfoEnabled && !string.IsNullOrEmpty( settings.EndDocumentText ) )
			{
				Log( settings.EndDocumentText );
			}
		}

		private void Log( string message )
		{
			systemLogger.Info( message );
			if ( logger != null )
			{
				logger.Info( message );
			}
		}

		private readonly RtfInterpreterLoggerSettings settings;
		private readonly ILogger logger;

		private static readonly ILogger systemLogger = Sys.Logging.Logger.GetLogger( typeof( RtfInterpreterListenerLogger ) );

	}

}

