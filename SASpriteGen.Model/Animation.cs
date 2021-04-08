using System.Collections.Generic;

namespace SASpriteGen.Model
{
	public class Animation
	{
		public string Name { get; }
		public string Source { get; }

		public int TotalHeight
		{
			get
			{
				return Frames.Count > 0 ? Frames[0].FrameHeight : 0;
			}
		}

		public int TotalWidth
		{
			get
			{
				return Frames.Count * Frames[0].FrameWidth;
			}
		}

		public readonly List<Frame> Frames;

		public Animation(string name, string source)
		{
			Name = name;
			Source = source;

			Frames = new List<Frame>();
		}
	}
}
