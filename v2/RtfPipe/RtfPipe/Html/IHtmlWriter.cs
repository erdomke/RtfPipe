namespace RtfPipe
{
  internal interface IHtmlWriter
  {
    Font DefaultFont { get; set; }

    void AddBreak(FormatContext format, IToken token);
    void AddText(FormatContext format, string text);
    void Close();
  }
}
