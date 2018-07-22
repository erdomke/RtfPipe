using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe
{
  internal class FormatContext : IEnumerable<IToken>
  {
    private readonly List<IToken> _formats = new List<IToken>();

    public void Add(IToken token)
    {
      if (token is SectionDefault)
      {
        RemoveWhere(t => t.Type == TokenType.SectionFormat);
      }
      else if (token is ParagraphDefault)
      {
        RemoveWhere(t => t.Type == TokenType.ParagraphFormat);
      }
      else if (token is PlainToken)
      {
        RemoveWhere(t => t.Type == TokenType.CharacterFormat);
      }
      else if (token is ControlWord<bool> boolean && !boolean.Value)
      {
        RemoveWhere(t => t is ControlWord<bool> boolStyle && boolStyle.Name == boolean.Name);
      }
      else if (token is NoSuperSubToken)
      {
        RemoveWhere(t => t is SuperStartToken || t is SubStartToken);
      }
      else
      {
        if (token is OffsetToken)
          RemoveWhere(t => t is OffsetToken);
        else if (token is TextAlign)
          RemoveWhere(t => t is TextAlign);
        else if (token is Font font)
          RemoveWhere(t => t is Font);
        else if (token is IWord word)
          RemoveWhere(t => t is IWord currWord && currWord.Name == word.Name);
        AddInternal(token);
      }
    }

    protected virtual void AddInternal(IToken token)
    {
      _formats.Add(token);
    }

    public void AddRange(IEnumerable<IToken> tokens)
    {
      foreach (var token in tokens)
        Add(token);
    }

    public void RemoveWhere(Func<IToken, bool> predicate)
    {
      var i = 0;
      while (i < _formats.Count)
      {
        if (predicate(_formats[i]))
          _formats.RemoveAt(i);
        else
          i++;
      }
    }

    public FormatContext Clone()
    {
      var result = new FormatContext();
      result._formats.AddRange(_formats);
      return result;
    }

    public IEnumerator<IToken> GetEnumerator()
    {
      return _formats.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
