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
        case "bullet":
          _textBuffer.Append('\u2022');
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
        case "chdate":
          _textBuffer.Append(Clock().ToString("d"));
          return null;
        case "chdpl":
          _textBuffer.Append(Clock().ToString("D"));
          return null;
        case "chdpa":
          var pattern = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern.Replace("dddd", "ddd").Replace("MMMM", "MMM");
          _textBuffer.Append(Clock().ToString(pattern));
          return null;
        case "chtime":
          _textBuffer.Append(Clock().ToString("t"));
          return null;

        // General
        case "*":
          return new IgnoreUnrecognized();
        case "generator":
          return new GeneratorTag();
        case "objattph":
          return new ObjectAttachment();
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

        // Page Setup
        case "header":
          return new Header();
        case "headerf":
          return new HeaderFirst();
        case "headerl":
          return new HeaderEven();
        case "headerr":
          return new HeaderOdd();
        case "footer":
          return new Footer();
        case "footerf":
          return new FooterFirst();
        case "footerl":
          return new FooterEven();
        case "footerr":
          return new FooterOdd();

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
        case "lin":
          return new LeftIndent(new UnitValue(number, UnitType.Twip));
        case "ri":
        case "rin":
          return new RightIndent(new UnitValue(number, UnitType.Twip));
        case "sa":
          return new SpaceAfter(new UnitValue(number, UnitType.Twip));
        case "sb":
          return new SpaceBefore(new UnitValue(number, UnitType.Twip));
        case "sl":
          return new SpaceBetweenLines(number);
        case "slmult":
          return new LineSpacingMultiple(number);
        case "outlinelevel":
          return new OutlineLevel(number);

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
        case "levelstartat":
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
        case "cbpat":
          return new ParaBackgroundColor(ColorByIndex(number));
        case "cb":
        case "chcbpat":
        case "highlight":
          return new BackgroundColor(number == 0 ? new ColorValue(255, 255, 255) : ColorByIndex(number));
        case "shading":
          var shade = (byte)(255 - Math.Min(Math.Max(0, number * 255 / 10000), 255));
          return new ParaBackgroundColor(new ColorValue(shade, shade, shade));
        case "cf":
          return new ForegroundColor(ColorByIndex(number));
        case "dn":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(number));
          return new OffsetToken(UnitValue.FromHalfPoint(6));
        case "embo":
          return new EmbossText(number != 0);
        case "i":
          return new ItalicToken(number != 0);
        case "impr":
          return new EngraveText(number != 0);
        case "nosupersub":
          return new NoSuperSubToken();
        case "outl":
          return new OutlineText(number != 0);
        case "plain":
          return new PlainToken();
        case "scaps":
          return new SmallCapitalToken(number != 0);
        case "shad":
          return new ShadowText(number != 0);
        case "strike":
          return new StrikeToken(number != 0);
        case "striked":
          return new StrikeDoubleToken(number != 0);
        case "sub":
          return new SubStartToken();
        case "super":
          return new SuperStartToken();
        case "ul":
          return new UnderlineToken(number != 0);
        case "ulc":
          return new UnderlineColor(ColorByIndex(number));
        case "uld":
          return new UnderlineDotted(number != 0);
        case "uldash":
          return new UnderlineDashed(number != 0);
        case "uldashd":
          return new UnderlineDashDot(number != 0);
        case "uldashdd":
          return new UnderlineDashDotDot(number != 0);
        case "uldb":
          return new UnderlineDouble(number != 0);
        case "ulhwave":
          return new UnderlineHeavyWave(number != 0);
        case "ulldash":
          return new UnderlineLongDash(number != 0);
        case "ulth":
          return new UnderlineThick(number != 0);
        case "ulthd":
          return new UnderlineThickDotted(number != 0);
        case "ulthdash":
          return new UnderlineThickDash(number != 0);
        case "ulthdashd":
          return new UnderlineThickDashDot(number != 0);
        case "ulthdashdd":
          return new UnderlineThickDashDotDot(number != 0);
        case "ulthldash":
          return new UnderlineThickLongDash(number != 0);
        case "ululdbwave":
          return new UnderlineDoubleWave(number != 0);
        case "ulw":
          return new UnderlineWord(number != 0);
        case "ulwave":
          return new UnderlineWave(number != 0);
        case "ulnone":
          return new UnderlineToken(false);
        case "up":
          if (number > 0)
            return new OffsetToken(UnitValue.FromHalfPoint(-1 * number));
          return new OffsetToken(UnitValue.FromHalfPoint(-6));
        case "v":
          return new HiddenToken(number != 0);

        // Pictures
        case "pict":
          return new PictureTag();
        case "shppict":
          return new ShapePictureTag();
        case "emfblip":
          return new EmfBlip();
        case "pngblip":
          return new PngBlip();
        case "jpegblip":
          return new JpegBlip();
        case "macpict":
          return new MacPict();
        case "pmmetafile":
          return new PmMetafile(number);
        case "wmetafile":
          return new WmMetafile(number);
        case "dibitmap":
          return new DiBitmap(number);
        case "wbitmap":
          return new WBitmap(number);
        case "picw":
          return new PictureWidth(new UnitValue(number, UnitType.Pixel));
        case "pich":
          return new PictureHeight(new UnitValue(number, UnitType.Pixel));
        case "picwgoal":
          return new PictureWidthGoal(new UnitValue(number, UnitType.Twip));
        case "pichgoal":
          return new PictureHeightGoal(new UnitValue(number, UnitType.Twip));
        case "bin":
          return new PictureBinaryLength(number);
        case "picscalex":
          return new PictureScaleX(number);
        case "picscaley":
          return new PictureScaleY(number);

        // Tables
        case "trowd":
          return new RowDefaults();
        case "row":
          return new RowBreak();
        case "tcelld":
          return new CellDefaults();
        case "cell":
          return new CellBreak();
        case "trgraph":
          return new CellSpacing(new UnitValue(number, UnitType.Twip));
        case "cellx":
          return new RightCellBoundary(new UnitValue(number, UnitType.Twip));
        case "trautofit":
          return new RowAutoFit(number == 1);
        case "trhdr":
          return new HeaderRow();
        case "trleft":
          return new RowLeft(new UnitValue(number, UnitType.Twip));
        case "trqc":
          return new RowAlign(TextAlignment.Center);
        case "trql":
          return new RowAlign(TextAlignment.Left);
        case "trqr":
          return new RowAlign(TextAlignment.Right);
        case "trbrdrt":
          return new TableBorderSide(BorderPosition.Top);
        case "trbrdrr":
          return new TableBorderSide(BorderPosition.Right);
        case "trbrdrb":
          return new TableBorderSide(BorderPosition.Bottom);
        case "trbrdrl":
          return new TableBorderSide(BorderPosition.Left);
        case "trpaddt":
          return new TablePaddingTop(new UnitValue(number, UnitType.Twip));
        case "trpaddr":
          return new TablePaddingRight(new UnitValue(number, UnitType.Twip));
        case "trpaddb":
          return new TablePaddingBottom(new UnitValue(number, UnitType.Twip));
        case "trpaddl":
          return new TablePaddingLeft(new UnitValue(number, UnitType.Twip));
        case "clbrdrt":
          return new CellBorderSide(BorderPosition.Top);
        case "clbrdrr":
          return new CellBorderSide(BorderPosition.Right);
        case "clbrdrb":
          return new CellBorderSide(BorderPosition.Bottom);
        case "clbrdrl":
          return new CellBorderSide(BorderPosition.Left);
        case "clftsWidth":
          return new CellWidthType((CellWidthUnit)number);
        case "clwWidth":
          return new CellWidth(number);
        case "intbl":
          return new InTable();
        case "clvertalt":
          return new CellVerticalAlign(VerticalAlignment.Top);
        case "clvertalc":
          return new CellVerticalAlign(VerticalAlignment.Center);
        case "clvertalb":
          return new CellVerticalAlign(VerticalAlignment.Bottom);
        case "clcbpat":
          return new CellBackgroundColor(ColorByIndex(number));
        case "nesttableprops":
          return new NestTableProperties();
        case "nonesttables":
          return new NoNestedTables();
        case "nestcell":
          return new NestCell();
        case "nestrow":
          return new NestRow();
        case "itap":
          return new NestingLevel(number >= 0 ? number : 1);

        // Borders and Shading
        case "brdrs": return new BorderStyleTag(BorderStyle.SingleThick);
        case "brdrth": return new BorderStyleTag(BorderStyle.DoubleThick);
        case "brdrsh": return new BorderShadow();
        case "brdrdb": return new BorderStyleTag(BorderStyle.Double);
        case "brdrdot": return new BorderStyleTag(BorderStyle.Dotted);
        case "brdrdash": return new BorderStyleTag(BorderStyle.Dashed);
        case "brdrhair": return new BorderStyleTag(BorderStyle.Hairline);
        case "brdrdashsm": return new BorderStyleTag(BorderStyle.DashedSmall);
        case "brdrdashdot":
        case "brdrdashd":
          return new BorderStyleTag(BorderStyle.DotDashed);
        case "brdrdashdotdot":
        case "brdrdashdd":
          return new BorderStyleTag(BorderStyle.DotDotDashed);
        case "brdrinset": return new BorderStyleTag(BorderStyle.Inset);
        case "brdrnone": return new BorderStyleTag(BorderStyle.None);
        case "brdroutset": return new BorderStyleTag(BorderStyle.Outset);
        case "brdrtriple": return new BorderStyleTag(BorderStyle.Triple);
        case "brdrtnthsg": return new BorderStyleTag(BorderStyle.ThickThinSmall);
        case "brdrthtnsg": return new BorderStyleTag(BorderStyle.ThinThickSmall);
        case "brdrtnthtnsg": return new BorderStyleTag(BorderStyle.ThinThickThinSmall);
        case "brdrtnthmg": return new BorderStyleTag(BorderStyle.ThickThinMedium);
        case "brdrthtnmg": return new BorderStyleTag(BorderStyle.ThinThickMedium);
        case "brdrtnthtnmg": return new BorderStyleTag(BorderStyle.ThinThickThinMedium);
        case "brdrtnthlg": return new BorderStyleTag(BorderStyle.ThickThinLarge);
        case "brdrthtnlg": return new BorderStyleTag(BorderStyle.ThinThickLarge);
        case "brdrtnthtnlg": return new BorderStyleTag(BorderStyle.ThinThickThinLarge);
        case "brdrwavy": return new BorderStyleTag(BorderStyle.Wavy);
        case "brdrwavydb": return new BorderStyleTag(BorderStyle.DoubleWavy);
        case "brdrdashdotstr": return new BorderStyleTag(BorderStyle.Striped);
        case "brdremboss": return new BorderStyleTag(BorderStyle.Embossed);
        case "brdrengrave": return new BorderStyleTag(BorderStyle.Engraved);
        case "brdrframe": return new BorderStyleTag(BorderStyle.Frame);
        case "brdrw":
          return new BorderWidth(new UnitValue(number, UnitType.Twip));
        case "brdrcf":
          return new BorderColor(ColorByIndex(number));
        case "brdrt":
          return new TableBorderSide(BorderPosition.Top);
        case "brdrr":
          return new TableBorderSide(BorderPosition.Right);
        case "brdrb":
          return new TableBorderSide(BorderPosition.Bottom);
        case "brdrl":
          return new TableBorderSide(BorderPosition.Left);
        case "brsp":
          return new BorderSpacing(new UnitValue(number, UnitType.Twip));

        // Fields and bookmarks
        case "field":
          return new Field();
        case "fldinst":
          return new FieldInstructions();
        case "fldrslt":
          return new FieldResult();
        case "bkmkstart":
          return new BookmarkStart();
        case "bkmkend":
          return new BookmarkEnd();

        // Tabs
        case "tab":
          return new Tab();
        case "deftab":
          return new DefaultTabWidth(new UnitValue(number, UnitType.Twip));
        case "tx":
          return new TabPosition(new UnitValue(number, UnitType.Twip));
        case "tqr":
          return new TabAlignment(TextAlignment.Right);
        case "tqc":
          return new TabAlignment(TextAlignment.Center);
        case "page":
        case "pagebb":
          return new PageBreak();

        // Other
        case "footnote":
          return new Footnote();

        default:
          if (number == int.MinValue)
            return new GenericTag(name);
          return new GenericWord(name, number);
      }
    }

    private ColorValue ColorByIndex(int number)
    {
      if (number >= 0 && number < _document.ColorTable.Count)
        return _document.ColorTable[number];
      return new ColorValue(0, 0, 0);
    }
  }
}
