using System;

namespace SASpriteGen.ViewModel
{
	public class StreamAvatarsFrameSequence : ViewModel
	{
		private string sequenceName;
		public string SequenceName
		{
			get
			{
				return sequenceName;
			}
			set
			{
				if (value != sequenceName)
				{
					sequenceName = value;
					NotifyPropertyChanged();
				}
			}
		}

		public uint FrameWidth
		{
			get
			{
				return AnimationPreview.FrameWidth;
			}
		}

		public uint FrameHeight
		{
			get
			{
				return AnimationPreview.FrameHeight;
			}
		}


		public uint FrameCount
		{
			get
			{
				return (uint)AnimationPreview.Data.Count;
			}
		}

		public AnimationPreviewViewModel AnimationPreview { get; set; }
	}
}
