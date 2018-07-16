namespace RtfPipe
{

	public interface IRtfVisualImage : IRtfVisual
	{

		RtfVisualImageFormat Format { get; }

		RtfTextAlignment Alignment { get; }

		int Width { get; }

		int Height { get; }

		int DesiredWidth { get; }

		int DesiredHeight { get; }

		int ScaleWidthPercent { get; }

		int ScaleHeightPercent { get; }

		string ImageDataHex { get; }

		byte[] ImageDataBinary { get; }

	}

}

