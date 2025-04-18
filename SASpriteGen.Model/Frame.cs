using ImageMagick;

namespace SASpriteGen.Model
{
	public class Frame
	{
		public readonly MagickImage SourceImage;

		public readonly uint FrameWidth;
		public readonly uint FrameHeight;

		public readonly int OffsetX;
		public readonly int OffsetY;
		public readonly double Scale;

		public Frame(MagickImage sourceImage, uint frameWidth, uint frameHeight, int offsetX, int offsetY, double scale)
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
