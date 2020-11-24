using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe
{
  internal class HyperlinkToken : IToken
  {
    public TokenType Type => TokenType.CharacterFormat;

    public string Url { get; set; }
    public string Target { get; set; }
    public string Title { get; set; }

    public HyperlinkToken() { }

    public HyperlinkToken(IList<IToken> args)
    {
      for (var i = 1; i < args.Count; i++)
      {
        if (args[i] is FieldSwitch fieldSwitch)
        {
          switch (fieldSwitch.Value)
          {
            case "l":
              if ((i + 1) < args.Count && args[i + 1] is TextToken bookmark)
              {
                Url = "#" + bookmark.Value;
                i++;
              }
              break;
            case "m": // Coordinates for an image map
              break;
            case "n":
              Target = "_blank";
              break;
            case "o":
              if ((i + 1) < args.Count && args[i + 1] is TextToken title)
              {
                Title = title.Value;
                i++;
              }
              break;
            case @"\t":
              if ((i + 1) < args.Count && args[i + 1] is TextToken target)
              {
                Target = target.Value;
                i++;
              }
              break;
          }
        }
        else if (args[i] is TextToken url)
        {
          Url = url.Value;
        }
      }
    }
  }
}
