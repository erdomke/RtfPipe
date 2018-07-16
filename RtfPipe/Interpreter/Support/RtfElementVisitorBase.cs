namespace RtfPipe.Support
{

	public class RtfElementVisitorBase : IRtfElementVisitor
	{

		public RtfElementVisitorBase( RtfElementVisitorOrder order )
		{
			this.order = order;
		}

		public void VisitTag( IRtfTag tag )
		{
			if ( tag != null )
			{
				DoVisitTag( tag );
			}
		}

		protected virtual void DoVisitTag( IRtfTag tag )
		{
		}

		public void VisitGroup( IRtfGroup group )
		{
			if ( group != null )
			{
				if ( order == RtfElementVisitorOrder.DepthFirst )
				{
					VisitGroupChildren( group );
				}
				DoVisitGroup( group );
				if ( order == RtfElementVisitorOrder.BreadthFirst )
				{
					VisitGroupChildren( group );
				}
			}
		}

		protected virtual void DoVisitGroup( IRtfGroup group )
		{
		}

		protected void VisitGroupChildren( IRtfGroup group )
		{
			foreach ( IRtfElement child in group.Contents )
			{
				child.Visit( this );
			}
		}

		public void VisitText( IRtfText text )
		{
			if ( text != null )
			{
				DoVisitText( text );
			}
		}

		protected virtual void DoVisitText( IRtfText text )
		{
		}

		private readonly RtfElementVisitorOrder order;

	}

}

