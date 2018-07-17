using System.Text;
using RtfPipe.Model;
using RtfPipe.Support;

namespace RtfPipe.Interpreter
{

  public sealed class RtfFontBuilder : RtfElementVisitorBase
  {

    public RtfFontBuilder() :
      base(RtfElementVisitorOrder.NonRecursive)
    {
      // we iterate over our children ourselves -> hence non-recursive
      Reset();
    }

    public string FontId
    {
      get { return fontId; }
    }

    public int FontIndex
    {
      get { return fontIndex; }
    }

    public int FontCharset
    {
      get { return fontCharset; }
    }

    public int FontCodePage
    {
      get { return fontCodePage; }
    }

    public RtfFontKind FontKind
    {
      get { return fontKind; }
    }

    public RtfFontPitch FontPitch
    {
      get { return fontPitch; }
    }

    public string FontName
    {
      get
      {
        string fontName = null;
        int len = fontNameBuffer.Length;
        if (len > 0 && fontNameBuffer[len - 1] == ';')
        {
          fontName = fontNameBuffer.ToString().Substring(0, len - 1).Trim();
          if (fontName.Length == 0)
          {
            fontName = null;
          }
        }
        return fontName;
      }
    }

    public Font CreateFont()
    {
      string fontName = FontName;
      if (string.IsNullOrEmpty(fontName))
      {
        fontName = "UnnamedFont_" + fontId;
      }
      return new Font(fontId, fontKind, fontPitch,
        fontCharset, fontCodePage, fontName);
    }

    public void Reset()
    {
      fontIndex = 0;
      fontCharset = 0;
      fontCodePage = 0;
      fontKind = RtfFontKind.Nil;
      fontPitch = RtfFontPitch.Default;
      fontNameBuffer.Remove(0, fontNameBuffer.Length);
    }

    protected override void DoVisitGroup(IRtfGroup group)
    {
      switch (group.Destination)
      {
        case RtfSpec.TagFont:
        case RtfSpec.TagThemeFontLoMajor:
        case RtfSpec.TagThemeFontHiMajor:
        case RtfSpec.TagThemeFontDbMajor:
        case RtfSpec.TagThemeFontBiMajor:
        case RtfSpec.TagThemeFontLoMinor:
        case RtfSpec.TagThemeFontHiMinor:
        case RtfSpec.TagThemeFontDbMinor:
        case RtfSpec.TagThemeFontBiMinor:
          VisitGroupChildren(group);
          break;
      }
    }

    protected override void DoVisitTag(IRtfTag tag)
    {
      switch (tag.Name)
      {
        case RtfSpec.TagThemeFontLoMajor:
        case RtfSpec.TagThemeFontHiMajor:
        case RtfSpec.TagThemeFontDbMajor:
        case RtfSpec.TagThemeFontBiMajor:
        case RtfSpec.TagThemeFontLoMinor:
        case RtfSpec.TagThemeFontHiMinor:
        case RtfSpec.TagThemeFontDbMinor:
        case RtfSpec.TagThemeFontBiMinor:
          // skip and ignore for the moment
          break;
        case RtfSpec.TagFont:
          fontId = tag.FullName;
          fontIndex = tag.ValueAsNumber;
          break;
        case RtfSpec.TagFontKindNil:
          fontKind = RtfFontKind.Nil;
          break;
        case RtfSpec.TagFontKindRoman:
          fontKind = RtfFontKind.Roman;
          break;
        case RtfSpec.TagFontKindSwiss:
          fontKind = RtfFontKind.Swiss;
          break;
        case RtfSpec.TagFontKindModern:
          fontKind = RtfFontKind.Modern;
          break;
        case RtfSpec.TagFontKindScript:
          fontKind = RtfFontKind.Script;
          break;
        case RtfSpec.TagFontKindDecor:
          fontKind = RtfFontKind.Decor;
          break;
        case RtfSpec.TagFontKindTech:
          fontKind = RtfFontKind.Tech;
          break;
        case RtfSpec.TagFontKindBidi:
          fontKind = RtfFontKind.Bidi;
          break;
        case RtfSpec.TagFontCharset:
          fontCharset = tag.ValueAsNumber;
          break;
        case RtfSpec.TagCodePage:
          fontCodePage = tag.ValueAsNumber;
          break;
        case RtfSpec.TagFontPitch:
          switch (tag.ValueAsNumber)
          {
            case 0:
              fontPitch = RtfFontPitch.Default;
              break;
            case 1:
              fontPitch = RtfFontPitch.Fixed;
              break;
            case 2:
              fontPitch = RtfFontPitch.Variable;
              break;
          }
          break;
      }
    }

    protected override void DoVisitText(IRtfText text)
    {
      fontNameBuffer.Append(text.Text);
    }

    private string fontId;
    private int fontIndex;
    private int fontCharset;
    private int fontCodePage;
    private RtfFontKind fontKind;
    private RtfFontPitch fontPitch;
    private readonly StringBuilder fontNameBuffer = new StringBuilder();

  }

}
