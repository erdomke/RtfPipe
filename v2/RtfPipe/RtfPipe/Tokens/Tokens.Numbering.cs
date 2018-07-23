using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class ParagraphNumbering : ControlTag
  {
    public override string Name => "pn";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class NumberingTextFallback : ControlTag
  {
    public override string Name => "pntext";
  }

  public class NumberLevelBullet : ControlTag
  {
    public override string Name => "pnlvlblt";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class NumberLevelBody : ControlTag
  {
    public override string Name => "pnlvlbody";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class NumberingIndent : ControlWord<int>
  {
    public override string Name => "pnindent";
    public override TokenType Type => TokenType.ParagraphFormat;

    public NumberingIndent(int value) : base(value) { }
  }

  public class BulletText : ControlTag
  {
    public override string Name => "pntxtb";
    public override TokenType Type => TokenType.ParagraphFormat;
  }

  public class NumberingStart : ControlWord<int>
  {
    public override string Name => "pnstart";
    public override TokenType Type => TokenType.ParagraphFormat;

    public NumberingStart(int value) : base(value) { }
  }

  public class NumberingTypeToken : ControlWord<NumberingType>
  {
    public override string Name => "pn" + ValueToTag();
    public override TokenType Type => TokenType.ParagraphFormat;

    public NumberingTypeToken(NumberingType value) : base(value) { }

    public override string ToString()
    {
      return "\\" + Name;
    }

    private string ValueToTag()
    {
      switch (Value)
      {
        case NumberingType.CardinalText:
          return "card";
        case NumberingType.Numbers:
          return "dec";
        case NumberingType.LowerLetter:
          return "lcltr";
        case NumberingType.LowerRoman:
          return "lcrm";
        case NumberingType.Ordinal:
          return "ord";
        case NumberingType.OrdinalText:
          return "ordt";
        case NumberingType.UpperLetter:
          return "ucltr";
        case NumberingType.UpperRoman:
          return "ucrm";
        default:
          throw new NotSupportedException();
      }
    }
  }

  public class NumberingTextAfter : ControlTag
  {
    public override string Name => "pntxta";
  }

  public class ListLevel : ControlTag
  {
    public override string Name => "listlevel";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class ListDefinition : ControlTag
  {
    public override string Name => "list";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class ListTemplateId : ControlWord<int>
  {
    public override string Name => "listtemplateid";

    public ListTemplateId(int value) : base(value) { }
  }

  public class ListId : ControlWord<int>
  {
    public override string Name => "listid";

    public ListId(int value) : base(value) { }
  }

  public class ListLevelType : ControlWord<NumberingType>
  {
    public override string Name => "levelnfc";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ListLevelType(NumberingType value) : base(value) { }
  }

  public class ListLevelJustification : ControlWord<TextAlignment>
  {
    public override string Name => "leveljc";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ListLevelJustification(TextAlignment value) : base(value) { }
  }

  public class LevelText : ControlTag
  {
    public override string Name => "leveltext";
  }

  public class LevelNumbers : ControlTag
  {
    public override string Name => "levelnumbers";
  }

  public class ListOverrideTable : ControlTag
  {
    public override string Name => "listoverridetable";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class ListOverride : ControlTag
  {
    public override string Name => "listoverride";
  }

  public class ListStyleId : ControlWord<int>
  {
    public override string Name => "ls";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ListStyleId(int value) : base(value) { }
  }

  public class ListLevelNumber : ControlWord<int>
  {
    public override string Name => "ilvl";
    public override TokenType Type => TokenType.ParagraphFormat;

    public ListLevelNumber(int value) : base(value) { }
  }

  public class ListTextFallback : ControlTag
  {
    public override string Name => "listtext";
  }
}
