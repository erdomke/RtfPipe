namespace RtfPipe
{

	public interface IRtfVisual
	{

		RtfVisualKind Kind { get; }

		void Visit( IRtfVisualVisitor visitor );

	}

}

