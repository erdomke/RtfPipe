namespace RtfPipe
{

	public interface IRtfInterpreterListener
	{

		void BeginDocument( IRtfInterpreterContext context );

		void InsertText( IRtfInterpreterContext context, string text );

		void InsertSpecialChar( IRtfInterpreterContext context, RtfVisualSpecialCharKind kind );

		void InsertBreak( IRtfInterpreterContext context, RtfVisualBreakKind kind );

		void InsertImage( IRtfInterpreterContext context, RtfVisualImageFormat format,
			int width, int height, int desiredWidth, int desiredHeight,
			int scaleWidthPercent, int scaleHeightPercent, string imageDataHex
		);

		void EndDocument( IRtfInterpreterContext context );

	}

}

