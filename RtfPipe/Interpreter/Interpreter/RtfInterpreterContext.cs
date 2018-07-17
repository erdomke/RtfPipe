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

    public Font DefaultFont
    {
      get
      {
        return fontTable[defaultFontId]
          ?? new Font(DefaultFontId, RtfFontKind.Nil, RtfFontPitch.Default, 0, 0, "serif");
      }
    }

    public RtfFontCollection FontTable
    {
      get { return fontTable; }
    }


    public IList<ColorValue> ColorTable
    {
      get { return colorTable; }
    }

    public string Generator
    {
      get { return generator; }
      set { generator = value; }
    }

    public IList<Style> UniqueTextFormats
    {
      get { return uniqueTextFormats; }
    }

    public Style CurrentTextFormat
    {
      get { return currentTextFormat; }
    }

    public Style GetSafeCurrentTextFormat()
    {
      return currentTextFormat ?? WritableCurrentTextFormat;
    }

    public Style GetUniqueTextFormatInstance(Style templateFormat)
    {
      if (templateFormat == null)
      {
        throw new ArgumentNullException("templateFormat");
      }
      Style uniqueInstance;
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

    public Style WritableCurrentTextFormat
    {
      get
      {
        if (currentTextFormat == null)
        {
          // set via property to ensure it will get added to the unique map
          WritableCurrentTextFormat = new Style(DefaultFont, RtfSpec.DefaultFontSize);
        }
        return currentTextFormat;
      }
      set
      {
        currentTextFormat = (Style)GetUniqueTextFormatInstance(value);
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
      currentTextFormat = (Style)textFormatStack.Pop();
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
    private readonly IList<ColorValue> colorTable = new List<ColorValue>();
    private string generator;
    private readonly IList<Style> uniqueTextFormats = new List<Style>();
    private readonly Stack<Style> textFormatStack = new Stack<Style>();
    private Style currentTextFormat;
    private readonly RtfDocumentInfo documentInfo = new RtfDocumentInfo();
    private readonly IList<IRtfDocumentProperty> userProperties = new List<IRtfDocumentProperty>();

  }

}
