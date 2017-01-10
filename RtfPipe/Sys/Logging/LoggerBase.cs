// -- FILE ------------------------------------------------------------------
// name       : LoggerBase.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.11
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;

namespace RtfPipe.Sys.Logging
{

  // ------------------------------------------------------------------------
  public abstract class LoggerBase
  {

    // ----------------------------------------------------------------------
    public virtual string Context
    {
      get { return string.Empty; }
    } // Context

    // ----------------------------------------------------------------------
    public virtual bool IsSupportedException( Exception exception )
    {
#if NETFULL
      return !(exception is System.Threading.ThreadAbortException);
#else
      return true;
#endif
    } // IsSupportedException

    // ----------------------------------------------------------------------
    public virtual IDisposable PushContext( string context )
    {
      if ( context == null )
      {
        throw new ArgumentNullException( "context" );
      }
      return new LoggerContextDisposable( Logger );
    } // PushContext

    // ----------------------------------------------------------------------
    public virtual int ContextDepth
    {
      get { return 0; }
    } // ContextDepth

    // ----------------------------------------------------------------------
    public virtual void PopContext()
    {
    } // PopContext

    // ----------------------------------------------------------------------
    protected abstract ILogger Logger { get; }

  } // class LoggerBase

} // namespace RtfPipe.Sys.Logging
// -- EOF -------------------------------------------------------------------
