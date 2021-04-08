using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace SASpriteGen.ViewModel
{
	public class AnimationPreviewViewModel : ViewModel
	{
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

		public int FrameWidth
		{
			get
			{
				return Data.Count > 0 ? Data[0].FramedImage.FrameWidth : 0;
			}
		}

		public int FrameHeight
		{
			get
			{
				return Data.Count > 0 ? Data[0].FramedImage.FrameHeight : 0;
			}
		}

		public ObservableCollection<SpriteFrameData> Data { get; set; }

		public AnimationPreviewViewModel(ObservableCollection<SpriteFrameData> data) 
		{
			Data = data;

			AnimationRunning = false;
		}

		public AnimationPreviewViewModel Clone()
		{
			return new AnimationPreviewViewModel(new ObservableCollection<SpriteFrameData>(Data))
			{
				CurrentPreviewFrameIndex = 0,
				AnimationRunning = true
			};
		}

		private DateTime NextTick;

		public void AnimationTick()
		{
			if (DateTime.Now < NextTick)
			{
				return;
			}

			if (!AnimationRunning)
			{
				return;
			}

			if (Data.Count == 0)
			{
				return;
			}

			StepPreviewFrame(1);
			NextTick = DateTime.Now.AddMilliseconds(100);
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
