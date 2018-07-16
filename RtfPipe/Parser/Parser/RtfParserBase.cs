using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Parser
{

  public abstract class RtfParserBase
  {

    protected RtfParserBase()
    {
    }

    protected RtfParserBase(params IRtfParserListener[] listeners)
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          AddParserListener(listener);
        }
      }
    }

    /// <summary>
    /// Determines whether to ignore all content after the root group ends.
    /// Set this to true when parsing content from streams which contain other
    /// data after the RTF or if the writer of the RTF is known to terminate the
    /// actual RTF content with a null byte (as some popular sources such as
    /// WordPad are known to behave).
    /// </summary>
    public bool IgnoreContentAfterRootGroup { get; set; }

    /// <summary>
    /// Adds a listener that will get notified along the parsing process.
    /// </summary>
    /// <param name="listener">the listener to add</param>
    /// <exception cref="ArgumentNullException">listener</exception>
    public void AddParserListener(IRtfParserListener listener)
    {
      if (listener == null)
      {
        throw new ArgumentNullException("listener");
      }
      if (listeners == null)
      {
        listeners = new List<IRtfParserListener>();
      }
      if (!listeners.Contains(listener))
      {
        listeners.Add(listener);
      }
    }

    /// <summary>
    /// Removes a listener from this instance.
    /// </summary>
    /// <param name="listener">the listener to remove</param>
    /// <exception cref="ArgumentNullException">listener</exception>
    public void RemoveParserListener(IRtfParserListener listener)
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

    /// <summary>
    /// Parses the given RTF text that is read from the given source.
    /// </summary>
    /// <param name="rtfTextSource">the source with RTF text to parse</param>
    /// <exception cref="ArgumentNullException">rtfTextSource</exception>
    public void Parse(IRtfSource rtfTextSource)
    {
      if (rtfTextSource == null)
      {
        throw new ArgumentNullException("rtfTextSource");
      }
      DoParse(rtfTextSource);
    }

    protected abstract void DoParse(IRtfSource rtfTextSource);

    protected void NotifyParseBegin()
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.ParseBegin();
        }
      }
    }

    protected void NotifyGroupBegin()
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.GroupBegin();
        }
      }
    }

    protected void NotifyTagFound(IRtfTag tag)
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.TagFound(tag);
        }
      }
    }

    protected void NotifyTextFound(IRtfText text)
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.TextFound(text);
        }
      }
    }

    protected void NotifyGroupEnd()
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.GroupEnd();
        }
      }
    }

    protected void NotifyParseSuccess()
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.ParseSuccess();
        }
      }
    }

    protected void NotifyParseFail(RtfException reason)
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.ParseFail(reason);
        }
      }
    }

    protected void NotifyParseEnd()
    {
      if (listeners != null)
      {
        foreach (IRtfParserListener listener in listeners)
        {
          listener.ParseEnd();
        }
      }
    }

    private List<IRtfParserListener> listeners;

  }

}

