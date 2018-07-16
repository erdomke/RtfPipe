using System.Text;
using RtfPipe.Support;

namespace RtfPipe.Interpreter
{

	public sealed class RtfTextBuilder : RtfElementVisitorBase
	{

		public RtfTextBuilder() :
			base( RtfElementVisitorOrder.DepthFirst )
		{
			Reset();
		}

		public string CombinedText
		{
			get { return buffer.ToString(); }
		}

		public void Reset()
		{
			buffer.Remove( 0, buffer.Length );
		}

		protected override void DoVisitText( IRtfText text )
		{
			buffer.Append( text.Text );
		}

		private readonly StringBuilder buffer = new StringBuilder();

	}

}

