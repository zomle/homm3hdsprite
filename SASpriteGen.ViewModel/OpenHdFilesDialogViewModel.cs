using System;
using System.Collections;

namespace SASpriteGen.ViewModel
{

	public class OpenHdFilesDialogViewModel : SynchedViewModel
	{
		public HdAssetCatalogBrowserViewModel AssetCatalogViewModel { get; set; }

		public OpenHdFilesDialogViewModel(Action<IEnumerable, object> registerCollectionSynchronization)
			: base(registerCollectionSynchronization)
		{
			AssetCatalogViewModel = new HdAssetCatalogBrowserViewModel(registerCollectionSynchronization);
		}
	}
}
