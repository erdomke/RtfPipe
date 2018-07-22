using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  [Flags]
  public enum TokenType
  {
    None = 0x0,
    Group = 0x2,
    GroupEnd = 0x3,
    Text = 0x4,
    Word = 0x8,
    CharacterFormat = 0x18,
    ParagraphFormat = 0x28,
    SectionFormat = 0x48,
    HtmlFormat = 0x88,
    HeaderTag = 0x1008,
  }
}
