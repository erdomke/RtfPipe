using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace RtfPipe.Converter.Html
{

	public class RtfHtmlElementPath
	{

		public int Count
		{
			get { return elements.Count; }
		}

		public string Current
		{
			get { return elements.Peek(); }
		}

		public bool IsCurrent( string tag )
		{
			return Current == tag;
		}

		public bool Contains( string tag )
		{
			return elements.Contains( tag );
		}

		public void Push( string tag )
		{
			elements.Push( tag );
		}

		public void Pop()
		{
			elements.Pop();
		}

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
		}

		private readonly Stack<string> elements = new Stack<string>();

	}

}

