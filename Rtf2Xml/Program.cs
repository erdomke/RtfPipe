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
using System.Xml;
using Itenso.Sys.Application;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Converter.Xml;

namespace Itenso.Solutions.Community.Rtf2Xml
{

	// ------------------------------------------------------------------------
	enum ProgramExitCode
	{
		Successfully = 0,
		InvalidSettings = -1,
		ParseRtf = -2,
		DestinationDirectory = -3,
		InterpretRtf = -4,
		ConvertXml = -5,
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
			get { return (ProgramExitCode)Environment.ExitCode; }
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
			IRtfDocument rtfDocument = InterpretRtf( rtfStructure );
			if ( ExitCode != ProgramExitCode.Successfully )
			{
				return;
			}

			// convert to xml
			ConvertXml( rtfDocument );

			Console.WriteLine( "successfully converted RTF to XML in " + settings.DestinationDirectory );
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
		private IRtfDocument InterpretRtf( IRtfGroup rtfStructure )
		{
			IRtfDocument rtfDocument;
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

				// rtf interpreter
				RtfInterpreterSettings interpreterSettings = new RtfInterpreterSettings();
				interpreterSettings.IgnoreDuplicatedFonts = settings.IgnoreDuplicatedFonts;
				interpreterSettings.IgnoreUnknownFonts = settings.IgnoreUnknownFonts;

				// interpret the rtf structure using the extractors
				rtfDocument = RtfInterpreterTool.BuildDoc( rtfStructure, interpreterLogger );
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

			return rtfDocument;
		} // InterpretRtf

		// ----------------------------------------------------------------------
		private void ConvertXml( IRtfDocument rtfDocument )
		{
			try
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = ( "  " );
				xmlWriterSettings.Encoding = settings.Encoding;

				string fileName = settings.BuildDestinationFileName( null, RtfXmlConverter.DefaultXmlFileExtension );
				using ( XmlWriter writer = XmlWriter.Create( fileName, xmlWriterSettings ) )
				{
					RtfXmlConvertSettings xmlConvertSettings = new RtfXmlConvertSettings();
					xmlConvertSettings.Prefix = settings.XmlPrefix;
					xmlConvertSettings.Ns = settings.XmlNamespace;
					xmlConvertSettings.IsShowHiddenText = settings.ShowHiddenText;
					RtfXmlConverter xmlConverter = new RtfXmlConverter( rtfDocument, writer, xmlConvertSettings );
					xmlConverter.Convert();
					writer.Flush();
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( "error while converting to xml: " + e.Message );
				ExitCode = ProgramExitCode.ConvertXml;
			}
		} // ConvertXml

		// ----------------------------------------------------------------------
		private static void ShowHelp()
		{
			Console.WriteLine();
			Console.WriteLine( "Convert RTF to XML" );
			Console.WriteLine();
			Console.WriteLine( "Rtf2Xml source-file [destination] [/CE:encoding] [/P:prefix] [/NS:namespace] [/LD:path]" );
			Console.WriteLine( "                    [/IDF] [/IUF] [/LP] [/LI] [/HT] [/?]" );
			Console.WriteLine();
			Console.WriteLine( "   source-file       source rtf file" );
			Console.WriteLine( "   destination       destination directory (default=source-file directory)" );
			Console.WriteLine( "   /CE:encoding      character encoding:" );
			Console.WriteLine( "                       ASCII, UTF7, UTF8, Unicode, BigEndianUnicode, UTF32, OperatingSystem (default=UTF8)" );
			Console.WriteLine( "   /P:prefix         xml prefix (default=none)" );
			Console.WriteLine( "   /NS:namespace     xml namespace (default=none)" );
			Console.WriteLine( "   /LD:path          log file directory (default=destination directory)" );
			Console.WriteLine( "   /IDF              ignore duplicated fonts (default=off)" );
			Console.WriteLine( "   /IUF              ignore unknown fonts (default=off)" );
			Console.WriteLine( "   /LP               write rtf parser log file (default=off)" );
			Console.WriteLine( "   /LI               write rtf interpreter log file (default=off)" );
			Console.WriteLine( "   /HT               show hidden text (default=off)" );
			Console.WriteLine( "   /?                this help" );
			Console.WriteLine();
			Console.WriteLine( "Samples:" );
			Console.WriteLine( "  Rtf2Xml MyText.rtf" );
			Console.WriteLine( "  Rtf2Xml MyText.rtf c:\\temp" );
			Console.WriteLine( "  Rtf2Xml MyText.rtf c:\\temp /NS:MyNs" );
			Console.WriteLine( "  Rtf2Xml MyText.rtf c:\\temp /LD:log /LP /LI" );
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

} // namespace Itenso.Solutions.Community.Rtf2Xml
// -- EOF -------------------------------------------------------------------
