using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public interface IWord : IToken
  {
    string Name { get; }
  }
}
