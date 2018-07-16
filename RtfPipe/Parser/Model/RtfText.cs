using System;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

	public sealed class RtfText : RtfElement, IRtfText
	{

		public RtfText( string text ) :
			base( RtfElementKind.Text )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( "text" );
			}
			this.text = text;
		}

		public string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return text;
		}

		protected override void DoVisit( IRtfElementVisitor visitor )
		{
			visitor.VisitText( this );
		}

		protected override bool IsEqual( object obj )
		{
			RtfText compare = obj as RtfText; // guaranteed to be non-null
			return compare != null && base.IsEqual( obj ) &&
				text.Equals( compare.text );
		}

		protected override int ComputeHashCode()
		{
			return HashTool.AddHashCode( base.ComputeHashCode(), text );
		}

		private readonly string text;

	}

}

