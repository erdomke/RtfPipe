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

    Format = 0x1000,
    Character = 0x10,
    Paragraph = 0x20,
    Section = 0x40,
    Html = 0x80,
    Row = 0x100,
    Cell = 0x200,

    CharacterFormat = Word | Character | Format,
    ParagraphFormat = Word | Paragraph | Format,
    SectionFormat = Word | Section | Format,
    HtmlFormat = Word | Html | Format,
    RowFormat = Word | Row | Format,
    CellFormat = Word | Cell | Format,

    BreakTag = 0x2000 | Word,

    HeaderTag = 0x100008,
    PictureTypeTag = 0x200008,
  }
}
