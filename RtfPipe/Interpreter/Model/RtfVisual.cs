using System;
using RtfPipe.Sys;

namespace RtfPipe.Model
{

	public abstract class RtfVisual : IRtfVisual
	{

		protected RtfVisual( RtfVisualKind kind )
		{
			this.kind = kind;
		}

		public RtfVisualKind Kind
		{
			get { return kind; }
		}

		public void Visit( IRtfVisualVisitor visitor )
		{
			if ( visitor == null )
			{
				throw new ArgumentNullException( "visitor" );
			}
			DoVisit( visitor );
		}

		public sealed override bool Equals( object obj )
		{
			if ( obj == this )
			{
				return true;
			}
			
			if ( obj == null || GetType() != obj.GetType() )
			{
				return false;
			}

			return IsEqual( obj );
		}

		public sealed override int GetHashCode()
		{
			return HashTool.AddHashCode( GetType().GetHashCode(), ComputeHashCode() );
		}

		protected abstract void DoVisit( IRtfVisualVisitor visitor );

		protected virtual bool IsEqual( object obj )
		{
			return true;
		}

		protected virtual int ComputeHashCode()
		{
			return 0x0f00ba11;
		}

		private readonly RtfVisualKind kind;

	}

}

