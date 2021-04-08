using ImageMagick;

namespace SASpriteGen.Model
{
	public class Frame
	{
		public readonly MagickImage SourceImage;

		public readonly int FrameWidth;
		public readonly int FrameHeight;

		public readonly int OffsetX;
		public readonly int OffsetY;
		public readonly double Scale;

		public Frame(MagickImage sourceImage, int frameWidth, int frameHeight, int offsetX, int offsetY, double scale)
		{
			SourceImage = sourceImage;
			FrameWidth = frameWidth;
			FrameHeight = frameHeight;
			OffsetX = offsetX;
			OffsetY = offsetY;
			Scale = scale;
		}
	}
}
