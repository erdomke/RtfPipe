using System;

namespace RtfPipe.Converter.Html
{

	[Flags]
	public enum RtfHtmlConvertScope
	{
		None = 0x00000000,

		Document = 0x00000001,
		Html = 0x00000010,
		Head = 0x00000100,
		Body = 0x00001000,
		Content = 0x00010000,

		All = Document | Html | Head | Body | Content,
	}

}

