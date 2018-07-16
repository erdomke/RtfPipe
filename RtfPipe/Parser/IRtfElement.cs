namespace RtfPipe
{

	public interface IRtfElement
	{

		RtfElementKind Kind { get; }

		void Visit( IRtfElementVisitor visitor );

	}

}

