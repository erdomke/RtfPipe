// -- FILE ------------------------------------------------------------------
// name       : LoggerMonitorLog4net.cs
// project    : System Framelet
// created    : Leon Poyyayil - 2006.05.12
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Sys.Logging
{

  // ------------------------------------------------------------------------
// ReSharper disable InconsistentNaming
  public sealed class LoggerMonitorLog4net : ILoggerMonitor
// ReSharper restore InconsistentNaming
  {

    // ----------------------------------------------------------------------
    public void Register( ILoggerListener loggerListener, string context )
    {
      if ( loggerListener == null )
      {
        throw new ArgumentNullException( "loggerListener" );
      }
      string checkedContext = ArgumentCheck.NonemptyTrimmedString( context, "context" );
      lock ( listenerListsByContext )
      {
        List<ILoggerListener> contextListeners;
        if (!listenerListsByContext.TryGetValue(checkedContext, out contextListeners))
        {
          contextListeners = new List<ILoggerListener>(5);
          listenerListsByContext.Add( checkedContext, contextListeners );
        }
        lock ( contextListeners )
        {
          contextListeners.Add( loggerListener );
        }
      }
    } // Register

    // ----------------------------------------------------------------------
    public void Unregister( ILoggerListener loggerListener, string context )
    {
      if ( loggerListener == null )
      {
        throw new ArgumentNullException( "loggerListener" );
      }
      string checkedContext = ArgumentCheck.NonemptyTrimmedString( context, "context" );
      lock ( listenerListsByContext )
      {
        List<ILoggerListener> contextListeners;
        if (listenerListsByContext.TryGetValue(checkedContext, out contextListeners))
        {
          lock ( contextListeners )
          {
            contextListeners.Remove( loggerListener );
          }
          if ( contextListeners.Count == 0 )
          {
            listenerListsByContext.Remove( checkedContext );
          }
        }
      }
    } // Unregister

    // ----------------------------------------------------------------------
    internal void Handle( ILoggerEvent loggerEvent )
    {
      if ( loggerEvent == null )
      {
        throw new ArgumentNullException( "loggerEvent" );
      }

      string eventContext = loggerEvent.Context;

      List<ILoggerListener> contextListeners = null;
      if ( listenerListsByContext.Count > 0 )
      {
        lock ( listenerListsByContext )
        {
          if (!listenerListsByContext.TryGetValue(eventContext, out contextListeners))
          {
            foreach ( var rootContext in listenerListsByContext.Keys )
            {
              if ( eventContext.StartsWith( rootContext ) )
              {
                contextListeners = listenerListsByContext[ rootContext ];
                break;
              }
            }
          }
        }
      }

      if ( contextListeners != null && contextListeners.Count > 0 )
      {
        lock ( contextListeners )
        {
          foreach ( var loggerListener in contextListeners )
          {
            loggerListener.Handle( loggerEvent );
          }
        }
      }
    } // Handle

    // ----------------------------------------------------------------------
    // members
    private readonly Dictionary<string, List<ILoggerListener>> listenerListsByContext = new Dictionary<string, List<ILoggerListener>>();

  } // class LoggerMonitorLog4net

} // namespace RtfPipe.Sys.Logging
// -- EOF -------------------------------------------------------------------
