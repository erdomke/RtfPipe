using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class Builder
  {
    public Element Build(Document doc)
    {
      var body = new List<IToken>();
      foreach (var token in doc.Contents)
      {
        if (token is DefaultFontRef defaultFont)
        {
          //_html.DefaultFont = doc.FontTable.TryGetValue(defaultFont.Value, out var font) ? font : doc.FontTable.FirstOrDefault().Value;
        }
        else if (token is DefaultTabWidth tabWidth)
        {
          //_html.DefaultTabWidth = tabWidth.Value;
        }
        else if (token is Group group)
        {
          if (group.Destination?.Type != TokenType.HeaderTag)
          {
            body.Add(token);
          }
        }
        else if (token.Type != TokenType.HeaderTag)
        {
          body.Add(token);
        }
      }

      var result = new Element(ElementType.Document, new Element(ElementType.Section, new Element(ElementType.Paragraph)));
      var paragraph = result.Elements().First().Elements().First();
      var groups = new Stack<TokenState>();
      groups.Push(new TokenState(body));
      
      while (groups.Count > 0)
      {
        while (groups.Peek().Tokens.MoveNext())
        {
          var token = groups.Peek().Tokens.Current;
          if (token is Group childGroup)
          {
            if (childGroup.Contents.Count > 1
              && childGroup.Contents[0] is IgnoreUnrecognized
              && (childGroup.Contents[1].GetType().Name == "GenericTag" || childGroup.Contents[1].GetType().Name == "GenericWord"))
            {
              // do nothing
            }
            else
            {
              groups.Push(new TokenState(childGroup.Contents, groups.Peek()));
            }
          }
          else if (token is TextToken text)
          {
            paragraph.Add(new Run()
            {
              Value = text.Value
            });
          }
          else if (token is LineBreak)
          {
            paragraph.Add(new ControlCharacter(ControlCharacterType.LineBreak));
          }
          else if (token is Tab)
          {
            paragraph.Add(new ControlCharacter(ControlCharacterType.Tab));
          }
          else if (token is ParagraphBreak)
          {
            if (paragraph.Type == ElementType.Cell)
            {
              var nodes = paragraph.Nodes().ToArray();
              foreach (var node in nodes)
                node.Remove();
              var cellParagraph = new Element(ElementType.Paragraph, nodes);
              paragraph.Add(cellParagraph);
              paragraph = cellParagraph;
            }
            var nextParagraph = groups.Peek().NewParagraph();
            paragraph.Parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is CellBreak || token is NestedCellBreak)
          {
            while (paragraph.Type != ElementType.Cell)
              paragraph = paragraph.Parent;
            var nextParagraph = groups.Peek().NewParagraph();
            paragraph.Parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is ListId || token is ListTextFallback)
          {
            paragraph.Type = ElementType.ListItem;
          }
          else if (token is InTable)
          {
            paragraph.Type = ElementType.Cell;
          }
          else if (token is RowBreak || token is NestedRowBreak)
          {
            var currLevel = paragraph.Level;
            var parent = paragraph.Parent;
            var cells = parent.Elements().Reverse()
              .TakeWhile(e => e.Type == ElementType.Cell && e.Level == currLevel).Reverse()
              .ToArray();
            if (cells.Length > 0)
            {
              foreach (var cell in cells)
                cell.Remove();
              parent.Add(new Element(ElementType.Row, cells) { Level = cells[0].Level });
            }
            var nextParagraph = groups.Peek().NewParagraph();
            parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is NestingLevel nestingLevel && nestingLevel.Value > 0)
          {
            paragraph.Level = nestingLevel.Value;
            paragraph.Type = ElementType.Cell;
            groups.Peek().AddStyle(token);
          }
          else if (token is ListLevelNumber listLevelNumber)
          {
            paragraph.Type = ElementType.ListItem;
            paragraph.Level = listLevelNumber.Value;
            groups.Peek().AddStyle(token);
          }
          else if (token.Type == TokenType.CellFormat
            || token.Type == TokenType.CharacterFormat
            || token.Type == TokenType.ParagraphFormat
            || token.Type == TokenType.RowFormat
            || token.Type == TokenType.SectionFormat)
          {
            groups.Peek().AddStyle(token);
          }
        }
        groups.Pop();
      }

      OrganizeLists(result);
      return result;
    }

    private void OrganizeLists(Element root)
    {
      var parents = root.Descendants()
        .Where(e => e.Type != ElementType.List && e.Elements().Any(c => c.Type == ElementType.ListItem))
        .ToList();

      foreach (var parent in parents)
      {
        var newList = new List<Node>();
        foreach (var node in parent.Nodes().ToList())
        {
          node.Remove();
          if (node is Element element && element.Type == ElementType.ListItem)
          {
            if (!(newList.LastOrDefault() is Element list && list.Type == ElementType.List))
            {
              list = new Element(ElementType.List);
              newList.Add(list);
            }

            for (var i = 0; i < element.Level; i++)
            {
              var lastItem = list.Elements().LastOrDefault();
              if (lastItem != null)
              {
                var lastElement = lastItem.Nodes().LastOrDefault() as Element;
                if (lastElement?.Type != ElementType.List)
                {
                  lastElement = new Element(ElementType.List);
                  lastItem.Add(lastElement);
                }
                list = lastElement;
              }
            }
            list.Add(element);
          }
          else
          {
            newList.Add(node);
          }
        }

        foreach (var node in newList)
          parent.Add(node);
      }
    }
    
    private class TokenState
    {
      private readonly List<IToken> _styles = new List<IToken>();

      public IEnumerator<IToken> Tokens { get; }
      public IEnumerable<IToken> Styles => _styles;

      public TokenState(IEnumerable<IToken> tokens)
      {
        Tokens = tokens.GetEnumerator();
      }

      public TokenState(IEnumerable<IToken> tokens, TokenState previous) : this(tokens)
      {
        _styles.AddRange(previous.Styles);
      }

      public Element NewParagraph()
      {
        return new Element(ElementType.Paragraph)
        {
          Level = Styles.OfType<ListLevelNumber>().FirstOrDefault()?.Value
            ?? Styles.OfType<NestingLevel>().FirstOrDefault()?.Value
            ?? 0
        };
      }

      public void AddStyle(IToken token)
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

      protected virtual Func<IToken, bool> SameTokenPredicate(IToken token)
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
    }
  }
}
