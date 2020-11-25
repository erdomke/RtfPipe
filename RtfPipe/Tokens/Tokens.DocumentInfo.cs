using System;

namespace RtfPipe.Tokens
{
  public class Info : ControlTag
  {
    public override string Name => "info";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Title : ControlTag
  {
    public override string Name => "title";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class Subject : ControlTag
  {
    public override string Name => "subject";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Author : ControlTag
  {
    public override string Name => "author";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Manager : ControlTag
  {
    public override string Name => "manager";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class Company : ControlTag
  {
    public override string Name => "company";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Operator : ControlTag
  {
    public override string Name => "operator";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Category : ControlTag
  {
    public override string Name => "category";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class Keywords : ControlTag
  {
    public override string Name => "keywords";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class Comment : ControlTag
  {
    public override string Name => "comment";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class DocComment : ControlTag
  {
    public override string Name => "doccomm";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class HyperlinkBase : ControlTag
  {
    public override string Name => "hlinkbase";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class CreateTime : ControlTag
  {
    public override string Name => "creatim";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class RevisionTime : ControlTag
  {
    public override string Name => "revtim";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class PrintTime : ControlTag
  {
    public override string Name => "printtim";
    public override TokenType Type => TokenType.HeaderTag;
  }
  
  public class BackupTime : ControlTag
  {
    public override string Name => "buptim";
    public override TokenType Type => TokenType.HeaderTag;
  }

  public class Year : ControlWord<int>
  {
    public override string Name => "yr";

    public Year(int value) : base(value) { }
  }

  public class Month : ControlWord<int>
  {
    public override string Name => "mo";

    public Month(int value) : base(value) { }
  }

  public class Day : ControlWord<int>
  {
    public override string Name => "dy";

    public Day(int value) : base(value) { }
  }

  public class Hour : ControlWord<int>
  {
    public override string Name => "hr";

    public Hour(int value) : base(value) { }
  }

  public class Minute : ControlWord<int>
  {
    public override string Name => "min";

    public Minute(int value) : base(value) { }
  }

  public class Second : ControlWord<int>
  {
    public override string Name => "sec";

    public Second(int value) : base(value) { }
  }
  
  public class Version : ControlWord<int>
  {
    public override string Name => "version";

    public Version(int value) : base(value) { }
  }
  
  public class EditingTime : ControlWord<TimeSpan>
  {
    public override string Name => "edmins";

    public EditingTime(TimeSpan value) : base(value) { }
  }

  public class NumPages : ControlWord<int>
  {
    public override string Name => "nofpages";

    public NumPages(int value) : base(value) { }
  }

  public class NumWords : ControlWord<int>
  {
    public override string Name => "nofwords";

    public NumWords(int value) : base(value) { }
  }

  public class NumChars : ControlWord<int>
  {
    public override string Name => "nofchars";

    public NumChars(int value) : base(value) { }
  }

  public class NumCharsWs : ControlWord<int>
  {
    public override string Name => "nofcharsws";

    public NumCharsWs(int value) : base(value) { }
  }

  public class InternalVersion : ControlWord<int>
  {
    public override string Name => "vern";

    public InternalVersion(int value) : base(value) { }
  }
}
