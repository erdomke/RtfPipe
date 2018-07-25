using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  internal class HyperlinkToken : IToken
  {
    public TokenType Type => TokenType.CharacterFormat;

    public string Url { get; set; }
    public string Target { get; set; }
    public string Title { get; set; }

    public HyperlinkToken(string[] args)
    {
      for (var i = 1; i < args.Length; i++)
      {
        switch (args[i])
        {
          case @"\l":
            if ((i + 1) < args.Length)
            {
              Url = "#" + args[i + 1].Trim('"');
              i++;
            }
            break;
          case @"\m": // Coordinates for an image map
            break;
          case @"\n":
            Target = "_blank";
            break;
          case @"\o":
            if ((i + 1) < args.Length)
            {
              Title = args[i + 1].Trim('"');
              i++;
            }
            break;
          case @"\t":
            if ((i + 1) < args.Length)
            {
              Target = args[i + 1].Trim('"');
              i++;
            }
            break;
          default:
            if (args[i].StartsWith("\""))
              Url = args[i].Trim('"');
            break;
        }
      }
    }
  }
}
