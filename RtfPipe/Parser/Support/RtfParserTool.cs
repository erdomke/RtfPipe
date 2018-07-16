using System.IO;
using RtfPipe.Parser;

namespace RtfPipe.Support
{

	public static class RtfParserTool
	{

		public static IRtfGroup Parse( string rtfText, params IRtfParserListener[] listeners )
		{
			return Parse( new RtfSource( rtfText ), listeners );
		}

		public static IRtfGroup Parse( TextReader rtfTextSource, params IRtfParserListener[] listeners )
		{
			return Parse( new RtfSource( rtfTextSource ), listeners );
		}

		public static IRtfGroup Parse( Stream rtfTextSource, params IRtfParserListener[] listeners )
		{
			return Parse( new RtfSource( rtfTextSource ), listeners );
		}

		public static IRtfGroup Parse( IRtfSource rtfTextSource, params IRtfParserListener[] listeners )
		{
			RtfParserListenerStructureBuilder structureBuilder = new RtfParserListenerStructureBuilder();
			RtfParser parser = new RtfParser( structureBuilder );
			if ( listeners != null )
			{
				foreach ( IRtfParserListener listener in listeners )
				{
					if ( listener != null )
					{
						parser.AddParserListener( listener );
					}
				}
			}
			parser.Parse( rtfTextSource );
			return structureBuilder.StructureRoot;
		}

	}

}

