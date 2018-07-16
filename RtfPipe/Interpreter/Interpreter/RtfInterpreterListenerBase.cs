namespace RtfPipe.Interpreter
{

	public class RtfInterpreterListenerBase : IRtfInterpreterListener
	{

		public void BeginDocument( IRtfInterpreterContext context )
		{
			if ( context != null )
			{
				DoBeginDocument( context );
			}
		}

		public void InsertText( IRtfInterpreterContext context, string text )
		{
			if ( context != null )
			{
				DoInsertText( context, text );
			}
		}

		public void InsertSpecialChar( IRtfInterpreterContext context, RtfVisualSpecialCharKind kind )
		{
			if ( context != null )
			{
				DoInsertSpecialChar( context, kind );
			}
		}

		public void InsertBreak( IRtfInterpreterContext context, RtfVisualBreakKind kind )
		{
			if ( context != null )
			{
				DoInsertBreak( context, kind );
			}
		}

		public void InsertImage( IRtfInterpreterContext context, RtfVisualImageFormat format,
			int width, int height, int desiredWidth, int desiredHeight,
			int scaleWidthPercent, int scaleHeightPercent, string imageDataHex
		)
		{
			if ( context != null )
			{
				DoInsertImage( context, format,
					width, height, desiredWidth, desiredHeight,
					scaleWidthPercent, scaleHeightPercent, imageDataHex );
			}
		}

		public void EndDocument( IRtfInterpreterContext context )
		{
			if ( context != null )
			{
				DoEndDocument( context );
			}
		}

		protected virtual void DoBeginDocument( IRtfInterpreterContext context )
		{
		}

		protected virtual void DoInsertText( IRtfInterpreterContext context, string text )
		{
		}

		protected virtual void DoInsertSpecialChar( IRtfInterpreterContext context, RtfVisualSpecialCharKind kind )
		{
		}

		protected virtual void DoInsertBreak( IRtfInterpreterContext context, RtfVisualBreakKind kind )
		{
		}

		protected virtual void DoInsertImage( IRtfInterpreterContext context,
			RtfVisualImageFormat format,
			int width, int height, int desiredWidth, int desiredHeight,
			int scaleWidthPercent, int scaleHeightPercent,
			string imageDataHex
		)
		{
		}

		protected virtual void DoEndDocument( IRtfInterpreterContext context )
		{
		}

	}

}

