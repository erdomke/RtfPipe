using System.Text;

namespace RtfPipe
{
  internal interface IValueBuffer
  {
    Encoding Encoding { get; set; }
    int Length { get; }

    IValueBuffer Append(byte value);
    IValueBuffer Append(char ch);
    IValueBuffer Append(int ch);
    void Clear();
  }
}
