using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RtfPipe
{
  internal class FormatContext : IEnumerable<IToken>
  {
    private readonly List<IToken> _formats = new List<IToken>();

    public bool InTable { get; set; }

    public void Add(IToken token)
    {
      if (token is SectionDefault)
      {
        RemoveWhere(t => t.Type == TokenType.SectionFormat);
      }
      else if (token is ParagraphDefault)
      {
        RemoveWhere(t => t.Type == TokenType.ParagraphFormat);
        InTable = false;
      }
      else if (token is RowDefaults)
      {
        RemoveWhere(t => t.Type == TokenType.RowFormat);
        InTable = true;
      }
      else if (token is InTable)
      {
        InTable = true;
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
        AddInternal(token);
      }
    }

    protected virtual Func<IToken, bool> SameTokenPredicate(IToken token)
    {
      if (token is OffsetToken)
        return t => t is OffsetToken;
      else if (token is TextAlign)
        return t => t is TextAlign;
      else if (token is Font font)
        return t => t is Font;
      else if (token is TabPosition || token is TabAlignment)
        return t => false;
      else if (token is IWord word)
        return t => t is IWord currWord && currWord.Name == word.Name;
      return t => false;
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

    protected void AddRange(FormatContext tokens)
    {
      if (_formats.Count < 1)
        _formats.AddRange(tokens);
      else
        AddRange((IEnumerable<IToken>)tokens);
    }

    public void RemoveRange(IEnumerable<IToken> tokens)
    {
      var i = 0;
      while (i < _formats.Count)
      {
        if (tokens.Contains(_formats[i]))
          _formats.RemoveAt(i);
        else
          i++;
      }
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

    public T RemoveFirstOfType<T>()
    {
      for (var i = 0; i < _formats.Count; i++)
      {
        if (_formats[i] is T result)
        {
          _formats.RemoveAt(i);
          return result;
        }
      }
      return default;
    }

    public bool TryGetValue<T>(out T value)
    {
      foreach (var token in _formats)
      {
        if (token is T result)
        {
          value = result;
          return true;
        }
      }

      value = default;
      return false;
    }

    public FormatContext Clone()
    {
      var result = new FormatContext() { InTable = InTable };
      result._formats.AddRange(_formats);
      return result;
    }

    public ParagraphTab GetTab(int idx, UnitValue defaultWidth, UnitValue startPosition = default)
    {
      var curr = 0;
      var tab = new ParagraphTab();
      var buffer = TextAlignment.Left;

      foreach (var token in _formats)
      {
        if (token is TabPosition tabPos)
        {
          tab.Position = tabPos.Value;
          tab.Alignment = buffer;
          buffer = TextAlignment.Left;
          if (tab.Position > startPosition)
            curr++;
          if (curr == idx)
            return tab;
        }
        else if (token is TabAlignment tabAlign)
        {
          buffer = tabAlign.Value;
        }
      }

      if (defaultWidth.ToPx() == 0)
        defaultWidth = new UnitValue(720, UnitType.Twip);

      tab.Position += defaultWidth * (idx - curr);
      tab.Alignment = TextAlignment.Left;
      return tab;
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
