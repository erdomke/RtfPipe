using RtfPipe.Sys;

namespace RtfPipe.Model
{

	public sealed class RtfVisualBreak : RtfVisual, IRtfVisualBreak
	{

		public RtfVisualBreak( RtfVisualBreakKind breakKind ) :
			base( RtfVisualKind.Break )
		{
			this.breakKind = breakKind;
		}

		public RtfVisualBreakKind BreakKind
		{
			get { return breakKind; }
		}

		public override string ToString()
		{
			return breakKind.ToString();
		}

		protected override void DoVisit( IRtfVisualVisitor visitor )
		{
			visitor.VisitBreak( this );
		}

		protected override bool IsEqual( object obj )
		{
			RtfVisualBreak compare = obj as RtfVisualBreak; // guaranteed to be non-null
			return 
				compare != null &&
				base.IsEqual( compare ) &&
				breakKind == compare.breakKind;
		}

		protected override int ComputeHashCode()
		{
			return HashTool.AddHashCode( base.ComputeHashCode(), breakKind );
		}

		private readonly RtfVisualBreakKind breakKind;

	}

}

