namespace RtfPipe.Support
{

	public class RtfVisualVisitorBase : IRtfVisualVisitor
	{

		public void VisitText( IRtfVisualText visualText )
		{
			if ( visualText != null )
			{
				DoVisitText( visualText );
			}
		}

		protected virtual void DoVisitText( IRtfVisualText visualText )
		{
		}

		public void VisitBreak( IRtfVisualBreak visualBreak )
		{
			if ( visualBreak != null )
			{
				DoVisitBreak( visualBreak );
			}
		}

		protected virtual void DoVisitBreak( IRtfVisualBreak visualBreak )
		{
		}

		public void VisitSpecial( IRtfVisualSpecialChar visualSpecialChar )
		{
			if ( visualSpecialChar != null )
			{
				DoVisitSpecial( visualSpecialChar );
			}
		}

		protected virtual void DoVisitSpecial( IRtfVisualSpecialChar visualSpecialChar )
		{
		}

		public void VisitImage( IRtfVisualImage visualImage )
		{
			if ( visualImage != null )
			{
				DoVisitImage( visualImage );
			}
		}

		protected virtual void DoVisitImage( IRtfVisualImage visualImage )
		{
		}

	}

}

