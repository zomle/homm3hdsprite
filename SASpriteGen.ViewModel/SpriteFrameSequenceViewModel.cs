using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

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

		public DefAnimation SequenceType { get; set; }

		public ObservableCollection<SpriteFrameData> Data { get; set; }

		public AnimationPreviewViewModel AnimationPreview { get; set; }

		public Command<FramedImageViewModel> ResetOffsets { get; set; }

		public Command StepAnimationBackward { get; set; }
		public Command StepAnimationForward { get; set; }
		public Command ToggleAnimation { get; set; }

		public int FrameMinLeft { get; internal set; }
		public int FrameMinTop { get; internal set; }

		public SpriteFrameSequenceViewModel(Action<IEnumerable, object> registerCollectionSynchronization) 
			: base(registerCollectionSynchronization)
		{
			Data = new ObservableCollection<SpriteFrameData>();

			RegisterCollectionSynchronization(Data);

			AnimationPreview = new AnimationPreviewViewModel(Data);

			ResetOffsets = new Command<FramedImageViewModel>(ResetOffsetsToDefault);

			ToggleAnimation = new Command(() => { AnimationPreview.AnimationRunning = !AnimationPreview.AnimationRunning; });
			StepAnimationForward = new Command(() => { AnimationPreview.AnimationRunning = false; AnimationPreview.StepPreviewFrame(1); });
			StepAnimationBackward = new Command(() => { AnimationPreview.AnimationRunning = false; AnimationPreview.StepPreviewFrame(-1); });

			AnimationPreview.AnimationRunning = false;
		}

		private void AddNewFrame(FramedImageViewModel framedImage)
		{
			
			Data.Add(new SpriteFrameData(Data.Count, framedImage));
		}

		private void ResetOffsetsToDefault(FramedImageViewModel data)
		{
			data.ResetOffsetsToDefault();
		}

		internal void Clear()
		{
			AnimationPreview.AnimationRunning = false;
			AnimationPreview.CurrentPreviewFrameIndex = 0;

			var dataArray = Data.ToArray();
			Data.Clear();

			foreach (var data in dataArray)
			{
				data.Dispose();
			}
		}

		internal void ChangeFrameSize(int dx, int dy)
		{
			foreach (var data in Data)
			{
				if (dx != 0)
				{
					data.FramedImage.FrameWidth += dx;
				}

				if (dy != 0)
				{
					data.FramedImage.FrameHeight += dy;
				}
			}
		}

		internal void AddNewFrame(DefGroupItem defItem, HdPakFrame frame, int targetFrameWidth, int targetFrameHeight, double scale)
		{
			var framedImage = new FramedImageViewModel(defItem, frame.Image, Data.Count == 0 ? null : Data[0].FramedImage, scale)
			{
				FrameWidth = targetFrameWidth,
				FrameHeight = targetFrameHeight
			};

			AddNewFrame(framedImage);
		}

		internal void ResetSizeAndScaling(int originalFrameWidth, int originalFrameHeight, double originalScale)
		{
			foreach (var data in Data)
			{
				data.FramedImage.FrameWidth = originalFrameWidth;
				data.FramedImage.FrameHeight = originalFrameHeight;
				data.FramedImage.Scale = originalScale;
			}
		}
	}
}
