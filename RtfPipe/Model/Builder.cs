using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfPipe.Model
{
  public class Builder
  {
    public Element Build(Document document)
    {
      var body = new List<IToken>();
      var defaultFont = default(Font);
      foreach (var token in document.Contents)
      {
        if (token is DefaultFontRef defaultFontRef)
        {
          defaultFont = document.FontTable.TryGetValue(defaultFontRef.Value, out var font) ? font : document.FontTable.FirstOrDefault().Value;
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
      var defaultStyles = new List<IToken>
      {
        new FontSize(UnitValue.FromHalfPoint(24))
      };
      if (document.ColorTable.Any())
        defaultStyles.Add(new ForegroundColor(document.ColorTable.First()));
      else
        defaultStyles.Add(new ForegroundColor(new ColorValue(0, 0, 0)));
      if (defaultFont != null)
        defaultStyles.Add(defaultFont);
      result.SetStyles(defaultStyles);
      result.Elements().First().SetStyles(defaultStyles);

      groups.Push(new TokenState(body, defaultStyles));

      
      while (groups.Count > 0)
      {
        while (groups.Peek().Tokens.MoveNext())
        {
          var token = groups.Peek().Tokens.Current;
          if (token is Group childGroup)
          {
            var dest = childGroup.Destination;
            if (childGroup.Contents.Count > 1
              && childGroup.Contents[0] is IgnoreUnrecognized
              && (childGroup.Contents[1].GetType().Name == "GenericTag" || childGroup.Contents[1].GetType().Name == "GenericWord"))
            {
              // Ignore groups with the "skip if unrecognized" flag
            }
            else if (dest is ListTextFallback)
            {
              paragraph.Type = ElementType.ListItem;
            }
            else if (dest is NumberingTextFallback
              || dest?.Type == TokenType.HeaderTag
              || dest is NoNestedTables)
            {
              // Ignore fallback content
            }
            else if (dest is Header
              || dest is HeaderEven
              || dest is HeaderFirst
              || dest is HeaderOdd
              || dest is Footer
              || dest is FooterFirst
              || dest is FooterOdd)
            {
              // skip for now
            }
            else
            {
              groups.Push(new TokenState(childGroup.Contents, groups.Peek()));
            }
          }
          else if (token is TextToken text)
          {
            paragraph.Add(new Run(text.Value, groups.Peek().Styles));
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
            paragraph.SetStyles(groups.Peek().NormalizedStyles(document));

            var nextParagraph = new Element(ElementType.Paragraph);
            paragraph.Parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is CellBreak || token is NestedCellBreak)
          {
            paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            var parent = paragraph.Parent;
            var cellContent = parent.Elements().Reverse()
              .TakeWhile(e => e.Type != ElementType.Cell 
                && e.Styles.OfType<InTable>().Any()
                && e.TableLevel == paragraph.TableLevel).Reverse()
              .ToArray();
            if (cellContent.Length == 1)
            {
              cellContent[0].Type = ElementType.Cell;
            }
            else
            {
              foreach (var content in cellContent)
                content.Remove();
              var cell = new Element(ElementType.Cell, cellContent);
              cell.SetStyles(groups.Peek().NormalizedStyles(document));
              parent.Add(cell);
            }
            
            var nextParagraph = new Element(ElementType.Paragraph);
            parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is ListId)
          {
            paragraph.Type = ElementType.ListItem;
          }
          else if (token is RowBreak || token is NestedRowBreak)
          {
            var parent = paragraph.Parent;
            if (paragraph.Type == ElementType.Cell)
            {
              paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            }
            else
            {
              paragraph.Remove();
              paragraph = parent.Elements().Last();
            }
            var currLevel = paragraph.TableLevel;
            var cells = parent.Elements().Reverse()
              .TakeWhile(e => e.Type == ElementType.Cell && e.TableLevel == currLevel).Reverse()
              .ToArray();
            if (cells.Length > 0)
            {
              foreach (var cell in cells)
                cell.Remove();
              var row = new Element(ElementType.Row, cells);
              row.SetStyles(new IToken[] { new NestingLevel(currLevel - 1), new InTable() });
              parent.Add(row);
            }
            var nextParagraph = new Element(ElementType.Paragraph);
            parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token.Type == TokenType.CellFormat
            || token.Type == TokenType.CharacterFormat
            || token.Type == TokenType.ParagraphFormat
            || token.Type == TokenType.RowFormat
            || token.Type == TokenType.SectionFormat)
          {
            if (token is ListLevelNumber listLevelNumber)
              paragraph.Type = ElementType.ListItem;
            groups.Peek().AddStyle(token);
          }
        }

        var state = groups.Pop();
        paragraph.SetStyles(state.NormalizedStyles(document));
      }

      if (!paragraph.Nodes().Any())
        paragraph.Remove();

      OrganizeTable(result);
      OrganizeLists(result);
      return result;
    }
    
    private void OrganizeTable(Element root)
    {
      var parents = root.Descendants()
        .Where(e => e.Type != ElementType.Table
          && e.Elements().Any(c => c.Type == ElementType.Row))
        .ToList();

      foreach (var parent in parents)
      {
        var nodeList = new List<Node>();
        foreach (var node in parent.Nodes().ToList())
        {
          node.Remove();
          if (node is Element element && element.Type == ElementType.Row)
          {
            if (!(nodeList.LastOrDefault() is Element table 
              && table.Type == ElementType.Table))
            {
              table = new Element(ElementType.Table);
              nodeList.Add(table);
            }
            
            table.Add(element);
          }
          else
          {
            nodeList.Add(node);
          }
        }

        foreach (var node in nodeList)
          parent.Add(node);
      }
    }

    private void OrganizeLists(Element root)
    {
      var parents = root.Descendants()
        .Where(e => e.Type != ElementType.List && e.Type != ElementType.OrderedList 
          && e.Elements().Any(c => c.Type == ElementType.ListItem))
        .ToList();

      foreach (var parent in parents)
      {
        var nodeList = new List<Node>();
        foreach (var node in parent.Nodes().ToList())
        {
          node.Remove();
          if (node is Element element && element.Type == ElementType.ListItem)
          {
            if (!(nodeList.LastOrDefault() is Element list 
              && (list.Type == ElementType.List || list.Type == ElementType.OrderedList)))
            {
              list = new Element(ElementType.List);
              nodeList.Add(list);
            }

            for (var i = 0; i < element.ListLevel; i++)
            {
              var lastItem = list.Elements().LastOrDefault();
              if (lastItem != null)
              {
                var lastElement = lastItem.Nodes().LastOrDefault() as Element;
                if (lastElement?.Type != ElementType.List
                  && lastElement?.Type != ElementType.OrderedList)
                {
                  lastElement = new Element(ElementType.List);
                  lastItem.Add(lastElement);
                }
                list = lastElement;
              }
            }

            if (!list.Elements().Any())
            {
              var numType = element.Styles.OfType<ListLevelType>().FirstOrDefault()?.Value
                ?? (element.Styles.OfType<NumberLevelBullet>().Any() ? (NumberingType?)NumberingType.Bullet : null)
                ?? element.Styles.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
                ?? NumberingType.Bullet;

              if (numType != NumberingType.Bullet && numType != NumberingType.NoNumber)
                list.Type = ElementType.OrderedList;
            }
            list.Add(element);
          }
          else
          {
            nodeList.Add(node);
          }
        }

        foreach (var node in nodeList)
          parent.Add(node);
      }
    }
    
    private class TokenState
    {
      private readonly StyleSet _styles = new StyleSet();
      private readonly List<IToken> _defaultStyles;

      public IEnumerator<IToken> Tokens { get; }
      public IEnumerable<IToken> Styles => _styles;

      public TokenState(IEnumerable<IToken> tokens, List<IToken> defaultStyles)
      {
        Tokens = tokens.GetEnumerator();
        _defaultStyles = defaultStyles;
        _styles.AddRange(defaultStyles);
      }

      public TokenState(IEnumerable<IToken> tokens, TokenState previous)
      {
        Tokens = tokens.GetEnumerator();
        _styles.AddRange(previous.Styles);
        _defaultStyles = previous._defaultStyles;
      }

      public IEnumerable<IToken> NormalizedStyles(Document document)
      {
        var result = Styles
          .Where(t => !HtmlVisitor.IsSpanElement(t))
          .ToList();
        var styleId = Styles.OfType<ListStyleId>().FirstOrDefault();
        if (styleId == null || !document.ListStyles.TryGetValue(styleId.Value, out var listStyle))
          return result;

        var levelNum = Styles.OfType<ListLevelNumber>().FirstOrDefault() ?? new ListLevelNumber(0);
        result.AddRange(listStyle.Style.Levels[levelNum.Value]);
        return result;
      }

      public void AddStyle(IToken token)
      {
        _styles.Add(token);
        if (token is PlainToken &&_defaultStyles?.Count > 0)
          _styles.AddRange(_defaultStyles);
      }
    }
  }
}
