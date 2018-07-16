namespace RtfPipe.Parser
{

	public class RtfParserListenerBase : IRtfParserListener
	{

		public int Level
		{
			get { return level; }
		}

		public void ParseBegin()
		{
			level = 0; // in case something interrupted the normal flow of things previously ...
			DoParseBegin();
		}

		protected virtual void DoParseBegin()
		{
		}

		public void GroupBegin()
		{
			DoGroupBegin();
			level++;
		}

		protected virtual void DoGroupBegin()
		{
		}

		public void TagFound( IRtfTag tag )
		{
			if ( tag != null )
			{
				DoTagFound( tag );
			}
		}

		protected virtual void DoTagFound( IRtfTag tag )
		{
		}

		public void TextFound( IRtfText text )
		{
			if ( text != null )
			{
				DoTextFound( text );
			}
		}

		protected virtual void DoTextFound( IRtfText text )
		{
		}

		public void GroupEnd()
		{
			level--;
			DoGroupEnd();
		}

		protected virtual void DoGroupEnd()
		{
		}

		public void ParseSuccess()
		{
			DoParseSuccess();
		}

		protected virtual void DoParseSuccess()
		{
		}

		public void ParseFail( RtfException reason )
		{
			DoParseFail( reason );
		}

		protected virtual void DoParseFail( RtfException reason )
		{
		}

		public void ParseEnd()
		{
			DoParseEnd();
		}

		protected virtual void DoParseEnd()
		{
		}

		private int level;

	}

}

