using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe.Tokens
{
  public class Field : ControlTag
  {
    public override string Name => "field";
  }

  public class FieldInstructions : ControlTag
  {
    public override string Name => "fldinst";
  }

  public class FieldResult : ControlTag
  {
    public override string Name => "fldrslt";
  }

  public class BookmarkStart : ControlTag
  {
    public override string Name => "bkmkstart";
  }

  public class BookmarkEnd : ControlTag
  {
    public override string Name => "bkmkend";
  }
}
