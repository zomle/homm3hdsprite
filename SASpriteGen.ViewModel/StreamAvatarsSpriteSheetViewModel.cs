using SASpriteGen.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SASpriteGen.ViewModel
{
	public class StreamAvatarsSpriteSheetViewModel : SynchedViewModel
	{
		public delegate void FrameSizesUpdatedEventHandler(object sender, FrameSizesUpdatedEventArgs e);
		public event FrameSizesUpdatedEventHandler FrameSizesUpdated;

		public string DefSourceId { get; set; }

		private StreamAvatarsAvatarAction activeAction;
		public StreamAvatarsAvatarAction ActiveAction
		{
			get
			{
				return activeAction;
			}
			set
			{
				activeAction = value;
				NotifyPropertyChanged();
			}
		}

		private string message;
		public string Message
		{
			get
			{
				return message;
			}

			set
			{
				if (message != value)
				{
					message = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Command<string> ChangeAction { get; set; }
		public Command<string> ChangeFrameWidth { get; set; }
		public Command<string> ChangeFrameHeight { get; set; }
		public Command AdjustFrameSizeToAnimation { get; set; }

		public ObservableCollection<StreamAvatarsAvatarAction> AvatarActions { get; set; }

		public int FrameWidth
		{
			get
			{
				return AvatarActions.Where(a => a.SelectedSequence != null).Select(a => a.SelectedSequence.FrameWidth).FirstOrDefault();
			}

			set
			{
				throw new InvalidOperationException();
			}
		}

		public int FrameHeight
		{
			get
			{
				return AvatarActions.Where(a => a.SelectedSequence != null).Select(a => a.SelectedSequence.FrameHeight).FirstOrDefault();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public int SpriteSheetWidth
		{
			get
			{
				int maxWidth = 0;
				foreach (var avatarAction in AvatarActions)
				{
					if (avatarAction.SelectedSequence == null)
					{
						continue;
					}

					var sequence = avatarAction.SelectedSequence;
					int tmpWidth = sequence.FrameWidth * sequence.FrameCount;

					maxWidth = maxWidth > tmpWidth ? maxWidth : tmpWidth;
				}
				return maxWidth;
			}

			set
			{
				throw new InvalidOperationException();
			}
		}

		public int SpriteSheetHeight 
		{
			get
			{
				int maxHeight = 0;
				foreach (var avatarAction in AvatarActions)
				{
					if (avatarAction.SelectedSequence == null || !avatarAction.IsSelected)
					{
						continue;
					}

					maxHeight += avatarAction.SelectedSequence.FrameHeight;
				}
				return maxHeight;
			}

			set
			{
				throw new InvalidOperationException();
			}
		}

		private IReadOnlyList<SpriteFrameSequenceViewModel> Sequences { get; set; }

		public StreamAvatarsSpriteSheetViewModel(Action<IEnumerable, object> registerCollectionSynchronizationCallback, ObservableCollection<SpriteFrameSequenceViewModel> sequences)
			: base(registerCollectionSynchronizationCallback)
		{
			Sequences = sequences;

			AvatarActions = new ObservableCollection<StreamAvatarsAvatarAction>();

			RegisterCollectionSynchronization(AvatarActions);

			ChangeFrameWidth = new Command<string>((arg) =>
			{
				var dw = Convert.ToInt32(arg);
				ChangeFrameSize(dw, 0);
			});

			ChangeFrameHeight = new Command<string>((arg) =>
			{
				var dh = Convert.ToInt32(arg);
				ChangeFrameSize(0, dh);
			});

			ChangeAction = new Command<string>(s => {
				var action = AvatarActions.FirstOrDefault(aa => aa.Name == s);
				if (action == null)
				{
					throw new InvalidOperationException();
				}
				ActiveAction = action;

				RefreshFrameSizes();
			});

			AdjustFrameSizeToAnimation = new Command(() =>
			{
				int minYOffset = int.MaxValue;
				int minXOffset = int.MaxValue;
				int maxXOffset = 0;

				foreach (var action in AvatarActions)
				{
					if (!action.IsSelected || action.SelectedSequence == null)
					{
						continue;
					}
					
					foreach (var data in action.SelectedSequence.AnimationPreview.Data)
					{
						minYOffset = Math.Min(minYOffset, data.FramedImage.OffsetY);
						minXOffset = Math.Min(minXOffset, data.FramedImage.OffsetX);
						maxXOffset = Math.Max(maxXOffset, data.FramedImage.OffsetX + (int)(data.FramedImage.Width * data.FramedImage.Scale));
					}
				}

				if (FrameWidth == 0)
				{
					return;
				}

				int diffHeight = -minYOffset;

				int diffLeft = minXOffset;
				int diffRight = FrameWidth - maxXOffset;
				int diffWidth = 0;

				if (diffLeft<0 || diffRight < 0)
				{
					diffWidth = Math.Abs(Math.Min(diffLeft, diffRight)) * 2;
				}
				else if (diffLeft > 0 && diffRight > 0)
				{
					diffWidth = -Math.Min(diffLeft, diffRight) * 2;
				}
				
				ChangeFrameSize(diffWidth, diffHeight);
			});
		}

		private void ChangeFrameSize(int dw, int dh)
		{
			if (dw == 0 && dh == 0)
			{
				return;
			}

			Sequences.ForAll(s => s.ChangeFrameSize(dw, dh));
			if (dw != 0)
			{ 
				NotifyPropertyChanged(nameof(FrameWidth));
				NotifyPropertyChanged(nameof(SpriteSheetWidth));
			}
			if (dh != 0)
			{
				NotifyPropertyChanged(nameof(FrameHeight));
				NotifyPropertyChanged(nameof(SpriteSheetHeight));
			}

			FrameSizesUpdated?.Invoke(this, new FrameSizesUpdatedEventArgs());
		}

		public bool ValidateExportSettings()
		{
			var missingActions = new List<string>();
			foreach (var action in AvatarActions)
			{
				if (action.IsSelected && action.SelectedSequence == null)
				{
					missingActions.Add(action.Name);
				}
			}

			if (missingActions.Count > 0)
			{
				if (missingActions.Count == 1)
				{
					Message = "The following action was selected for export, but doesn't have an associated animation sequence: " + string.Join(", ", missingActions);
				}
				else
				{
					Message = "The following actions were selected for export, but don't have an associated animation sequence: " + string.Join(", ", missingActions);
				}
				return false;
			}

			return true;
		}

		public void ExportAsSpriteSheet(string imageFilePath)
		{
			Message = string.Empty;

			if (!ValidateExportSettings())
			{
				return;
			}

			var generator = new SpriteGenerator();
			SpriteSheet spriteSheet;

			try
			{
				spriteSheet = CreateSpriteSheet();
			}
			catch (Exception e)
			{
				Message = "Failed to generate sprite sheet for export: " + e.Message;
				return;
			}

			ImageMagick.MagickImage image;
			try
			{ 
				image = generator.CreateSpriteSheet(spriteSheet);
				try
				{
					image.Write(imageFilePath);
					Message += "Sprite sheet image is saved to " + imageFilePath + Environment.NewLine;
				}
				catch(Exception e)
				{
					Message += "Failed to save sprite sheet image to file: " + e.Message + Environment.NewLine;
				}
				finally
				{
					image?.Dispose();
				}
			}
			catch (Exception e)
			{
				Message += "Failed to generate sprite sheet image: " + e.Message + Environment.NewLine;
			}

			string info;
			try
			{
				info = generator.CreateSpriteSheetInfo(spriteSheet);
			}
			catch (Exception e)
			{
				Message += "Failed to generate sprite sheet information: " + e.Message;
				return;
			}

			try
			{
				var infoFilePath = Path.Combine(Path.GetDirectoryName(imageFilePath), Path.GetFileNameWithoutExtension(imageFilePath) + @"_info.txt");
				File.WriteAllText(infoFilePath, info);
				Message += "Sprite sheet information is saved to " + infoFilePath;
			}
			catch (Exception e)
			{
				Message += "Failed to save sprite sheet information to file: " + e.Message + Environment.NewLine;
				return;
			}
		}

		private SpriteSheet CreateSpriteSheet()
		{
			var result = new SpriteSheet(DefSourceId);
			foreach (var action in AvatarActions)
			{
				if (!action.IsSelected)
				{
					continue;
				}

				result.Animations.Add(action.CreateAnimation());
			}

			return result;
		}

		public void RefreshFrameSizes()
		{
			NotifyPropertyChanged(nameof(FrameWidth));
			NotifyPropertyChanged(nameof(FrameHeight));
			NotifyPropertyChanged(nameof(SpriteSheetWidth));
			NotifyPropertyChanged(nameof(SpriteSheetHeight));
		}

		public void AddSequence(string name, bool isOptional, bool isSelected)
		{
			AvatarActions.Add(new StreamAvatarsAvatarAction(RegisterCollectionSynchronizationCallback)
			{
				Name = name,
				IsOptional = isOptional,
				IsSelected = isSelected
			});

			if (ActiveAction == null)
			{
				ActiveAction = AvatarActions[0];
			}
		}

		internal void LoadAvailableSequences(IEnumerable<SpriteFrameSequenceViewModel> sequences)
		{
			Message = string.Empty;

			foreach (var avatarAction in AvatarActions)
			{
				avatarAction.LoadAvailableSequences(sequences);
			}

			NotifyPropertyChanged(nameof(FrameWidth));
			NotifyPropertyChanged(nameof(SpriteSheetWidth));
			NotifyPropertyChanged(nameof(FrameHeight));
			NotifyPropertyChanged(nameof(SpriteSheetHeight));
		}
	}
}
