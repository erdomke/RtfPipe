// -- FILE ------------------------------------------------------------------
// name       : Program.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.05.30
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using System.Diagnostics;
using Itenso.Sys.Application;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Converter.Image;

namespace Itenso.Solutions.Community.Rtf2Raw
{

	// ------------------------------------------------------------------------
	enum ProgramExitCode
	{
		Successfully = 0,
		InvalidSettings = -1,
		ParseRtf = -2,
		DestinationDirectory = -3,
		InterpretRtf = -4,
		SaveText = -5,
	} // enum ProgramExitCode

	// ------------------------------------------------------------------------
	class Program
	{

		// ----------------------------------------------------------------------
		public Program()
		{
			settings = new ProgramSettings();
		} // Program

		// ----------------------------------------------------------------------
		private static ProgramExitCode ExitCode
		{
			get { return (ProgramExitCode) Environment.ExitCode; }
			set { Environment.ExitCode = (int)value; }
		} // ExitCode

		// ----------------------------------------------------------------------
		public void Execute()
		{
			Console.WriteLine( string.Concat(
				ApplicationInfo.ShortCaption,
				", ",
				ApplicationInfo.Copyright ) );

			// program settings
			if ( ValidateProgramSettings() == false )
			{
				return;
			}

			// parse rtf
			IRtfGroup rtfStructure = ParseRtf();
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// destination directory
			EnsureDestinationDirectory();
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// interpret rtf
			string text = InterpretRtf( rtfStructure );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// save text
			string fileName = SaveText( text );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// open text file
			OpenTextFile( fileName );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// display raw text
			DisplayRawText( text );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			Console.WriteLine( "successfully converted RTF to Raw data in " + settings.DestinationDirectory );
		} // Execute

		// ----------------------------------------------------------------------
		private bool ValidateProgramSettings()
		{
			if ( settings.IsHelpMode )
			{
				ShowHelp();
				return false;
			}

			if ( !settings.IsValid )
			{
				ShowHelp();
				ExitCode = ProgramExitCode.InvalidSettings;
				return false;
			}

			return true;
		} // ValidateProgramSettings

