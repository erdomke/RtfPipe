using RtfPipe.Tokens;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public class Font : IToken
  {
    private readonly List<IToken> _tokens = new List<IToken>();

    public int Id { get; }
    public string Name { get; private set; }
    public Encoding Encoding { get; private set; }
    public TokenType Type => TokenType.CharacterFormat;
    public FontFamilyCategory Category { get; private set; }

    public Font(int id)
    {
      Id = id;
    }

    internal void Add(IToken token)
    {
      if (token is TextToken text)
        Name = text.Value.TrimEnd(';');
      else if (token is FontCharSet charSet)
        Encoding = charSet.Value;
      else if (token is FontCategory category)
        Category = category.Value;
      else
        _tokens.Add(token);
    }
  }
}
