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

		public int FrameWidth
		{
			get
			{
				return AnimationPreview.FrameWidth;
			}
		}

		public int FrameHeight
		{
			get
			{
				return AnimationPreview.FrameHeight;
			}
		}


		public int FrameCount
		{
			get
			{
				return AnimationPreview.Data.Count;
			}
		}

		public AnimationPreviewViewModel AnimationPreview { get; set; }
	}
}
