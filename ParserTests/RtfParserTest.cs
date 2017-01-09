// -- FILE ------------------------------------------------------------------
// name       : RtfParserTest.cs
// project    : RTF Framelet
// created    : Leon Poyyayil - 2008.05.19
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.IO;
using NUnit.Framework;
using Itenso.Sys.Test;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

namespace Itenso.Rtf.ParserTests
{

	// ------------------------------------------------------------------------
	[TestFixture]
	public sealed class RtfParserTest : TestUnitBase
	{

		// ----------------------------------------------------------------------
		[Test]
		public void ParseResourcesTest()
		{
			IterateResourceTestCases( "", "rtf", true );
		} // ParseResourcesTest

		// ----------------------------------------------------------------------
		protected override void DoTest( string kind, Stream testRes, string testCaseName )
		{
			RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
			RtfParser parser = new RtfParser();
			//parser.AddParserListener( new RtfParserListenerLogger() );
			parser.AddParserListener( structureBuilder );

			parser.Parse( new RtfSource( testRes ) );
			Assert.IsNotNull( structureBuilder.StructureRoot );
		} // DoTest

		// ----------------------------------------------------------------------
		[Test]
		public void ParseStructureBuilderTest()
		{
			RtfParser parser = new RtfParser();
			RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
			parser.AddParserListener( structureBuilder );
			parser.Parse( new RtfSource( @"{\rtf1foobar}" ) );

			IRtfGroup rtfStructure = structureBuilder.StructureRoot;
			Assert.IsNotNull( rtfStructure );

			Assert.AreEqual( RtfElementKind.Group, rtfStructure.Kind );
			Assert.AreEqual( 2, rtfStructure.Contents.Count );

			Assert.AreEqual( RtfElementKind.Tag, rtfStructure.Contents[ 0 ].Kind );
			Assert.AreEqual( "rtf", ((IRtfTag)rtfStructure.Contents[ 0 ]).Name );
			Assert.AreEqual( true, ((IRtfTag)rtfStructure.Contents[ 0 ]).HasValue );
			Assert.AreEqual( "1", ((IRtfTag)rtfStructure.Contents[ 0 ]).ValueAsText );
			Assert.AreEqual( 1, ((IRtfTag)rtfStructure.Contents[ 0 ]).ValueAsNumber );

			Assert.AreEqual( RtfElementKind.Text, rtfStructure.Contents[ 1 ].Kind );
			Assert.AreEqual( "foobar", ((IRtfText)rtfStructure.Contents[ 1 ]).Text );
		} // ParseStructureBuilderTest

		// ----------------------------------------------------------------------
		[Test]
		public void ParseLoggerTest()
		{
			RtfParser parser = new RtfParser();
			const bool enableLogging = false;
			parser.AddParserListener( new RtfParserListenerLogger( new RtfParserLoggerSettings( enableLogging ) ) );
			parser.Parse( new RtfSource( @"{\rtf1foobar}" ) );
			parser.Parse( new RtfSource( GetTestResource( "minimal.rtf" ) ) );
		} // ParseLoggerTest

		// ----------------------------------------------------------------------
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void ArgNullTest2()
		{
			new RtfParser().RemoveParserListener( null );
		} // ArgNullTest2

		// ----------------------------------------------------------------------
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void ArgNullTest1()
		{
			new RtfParser().AddParserListener( null );
		} // ArgNullTest1

		// ----------------------------------------------------------------------
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void ArgNullTest0()
		{
			new RtfParser().Parse( null );
		} // ArgNullTest0

	} // class RtfParserTest

} // namespace Itenso.Rtf.ParserTests
// -- EOF -------------------------------------------------------------------
