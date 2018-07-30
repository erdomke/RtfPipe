using System;
using System.Collections.Generic;
using System.Text;

namespace RtfPipe
{
  public enum NumberingType
  {
    ///<summary>Decimal numbering (1, 2, 3).</summary>
    Numbers = 0,
    ///<summary>Cardinal numbering (One, Two, Three).</summary>
    CardinalText = 6,
    ///<summary>Uppercase alphabetical numbering (A, B, C).</summary>
    UpperLetter = 3,
    ///<summary>Uppercase Roman numbering (I, II, III).</summary>
    UpperRoman = 1,
    ///<summary>Lowercase alphabetical numbering (a, b, c).</summary>
    LowerLetter = 4,
    ///<summary>Lowercase Roman numbering (i, ii, iii).</summary>
    LowerRoman = 2,
    ///<summary>Ordinal numbering (1st, 2nd, 3rd).</summary>
    Ordinal = 5,
    ///<summary>Ordinal text numbering (First, Second, Third)</summary>
    OrdinalText = 7,
    ///<summary>Kanji numbering without the digit character (DBNUM1)</summary>
    Kanji = 10,
    ///<summary>Kanji numbering with the digit character (DBNUM2)</summary>
    KanjiDigit = 11,
    ///<summary>46 phonetic katakana characters in "aiueo" order (AIUEO) (newer form – "あいうえお。。。” based on phonem matrix)</summary>
    Katana1 = 12,
    ///<summary>46 phonetic katakana characters in "iroha" order (IROHA) (old form – “いろはにほへとちりぬるお。。。” based on haiku from long ago)</summary>
    Katana2 = 13,
    ///<summary>Double-byte character</summary>
    DoubleByte = 14,
    ///<summary>Single-byte character</summary>
    SingleByte = 15,
    ///<summary>Kanji numbering 3 (DBNUM3)</summary>
    Kanji3 = 16,
    ///<summary>Kanji numbering 4 (DBNUM4)</summary>
    Kanji4 = 17,
    ///<summary>Circle numbering (CIRCLENUM)</summary>
    Circle = 18,
    ///<summary>Double-byte Arabic numbering</summary>
    DoubleByteArabic = 19,
    ///<summary>46 phonetic double-byte katakana characters (AIUEO DBCHAR)</summary>
    DoubleByteKatana1 = 20,
    ///<summary>46 phonetic double-byte katakana characters (IROHA DBCHAR)</summary>
    DoubleByteKatana2 = 21,
    ///<summary>Arabic with leading zero (01, 02, 03, ..., 10, 11)</summary>
    LeadingZeroArabic = 22,
    ///<summary>Bullet (no number at all)</summary>
    Bullet = 23,
    ///<summary>Korean numbering 2 (GANADA)</summary>
    Korean2 = 24,
    ///<summary>Korean numbering 1 (CHOSUNG)</summary>
    Korean1 = 25,
    ///<summary>Chinese numbering 1 (GB1)</summary>
    Chinese1 = 26,
    ///<summary>Chinese numbering 2 (GB2)</summary>
    Chinese2 = 27,
    ///<summary>Chinese numbering 3 (GB3)</summary>
    Chinese3 = 28,
    ///<summary>Chinese numbering 4 (GB4)</summary>
    Chinese4 = 29,
    ///<summary>Chinese Zodiac numbering 1 (ZODIAC1)</summary>
    Zodiac1 = 30,
    ///<summary>Chinese Zodiac numbering 2 (ZODIAC2)</summary>
    Zodiac2 = 31,
    ///<summary>Chinese Zodiac numbering 3 (ZODIAC3)</summary>
    Zodiac3 = 32,
    ///<summary>Taiwanese double-byte numbering 1</summary>
    TaiwaneseDoubleByte1 = 33,
    ///<summary>Taiwanese double-byte numbering 2</summary>
    TaiwaneseDoubleByte2 = 34,
    ///<summary>Taiwanese double-byte numbering 3</summary>
    TaiwaneseDoubleByte3 = 35,
    ///<summary>Taiwanese double-byte numbering 4</summary>
    TaiwaneseDoubleByte4 = 36,
    ///<summary>Chinese double-byte numbering 1</summary>
    ChineseDoubleByte1 = 37,
    ///<summary>Chinese double-byte numbering 2</summary>
    ChineseDoubleByte2 = 38,
    ///<summary>Chinese double-byte numbering 3</summary>
    ChineseDoubleByte3 = 39,
    ///<summary>Chinese double-byte numbering 4</summary>
    ChineseDoubleByte4 = 40,
    ///<summary>Korean double-byte numbering 1</summary>
    KoreanDoubleByte1 = 41,
    ///<summary>Korean double-byte numbering 2</summary>
    KoreanDoubleByte2 = 42,
    ///<summary>Korean double-byte numbering 3</summary>
    KoreanDoubleByte3 = 43,
    ///<summary>Korean double-byte numbering 4</summary>
    KoreanDoubleByte4 = 44,
    ///<summary>Hebrew non-standard decimal</summary>
    Hebrew = 45,
    ///<summary>Arabic Alif Ba Tah</summary>
    ArabicAlif = 46,
    ///<summary>Hebrew Biblical standard</summary>
    HebrewBiblical = 47,
    ///<summary>Arabic Abjad style</summary>
    ArabicAbjad = 48,
    ///<summary>Hindi vowels</summary>
    HindiVowels = 49,
    ///<summary>Hindi consonants</summary>
    HindiConsonants = 50,
    ///<summary>Hindi numbers</summary>
    HindiNumbers = 51,
    ///<summary>Hindi descriptive (cardinals)</summary>
    HindiCardinals = 52,
    ///<summary>Thai letters</summary>
    ThaiLetters = 53,
    ///<summary>Thai numbers</summary>
    ThaiNumbers = 54,
    ///<summary>Thai descriptive (cardinals)</summary>
    ThaiCardinals = 55,
    ///<summary>Vietnamese descriptive (cardinals)</summary>
    VietnameseCardinals = 56,
    ///<summary>Page number format - # -</summary>
    PageNumber = 57,
    ///<summary>Lower case Russian alphabet</summary>
    LowerRussian = 58,
    ///<summary>Upper case Russian alphabet</summary>
    UpperRussian = 59,
    ///<summary>Lower case Greek numerals (alphabet based)</summary>
    LowerGreekNumerals = 60,
    ///<summary>Upper case Greek numerals (alphabet based)</summary>
    UpperGreekNumerals = 61,
    ///<summary>2 leading zeros: 001, 002, ..., 100, ...</summary>
    LeadingZeroArabic2 = 62,
    ///<summary>3 leading zeros: 0001, 0002, ..., 1000, ...</summary>
    LeadingZeroArabic3 = 63,
    ///<summary>4 leading zeros: 00001, 00002, ..., 10000, ...</summary>
    LeadingZeroArabic4 = 64,
    ///<summary>Lower case Turkish alphabet</summary>
    LowerTurkish = 65,
    ///<summary>Upper case Turkish alphabet</summary>
    UpperTurkish = 66,
    ///<summary>Lower case Bulgarian alphabet</summary>
    LowerBulgarian = 67,
    ///<summary>Upper case Bulgarian alphabet</summary>
    UpperBulgarian = 68,
    ///<summary>No number</summary>
    NoNumber = 255,
  }
}
