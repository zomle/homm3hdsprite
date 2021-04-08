using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace SASpriteGen.ViewModel
{
	public class AnimationPreviewViewModel : ViewModel
	{
		private int frameWidth;
		public int FrameWidth
		{
			get
			{
				return frameWidth;
			}
			set
			{
				if (frameWidth != value)
				{
					frameWidth = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int frameHeight;
		public int FrameHeight
		{
			get
			{
				return frameHeight;
			}
			set
			{
				if (frameHeight != value)
				{
					frameHeight = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int currentPreviewFrameIndex;
		public int CurrentPreviewFrameIndex
		{
			get
			{
				return currentPreviewFrameIndex;
			}
			set
			{
				if (value != currentPreviewFrameIndex)
				{
					currentPreviewFrameIndex = value;
					NotifyPropertyChanged();
				}
			}
		}

		public bool AnimationRunning { get; set; }

		public ObservableCollection<SpriteFrameData> Data { get; set; }

		public AnimationPreviewViewModel(ObservableCollection<SpriteFrameData> data) 
		{
			Data = data;

			AnimationRunning = false;
		}

		public void AnimationTick()
		{
			if (!AnimationRunning)
			{
				return;
			}

			if (Data.Count == 0)
			{
				return;
			}

			StepPreviewFrame(1);
		}

		public void StepPreviewFrame(int delta)
		{
			if (Data.Count == 0)
			{
				return;
			}
			else if (CurrentPreviewFrameIndex >= Data.Count)
			{
				CurrentPreviewFrameIndex = Data.Count - 1;
			}

			Data[CurrentPreviewFrameIndex].CurrentPreviewFrame = false;

			var val = CurrentPreviewFrameIndex + delta;

			if (val >= Data.Count)
			{
				CurrentPreviewFrameIndex = 0;
			}
			else if (val < 0)
			{
				CurrentPreviewFrameIndex = Data.Count - 1;
			}
			else
			{
				CurrentPreviewFrameIndex = val;
			}

			Data[CurrentPreviewFrameIndex].CurrentPreviewFrame = true;
		}
	}
}
