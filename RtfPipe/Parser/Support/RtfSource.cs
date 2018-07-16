using System;
using System.IO;

namespace RtfPipe.Support
{


  public sealed class RtfSource : IRtfSource
  {

    public RtfSource( string rtf )
    {
      if ( rtf == null )
      {
        throw new ArgumentNullException( "rtf" );
      }
      reader = new StringReader( rtf );
    }

    public RtfSource( TextReader rtf )
    {
      if ( rtf == null )
      {
        throw new ArgumentNullException( "rtf" );
      }
      reader = rtf;
    }

    public RtfSource( Stream rtf )
    {
      if ( rtf == null )
      {
        throw new ArgumentNullException( "rtf" );
      }
      reader = new StreamReader( rtf, RtfSpec.AnsiEncoding );
    }

    public TextReader Reader
    {
      get { return reader; }
    }

    private readonly TextReader reader;

    public static implicit operator RtfSource(string value)
    {
      return new RtfSource(value);
    }
    public static implicit operator RtfSource(Stream value)
    {
      return new RtfSource(value);
    }
  }

}

