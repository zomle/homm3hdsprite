using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SASpriteGen.ViewModel
{
	public abstract class SynchedViewModel : ViewModel
	{
		protected readonly object SynchronizationObject;

		protected Action<IEnumerable, object> RegisterCollectionSynchronizationCallback { get; }

		public SynchedViewModel(Action<IEnumerable, object> registerCollectionSynchronizationCallback)
		{
			SynchronizationObject = new object();
			RegisterCollectionSynchronizationCallback = registerCollectionSynchronizationCallback;
		}

		protected void RegisterCollectionSynchronization(IEnumerable collection)
		{
			RegisterCollectionSynchronizationCallback(collection, SynchronizationObject);
		}
	}

	public abstract class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
