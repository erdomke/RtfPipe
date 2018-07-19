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

    public DefaultFontRef(int value) : base(value) { }
  }

  public class FontTableTag : ControlTag
  {
    public override string Name => "fonttbl";
  }

  public class FontCategory : ControlTag
  {
    public override string Name { get { return "f" + Value.ToString().ToLowerInvariant(); } }
    public FontFamilyCategory Value { get; }

    public FontCategory(FontFamilyCategory category)
    {
      Value = category;
    }
  }

  public class FontCharSet : ControlWord<Encoding>
  {
    public override string Name => "fcharset";

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

    public DefaultCodePage(Encoding value) : base(value) { }
  }

  public class DefaultNamedCodePage : ControlWord<Encoding>
  {
    public override string Name { get { return CodePage.ToString().ToLowerInvariant(); } }
    public NamedCodePage CodePage { get; }

    public DefaultNamedCodePage(NamedCodePage value) : base(TextEncoding.EncodingFromCodePage((int)value))
    {
      CodePage = value;
    }
  }

  public class FontPitchToken : ControlWord<FontPitch>
  {
    public override string Name => "fprq";

    public FontPitchToken(FontPitch value) : base(value) { }
  }

  public class FontSize : ControlWord<UnitValue>
  {
    public override string Name => "fs";

    public FontSize(UnitValue value) : base(value) { }
  }

  public class UnicodeTextTag : ControlTag
  {
    public override string Name => "ud";
  }
}
