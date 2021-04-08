using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace SASpriteGen.ViewModel
{
	public class MainWindowViewModel : SynchedViewModel
	{
		public Homm3HdSpriteSheetViewModel Homm3HdSpriteSheet { get; private set; }
		public HdAssetCatalogBrowserViewModel HdAssetCatalog { get; private set; }
		public StreamAvatarsSpriteSheetViewModel StreamAvatarsSpriteSheet { get; private set; }

		public Command LoadSelectedHdAsset { get; set; }

		private ActiveTab selectedTab;

		public ActiveTab SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{
				if (selectedTab != value)
				{
					selectedTab = value;
					NotifyPropertyChanged();
				}
			}
		}

		public MainWindowViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			Homm3HdSpriteSheet = new Homm3HdSpriteSheetViewModel(registerCollectionSynchronization);
			HdAssetCatalog = new HdAssetCatalogBrowserViewModel(registerCollectionSynchronization);
			StreamAvatarsSpriteSheet = new StreamAvatarsSpriteSheetViewModel(registerCollectionSynchronization);
			StreamAvatarsSpriteSheet.AddSequence("Idle", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Run", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Sit", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Stand", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Jump", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Attack", true, true);

			LoadSelectedHdAsset = new Command(() =>
			{
				SelectedTab = ActiveTab.Homm3HdSpriteSheet;

				(var defFile, var catalogItem) = HdAssetCatalog.GetSelection();
				Homm3HdSpriteSheet.Load(defFile, catalogItem);
			});
		}
	}
}
