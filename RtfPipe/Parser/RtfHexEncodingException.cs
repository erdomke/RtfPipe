// -- FILE ------------------------------------------------------------------
// name       : RtfHexEncodingException.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2009.02.19
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
#if SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace RtfPipe
{

  // ------------------------------------------------------------------------
  /// <summary>Thrown upon RTF specific error conditions.</summary>
#if SERIALIZATION
  [Serializable]
#endif
  public class RtfHexEncodingException : RtfEncodingException
  {

    // ----------------------------------------------------------------------
    /// <summary>Creates a new instance.</summary>
    public RtfHexEncodingException()
    {
    } // RtfHexEncodingException

    // ----------------------------------------------------------------------
    /// <summary>Creates a new instance with the given message.</summary>
    /// <param name="message">the message to display</param>
    public RtfHexEncodingException( string message ) :
      base( message )
    {
    } // RtfHexEncodingException

    // ----------------------------------------------------------------------
    /// <summary>Creates a new instance with the given message, based on the given cause.</summary>
    /// <param name="message">the message to display</param>
    /// <param name="cause">the original cause for this exception</param>
    public RtfHexEncodingException( string message, Exception cause ) :
      base( message, cause )
    {
    } // RtfHexEncodingException

#if SERIALIZATION
    // ----------------------------------------------------------------------
    /// <summary>Serialization support.</summary>
    /// <param name="info">the info to use for serialization</param>
    /// <param name="context">the context to use for serialization</param>
    protected RtfHexEncodingException( SerializationInfo info, StreamingContext context ) :
      base( info, context )
    {
    } // RtfHexEncodingException
#endif

  } // class RtfHexEncodingException

} // namespace RtfPipe
// -- EOF -------------------------------------------------------------------
