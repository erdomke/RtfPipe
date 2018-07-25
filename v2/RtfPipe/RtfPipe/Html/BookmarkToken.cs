using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal class BookmarkToken : IToken
  {
    public TokenType Type => TokenType.CharacterFormat;
    public string Id { get; set; }
    public bool Start { get; set; } = true;
  }
}
