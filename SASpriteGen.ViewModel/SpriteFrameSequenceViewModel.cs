using ImageMagick;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace SASpriteGen.ViewModel
{
	
	public class SpriteFrameSequenceViewModel : SynchedViewModel
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

		//private double scalex;
		//public double ScaleX
		//{
		//	get
		//	{
		//		return scalex;
		//	}
		//	set
		//	{
		//		if (scalex != value)
		//		{
		//			scalex = Math.Round(value, 2);
		//			NotifyPropertyChanged();
		//		}
		//	}
		//}

		//private double scaley;
		//public double ScaleY
		//{
		//	get
		//	{
		//		return scaley;
		//	}
		//	set
		//	{
		//		if (scaley != value)
		//		{
		//			scaley = Math.Round(value, 2);
		//			NotifyPropertyChanged();
		//		}
		//	}
		//}

		public ObservableCollection<SpriteFrameData> Data { get; set; }

		public Command<SpriteFrameData> IncreaseXOffset { get; set; }
		public Command<SpriteFrameData> DecreaseXOffset { get; set; }
		public Command<SpriteFrameData> IncreaseYOffset { get; set; }
		public Command<SpriteFrameData> DecreaseYOffset { get; set; }

		public Command<SpriteFrameData> ResetOffsets { get; set; }

		public Command StepAnimationBackward { get; set; }
		public Command StepAnimationForward { get; set; }
		public Command ToggleAnimation { get; set; }

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

		public SpriteFrameSequenceViewModel(Action<IEnumerable, object> registerCollectionSynchronization) 
			: base(registerCollectionSynchronization)
		{
			Data = new ObservableCollection<SpriteFrameData>();

			RegisterCollectionSynchronization(Data);

			IncreaseXOffset = new Command<SpriteFrameData>((data) => { ChangeOffset(data, +1, 0); });
			DecreaseXOffset = new Command<SpriteFrameData>((data) => { ChangeOffset(data, -1, 0); });
			IncreaseYOffset = new Command<SpriteFrameData>((data) => { ChangeOffset(data, 0, +1); });
			DecreaseYOffset = new Command<SpriteFrameData>((data) => { ChangeOffset(data, 0, -1); });

			ResetOffsets = new Command<SpriteFrameData>(ResetOffsetsToDefault);

			ToggleAnimation = new Command(() => { AnimationRunning = !AnimationRunning; });
			StepAnimationForward = new Command(() => { AnimationRunning = false; StepPreviewFrame(1); });
			StepAnimationBackward = new Command(() => { AnimationRunning = false; StepPreviewFrame(-1); });

			AnimationRunning = false;
		}


		public void AddNewFrame(MagickImage image, double offsetX, double offsetY, double highResScaleX, double highResScaleY, double scaleX, double scaleY)
		{
			Data.Add(new SpriteFrameData(Data.Count, image, offsetX, offsetY, highResScaleX, highResScaleY, scaleX, scaleY));
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

		private void ResetOffsetsToDefault(SpriteFrameData data)
		{
			data.OffsetX = data.OriginalOffsetX;
			data.OffsetY = data.OriginalOffsetY;
		}

		private void ChangeOffset(SpriteFrameData data, int dx, int dy)
		{
			data.OffsetX += dx;
			data.OffsetY += dy;
		}

		internal void Clear()
		{
			AnimationRunning = false;
			CurrentPreviewFrameIndex = 0;

			Data.Clear();
		}
	}
}
