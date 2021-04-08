using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SASpriteGen.ViewModel
{
	public class StreamAvatarsAvatarAction : SynchedViewModel
	{
		private string name;
		public string Name
		{
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool isOptional;
		public bool IsOptional
		{
			get { return isOptional; }
			set
			{
				if (isOptional != value)
				{
					isOptional = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				if (isSelected != value)
				{
					isSelected = value;
					NotifyPropertyChanged();
				}
			}
		}

		public Command<string> AdjustAllXOffset { get; set; }
		public Command<string> AdjustAllYOffset { get; set; }
		public Command ResetAllOffsets { get; set; }

		private StreamAvatarsFrameSequence selectedSequence;
		public StreamAvatarsFrameSequence SelectedSequence
		{
			get
			{
				return selectedSequence;
			}

			set
			{
				if (selectedSequence != value)
				{
					selectedSequence = value;
					NotifyPropertyChanged();
				}
			}
		}

		public ObservableCollection<StreamAvatarsFrameSequence> AvailableSequences { get; set; }

		public StreamAvatarsAvatarAction(Action<IEnumerable, object> registerCollectionSynchronizationCallback)
			: base(registerCollectionSynchronizationCallback)
		{
			AvailableSequences = new ObservableCollection<StreamAvatarsFrameSequence>();

			RegisterCollectionSynchronization(AvailableSequences);

			AdjustAllXOffset = new Command<string>((offset) => { ChangeAllOffsets(Convert.ToInt32(offset), 0); });
			AdjustAllYOffset = new Command<string>((offset) => { ChangeAllOffsets(0, Convert.ToInt32(offset)); });
			ResetAllOffsets = new Command(() => ResetAllOffsetsToDefault());
		}

		private void ResetAllOffsetsToDefault()
		{
			if (SelectedSequence == null)
			{
				return;
			}

			foreach (var data in SelectedSequence.AnimationPreview.Data)
			{
				data.ResetOffsetsToDefault();
			}
		}

		private void ChangeAllOffsets(int dx, int dy)
		{
			if (SelectedSequence == null)
			{
				return;
			}

			foreach (var data in SelectedSequence.AnimationPreview.Data)
			{
				data.FramedImage.ManualOffsetX += dx;
				data.FramedImage.ManualOffsetY += dy;
			}
		}

		public void LoadAvailableSequences(IEnumerable<SpriteFrameSequenceViewModel> availableSequences)
		{
			SelectedSequence = null;
			AvailableSequences.Clear();

			foreach (var sequence in availableSequences)
			{
				var newSequence = new StreamAvatarsFrameSequence()
				{
					SequenceName = sequence.SequenceName,
					AnimationPreview = sequence.AnimationPreview.Clone()
				};

				AvailableSequences.Add(newSequence);

				if (IsDefaultSequence(sequence.SequenceType, Name))
				{
					SelectedSequence = newSequence;
				}
			}
		}

		private bool IsDefaultSequence(DefAnimation homm3Animation, string streamAvatarAction)
		{
			if (streamAvatarAction?.ToLower() == "idle" && homm3Animation == DefAnimation.MouseOver)
			{
				return true;
			}
			else if (streamAvatarAction?.ToLower() == "run" && homm3Animation == DefAnimation.Moving)
			{
				return true;
			}
			else if (streamAvatarAction?.ToLower() == "sit" && homm3Animation == DefAnimation.Death)
			{
				return true;
			}
			else if (streamAvatarAction?.ToLower() == "stand" && homm3Animation == DefAnimation.Standing)
			{
				return true;
			}
			else if (streamAvatarAction?.ToLower() == "jump" && homm3Animation == DefAnimation.Standing)
			{
				return true;
			}
			else if (streamAvatarAction?.ToLower() == "attack" && homm3Animation == DefAnimation.AttackStraight)
			{
				return true; 
			}

			return false;
		}

		internal Animation CreateAnimation()
		{
			var result = new Animation(Name, SelectedSequence.SequenceName);
			foreach (var data in SelectedSequence.AnimationPreview.Data)
			{
				result.Frames.Add(data.FramedImage.CreateFrame());
			}
			return result;
		}
	}
}
