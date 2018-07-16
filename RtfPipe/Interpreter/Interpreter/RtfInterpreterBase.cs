using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Interpreter
{


  public abstract class RtfInterpreterBase : IRtfInterpreter
  {

    protected RtfInterpreterBase(params IRtfInterpreterListener[] listeners) :
      this(new RtfInterpreterSettings(), listeners)
    {
    }

    protected RtfInterpreterBase(IRtfInterpreterSettings settings, params IRtfInterpreterListener[] listeners)
    {
      if (settings == null)
      {
        throw new ArgumentNullException("settings");
      }

      this.settings = settings;
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          AddInterpreterListener(listener);
        }
      }
    }

    public IRtfInterpreterSettings Settings
    {
      get { return settings; }
    }

    public void AddInterpreterListener(IRtfInterpreterListener listener)
    {
      if (listener == null)
      {
        throw new ArgumentNullException("listener");
      }
      if (listeners == null)
      {
        listeners = new List<IRtfInterpreterListener>();
      }
      if (!listeners.Contains(listener))
      {
        listeners.Add(listener);
      }
    }

    public void RemoveInterpreterListener(IRtfInterpreterListener listener)
    {
      if (listener == null)
      {
        throw new ArgumentNullException("listener");
      }
      if (listeners != null)
      {
        if (listeners.Contains(listener))
        {
          listeners.Remove(listener);
        }
        if (listeners.Count == 0)
        {
          listeners = null;
        }
      }
    }

    public void Interpret(IRtfGroup rtfDocument)
    {
      if (rtfDocument == null)
      {
        throw new ArgumentNullException("rtfDocument");
      }
      DoInterpret(rtfDocument);
    }

    protected abstract void DoInterpret(IRtfGroup rtfDocument);

    protected void NotifyBeginDocument()
    {
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          listener.BeginDocument(context);
        }
      }
    }

    protected void NotifyInsertText(string text)
    {
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          listener.InsertText(context, text);
        }
      }
    }

    protected void NotifyInsertSpecialChar(RtfVisualSpecialCharKind kind)
    {
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          listener.InsertSpecialChar(context, kind);
        }
      }
    }

    protected void NotifyInsertBreak(RtfVisualBreakKind kind)
    {
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          listener.InsertBreak(context, kind);
        }
      }
    }

    protected void NotifyInsertImage(RtfVisualImageFormat format,
      int width, int height, int desiredWidth, int desiredHeight,
      int scaleWidthPercent, int scaleHeightPercent, string imageDataHex
    )
    {
      if (listeners != null)
      {
        foreach (IRtfInterpreterListener listener in listeners)
        {
          listener.InsertImage(
            context,
            format,
            width,
            height,
            desiredWidth,
            desiredHeight,
            scaleWidthPercent,
            scaleHeightPercent,
            imageDataHex);
        }
      }
    }

    protected void NotifyEndDocument()
    {
      if (listeners != null)
      {
        foreach (var listener in listeners)
        {
          listener.EndDocument(context);
        }
      }
    }

    protected RtfInterpreterContext Context
    {
      get { return context; }
    }

    private readonly RtfInterpreterContext context = new RtfInterpreterContext();
    private readonly IRtfInterpreterSettings settings;
    private List<IRtfInterpreterListener> listeners;

  }

}
