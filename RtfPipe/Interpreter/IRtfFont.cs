namespace RtfPipe
{

	public interface IRtfFont
	{

		string Id { get; }

		RtfFontKind Kind { get; }

		RtfFontPitch Pitch { get; }

		int CharSet { get; }

		int CodePage { get; }

		string Name { get; }

	}

}

