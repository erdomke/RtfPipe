using System.Collections;
using RtfPipe.Model;
using System.Collections.Generic;

namespace RtfPipe.Parser
{

	public sealed class RtfParserListenerStructureBuilder : RtfParserListenerBase
	{

		public IRtfGroup StructureRoot
		{
			get { return structureRoot; }
		}

		protected override void DoParseBegin()
		{
			openGroupStack.Clear();
			curGroup = null;
			structureRoot = null;
		}

		protected override void DoGroupBegin()
		{
			RtfGroup newGroup = new RtfGroup();
			if ( curGroup != null )
			{
				openGroupStack.Push( curGroup );
				curGroup.Contents.Add( newGroup );
			}
			curGroup = newGroup;
		}

		protected override void DoTagFound( IRtfTag tag )
		{
			if ( curGroup == null )
			{
				throw new RtfStructureException( Strings.MissingGroupForNewTag );
			}
			curGroup.Contents.Add( tag );
		}

		protected override void DoTextFound( IRtfText text )
		{
			if ( curGroup == null )
			{
				throw new RtfStructureException( Strings.MissingGroupForNewText );
			}
			curGroup.Contents.Add( text );
		}

		protected override void DoGroupEnd()
		{
			if ( openGroupStack.Count > 0 )
			{
				curGroup = openGroupStack.Pop();
			}
			else
			{
				if ( structureRoot != null )
				{
					throw new RtfStructureException( Strings.MultipleRootLevelGroups );
				}
				structureRoot = curGroup;
				curGroup = null;
			}
		}

		protected override void DoParseEnd()
		{
			if ( openGroupStack.Count > 0 )
			{
				throw new RtfBraceNestingException( Strings.UnclosedGroups );
			}
		}

		private readonly Stack<RtfGroup> openGroupStack = new Stack<RtfGroup>();
		private RtfGroup curGroup;
		private RtfGroup structureRoot;

	}

}

