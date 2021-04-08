using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace SASpriteGen.ViewModel
{
	public class StreamAvatarsFrameSequence : SynchedViewModel
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

		public SpriteFrameSequenceViewModel SelectedSequence { get; set; }

		
		public StreamAvatarsFrameSequence(Action<IEnumerable, object> registerCollectionSynchronizationCallback)
			: base(registerCollectionSynchronizationCallback)
		{
		}
	}

	public class StreamAvatarsSpriteSheetViewModel : SynchedViewModel
	{
		public ObservableCollection<StreamAvatarsFrameSequence> Sequences { get; set; }

		public StreamAvatarsSpriteSheetViewModel(Action<IEnumerable, object> registerCollectionSynchronizationCallback)
			: base(registerCollectionSynchronizationCallback)
		{
			Sequences = new ObservableCollection<StreamAvatarsFrameSequence>();
			RegisterCollectionSynchronization(Sequences);
		}

		public void AddSequence(string name, bool isOptional, bool isSelected)
		{
			Sequences.Add(new StreamAvatarsFrameSequence(RegisterCollectionSynchronizationCallback) { Name = name, IsOptional = isOptional, IsSelected = isSelected });
		}
	}
}
