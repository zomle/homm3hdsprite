using System.Collections.Generic;

namespace SASpriteGen.Model
{
	public class SpriteSheet
	{
		public string SourceId { get; }

		public uint TotalHeight
		{
			get
			{
				uint totalHeight = 0;
				foreach (var animation in Animations)
				{
					totalHeight += animation.TotalHeight;
				}
				return totalHeight;
			}
		}

		public uint TotalWidth
		{
			get
			{
				uint maxWidth = 0;
				foreach (var animation in Animations)
				{
					maxWidth = animation.TotalWidth > maxWidth ? animation.TotalWidth : maxWidth;
				}
				return maxWidth;
			}
		}

		public readonly List<Animation> Animations;

		public SpriteSheet(string sourceId)
		{
			Animations = new List<Animation>();
			SourceId = sourceId;
		}
	}
}
