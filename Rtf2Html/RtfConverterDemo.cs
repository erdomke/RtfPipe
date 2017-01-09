// -- FILE ------------------------------------------------------------------
// name       : RtfConverterDemo.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2013.02.16
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Drawing.Imaging;
using System.IO;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

namespace Itenso.Solutions.Community.Rtf2Html
{

	// ------------------------------------------------------------------------
	public class RtfConverterDemo
	{

		// ----------------------------------------------------------------------
		public string ParserLogFileName { get; set; }

		// ----------------------------------------------------------------------
		public string InterpreterLogFileName { get; set; }

		// ----------------------------------------------------------------------
		public string ConvertRtf2Txt( string fileName )
		{
			// parser
			IRtfGroup rtfStructure = ParseRtf( fileName );
			if ( rtfStructure == null )
			{
				return string.Empty;
			}

			// interpreter logger
			RtfInterpreterListenerFileLogger interpreterLogger = null;
			if ( !string.IsNullOrEmpty( InterpreterLogFileName ) )
			{
				interpreterLogger = new RtfInterpreterListenerFileLogger( InterpreterLogFileName );
			}

			// text converter
			RtfTextConvertSettings textConvertSettings = new RtfTextConvertSettings();
			textConvertSettings.BulletText = "-";
			RtfTextConverter textConverter = new RtfTextConverter( textConvertSettings );

			// text interpreter
			RtfInterpreterTool.Interpret( rtfStructure, interpreterLogger, textConverter );
			return textConverter.PlainText;
		} // ConvertRtf2Txt

		// ----------------------------------------------------------------------
		public string ConvertRtf2Html( string fileName )
		{
			// parser
			IRtfGroup rtfStructure = ParseRtf( fileName );
			if ( rtfStructure == null )
			{
				return string.Empty;
			}

			// interpreter logger
			RtfInterpreterListenerFileLogger interpreterLogger = null;
			if ( !string.IsNullOrEmpty( InterpreterLogFileName ) )
			{
				interpreterLogger = new RtfInterpreterListenerFileLogger( InterpreterLogFileName );
			}

			// image converter
			RtfVisualImageAdapter imageAdapter = new RtfVisualImageAdapter( ImageFormat.Jpeg );
			RtfImageConvertSettings imageConvertSettings = new RtfImageConvertSettings( imageAdapter );
			imageConvertSettings.ScaleImage = true; // scale images
			RtfImageConverter imageConverter = new RtfImageConverter( imageConvertSettings );

			// rtf interpreter
			RtfInterpreterSettings interpreterSettings = new RtfInterpreterSettings();
			IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc( rtfStructure, interpreterSettings, interpreterLogger, imageConverter );

			// html converter
			RtfHtmlConvertSettings htmlConvertSettings = new RtfHtmlConvertSettings();
			htmlConvertSettings.ConvertScope = RtfHtmlConvertScope.Content;
			RtfHtmlConverter htmlConverter = new RtfHtmlConverter( rtfDocument );
			return htmlConverter.Convert();
		} // ConvertRtf2Html

		// ----------------------------------------------------------------------
		private IRtfGroup ParseRtf( string fileName )
		{
			IRtfGroup rtfStructure;

			using ( FileStream stream = File.Open( fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
			{
				RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
				RtfParser parser = new RtfParser( structureBuilder );
				parser.IgnoreContentAfterRootGroup = true; // support WordPad documents
				if ( !string.IsNullOrEmpty( ParserLogFileName ) )
				{
					parser.AddParserListener( new RtfParserListenerFileLogger( ParserLogFileName ) );
				}
				parser.Parse( new RtfSource( stream ) );
				rtfStructure = structureBuilder.StructureRoot;
			}
			return rtfStructure;
		} // ParseRtf

	} // class RtfConverterDemo

} // namespace Itenso.Solutions.Community.Rtf2Html
// -- EOF -------------------------------------------------------------------
