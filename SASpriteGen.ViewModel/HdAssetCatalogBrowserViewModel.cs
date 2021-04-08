using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace SASpriteGen.ViewModel
{
	public class HdAssetCatalogBrowserViewModel : SynchedViewModel
	{
		public ObservableCollection<HdAssetCatalogItem> Items { get; set; }


		private HdAssetCatalogItem selectedItem;
		public HdAssetCatalogItem SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				selectedItem = value;
				NotifyPropertyChanged(nameof(HasSelectedItem));
			}
		}

		public bool HasSelectedItem
		{
			get
			{
				return SelectedItem != null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		private string lodFilePath;
		public string LodFilePath
		{
			get { return lodFilePath; }
			set
			{
				if (lodFilePath != value)
				{
					lodFilePath = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int lodTocItemCount;
		public int LodTocItemCount
		{
			get { return lodTocItemCount; }
			set
			{
				if (lodTocItemCount != value)
				{
					lodTocItemCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int lodTocItemProcessed;
		public int LodTocItemProcessed
		{
			get { return lodTocItemProcessed; }
			set
			{
				if (lodTocItemProcessed != value)
				{
					lodTocItemProcessed = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool lodLoadInProgress;
		public bool LodLoadInProgress
		{
			get
			{
				return lodLoadInProgress;
			}
			set
			{
				if (lodLoadInProgress != value)
				{
					lodLoadInProgress = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool isLodFileLoaded;
		public bool IsLodFileLoaded
		{
			get { return isLodFileLoaded; }
			set
			{
				if (isLodFileLoaded != value)
				{
					isLodFileLoaded = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string pakx2FilePath;
		public string Pakx2FilePath
		{
			get { return pakx2FilePath; }
			set
			{
				if (pakx2FilePath != value)
				{
					pakx2FilePath = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool isPakx2FileLoaded;
		public bool IsPakx2FileLoaded
		{
			get { return isPakx2FileLoaded; }
			set
			{
				if (isPakx2FileLoaded != value)
				{
					isPakx2FileLoaded = value;
					NotifyPropertyChanged();
				}
			}
		}

		private string pakx3FilePath;
		public string Pakx3FilePath
		{
			get { return pakx3FilePath; }
			set
			{
				if (pakx3FilePath != value)
				{
					pakx3FilePath = value;
					NotifyPropertyChanged();
				}
			}
		}

		private bool isPakLoadInProgress;
		public bool IsPakLoadInProgress
		{
			get { return isPakLoadInProgress; }
			set
			{
				if (isPakLoadInProgress != value)
				{
					isPakLoadInProgress = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int totalAssetCount;
		public int TotalAssetCount
		{
			get { return totalAssetCount; }
			set
			{
				if (totalAssetCount != value)
				{
					totalAssetCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		private int loadedAssetCount;
		public int LoadedAssetCount
		{
			get { return loadedAssetCount; }
			set
			{
				if (loadedAssetCount != value)
				{
					loadedAssetCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		private readonly Dictionary<string, DefFile> DefFiles;

		private HdPakHandler HdPakHandler;

		public HdAssetCatalogBrowserViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			HdPakHandler = new HdPakHandler();

			DefFiles = new Dictionary<string, DefFile>();
			Items = new ObservableCollection<HdAssetCatalogItem>();

			RegisterCollectionSynchronization(Items);

			LodTocItemCount = 1;
			LodTocItemProcessed = 0;

			LoadedAssetCount = 0;
			TotalAssetCount = 1;
		}

		public (DefFile, HdAssetCatalogItem) GetSelection()
		{
			if (SelectedItem == null)
			{
				return (null, null);
			}

			DefFiles.TryGetValue(SelectedItem.Name, out var def);
			return (def, SelectedItem);
		}

		public void LoadPakx2File()
		{
			new Thread(LoadPakx2ThreadFunc).Start();
		}

		public void LoadPakx3File()
		{
			new Thread(LoadPakx3ThreadFunc).Start();
		}

		public void LoadLodFile()
		{
			new Thread(LoadLodThreadFunc).Start();
		}

		private void LoadLodThreadFunc()
		{
			LodLoadInProgress = true;
			LodTocItemProcessed = 0;
			IsLodFileLoaded = false;
			IsPakx2FileLoaded = false;

			DefFiles.Clear();

			var handler = new LodHandler(LodFilePath);
			handler.LoadTableOfContents(ti => ti.FileType == (int)DefType.Creature);

			LodTocItemCount = handler.TocItemCount;

			foreach (var item in handler.LoadDefFiles())
			{
				if (item != null)
				{
					DefFiles.Add(item.Name, item);
				}

				LodTocItemProcessed += 1;
			}

			LodTocItemProcessed = LodTocItemCount;
			LodLoadInProgress = false;
			IsLodFileLoaded = true;
		}

		private void LoadPakx2ThreadFunc()
		{
			IsPakLoadInProgress = true;
			IsPakx2FileLoaded = false;
			Items.Clear();
			LoadedAssetCount = 0;
			TotalAssetCount = 1;

			var catalog = HdPakHandler.LoadIntoCatalog(Pakx2FilePath, 2, ti => DefFiles.TryGetValue(ti.Name, out var def) && def.Type == DefType.Creature);

			TotalAssetCount = catalog.Items.Count;

			IsPakLoadInProgress = false;
			IsPakx2FileLoaded = true;
		}

		private void LoadPakx3ThreadFunc()
		{
			IsPakLoadInProgress = true;

			var catalog = HdPakHandler.LoadIntoCatalog(Pakx3FilePath, 3, ti => DefFiles.TryGetValue(ti.Name, out var def) && def.Type == DefType.Creature);
			TotalAssetCount = catalog.Items.Count;
			LoadedAssetCount = 0;

			foreach (var item in catalog.Items.Values)
			{
				item.LoadPreviewImage();
				Items.Add(item);
				LoadedAssetCount++;
			}

			IsPakLoadInProgress = false;
		}
	}
}
