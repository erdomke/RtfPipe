namespace RtfPipe
{

	public interface IRtfTag : IRtfElement
	{

		/// <summary>
		/// Returns the name together with the concatenated value as it stands in the rtf.
		/// </summary>
		string FullName { get; }

		string Name { get; }

		bool HasValue { get; }

		string ValueAsText { get; }

		int ValueAsNumber { get; }

	}

}

