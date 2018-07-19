using System.Text;

namespace RtfPipe
{
  internal interface IRtfReader
  {
    Encoding Encoding { get; set; }

    int Peek();
    int Read();
  }
}
