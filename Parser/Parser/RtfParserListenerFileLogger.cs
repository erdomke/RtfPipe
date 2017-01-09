// -- FILE ------------------------------------------------------------------
// name       : RtfParserListenerFileLogger.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.03
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace Itenso.Rtf.Parser
{

	// ------------------------------------------------------------------------
	public class RtfParserListenerFileLogger : RtfParserListenerBase, IDisposable
	{

		// ----------------------------------------------------------------------
		public const string DefaultLogFileExtension = ".parser.log";

		// ----------------------------------------------------------------------
		public RtfParserListenerFileLogger( string fileName ) :
			this( fileName, new RtfParserLoggerSettings() )
		{
		} // RtfParserListenerFileLogger

		// ----------------------------------------------------------------------
		public RtfParserListenerFileLogger( string fileName, RtfParserLoggerSettings settings )
		{
			if ( fileName == null )
			{
				throw new ArgumentNullException( "fileName" );
			}
			if ( settings == null )
			{
				throw new ArgumentNullException( "settings" );
			}

			this.fileName = fileName;
			this.settings = settings;
		} // RtfParserListenerFileLogger

		// ----------------------------------------------------------------------
		public string FileName
		{
			get { return fileName; }
		} // FileName

		// ----------------------------------------------------------------------
		public RtfParserLoggerSettings Settings
		{
			get { return settings; }
		} // Settings

		// ----------------------------------------------------------------------
		public virtual void Dispose()
		{
			CloseStream();
		} // Dispose

		// ----------------------------------------------------------------------
		protected override void DoParseBegin()
		{
			EnsureDirectory();
			OpenStream();

			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseBeginText ) )
			{
				WriteLine( settings.ParseBeginText );
			}
		} // DoParseBegin

		// ----------------------------------------------------------------------
		protected override void DoGroupBegin()
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseGroupBeginText ) )
			{
				WriteLine( settings.ParseGroupBeginText );
			}
		} // DoGroupBegin

		// ----------------------------------------------------------------------
		protected override void DoTagFound( IRtfTag tag )
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseTagText ) )
			{
				WriteLine( string.Format(
					CultureInfo.InvariantCulture,
					settings.ParseTagText,
					tag ) );
			}
		} // DoTagFound

		// ----------------------------------------------------------------------
		protected override void DoTextFound( IRtfText text )
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseTextText ) )
			{
				string msg = text.Text;
				if ( msg.Length > settings.TextMaxLength && !string.IsNullOrEmpty( settings.TextOverflowText ) )
				{
					msg = msg.Substring( 0, msg.Length - settings.TextOverflowText.Length ) + settings.TextOverflowText;
				}
				WriteLine( string.Format(
					CultureInfo.InvariantCulture,
					settings.ParseTextText,
					msg ) );
			}
		} // DoTextFound

		// ----------------------------------------------------------------------
		protected override void DoGroupEnd()
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseGroupEndText ) )
			{
				WriteLine( settings.ParseGroupEndText );
			}
		} // DoGroupEnd

		// ----------------------------------------------------------------------
		protected override void DoParseSuccess()
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseSuccessText ) )
			{
				WriteLine( settings.ParseSuccessText );
			}
		} // DoParseSuccess

		// ----------------------------------------------------------------------
		protected override void DoParseFail( RtfException reason )
		{
			if ( settings.Enabled )
			{
				if ( reason != null )
				{
					if ( !string.IsNullOrEmpty( settings.ParseFailKnownReasonText ) )
					{
						WriteLine( string.Format(
							CultureInfo.InvariantCulture,
							settings.ParseFailKnownReasonText,
							reason.Message ) );
					}
				}
				else
				{
					if ( !string.IsNullOrEmpty( settings.ParseFailUnknownReasonText ) )
					{
						WriteLine( settings.ParseFailUnknownReasonText );
					}
				}
			}
		} // DoParseFail

		// ----------------------------------------------------------------------
		protected override void DoParseEnd()
		{
			if ( settings.Enabled && !string.IsNullOrEmpty( settings.ParseEndText ) )
			{
				WriteLine( settings.ParseEndText );
			}

			CloseStream();
		} // DoParseEnd

		// ----------------------------------------------------------------------
		private void WriteLine( params string[] msg )
		{
			if ( streamWriter == null )
			{
				return;
			}
			string logText = Indent( msg );
			streamWriter.WriteLine( logText );
			streamWriter.Flush();
		} // WriteLine

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
		private void EnsureDirectory()
		{
			FileInfo fi = new FileInfo( fileName );
			if ( !string.IsNullOrEmpty( fi.DirectoryName ) && !Directory.Exists( fi.DirectoryName ) )
			{
				Directory.CreateDirectory( fi.DirectoryName );
			}
		} // EnsureDirectory

		// ----------------------------------------------------------------------
		private void OpenStream()
		{
			if ( streamWriter != null )
			{
				return;
			}
			streamWriter = new StreamWriter( fileName );
		} // OpenStream

		// ----------------------------------------------------------------------
		private void CloseStream()
		{
			if ( streamWriter == null )
			{
				return;
			}
			streamWriter.Close();
			streamWriter.Dispose();
			streamWriter = null;
		} // OpenStream

		// ----------------------------------------------------------------------
		// members
		private readonly string fileName;
		private readonly RtfParserLoggerSettings settings;
		private StreamWriter streamWriter;

	} // class RtfParserListenerFileLogger

} // namespace Itenso.Rtf.Parser
// -- EOF -------------------------------------------------------------------
