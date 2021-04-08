using System.Collections.Generic;

namespace SASpriteGen.Model.Pak
{
	public class HdAssetCatalog
	{
		//CNOSFE -> data
		public Dictionary<string, HdAssetCatalogItem> Items { get; set; }

		public HdAssetCatalog()
		{
			Items = new Dictionary<string, HdAssetCatalogItem>();
		}

		public void Add(string name, string sourceFile, int dataOffset, int dataLength, IEnumerable<ImageSliceInfo> imageSlices)
		{
			if (!Items.TryGetValue(name, out var catalogItem))
			{
				catalogItem = new HdAssetCatalogItem(name);
				Items.Add(name, catalogItem);
			}

			foreach (var imageSlice in imageSlices)
			{
				catalogItem.AddImage(sourceFile, dataOffset, dataLength, imageSlice);
			}
		}
	}
}
