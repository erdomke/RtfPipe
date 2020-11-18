using RtfPipe.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RtfPipe.Model
{
  internal class StyleList : List<IToken>
  {
    public StyleList() { }

    public StyleList(IEnumerable<IToken> tokens) : base(tokens) { }

    public StyleList Merge(IToken token)
    {
      if (token is SectionDefault)
      {
        this.RemoveWhere(t => t.Type == TokenType.SectionFormat || t.Type == TokenType.ParagraphFormat || t.Type == TokenType.RowFormat || t.Type == TokenType.CellFormat);
      }
      else if (token is ParagraphDefault)
      {
        this.RemoveWhere(t => t.Type == TokenType.ParagraphFormat);
      }
      else if (token is RowDefaults)
      {
        this.RemoveWhere(t => t.Type == TokenType.RowFormat || t.Type == TokenType.CellFormat);
      }
      else if (token is CellDefaults)
      {
        this.RemoveWhere(t => t.Type == TokenType.CellFormat);
      }
      else if (token is PlainToken)
      {
        this.RemoveWhere(t => t.Type == TokenType.CharacterFormat);
      }
      else if (token is BookmarkToken bookmark && !bookmark.Start)
      {
        this.RemoveWhere(t => t is BookmarkToken bkmkStart && bkmkStart.Id == bookmark.Id);
      }
      else if (IsUnderline(token))
      {
        this.RemoveWhere(IsUnderline);
        if (!(token is ControlWord<bool> ulBool) || ulBool.Value)
          Add(token);
      }
      else if (token is ControlWord<bool> boolean && !boolean.Value)
      {
        this.RemoveWhere(t => t is ControlWord<bool> boolStyle && boolStyle.Name == boolean.Name);
      }
      else if (token is NoSuperSubToken)
      {
        this.RemoveWhere(t => t is SuperStartToken || t is SubStartToken);
      }
      else if (token is ControlWord<BorderPosition> borderPosition)
      {
        Add(new BorderToken(borderPosition));
      }
      else if (token is RightCellBoundary)
      {
        var i = Count - 1;
        while (i >= 0 && this[i].Type == TokenType.CellFormat && !(this[i] is CellToken))
          i--;
        var previous = i >= 0 && this[i] is CellToken prevCell ? prevCell : null;
        i++;
        var cellToken = new CellToken(this.Skip(i).Concat(new[] { token }), previous);
        while (i < Count)
          RemoveAt(i);
        Add(cellToken);
      }
      else if (Count > 0 && this[Count - 1] is BorderToken borderToken && borderToken.Add(token))
      {
        // Do nothing
      }
      else
      {
        this.RemoveWhere(SameTokenPredicate(token));
        Add(token);
      }
      return this;
    }

    public StyleList MergeRange(IEnumerable<IToken> tokens)
    {
      foreach (var token in tokens)
        Merge(token);
      return this;
    }

    public void Set(IEnumerable<IToken> styles)
    {
      Clear();
      AddRange(styles);
    }
    
    public bool TryRemoveFirst<S>(out S result) where S : IToken
    {
      result = default(S);
      for (var i = 0; i < Count; i++)
      {
        if (this[i] is S)
        {
          result = (S)this[i];
          RemoveAt(i);
          return true;
        }
      }
      return false;
    }

    public bool TryRemoveFirstTrue<S>(out S result) where S : ControlWord<bool>
    {
      result = default(S);
      for (var i = 0; i < Count; i++)
      {
        if (this[i] is S res && res.Value)
        {
          result = res;
          RemoveAt(i);
          return true;
        }
      }
      return false;
    }

    public bool TryRemoveMany(Func<IToken, bool> predicate, out IList<IToken> results)
    {
      var i = 0;
      results = new List<IToken>();
      while (i < Count)
      {
        if (predicate(this[i]))
        {
          results.Add(this[i]);
          RemoveAt(i);
        }
        else
        {
          i++;
        }
      }
      return results.Count > 0;
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
      else if (token is BorderToken && token.Type == TokenType.CellFormat)
        return t => false;
      else if (token is BorderToken borderToken)
        return t => t is BorderToken compareBorder && compareBorder.Side == borderToken.Side;
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
  }
}
