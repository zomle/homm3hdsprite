using ImageMagick;
using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SASpriteGen.ViewModel
{

	public class Homm3HdSpriteSheetViewModel : SynchedViewModel
	{
		public int OriginalFrameWidth { get; set; }
		public int OriginalFrameHeight { get; set; }
		public double OriginalScaleX { get; set; }
		public double OriginalScaleY { get; set; }

		public int FrameWidth
		{
			get { return Sequences.Count == 0 ? 0 : Sequences[0].FrameWidth; }
			set { throw new InvalidOperationException(); }
		}

		public int FrameHeight
		{
			get { return Sequences.Count == 0 ? 0 : Sequences[0].FrameHeight; }
			set { throw new InvalidOperationException(); }
		}

		public double Scaling
		{
			get { return (Sequences.Count == 0 || Sequences[0].Data.Count == 0) ? 0 : Sequences[0].Data[0].ScaleX; }
			set { throw new InvalidOperationException(); }
		}

		public Command IncreaseFrameWidth { get; set; }
		public Command DecreaseFrameWidth { get; set; }

		public Command IncreaseFrameHeight { get; set; }
		public Command DecreaseFrameHeight { get; set; }

		public Command IncreaseScaling { get; set; }
		public Command DecreaseScaling { get; set; }

		public Command ResetSizing { get; set; }
		public Command RecalculateFrameSize { get; set; }

		public ObservableCollection<SpriteFrameSequenceViewModel> Sequences { get; set; }

		public Homm3HdSpriteSheetViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			Sequences = new ObservableCollection<SpriteFrameSequenceViewModel>();

			RegisterCollectionSynchronization(Sequences);

			IncreaseFrameWidth = new Command(() =>
			{
				Sequences.ForAll(s => s.FrameWidth += 1);
				NotifyPropertyChanged(nameof(FrameWidth));
			});

			DecreaseFrameWidth = new Command(() =>
			{
				Sequences.ForAll(s => s.FrameWidth -= 1);
				NotifyPropertyChanged(nameof(FrameWidth));
			});

			IncreaseFrameHeight = new Command(() =>
			{
				Sequences.ForAll(s => s.FrameHeight += 1);
				NotifyPropertyChanged(nameof(FrameHeight));
			});

			DecreaseFrameHeight = new Command(() =>
			{
				Sequences.ForAll(s => s.FrameHeight -= 1);
				NotifyPropertyChanged(nameof(FrameHeight));
			});

			IncreaseScaling = new Command(() =>
			{
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleX += 0.01));
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleY += 0.01));
				NotifyPropertyChanged(nameof(Scaling));
			});

			DecreaseScaling = new Command(() =>
			{
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleX -= 0.01));
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleY -= 0.01));
				NotifyPropertyChanged(nameof(Scaling));
			});

			ResetSizing = new Command(() =>
			{
				Sequences.ForAll(s => s.FrameWidth = OriginalFrameWidth);
				Sequences.ForAll(s => s.FrameHeight = OriginalFrameHeight);
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleX = OriginalScaleX));
				Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleY = OriginalScaleY));

				NotifyPropertyChanged(nameof(FrameWidth));
				NotifyPropertyChanged(nameof(FrameHeight));
				NotifyPropertyChanged(nameof(Scaling));
			});

			RecalculateFrameSize = new Command(() =>
			{
				var maxWidth = (int)Math.Ceiling(Sequences.Max(s => s.Data.Max(d => d.Width * d.ScaleX)));
				var maxHeight = (int)Math.Ceiling(Sequences.Max(s => s.Data.Max(d => d.Height * d.ScaleY)));

				Sequences.ForAll(s => s.FrameWidth = maxWidth);
				Sequences.ForAll(s => s.FrameHeight = maxHeight);
			});

			OriginalFrameHeight = 200;
			OriginalFrameWidth = 200;
			OriginalScaleX = 1.00;
			OriginalScaleY = 1.0;

			Sequences.ForAll(s => s.FrameWidth = OriginalFrameWidth);
			Sequences.ForAll(s => s.FrameHeight = OriginalFrameHeight);

			Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleX = OriginalScaleX));
			Sequences.ForAll(s => s.Data.ForAll(d => d.ScaleY = OriginalScaleY));
		}

		public void Load(DefFile defFile, HdAssetCatalogItem catalogItem)
		{
			Clear();

			new Thread(() => LoadSpriteSheetThreadFunc(defFile, catalogItem)).Start();
		}

		private void LoadSpriteSheetThreadFunc(DefFile defFile, HdAssetCatalogItem catalogItem)
		{
			double minScale = double.MaxValue;
			foreach (var group in defFile.Groups)
			{
				foreach (var item in group.Value.Items)
				{
					var frameId = Path.GetFileNameWithoutExtension(item.FileName).ToUpper();
					if (!catalogItem.HdPakFrames.TryGetValue(frameId, out var frame))
					{
						continue;
					}

					var image = frame.GetImage();

					var scaleX = (double)OriginalFrameWidth / image.Width;
					var scaleY = (double)OriginalFrameHeight / image.Height;
					var tmpScale = Math.Min(scaleX, scaleY);

					minScale = Math.Min(minScale, tmpScale);
				}
			}

			OriginalScaleX = minScale;
			OriginalScaleY = minScale;

			foreach (var group in defFile.Groups)
			{
				var ix = AddSequence(group.Value.GetGroupName(defFile.Type));
				var sequence = Sequences[ix];

				var leftest = group.Value.Items.Min(v => v.FrameLeft);
				var topest = group.Value.Items.Min(v => v.FrameTop);

				Debug.WriteLine($"Group: {ix}; Leftest: {leftest}; Topest: {topest}");

				foreach (var item in group.Value.Items)
				{
					var frameId = Path.GetFileNameWithoutExtension(item.FileName).ToUpper();
					if (!catalogItem.HdPakFrames.TryGetValue(frameId, out var frame))
					{
						continue;
					}

					var image = frame.GetImage();

					int frameIx = sequence.Data.Count;
					Debug.WriteLine($"Group: {ix}; Frame: {frameIx}; Image Width: {image.Width}; Image Height: {image.Height}");
					Debug.WriteLine($"Group: {ix}; Frame: {frameIx}; Frame Top: {item.FrameTop}; Frame Left: {item.FrameLeft}; Frame Width: {item.FrameWidth}; Frame Height: {item.FrameHeight}; Width: {item.Width}; Height: {item.Height}");
					Debug.WriteLine($"Group: {ix}; Frame: {frameIx}; {frame.Metadata.GetDebugString()}");

					var highresScaleX = (double)image.Width / item.FrameWidth;
					var highresScaleY = (double)image.Height / item.FrameHeight;

					sequence.AddNewFrame(image, item.FrameLeft - leftest, item.FrameTop - topest, highresScaleX, highresScaleY, OriginalScaleX, OriginalScaleY);
				}

				Sequences[ix].AnimationRunning = true;
			}

			NotifyPropertyChanged(nameof(FrameWidth));
			NotifyPropertyChanged(nameof(FrameHeight));
			NotifyPropertyChanged(nameof(Scaling));
		}

		public int AddSequence(string sequenceName)
		{
			Sequences.Add(new SpriteFrameSequenceViewModel(RegisterCollectionSynchronizationCallback)
			{
				SequenceName = sequenceName,
				FrameWidth = OriginalFrameWidth,
				FrameHeight = OriginalFrameHeight,
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
		}
	}
}
