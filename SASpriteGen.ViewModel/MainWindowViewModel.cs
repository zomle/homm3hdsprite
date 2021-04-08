using System;
using System.Collections;

namespace SASpriteGen.ViewModel
{
	public class MainWindowViewModel : SynchedViewModel
	{
		public Homm3HdSpriteSheetViewModel Homm3HdSpriteSheet { get; private set; }
		public HdAssetCatalogBrowserViewModel HdAssetCatalog { get; private set; }
		public StreamAvatarsSpriteSheetViewModel StreamAvatarsSpriteSheet { get; private set; }

		public Command LoadSelectedHdAsset { get; set; }
		public Command<string> ChangeImageBackgroundColor { get; set; }

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

		private bool showCenterSeparator;
		public bool ShowCenterSeparator
		{
			get
			{ return showCenterSeparator; }
			set
			{
				showCenterSeparator = value;
				NotifyPropertyChanged();
			}
		}

		private string imageBackgroundColor;
		public string ImageBackgroundColor
		{
			get
			{
				return imageBackgroundColor;
			}
			set
			{
				if (imageBackgroundColor != value)
				{
					imageBackgroundColor = value;
					NotifyPropertyChanged();
				}
			}
		}

		public MainWindowViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			Homm3HdSpriteSheet = new Homm3HdSpriteSheetViewModel(registerCollectionSynchronization);
			HdAssetCatalog = new HdAssetCatalogBrowserViewModel(registerCollectionSynchronization);
			StreamAvatarsSpriteSheet = new StreamAvatarsSpriteSheetViewModel(registerCollectionSynchronization, Homm3HdSpriteSheet.Sequences);
			StreamAvatarsSpriteSheet.AddSequence("Idle", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Run", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Sit", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Stand", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Jump", false, true);
			StreamAvatarsSpriteSheet.AddSequence("Attack", true, true);

			ImageBackgroundColor = "AntiqueWhite";

			ChangeImageBackgroundColor = new Command<string>((arg) =>
			{
				ImageBackgroundColor = arg;
			});

			LoadSelectedHdAsset = new Command(() =>
			{
				SelectedTab = ActiveTab.Homm3HdSpriteSheet;

				(var defFile, var catalogItem) = HdAssetCatalog.GetSelection();

				Homm3HdSpriteSheet.Load(defFile, catalogItem);
				StreamAvatarsSpriteSheet.DefSourceId = defFile.Name;
				
			});

			Homm3HdSpriteSheet.SequenceCollectionUpdated += (_, e) =>
			{
				StreamAvatarsSpriteSheet.LoadAvailableSequences(e.NewSequences);
			};

			Homm3HdSpriteSheet.FrameSizesUpdated += (s, e) =>
			{
				StreamAvatarsSpriteSheet.RefreshFrameSizes();
			};

			StreamAvatarsSpriteSheet.FrameSizesUpdated += (s, e) =>
			{
				Homm3HdSpriteSheet.RefreshFrameSizes();
			};
		}
	}
}
