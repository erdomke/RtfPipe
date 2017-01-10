// -- FILE ------------------------------------------------------------------
// name       : RtfParser.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.20
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using RtfPipe.Model;
using RtfPipe.Support;
using RtfPipe.Interpreter;

namespace RtfPipe.Parser
{

	// ------------------------------------------------------------------------
	public class RtfParserDump
	{
		static void Main( string[] args )
		{
			RtfSympleParser();
			RtfSympleInterpreter();
			Console.ReadKey();
		} // Main

		// ----------------------------------------------------------------------
		static void RtfSympleParser()
		{
			RtfParser parser = new RtfParser();
			RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
			parser.AddParserListener( structureBuilder );
			parser.Parse( new RtfSource( @"{\rtf1foobar}" ) );
			RtfDumpElement( structureBuilder.StructureRoot );
		} // RtfSympleParser
  
		// ----------------------------------------------------------------------
		static void RtfDumpElement( IRtfElement rtfElement )
		{
			switch ( rtfElement.Kind )
			{
				case RtfElementKind.Group:
					Console.WriteLine( "Group: " + ((IRtfGroup)rtfElement).Destination );
					foreach ( IRtfElement rtfChildElement in ((IRtfGroup)rtfElement).Contents )
					{
						RtfDumpElement( rtfChildElement );
					}
					break;
				case RtfElementKind.Tag:
					Console.WriteLine( "Tag: " + ((IRtfTag)rtfElement).FullName );
					break;
				case RtfElementKind.Text:
					Console.WriteLine( "Text: " + ((IRtfText)rtfElement).Text );
					break;
			}
		} // RtfDumpElement

		// ----------------------------------------------------------------------
		static void RtfSympleInterpreter()
		{
			RtfInterpreterListenerFileLogger logger = new RtfInterpreterListenerFileLogger( @"c:\temp\RtfInterpreter.log" );
			IRtfDocument document = RtfInterpreterTool.BuildDoc( @"{\rtf1foobar}", logger );
			RtfWriteDocument( document );
		} // RtfSympleInterpreter

		// ----------------------------------------------------------------------
		static void RtfWriteDocument( IRtfDocument document )
		{
			Console.WriteLine( "RTF Version: " + document.RtfVersion.ToString() );

			// document info
			Console.WriteLine( "Title: " + document.DocumentInfo.Title );
			Console.WriteLine( "Subject: " + document.DocumentInfo.Subject );
			Console.WriteLine( "Author: " + document.DocumentInfo.Author );
			// ...

			// fonts
			foreach ( IRtfFont font in document.FontTable )
			{
			//	Console.WriteLine( "Font: " + font.Name );
			}

			// colors
			foreach ( IRtfColor color in document.ColorTable )
			{
			//	Console.WriteLine( "Color: " + color.AsDrawingColor.ToString() );
			}

			// user properties
			foreach ( IRtfDocumentProperty documentProperty in document.UserProperties )
			{
				Console.WriteLine( "User property: " + documentProperty.Name );
			}

			// visuals
			foreach ( IRtfVisual visual in document.VisualContent )
			{
				switch ( visual.Kind )
				{
					case RtfVisualKind.Text:
						Console.WriteLine( "Text: " + ( (IRtfVisualText)visual ).Text );
						break;
					case RtfVisualKind.Break:
						Console.WriteLine( "Tag: " + ( (IRtfVisualBreak)visual ).BreakKind.ToString() );
						break;
					case RtfVisualKind.Special:
						Console.WriteLine( "Text: " + ( (IRtfVisualSpecialChar)visual ).CharKind.ToString() );
						break;
					case RtfVisualKind.Image:
						IRtfVisualImage image = (IRtfVisualImage)visual;
						Console.WriteLine( "Text: " + image.Format.ToString() +
						 " " + image.Width.ToString() + "x" + image.Height.ToString() );
						break;
				}
			}
		} // RtfWriteElement

	} // class RtfParser

} // namespace RtfPipe.Parser
// -- EOF -------------------------------------------------------------------
