namespace RtfPipe
{

	public interface IRtfElementVisitor
	{

		void VisitTag( IRtfTag tag );

		void VisitGroup( IRtfGroup group );

		void VisitText( IRtfText text );

	}

}

