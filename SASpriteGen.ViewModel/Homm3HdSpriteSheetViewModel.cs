using ImageMagick;
using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace SASpriteGen.ViewModel
{
	public class Homm3HdSpriteSheetViewModel : SynchedViewModel
	{
		public int OriginalFrameWidth { get; set; }
		public int OriginalFrameHeight { get; set; }
		public double OriginalScale { get; set; }

		public int FrameWidth
		{
			get { return Sequences.Count == 0 || Sequences[0].Data.Count == 0 ? 0 : Sequences[0].Data[0].FramedImage.FrameWidth; }
			set { throw new InvalidOperationException(); }
		}

		public int FrameHeight
		{
			get { return Sequences.Count == 0 || Sequences[0].Data.Count == 0 ? 0 : Sequences[0].Data[0].FramedImage.FrameHeight; }
			set { throw new InvalidOperationException(); }
		}

		public double Scaling
		{
			get { return (Sequences.Count == 0 || Sequences[0].Data.Count == 0) ? 0 : Sequences[0].Data[0].FramedImage.Scale; }
			set { throw new InvalidOperationException(); }
		}

		private int frameCount;
		public int FrameCount
		{
			get { return frameCount; }
			set
			{
				if (frameCount != value)
				{
					frameCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int framesProcessed;
		public int FramesProcessed
		{
			get { return framesProcessed; }
			set
			{
				if (framesProcessed != value)
				{
					framesProcessed = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool frameLoadInProgress;
		public bool FrameLoadInProgress
		{
			get
			{
				return frameLoadInProgress;
			}
			set
			{
				if (frameLoadInProgress != value)
				{
					frameLoadInProgress = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool sequencesLoaded;
		public bool SequencesLoaded
		{
			get
			{
				return sequencesLoaded;
			}
			set
			{
				if (sequencesLoaded != value)
				{
					sequencesLoaded = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Command<string> ChangeFrameWidth { get; set; }
		public Command<string> ChangeFrameHeight { get; set; }
		public Command<string> ChangeScaling { get; set; }

		public Command ResetSizing { get; set; }
		public Command RecalculateFrameSize { get; set; }

		public Command Unload { get; set; }

		public ObservableCollection<SpriteFrameSequenceViewModel> Sequences { get; set; }

		public Homm3HdSpriteSheetViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			Sequences = new ObservableCollection<SpriteFrameSequenceViewModel>();

			RegisterCollectionSynchronization(Sequences);

			ChangeFrameWidth = new Command<string>((arg) =>
			{
				var dw = Convert.ToInt32(arg);
				Sequences.ForAll(s => s.ChangeFrameSize(dw, 0));
				NotifyPropertyChanged(nameof(FrameWidth));
				FrameSizesUpdated?.Invoke(this, new FrameSizesUpdatedEventArgs());
			});

			ChangeFrameHeight = new Command<string>((arg) =>
			{
				var dh = Convert.ToInt32(arg);
				Sequences.ForAll(s => s.ChangeFrameSize(0, dh));
				NotifyPropertyChanged(nameof(FrameHeight));
				FrameSizesUpdated?.Invoke(this, new FrameSizesUpdatedEventArgs());
			});

			ChangeScaling = new Command<string>((arg) =>
			{
				var ds = Convert.ToDouble(arg);
				Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.Scale += ds));
				NotifyPropertyChanged(nameof(Scaling));
			});

			ResetSizing = new Command(() =>
			{
				Sequences.ForAll(s => s.ResetSizeAndScaling(OriginalFrameWidth, OriginalFrameHeight, OriginalScale));

				NotifyPropertyChanged(nameof(FrameWidth));
				NotifyPropertyChanged(nameof(FrameHeight));
				NotifyPropertyChanged(nameof(Scaling));

				FrameSizesUpdated?.Invoke(this, new FrameSizesUpdatedEventArgs());
			});

			RecalculateFrameSize = new Command(() =>
			{
				int leftest = int.MaxValue;
				int rightest = int.MinValue;

				int topest = int.MaxValue;
				int bottomest = int.MinValue;
				foreach (var sequence in Sequences)
				{
					foreach (var data in sequence.Data)
					{
						var left = (int)(data.FramedImage.OffsetX - data.FramedImage.Width/2 * data.FramedImage.Scale);
						var right = (int)(data.FramedImage.OffsetX + data.FramedImage.Width/2 * data.FramedImage.Scale);

						var bottom = (int)(data.FramedImage.OffsetY + data.FramedImage.Height * data.FramedImage.Scale);
						var top = (int)(data.FramedImage.OffsetY);

						leftest = leftest < left ? leftest : left;
						rightest = rightest > right ? rightest : right;

						bottomest = bottomest > bottom ? bottomest : bottom;
						topest = topest < top ? topest : top;
					}
				}

				var maxWidth = rightest - leftest;
				var maxHeight = bottomest - topest;

				Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.FrameWidth = maxWidth));
				Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.FrameHeight = maxHeight));

				NotifyPropertyChanged(nameof(FrameWidth));
				NotifyPropertyChanged(nameof(FrameHeight));

				FrameSizesUpdated?.Invoke(this, new FrameSizesUpdatedEventArgs());
			});

			Initialize();

			Unload = new Command(() =>
			{
				SequencesLoaded = false;
				Clear();
			});
		}

		private void Initialize()
		{
			OriginalFrameHeight = 200;
			OriginalFrameWidth = 200;
			OriginalScale = 1.0;

			Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.FrameWidth = OriginalFrameWidth));
			Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.FrameHeight = OriginalFrameHeight));

			Sequences.ForAll(s => s.Data.ForAll(d => d.FramedImage.Scale = OriginalScale));
		}

		public void RefreshFrameSizes()
		{
			NotifyPropertyChanged(nameof(FrameWidth));
			NotifyPropertyChanged(nameof(FrameHeight));
		}

		public delegate void SequenceCollectionUpdatedEventHandler(object sender, SequenceCollectionUpdatedEventArgs e);
		public event SequenceCollectionUpdatedEventHandler SequenceCollectionUpdated;

		public delegate void FrameSizesUpdatedEventHandler(object sender, FrameSizesUpdatedEventArgs e);
		public event FrameSizesUpdatedEventHandler FrameSizesUpdated;

		private HdAssetCatalogItem LoadedCatalogItem { get; set; }

		public void Load(DefFile defFile, HdAssetCatalogItem catalogItem)
		{
			Clear();

			new Thread(() => LoadSpriteSheetThreadFunc(defFile, catalogItem)).Start();
		}

		private void LoadSpriteSheetThreadFunc(DefFile defFile, HdAssetCatalogItem catalogItem)
		{
			double minScale = double.MaxValue;
			FrameLoadInProgress = true;
			SequencesLoaded = false;
			FramesProcessed = 0;

			FrameCount = catalogItem.HdPakFrames.Count;

			catalogItem.LoadFrames(() => FramesProcessed++);

			foreach (var group in defFile.Groups)
			{
				foreach (var item in group.Value.Items)
				{
					var frame = catalogItem.GetFrameForFileName(item.FileName);
					if (frame == null) 
					{
						continue;
					}

					var image = frame.Image;
					var scaleX = (double)OriginalFrameWidth / image.Width;
					var scaleY = (double)OriginalFrameHeight / image.Height;
					var tmpScale = Math.Min(scaleX, scaleY);

					minScale = Math.Min(minScale, tmpScale);
				}
			}
			minScale = Math.Min(1.0, minScale);

			OriginalScale = minScale;

			foreach (var group in defFile.Groups)
			{
				var ix = AddSequence(group.Value.GetGroupName(defFile.Type), group.Value.GroupNum);
				var sequence = Sequences[ix];

				sequence.FrameMinLeft = group.Value.Items.Min(v => v.FrameLeft);
				sequence.FrameMinTop = group.Value.Items.Min(v => v.FrameTop);

				for (int i = 0; i < group.Value.Items.Count; i++)
				{
					var item = group.Value.Items[i];

					var frame = catalogItem.GetFrameForFileName(item.FileName);
					if (frame == null)
					{
						continue;
					}

					sequence.AddNewFrame(item, frame, OriginalFrameWidth, OriginalFrameHeight, OriginalScale);
				}

				if (sequence.Data.Count == 0)
				{
					Sequences.Remove(sequence);
				}
			}

			Sequences.ForAll(s =>
			{
				s.AnimationPreview.AnimationRunning = true;
				Thread.Sleep(1);
			});

			NotifyPropertyChanged(nameof(FrameWidth));
			NotifyPropertyChanged(nameof(FrameHeight));
			NotifyPropertyChanged(nameof(Scaling));

			SequenceCollectionUpdated?.Invoke(this, new SequenceCollectionUpdatedEventArgs(Sequences));

			LoadedCatalogItem = catalogItem;

			FrameLoadInProgress = false;
			SequencesLoaded = true;
		}

		public int AddSequence(string sequenceName, DefAnimation sequenceType)
		{
			Sequences.Add(new SpriteFrameSequenceViewModel(RegisterCollectionSynchronizationCallback)
			{
				SequenceName = sequenceName,
				SequenceType = sequenceType
			});
			return Sequences.Count - 1;
		}

		internal void Clear()
		{
			foreach (var sequence in Sequences)
			{
				sequence.Clear();
			}

			Sequences.Clear();

			LoadedCatalogItem?.DisposeImages();

			Initialize();
		}
	}
}
