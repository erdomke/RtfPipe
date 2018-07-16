using System;
using System.Text;
using RtfPipe.Sys;
using System.Collections.Generic;

namespace RtfPipe.Model
{

	public sealed class RtfGroup : RtfElement, IRtfGroup
	{

		public RtfGroup() :
			base( RtfElementKind.Group )
		{
		}

		public IList<IRtfElement> Contents
		{
			get { return contents; }
		}

		public string Destination
		{
			get
			{
				if ( contents.Count > 0 )
				{
					IRtfElement firstChild = contents[ 0 ];
					if ( firstChild.Kind == RtfElementKind.Tag )
					{
						IRtfTag firstTag = (IRtfTag)firstChild;
						if ( RtfSpec.TagExtensionDestination.Equals( firstTag.Name ) )
						{
							if ( contents.Count > 1 )
							{
								IRtfElement secondChild = contents[ 1 ];
								if ( secondChild.Kind == RtfElementKind.Tag )
								{
									IRtfTag secondTag = (IRtfTag)secondChild;
									return secondTag.Name;
								}
							}
						}
						return firstTag.Name;
					}
				}
				return null;
			}
		}

		public bool IsExtensionDestination
		{
			get
			{
				if ( contents.Count > 0 )
				{
					IRtfElement firstChild = contents[ 0 ];
					if ( firstChild.Kind == RtfElementKind.Tag )
					{
						IRtfTag firstTag = (IRtfTag)firstChild;
						if ( RtfSpec.TagExtensionDestination.Equals( firstTag.Name ) )
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public IRtfGroup SelectChildGroupWithDestination( string destination )
		{
			if ( destination == null )
			{
				throw new ArgumentNullException( "destination" );
			}
			foreach ( IRtfElement child in contents )
			{
				if ( child.Kind == RtfElementKind.Group )
				{
					IRtfGroup group = (IRtfGroup)child;
					if ( destination.Equals( group.Destination ) )
					{
						return group;
					}
				}
			}
			return null;
		}

		public override string ToString()
		{
			StringBuilder buf = new StringBuilder( "{" );
			int count = contents.Count;
			buf.Append( count );
			buf.Append( " items" );
			if ( count > 0 )
			{
				// visualize the first two child elements for convenience during debugging
				buf.Append( ": [" );
				buf.Append( contents[ 0 ] );
				if ( count > 1 )
				{
					buf.Append( ", " );
					buf.Append( contents[ 1 ] );
					if ( count > 2 )
					{
						buf.Append( ", " );
						if ( count > 3 )
						{
							buf.Append( "..., " );
						}
						buf.Append( contents[ count - 1 ] );
					}
				}
				buf.Append( "]" );
			}
			buf.Append( "}" );
			return buf.ToString();
		}

		protected override void DoVisit( IRtfElementVisitor visitor )
		{
			visitor.VisitGroup( this );
		}

		protected override bool IsEqual( object obj )
		{
			RtfGroup compare = obj as RtfGroup; // guaranteed to be non-null
			return compare != null && base.IsEqual( obj ) &&
				contents.Equals( compare.contents );
		}

		protected override int ComputeHashCode()
		{
			return HashTool.AddHashCode( base.ComputeHashCode(), contents );
		}

		private readonly List<IRtfElement> contents = new List<IRtfElement>();

	}

}

