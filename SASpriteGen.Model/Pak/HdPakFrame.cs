﻿using ImageMagick;

namespace SASpriteGen.Model.Pak
{
	public class HdPakFrame
	{
		public string SourceFilePath { get; private set; }

		public int SheetDataOffset { get; set; }
		public int SheetCompressedLength { get; set; }

		public ImageSliceInfo Metadata { get; private set; }

		private MagickImage Image { get; set; }

		public HdPakFrame(string sourceFilePath, int sheetDataOffset, int sheetCompressedLength, ImageSliceInfo metadata)
		{
			SourceFilePath = sourceFilePath;
			SheetDataOffset = sheetDataOffset;
			SheetCompressedLength = sheetCompressedLength;
			Metadata = metadata;
		}

		public MagickImage GetImage()
		{
			if (Image == null)
			{
				Image = HdPakHandler.LoadFrame(this);
			}

			return Image;
		}
	}
}
