using System;
using System.Collections;
using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{


  public sealed class RtfInterpreterContext : IRtfInterpreterContext
  {

    public RtfInterpreterState State
    {
      get { return state; }
      set { state = value; }
    }

    public int RtfVersion
    {
      get { return rtfVersion; }
      set { rtfVersion = value; }
    }

    public string DefaultFontId
    {
      get { return defaultFontId; }
      set { defaultFontId = value; }
    }

    public IRtfFont DefaultFont
    {
      get
      {
        return fontTable[defaultFontId]
          ?? new RtfFont(DefaultFontId, RtfFontKind.Nil, RtfFontPitch.Default, 0, 0, "serif");
      }
    }

    public RtfFontCollection FontTable
    {
      get { return fontTable; }
    }


    public IList<IRtfColor> ColorTable
    {
      get { return colorTable; }
    }

    public string Generator
    {
      get { return generator; }
      set { generator = value; }
    }

    public IList<IRtfTextFormat> UniqueTextFormats
    {
      get { return uniqueTextFormats; }
    }

    public IRtfTextFormat CurrentTextFormat
    {
      get { return currentTextFormat; }
    }

    public IRtfTextFormat GetSafeCurrentTextFormat()
    {
      return currentTextFormat ?? WritableCurrentTextFormat;
    }

    public IRtfTextFormat GetUniqueTextFormatInstance(IRtfTextFormat templateFormat)
    {
      if (templateFormat == null)
      {
        throw new ArgumentNullException("templateFormat");
      }
      IRtfTextFormat uniqueInstance;
      int existingEquivalentPos = uniqueTextFormats.IndexOf(templateFormat);
      if (existingEquivalentPos >= 0)
      {
        // we already know an equivalent format -> reference that one for future use
        uniqueInstance = uniqueTextFormats[existingEquivalentPos];
      }
      else
      {
        // this is a yet unknown format -> add it to the known formats and use it
        uniqueTextFormats.Add(templateFormat);
        uniqueInstance = templateFormat;
      }
      return uniqueInstance;
    }

    public RtfTextFormat WritableCurrentTextFormat
    {
      get
      {
        if (currentTextFormat == null)
        {
          // set via property to ensure it will get added to the unique map
          WritableCurrentTextFormat = new RtfTextFormat(DefaultFont, RtfSpec.DefaultFontSize);
        }
        return currentTextFormat;
      }
      set
      {
        currentTextFormat = (RtfTextFormat)GetUniqueTextFormatInstance(value);
      }
    }

    public IRtfDocumentInfo DocumentInfo
    {
      get { return documentInfo; }
    }

    public RtfDocumentInfo WritableDocumentInfo
    {
      get { return documentInfo; }
    }

    public IList<IRtfDocumentProperty> UserProperties
    {
      get { return userProperties; }
    }

    public void PushCurrentTextFormat()
    {
      textFormatStack.Push(WritableCurrentTextFormat);
    }

    public void PopCurrentTextFormat()
    {
      if (textFormatStack.Count == 0)
      {
        throw new RtfStructureException(Strings.InvalidTextContextState);
      }
      currentTextFormat = (RtfTextFormat)textFormatStack.Pop();
    }

    public void Reset()
    {
      state = RtfInterpreterState.Init;
      rtfVersion = RtfSpec.RtfVersion1;
      defaultFontId = "f0";
      fontTable.Clear();
      colorTable.Clear();
      generator = null;
      uniqueTextFormats.Clear();
      textFormatStack.Clear();
      currentTextFormat = null;
      documentInfo.Reset();
      userProperties.Clear();
    }

    private RtfInterpreterState state;
    private int rtfVersion;
    private string defaultFontId;
    private readonly RtfFontCollection fontTable = new RtfFontCollection();
    private readonly IList<IRtfColor> colorTable = new List<IRtfColor>();
    private string generator;
    private readonly IList<IRtfTextFormat> uniqueTextFormats = new List<IRtfTextFormat>();
    private readonly Stack<RtfTextFormat> textFormatStack = new Stack<RtfTextFormat>();
    private RtfTextFormat currentTextFormat;
    private readonly RtfDocumentInfo documentInfo = new RtfDocumentInfo();
    private readonly IList<IRtfDocumentProperty> userProperties = new List<IRtfDocumentProperty>();

  }

}
