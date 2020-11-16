namespace RtfPipe
{
  internal interface IHtmlWriter
  {
    Font DefaultFont { get; set; }
    UnitValue DefaultTabWidth { get; set; }

    void AddBreak(FormatContext format, IToken token, int count = 1);
    void AddText(FormatContext format, string text);
    void AddPicture(FormatContext format, Picture_Orig picture);
    void Close();
  }
}