		// ----------------------------------------------------------------------
		private IRtfGroup ParseRtf()
		{
			IRtfGroup rtfStructure;
			RtfParserListenerFileLogger parserLogger = null;
			try
			{
				// logger
				if ( settings.LogParser )
				{
					string logFileName = settings.BuildDestinationFileName( 
						settings.LogDirectory,
						RtfParserListenerFileLogger.DefaultLogFileExtension );
					parserLogger = new RtfParserListenerFileLogger( logFileName );
				}

				// rtf parser
				// open readonly - in case of dominant locks...
				using ( FileStream stream = File.Open( settings.SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
				{
					// parse the rtf structure
					RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
					RtfParser parser = new RtfParser( structureBuilder );
					parser.IgnoreContentAfterRootGroup = true; // support WordPad documents
					if ( parserLogger != null )
					{
						parser.AddParserListener( parserLogger );
					}
					parser.Parse( new RtfSource( stream ) );
					rtfStructure = structureBuilder.StructureRoot;
				}
			}
			catch ( Exception e )
			{
				if ( parserLogger != null )
				{
					parserLogger.Dispose();
				}

				Console.WriteLine( "error while parsing rtf: " + e.Message );
				ExitCode = ProgramExitCode.ParseRtf;
				return null;
			}

			return rtfStructure;
		} // ParseRtf

		// ----------------------------------------------------------------------
		private void EnsureDestinationDirectory()
		{
			if ( !settings.HasDestinationOutput )
			{
				return;
			}

			try
			{
				if ( !Directory.Exists( settings.DestinationDirectory ) )
				{
					Directory.CreateDirectory( settings.DestinationDirectory );
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while creating destination directory: " + e.Message );
				ExitCode = ProgramExitCode.DestinationDirectory;
			}
		} // EnsureDestinationDirectory

		// ----------------------------------------------------------------------
		private string InterpretRtf( IRtfGroup rtfStructure )
		{
			string text = null;

			RtfInterpreterListenerFileLogger interpreterLogger = null;
			try
			{
				// logger
				if ( settings.LogInterpreter )
				{
					string logFileName = settings.BuildDestinationFileName( 
						settings.LogDirectory,
						RtfInterpreterListenerFileLogger.DefaultLogFileExtension );
					interpreterLogger = new RtfInterpreterListenerFileLogger( logFileName );
				}

				// text converter
				RtfTextConverter textConverter = null;
				if ( settings.SaveText )
				{
					RtfTextConvertSettings textConvertSettings = new RtfTextConvertSettings();
					textConvertSettings.IsShowHiddenText = settings.ShowHiddenText;
					textConverter = new RtfTextConverter( textConvertSettings );
				}

				// image converter
				RtfImageConverter imageConverter = null;
				if ( settings.SaveImage )
				{
					RtfVisualImageAdapter imageAdapter = new RtfVisualImageAdapter( 
						settings.ImageFileNamePattern,
						settings.ImageFormat );
					RtfImageConvertSettings imageConvertSettings = new RtfImageConvertSettings( imageAdapter );
					imageConvertSettings.ImagesPath = settings.DestinationDirectory;
					imageConvertSettings.ScaleImage = settings.ScaleImage;
					imageConvertSettings.BackgroundColor = settings.ImageBackgroundColor;
					imageConvertSettings.ScaleImage = !settings.UnscaledImages;
					if ( settings.ExtendedImageScale )
					{
						imageConvertSettings.ScaleExtension = 0.5f;
					}
					imageConverter = new RtfImageConverter( imageConvertSettings );
				}

				// rtf interpreter
				RtfInterpreterSettings interpreterSettings = new RtfInterpreterSettings();
				interpreterSettings.IgnoreDuplicatedFonts = settings.IgnoreDuplicatedFonts;
				interpreterSettings.IgnoreUnknownFonts = settings.IgnoreUnknownFonts;

				// interpret the rtf structure using the extractors
				RtfInterpreterTool.Interpret( rtfStructure, interpreterLogger, textConverter, imageConverter );

				// get the resulting text
				if ( textConverter != null )
				{
					text = textConverter.PlainText;
				}
			}
			catch ( Exception e )
			{
				if ( interpreterLogger != null )
				{
					interpreterLogger.Dispose();
				}

				Console.WriteLine( "error while interpreting rtf: " + e.Message );
				ExitCode = ProgramExitCode.InterpretRtf;
				return null;
			}

			return text;
		} // InterpretRtf

		// ----------------------------------------------------------------------
		private string SaveText( string text )
		{
			if ( !settings.SaveText )
			{
				return null;
			}

			string fileName = settings.BuildDestinationFileName( null, RtfTextConverter.DefaultTextFileExtension );
			try
			{
				using ( TextWriter writer = new StreamWriter( fileName, false, settings.Encoding ) )
				{
					writer.Write( text );
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while saving raw text: " + e.Message );
				ExitCode = ProgramExitCode.SaveText;
				return null;
			}

			return fileName;
		} // SaveText

		// ----------------------------------------------------------------------
		private void OpenTextFile( string fileName )
		{
			if ( !settings.SaveText || !settings.OpenTextFile )
			{
				return;
			}
			Process.Start( fileName );
		} // OpenTextFile

		// ----------------------------------------------------------------------
		private void DisplayRawText( string text )
		{
			if ( !settings.DisplayRawText )
			{
				return;
			}
			Console.WriteLine( text );
		} // DisplayRawText

		// ----------------------------------------------------------------------
		private static void ShowHelp()
		{
			Console.WriteLine();
			Console.WriteLine( "Convert RTF to Raw data" );
			Console.WriteLine();
			Console.WriteLine("Rtf2Raw source-file [destination] [/IT:format] [/CE:encoding] [/IS] [/BC:color] [/XS] [/UI]");
			Console.WriteLine( "                    [/ST] [/SI] [/LD:path] [/IDF] [/IUF] [/LP] [/LI] [/D] [/O] [/HT] [/?]" );
			Console.WriteLine();
			Console.WriteLine( "   source-file       source rtf file" );
			Console.WriteLine( "   destination       destination directory (default=source-file directory)" );
			Console.WriteLine( "   /IT:format        images type format:" );
			Console.WriteLine( "                       bmp, emf, exif, gif, icon, jpg, png, tiff or wmf (default=original)" );
			Console.WriteLine( "   /CE:encoding      character encoding:" );
			Console.WriteLine( "                       ASCII, UTF7, UTF8, Unicode, BigEndianUnicode, UTF32, OperatingSystem (default=UTF8)" );
			Console.WriteLine( "   /IS               image scale (default=off)" );
			Console.WriteLine( "   /BC:color         image background color name (default=none)" );
			Console.WriteLine( "   /XS               extended image scale - border fix (default=off)" );
			Console.WriteLine( "   /UI               enforce unscaled images (default=off)" );
			Console.WriteLine( "   /ST               don't save text to the destination (default=on)" );
			Console.WriteLine( "   /SI               don't save images to the destination (default=on)" );
			Console.WriteLine( "   /LD:path          log file directory (default=destination directory)" );
			Console.WriteLine( "   /IDF              ignore duplicated fonts (default=off)" );
			Console.WriteLine( "   /IUF              ignore unknown fonts (default=off)" );
			Console.WriteLine( "   /LP               write rtf parser log (default=off)" );
			Console.WriteLine( "   /LI               write rtf interpreter log (default=off)" );
			Console.WriteLine( "   /D                write text to screen (default=off)" );
			Console.WriteLine( "   /O                open text in associated application (default=off)" );
			Console.WriteLine( "   /HT               show hidden text (default=off)" );
			Console.WriteLine( "   /?                this help" );
			Console.WriteLine();
			Console.WriteLine( "Samples:" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf c:\\temp" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf c:\\temp /CSS:MyCompany.css" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css /IT:png /BC:white" );
			Console.WriteLine( "  Rtf2Raw MyText.rtf c:\\temp /CSS:MyCompany.css,ThisProject.css /IT:png /BC:white /LD:log /LP /LI" );
			Console.WriteLine();
		} // ShowHelp

		// ----------------------------------------------------------------------
		static void Main()
		{
			new Program().Execute();
		} // Main

		// ----------------------------------------------------------------------
		// members
		private readonly ProgramSettings settings;

	} // class Program

} // namespace Itenso.Solutions.Community.Rtf2Raw
// -- EOF -------------------------------------------------------------------
