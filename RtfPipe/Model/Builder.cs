using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RtfPipe.Model
{
  internal class Builder
  {
    public RtfHtml Build(Document document)
    {
      var attachmentIndex = 0;
      var result = new RtfHtml();
      var body = new List<IToken>();
      var defaultStyles = new List<IToken>
      {
        new FontSize(UnitValue.FromHalfPoint(24))
      };
      var stylesBeforeReset = default(List<IToken>);

      if (document.ColorTable.Any())
        defaultStyles.Add(new ForegroundColor(document.ColorTable.First()));
      else
        defaultStyles.Add(new ForegroundColor(new ColorValue(0, 0, 0)));

      foreach (var token in document.Contents)
      {
        if (token is DefaultFontRef defaultFontRef)
        {
          var defaultFont = document.FontTable.TryGetValue(defaultFontRef.Value, out var font) ? font : document.FontTable.FirstOrDefault().Value;
          if (defaultFont != null)
            defaultStyles.Add(defaultFont);
        }
        else if (token is DefaultTabWidth tabWidth)
        {
          result.DefaultTabWidth = tabWidth.Value;
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

      result.Root = new Element(ElementType.Document, new Element(ElementType.Section, new Element(ElementType.Paragraph)));
      var paragraph = result.Root.Elements().First().Elements().First();

      var groups = new Stack<TokenState>();
      result.Root.SetStyles(defaultStyles);
      result.Root.Elements().First().SetStyles(defaultStyles);

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
              || dest is NoNestedTables
              || dest is BookmarkEnd)
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
            else if (dest is FieldInstructions)
            {
              var instructions = childGroup.Contents
                .OfType<Group>().LastOrDefault(g => g.Destination == null && g.Contents.OfType<TextToken>().Any())
                ?.Contents.OfType<TextToken>().FirstOrDefault()?.Value?.Trim();
              if (string.IsNullOrEmpty(instructions)
                && !childGroup.Contents.OfType<Group>().Any()
                && childGroup.Contents.Count == 3)
              {
                instructions = (childGroup.Contents[2] as TextToken)?.Value;
              }

              if (!string.IsNullOrEmpty(instructions))
              {
                var args = instructions.Split(' ');
                if (args[0] == "HYPERLINK")
                  groups.Peek().AddStyle(new HyperlinkToken(args));
              }
            }
            else if (dest is BookmarkStart)
            {
              groups.Peek().AddStyle(new BookmarkToken()
              {
                Start = true,
                Id = childGroup.Contents.OfType<TextToken>().FirstOrDefault()?.Value
              });
            }
            else if (dest is PictureTag)
            {
              paragraph.Add(new Picture(childGroup));
            }
            else
            {
              groups.Push(new TokenState(childGroup.Contents, groups.Peek()));
            }
          }
          else if (token is TextToken text)
          {
            var newRun = new Run(text.Value, groups.Peek().Styles);
            if (paragraph.Nodes().LastOrDefault() is Run run
              && run.Styles.CollectionEquals(newRun.Styles))
            {
              run.Value += text.Value;
            }
            else
            {
              paragraph.Add(newRun);
            }
          }
          else if (token is LineBreak)
          {
            if (paragraph.Nodes().LastOrDefault() is Run run
              && run.Styles.OfType<FontSize>().FirstOrDefault()?.Value
                == groups.Peek().Styles.OfType<FontSize>().FirstOrDefault()?.Value)
            {
              run.Value += "\n";
            }
            else
            {
              paragraph.Add(new Run("\n", groups.Peek().Styles));
            }
          }
          else if (token is Tab)
          {
            if (paragraph.Nodes().LastOrDefault() is Run run
              && run.Styles.OfType<FontSize>().FirstOrDefault()?.Value
                == groups.Peek().Styles.OfType<FontSize>().FirstOrDefault()?.Value)
            {
              run.Value += "\t";
            }
            else
            {
              paragraph.Add(new Run("\t", groups.Peek().Styles));
            }
          }
          else if (token is PageBreak || token is SectionBreak)
          {
            paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            stylesBeforeReset = null;

            paragraph = new Element(ElementType.Paragraph);
            var section = new Element(ElementType.Section, paragraph);
            section.SetStyles(groups.Peek().NormalizedStyles(document)
              .Where(t => t.Type != TokenType.ParagraphFormat && t.Type != TokenType.RowFormat && t.Type != TokenType.CellFormat));
            result.Root.Add(section);
          }
          else if (token is ParagraphBreak)
          {
            if (!paragraph.Styles.Any())
              paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            stylesBeforeReset = null;
            
            var nextParagraph = new Element(ElementType.Paragraph);
            paragraph.Parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is CellBreak || token is NestedCellBreak)
          {
            paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            stylesBeforeReset = null;

            var parent = paragraph.Parent;
            var cellContent = parent.Elements().Reverse()
              .TakeWhile(e => e.Type != ElementType.TableCell 
                && e.Styles.OfType<InTable>().Any()
                && e.TableLevel == paragraph.TableLevel).Reverse()
              .ToArray();
            if (cellContent.Length == 1)
            {
              cellContent[0].Type = ElementType.TableCell;
            }
            else
            {
              foreach (var content in cellContent)
                content.Remove();
              var cell = new Element(ElementType.TableCell, cellContent);
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
            if (paragraph.Type == ElementType.TableCell)
            {
              paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
              stylesBeforeReset = null;
            }
            else
            {
              paragraph.Remove();
              paragraph = parent.Elements().Last();
            }
            var currLevel = paragraph.TableLevel;
            var cells = parent.Elements().Reverse()
              .TakeWhile(e => e.Type == ElementType.TableCell && e.TableLevel == currLevel).Reverse()
              .ToArray();
            if (cells.Length > 0)
            {
              foreach (var cell in cells)
                cell.Remove();
              var row = new Element(ElementType.TableRow, cells);
              var rowStyles = new StyleList(groups.Peek().Styles);
              rowStyles.Merge(new NestingLevel(Math.Max(currLevel - 1, 0)));
              var cellFormats = rowStyles.OfType<CellToken>().ToList();
              if (cellFormats.Count >= cells.Length)
              {
                for (var i = 0; i < cells.Length; i++)
                  cells[i].SetStyles(cells[i].Styles.Where(t => !(t is CellToken)).Concat(new[] { cellFormats[i] }));
              }
              rowStyles.RemoveWhere(t => t is HalfCellPadding);
              row.SetStyles(rowStyles);
              parent.Add(row);
            }
            var nextParagraph = new Element(ElementType.Paragraph);
            parent.Add(nextParagraph);
            paragraph = nextParagraph;
          }
          else if (token is ObjectAttachment)
          {
            paragraph.Add(new Attachment(attachmentIndex));
            attachmentIndex++;
          }
          else if (token is OutlineLevel outlineLevel && outlineLevel.Value >= 0 && outlineLevel.Value < 6)
          {
            switch (outlineLevel.Value + 1)
            {
              case 2:
                paragraph.Type = ElementType.Header2;
                break;
              case 3:
                paragraph.Type = ElementType.Header3;
                break;
              case 4:
                paragraph.Type = ElementType.Header4;
                break;
              case 5:
                paragraph.Type = ElementType.Header5;
                break;
              case 6:
                paragraph.Type = ElementType.Header6;
                break;
              default:
                paragraph.Type = ElementType.Header1;
                break;
            }
          }
          else if (token.Type == TokenType.CellFormat
            || token.Type == TokenType.CharacterFormat
            || token.Type == TokenType.ParagraphFormat
            || token.Type == TokenType.RowFormat
            || token.Type == TokenType.SectionFormat)
          {
            if ((token is ParagraphDefault || token is SectionDefault) && paragraph.Nodes().Any())
              paragraph.SetStyles(groups.Peek().NormalizedStyles(document));
            //stylesBeforeReset = groups.Peek().Styles.ToList();
            else if (token is ListLevelNumber listLevelNumber)
              paragraph.Type = ElementType.ListItem;
            groups.Peek().AddStyle(token);
          }
        }

        var state = groups.Pop();
        if (!paragraph.Styles.Any() && paragraph.Nodes().Any())
          paragraph.SetStyles(stylesBeforeReset ?? state.NormalizedStyles(document));
      }

      if (!paragraph.Nodes().Any())
        paragraph.Remove();

      OrganizeTable(result.Root);
      OrganizeLists(result.Root);
      return result;
    }
    
    private void OrganizeTable(Element root)
    {
      var parents = root.Descendants()
        .Where(e => e.Type != ElementType.Table
          && e.Elements().Any(c => c.Type == ElementType.TableRow))
        .ToList();
      
      foreach (var parent in parents)
      {
        var nodeList = new List<Node>();
        foreach (var node in parent.Nodes().ToList())
        {
          node.Remove();
          if (node is Element row && row.Type == ElementType.TableRow)
          {
            if (!(nodeList.LastOrDefault() is Element table 
              && table.Type == ElementType.Table))
            {
              table = new Element(ElementType.Table);
              nodeList.Add(table);
            }
            
            table.Add(row);
            if (!table.Styles.Any())
              table.SetStyles(row.Styles.Where(t => t.Type != TokenType.CellFormat 
                && t.Type != TokenType.RowFormat));
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
          if (node is Element listItem && listItem.Type == ElementType.ListItem)
          {
            if (!(nodeList.LastOrDefault() is Element list 
              && (list.Type == ElementType.List || list.Type == ElementType.OrderedList)))
            {
              list = new Element(ElementType.List);
              nodeList.Add(list);
            }

            for (var i = 0; i < listItem.ListLevel; i++)
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
              var numType = listItem.Styles.OfType<ListLevelType>().FirstOrDefault()?.Value
                ?? (listItem.Styles.OfType<NumberLevelBullet>().Any() ? (NumberingType?)NumberingType.Bullet : null)
                ?? listItem.Styles.OfType<NumberingTypeToken>().FirstOrDefault()?.Value
                ?? NumberingType.Bullet;

              if (numType != NumberingType.Bullet && numType != NumberingType.NoNumber)
                list.Type = ElementType.OrderedList;
            }

            list.Add(listItem);
            if (!list.Styles.Any())
              list.SetStyles(listItem.Styles);
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
      private readonly StyleList _styles = new StyleList();
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
        var result = new StyleList(Styles
          .Where(t => !HtmlVisitor.IsSpanElement(t)));
        var styleId = Styles.OfType<ListStyleId>().FirstOrDefault();
        if (styleId == null || !document.ListStyles.TryGetValue(styleId.Value, out var listStyle))
          return result;

        var levelNum = Styles.OfType<ListLevelNumber>().FirstOrDefault() ?? new ListLevelNumber(0);
        result.MergeRange(listStyle.Style.Levels[levelNum.Value]
          .Where(t =>
          {
            // This is a bit of a hack, but not sure how MS Word is interpreting this
            if (t is FirstLineIndent firstLine)
              return firstLine.Value > new UnitValue(-1, UnitType.Inch);
            return t.Type == TokenType.ParagraphFormat;
          }));
        return result;
      }

      public void AddStyle(IToken token)
      {
        _styles.Merge(token);
        if (token is PlainToken && _defaultStyles?.Count > 0)
          _styles.MergeRange(_defaultStyles);
      }
    }
  }
}
