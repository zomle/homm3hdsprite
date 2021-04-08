using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SASpriteGen.Model.Pak
{
	public class HdAssetCatalogItem
	{
		public string Name { get; set; }
		
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

		public void LoadFrames(Action frameLoadedCallback)
		{
			HdPakHandler.LoadFrames(this, frameLoadedCallback);
		}

		public HdPakFrame GetFrameForFileName(string fileName)
		{
			var frameId = Path.GetFileNameWithoutExtension(fileName).ToUpper();
			if (!HdPakFrames.TryGetValue(frameId, out var frame))
			{
				return null;
			}
			return frame;
		}

		public void AddImage(string sourceFile, int dataOffset, int compressedLength, ImageSliceInfo imageSlice)
		{
			if (!HdPakFrames.TryGetValue(imageSlice.Name, out var image))
			{
				image = new HdPakFrame(sourceFile, dataOffset, compressedLength, imageSlice);
				HdPakFrames.Add(imageSlice.Name, image);
			}
		}

		public void DisposeImages()
		{
			foreach (var frame in HdPakFrames.Values)
			{
				frame.DisposeImage();
			}
		}
	}
}
