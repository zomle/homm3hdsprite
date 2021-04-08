using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SASpriteGen.Model.Pak
{
	public class HdAssetCatalogItem
	{
		//CNOSFE
		public string Name { get; set; }
		
		//CNOSFE01 -> {file, offset, image position}
		public Dictionary<string, HdPakFrame> HdPakFrames { get; set; }

		public Stream PreviewImage { get; set; }

		public HdAssetCatalogItem(string name)
		{
			HdPakFrames = new Dictionary<string, HdPakFrame>();
			Name = name;
		}

		public void LoadPreviewImage()
		{
			var firstFrame = HdPakFrames.OrderBy(kv => kv.Key).Select(kc => kc.Value).FirstOrDefault();
			PreviewImage = HdPakHandler.GetPreviewImage(firstFrame);
		}

		internal void AddImage(string sourceFile, int dataOffset, int compressedLength, ImageSliceInfo imageSlice)
		{
			if (!HdPakFrames.TryGetValue(imageSlice.Name, out var image))
			{
				image = new HdPakFrame(sourceFile, dataOffset, compressedLength, imageSlice);
				HdPakFrames.Add(imageSlice.Name, image);
			}
		}
	}
}
