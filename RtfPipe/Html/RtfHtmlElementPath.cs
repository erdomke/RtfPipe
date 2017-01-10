// -- FILE ------------------------------------------------------------------
// name       : RtfHtmlElementPath.cs
// project    : RTF Framelet
// created    : Jani Giannoudis - 2008.06.09
// language   : c#
// environment: .NET 2.0
// copyright  : (c) 2004-2013 by Jani Giannoudis, Switzerland
// --------------------------------------------------------------------------
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

	// ------------------------------------------------------------------------
	public class RtfHtmlElementPath
	{

		// ----------------------------------------------------------------------
		public int Count
		{
			get { return elements.Count; }
		} // Count

		// ----------------------------------------------------------------------
		public string Current
		{
			get { return elements.Peek(); }
		} // Current

		// ----------------------------------------------------------------------
		public bool IsCurrent( string tag )
		{
			return Current == tag;
		} // IsCurrent

		// ----------------------------------------------------------------------
		public bool Contains( string tag )
		{
			return elements.Contains( tag );
		} // Contains

		// ----------------------------------------------------------------------
		public void Push( string tag )
		{
			elements.Push( tag );
		} // Push

		// ----------------------------------------------------------------------
		public void Pop()
		{
			elements.Pop();
		} // Pop

		// ----------------------------------------------------------------------
		public override string ToString()
		{
			if ( elements.Count == 0 )
			{
				return base.ToString();
			}

			StringBuilder sb = new StringBuilder();
			bool first = true;
			foreach ( var element in elements )
			{
				if ( !first )
				{
					sb.Insert( 0, " > " );
				}
				sb.Insert( 0, element.ToString() );
				first = false;
			}

			return sb.ToString();
		} // ToString

		// ----------------------------------------------------------------------
		// members
		private readonly Stack<string> elements = new Stack<string>();

	} // class RtfHtmlElementPath

} // namespace RtfPipe.Converter.Html
// -- EOF -------------------------------------------------------------------
