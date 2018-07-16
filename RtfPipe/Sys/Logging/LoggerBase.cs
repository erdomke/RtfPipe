// project    : System Framelet
using System;

namespace RtfPipe.Sys.Logging
{


  public abstract class LoggerBase
  {

    public virtual string Context
    {
      get { return string.Empty; }
    }

    public virtual bool IsSupportedException( Exception exception )
    {
#if NETFULL
      return !(exception is System.Threading.ThreadAbortException);
#else
      return true;
#endif
    }

    public virtual IDisposable PushContext( string context )
    {
      if ( context == null )
      {
        throw new ArgumentNullException( "context" );
      }
      return new LoggerContextDisposable( Logger );
    }

    public virtual int ContextDepth
    {
      get { return 0; }
    }

    public virtual void PopContext()
    {
    }

    protected abstract ILogger Logger { get; }

  }

}

