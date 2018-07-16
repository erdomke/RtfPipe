namespace RtfPipe
{

	public interface IRtfDocumentProperty
	{

		int PropertyKindCode { get; }

		RtfPropertyKind PropertyKind { get; }

		string Name { get; }

		string StaticValue { get; }

		string LinkValue { get; }

	}

}

