using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public partial class Parser
  {

    private IToken GetControlWord(string name, int number = int.MinValue)
    {
      switch (name)
      {
        // Characters
        case "emdash":
          _textBuffer.Append('\u2014');
          return null;
        case "endash":
          _textBuffer.Append('\u2013');
          return null;
        case "emspace":
          _textBuffer.Append('\u2003');
          return null;
        case "enspace":
          _textBuffer.Append('\u2002');
          return null;
        case "lquote":
          _textBuffer.Append('\u2018');
          return null;
        case "rquote":
          _textBuffer.Append('\u2019');
          return null;
        case "ldblquote":
          _textBuffer.Append('\u201C');
          return null;
        case "rdblquote":
          _textBuffer.Append('\u201D');
          return null;
        case "-":
          _textBuffer.Append('\u00AD'); // soft hyphen
          return null;
        case "~":
          _textBuffer.Append('\u00A0'); // non-breaking space
          return null;
        case "_":
          _textBuffer.Append('\u2011'); // non-breaking hyphen
          return null;
        case "u":
          if (number < 0 && number != int.MinValue)
            number += 65536;
          _textBuffer.Append(number);
          return null;
        case "upr":
          _ignoreDepth = Depth + 1;
          return null;
        case "ud":
          return new UnicodeTextTag();

        // General
        case "*":
          return new IgnoreUnrecognized();
        case "fromhtml":
          return new FromHtml(number != 0);
        case "htmltag":
          return new HtmlTag((HtmlEncapsulation)number);
        case "htmlrtf":
          return new HtmlRtf(number != 0);

        // Info
        case "info":
          return new Info();
        case "title":
          return new Title();
        case "subject":
          return new Subject();
        case "author":
          return new Author();
        case "manager":
          return new Manager();
        case "company":
          return new Company();
        case "operator":
          return new Operator();
        case "category":
          return new Category();
        case "keywords":
          return new Keywords();
        case "comment":
          return new Comment();
        case "doccomm":
          return new DocComment();
        case "hlinkbase":
          return new HyperlinkBase();
        case "creatim":
          return new CreateTime();
        case "revtim":
          return new RevisionTime();
        case "printim":
          return new PrintTime();
        case "buptim":
          return new BackupTime();
        case "yr":
          return new Year(number);
        case "mo":
          return new Month(number);
        case "dy":
          return new Day(number);
        case "hr":
          return new Hour(number);
        case "min":
          return new Minute(number);
        case "sec":
          return new Second(number);
        case "version":
          return new Tokens.Version(number);
        case "vern":
          return new InternalVersion(number);
        case "edmins":
          return new EditingTime(TimeSpan.FromMinutes(number));
        case "nofpages":
          return new NumPages(number);
        case "nofwords":
          return new NumWords(number);
        case "nofchars":
          return new NumChars(number);
        case "nofcharsws":
          return new NumCharsWs(number);


        // Font Tags
        case "f":
          if (_document.FontTable.TryGetValue(number, out var font))
            return font;
          return new FontRef(number);
        case "fnil":
          return new FontCategory(FontFamilyCategory.Nil);
        case "froman":
          return new FontCategory(FontFamilyCategory.Roman);
        case "fswiss":
          return new FontCategory(FontFamilyCategory.Swiss);
        case "fmodern":
          return new FontCategory(FontFamilyCategory.Modern);
        case "fscript":
          return new FontCategory(FontFamilyCategory.Script);
        case "fdecor":
          return new FontCategory(FontFamilyCategory.Decor);
        case "ftech":
          return new FontCategory(FontFamilyCategory.Tech);
        case "fbidi":
          return new FontCategory(FontFamilyCategory.Bidi);
        case "fcharset":
          return new FontCharSet(TextEncoding.EncodingFromCharSet(number));
        case "cpg":
          return new CodePage(TextEncoding.EncodingFromCodePage(number));
        case "ansicpg":
          return new DefaultCodePage(TextEncoding.EncodingFromCodePage(number));
        case "ansi":
          return new DefaultNamedCodePage(NamedCodePage.Ansi);
        case "mac":
          return new DefaultNamedCodePage(NamedCodePage.Mac);
        case "pc":
          return new DefaultNamedCodePage(NamedCodePage.Pc);
        case "pca":
          return new DefaultNamedCodePage(NamedCodePage.Pca);
        case "fprq":
          return new FontPitchToken((FontPitch)number);
        case "fs":
          return new FontSize(UnitValue.FromHalfPoint(number));
        case "deff":
          return new DefaultFontRef(number);
        case "uc":
          if (_context.Count > 0)
            _context.Peek().AsciiFallbackChars = number;
          return new GenericWord(name, number);
        case "fonttbl":
          return new FontTableTag();

        // Style Sheet
        case "stylesheet":
          return new StyleSheetTag();
        case "s":
          return new StyleRef(number);
        case "generator":
          return new GeneratorTag();

        // Color
        case "colortbl":
          return new ColorTable();
        case "red":
          return new Red((byte)number);
        case "green":
          return new Green((byte)number);
        case "blue":
          return new Blue((byte)number);

        // Paragraph tags
        case "par":
          return new ParagraphBreak();
        case "pard":
          return new ParagraphDefault();
        case "line":
          return new LineBreak();
        case "qc":
          return new TextAlign(TextAlignment.Center);
        case "qj":
          return new TextAlign(TextAlignment.Justify);
        case "ql":
          return new TextAlign(TextAlignment.Left);
        case "qr":
          return new TextAlign(TextAlignment.Right);
        case "fi":
          return new FirstLineIndent(new UnitValue(number, UnitType.Twip));
        case "li":
          return new LeftIndent(new UnitValue(number, UnitType.Twip));
        case "sa":
          return new SpaceAfter(new UnitValue(number, UnitType.Twip));
        case "sl":
          return new SpaceBetweenLines(number);
        case "slmult":
          return new LineSpacingMultiple(number);

        // Bullets & Numbering
        case "pntext":
          return new NumberingTextFallback();
        case "pn":
          return new ParagraphNumbering();
        case "pnlvlblt":
          return new NumberLevelBullet();
        case "pnlvlbody":
          return new NumberLevelBody();
        case "pnindent":
          return new NumberingIndent(number);
        case "pntxtb":
          return new BulletText();
        case "pnstart":
          return new NumberingStart(number);
        case "pncard":
          return new NumberingTypeToken(NumberingType.CardinalText);
        case "pndec":
          return new NumberingTypeToken(NumberingType.Numbers);
        case "pnlcltr":
          return new NumberingTypeToken(NumberingType.LowerLetter);
        case "pnlcrm":
          return new NumberingTypeToken(NumberingType.LowerRoman);
        case "pnord":
          return new NumberingTypeToken(NumberingType.Ordinal);
        case "pnordt":
          return new NumberingTypeToken(NumberingType.OrdinalText);
        case "pnucltr":
          return new NumberingTypeToken(NumberingType.UpperLetter);
        case "pnucrm":
          return new NumberingTypeToken(NumberingType.UpperRoman);
        case "pntxta":
          return new NumberingTextAfter();

        case "listlevel":
          return new ListLevel();
        case "list":
          return new ListDefinition();
        case "listtemplateid":
          return new ListTemplateId(number);
        case "listid":
          return new ListId(number);
        case "levelnfc":
        case "levelnfcn":
          return new ListLevelType((NumberingType)number);
        case "leveljc":
        case "leveljcn":
          return new ListLevelJustification((TextAlignment)number);
        case "leveltext":
          return new LevelText();
        case "levelnumbers":
          return new LevelNumbers();
        case "listoverridetable":
          return new ListOverrideTable();
        case "listoverride":
          return new ListOverride();
        case "ls":
          return new ListStyleId(number);
        case "ilvl":
          return new ListLevelNumber(number);
        case "listtext":
          return new ListTextFallback();

        // Section tags
        case "sect":
          return new SectionBreak();
        case "sectd":
          return new SectionDefault();

        // Character Tags
        case "b":
          return new BoldToken(number != 0);
        case "caps":
          return new CapitalToken(number != 0);
        case "cb":
        case "chcbpat":
        case "highlight":
          return new BackgroundColor(_document.ColorTable[number]);
        case "cf":
          return new ForegroundColor(_document.ColorTable[number]);
        case "dn":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(-1 * number));
          return new OffsetToken(UnitValue.FromHalfPoint(-6));
        case "i":
          return new ItalicToken(number != 0);
        case "nosupersub":
          return new NoSuperSubToken();
        case "plain":
          return new PlainToken();
        case "up":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(number));
          return new OffsetToken(UnitValue.FromHalfPoint(6));
        case "strike":
          return new StrikeToken(number != 0);
        case "sub":
          return new SubStartToken();
        case "super":
          return new SuperStartToken();
        case "ul":
          return new UnderlineToken(number != 0);
        case "ulnone":
          return new UnderlineToken(false);
        case "v":
          return new HiddenToken(number != 0);

        default:
          if (number == int.MinValue)
            return new GenericTag(name);
          return new GenericWord(name, number);
      }
    }
  }
}
