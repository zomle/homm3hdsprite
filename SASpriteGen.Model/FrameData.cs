using ImageMagick;

namespace SASpriteGen.Model
{
	public class FrameData
	{
		public int OldWidth;
		public int OldHeight;
		public int OldFrameWidth;
		public int OldFrameHeight;
		public int OldFrameLeft;
		public int OldFrameTop;

		public int AdjustedXOffset;
		public int AdjustedYOffset;

		public MagickImage NewImage;
	}
}
