using System.Collections.Generic;

namespace SASpriteGen.Model
{
	public class AnimationData
	{
		public int MaxWidth;
		public int MaxHeight;

		public List<FrameData> Frames;

		public AnimationData()
		{
			Frames = new List<FrameData>();
		}
	}
}
