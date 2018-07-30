using System.Text;

namespace RtfPipe.Tokens
{
  public class FontRef : ControlWord<int>
  {
    public override string Name => "f";

    public FontRef(int value) : base(value) { }
  }

  public class DefaultFontRef : ControlWord<int>
  {
    public override string Name => "deff";
    public override TokenType Type => TokenType.HeaderTag;

    public DefaultFontRef(int value) : base(value) { }
  }

  public class FontTableTag : ControlTag
  {
    public override string Name => "fonttbl";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class FontCategory : ControlTag
  {
    public override string Name { get { return "f" + Value.ToString().ToLowerInvariant(); } }
    public override TokenType Type => TokenType.HeaderTag;
    public FontFamilyCategory Value { get; }

    public FontCategory(FontFamilyCategory category)
    {
      Value = category;
    }
  }

  public class FontCharSet : ControlWord<Encoding>
  {
    public override string Name => "fcharset";
    public override TokenType Type => TokenType.HeaderTag;

    public FontCharSet(Encoding value) : base(value) { }
  }

  public class CodePage : ControlWord<Encoding>
  {
    public override string Name => "cpg";

    public CodePage(Encoding value) : base(value) { }
  }

  public class DefaultCodePage : ControlWord<Encoding>
  {
    public override string Name => "ansicpg";
    public override TokenType Type => TokenType.HeaderTag;

    public DefaultCodePage(Encoding value) : base(value) { }
  }

  public class DefaultNamedCodePage : ControlWord<Encoding>
  {
    public override string Name { get { return CodePage.ToString().ToLowerInvariant(); } }
    public override TokenType Type => TokenType.HeaderTag;
    public NamedCodePage CodePage { get; }

    public DefaultNamedCodePage(NamedCodePage value) : base(TextEncoding.EncodingFromCodePage((int)value))
    {
      CodePage = value;
    }
  }

  public class FontPitchToken : ControlWord<FontPitch>
  {
    public override string Name => "fprq";
    public override TokenType Type => TokenType.HeaderTag;

    public FontPitchToken(FontPitch value) : base(value) { }
  }

  public class FontSize : ControlWord<UnitValue>
  {
    public override string Name => "fs";
    public override TokenType Type => TokenType.CharacterFormat;

    public FontSize(UnitValue value) : base(value) { }
  }

  public class UnicodeTextTag : ControlTag
  {
    public override string Name => "ud";
  }
}
