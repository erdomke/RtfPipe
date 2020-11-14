using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Model
{
  public class StyleSet : ICollection<IToken>
  {
    private List<IToken> _styles = new List<IToken>();

    public int Count => _styles.Count;

    public bool IsReadOnly => false;

    public StyleSet() { }

    public StyleSet(IEnumerable<IToken> tokens)
    {
      _styles.AddRange(tokens);
    }

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
      else if (token is RowDefaults)
      {
        RemoveWhere(t => t.Type == TokenType.RowFormat);
      }
      else if (token is CellDefaults)
      {
        RemoveWhere(t => t.Type == TokenType.CellFormat);
      }
      else if (token is PlainToken)
      {
        RemoveWhere(t => t.Type == TokenType.CharacterFormat);
      }
      else if (token is BookmarkToken bookmark && !bookmark.Start)
      {
        RemoveWhere(t => t is BookmarkToken bkmkStart && bkmkStart.Id == bookmark.Id);
      }
      else if (IsUnderline(token))
      {
        RemoveWhere(IsUnderline);
        if (!(token is ControlWord<bool> ulBool) || ulBool.Value)
          _styles.Add(token);
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
        RemoveWhere(SameTokenPredicate(token));
        _styles.Add(token);
      }
    }

    public void AddRange(IEnumerable<IToken> tokens)
    {
      foreach (var token in tokens)
        Add(token);
    }

    public void Clear()
    {
      _styles.Clear();
    }
    
    public IEnumerator<IToken> GetEnumerator()
    {
      return _styles.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void RemoveWhere(Func<IToken, bool> predicate)
    {
      var i = 0;
      while (i < _styles.Count)
      {
        if (predicate(_styles[i]))
          _styles.RemoveAt(i);
        else
          i++;
      }
    }
    
    private Func<IToken, bool> SameTokenPredicate(IToken token)
    {
      if (token is OffsetToken)
        return t => t is OffsetToken;
      else if (token is TextAlign)
        return t => t is TextAlign;
      else if (token is Font font)
        return t => t is Font;
      else if (token is TabPosition || token is TabAlignment || token is RightCellBoundary)
        return t => false;
      else if (token is IWord word)
        return t => t is IWord currWord && currWord.Name == word.Name;
      return t => false;
    }

    public static bool IsUnderline(IToken token)
    {
      return token.Type == TokenType.CharacterFormat
        && token is IWord underline
        && underline.Name.StartsWith("ul")
        && !(underline is UnderlineColor);
    }

    public bool Contains(IToken item)
    {
      return _styles.Contains(item);
    }

    public void CopyTo(IToken[] array, int arrayIndex)
    {
      _styles.CopyTo(array, arrayIndex);
    }

    public bool Remove(IToken item)
    {
      return _styles.Remove(item);
    }
  }
}
