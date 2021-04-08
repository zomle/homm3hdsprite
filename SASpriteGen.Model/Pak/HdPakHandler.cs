using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SASpriteGen.Model.Pak
{
	public class HdPakHandler
	{
		private HdAssetCatalog AssetCatalog { get; set; }

		private static readonly Dictionary<string, MagickImage> DecompressedSheetCache;

		static HdPakHandler()
		{
			DecompressedSheetCache = new Dictionary<string, MagickImage>();
		}

		public HdPakHandler()
		{
			AssetCatalog = new HdAssetCatalog();
		}

		public HdAssetCatalog LoadIntoCatalog(string pakPath, int scaling, Predicate<HdPakTocItem> loadPredicate)
		{
			using var fs = new FileStream(pakPath, FileMode.Open, FileAccess.Read);
			var tocOffset = fs.ReadInt32(4, SeekOrigin.Begin);

			var toc = ReadToc(fs, tocOffset);

			foreach (var tocItem in toc)
			{
				if (!loadPredicate(tocItem))
				{
					continue;
				}

				var metadata = fs.ReadString(tocItem.MetadataLength, Encoding.ASCII, tocItem.MetadataOffset, SeekOrigin.Begin);
				var imageSlices = CreateImageSlices(metadata, scaling);

				AssetCatalog.Add(tocItem.Name, pakPath, (int)fs.Position, tocItem.CompressedDataSize, imageSlices);
			}

			return AssetCatalog;
		}

		private static IReadOnlyList<ImageSliceInfo> CreateImageSlices(string metadata, int scaling)
		{
			var result = new List<ImageSliceInfo>();

			var reader = new StringReader(metadata);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					continue;
				}

				var tokens = line.Split(' ');

				if (tokens[1] != "0")
				{
					//have no clue what this is. Maybe different channel for shadow?
					continue;
				}
				var slice = new ImageSliceInfo
				{
					Name = tokens[0],
					X = int.Parse(tokens[6]),
					Y = int.Parse(tokens[7]),
					Width = int.Parse(tokens[8]),
					Height = int.Parse(tokens[9]),
					Rotation = int.Parse(tokens[10]),
					Scaling = scaling
				};
				result.Add(slice);
			}
			return result;
		}

		private static byte[] Decompress(byte[] compData, int uncompressedSize = 0)
		{
			using var ds = new Ionic.Zlib.ZlibStream(new MemoryStream(compData), Ionic.Zlib.CompressionMode.Decompress);
			using var decompressed = new MemoryStream(uncompressedSize == 0 ? compData.Length * 2 : uncompressedSize);
			ds.CopyTo(decompressed);

			return decompressed.ToArray();
		}

		private static MagickImage GetSpriteSheet(HdPakFrame frame)
		{
			return GetSpriteSheet(frame.SourceFilePath, frame.SheetDataOffset, frame.SheetCompressedLength);
		}

		private static MagickImage GetSpriteSheet(string sourceFilePath, int dataOffset, int dataLength)
		{
			var key = $"{sourceFilePath}|{dataOffset}";

			if (!DecompressedSheetCache.TryGetValue(key, out var sheetImage))
			{
				using var fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);
				fs.Seek(dataOffset, SeekOrigin.Begin);

				var compData = fs.ReadBytes(dataLength);
				var decompressedImage = Decompress(compData);

				sheetImage = new MagickImage(decompressedImage, MagickFormat.Dds);

				DecompressedSheetCache.Add(key, sheetImage);
			}

			return sheetImage;
		}

		internal static MagickImage LoadFrame(HdPakFrame frame)
		{
			var image = GetSpriteSheet(frame.SourceFilePath, frame.SheetDataOffset, frame.SheetCompressedLength);

			var result = (MagickImage)image.Clone(frame.Metadata.X, frame.Metadata.Y, frame.Metadata.Width, frame.Metadata.Height);
			result.Rotate(90.0 * frame.Metadata.Rotation);
			//result.Resize(new Percentage(200.0 / frame.Metadata.Scaling));
			
			result.AdaptiveResize(new MagickGeometry(new Percentage(200.0 / frame.Metadata.Scaling), new Percentage(200.0 / frame.Metadata.Scaling)));
			return result;
		}

		public static Stream GetPreviewImage(HdPakFrame frame)
		{
			var previewImage = LoadFrame(frame);
			previewImage.Resize(150, 150);

			var result = new MemoryStream();
			previewImage.Write(result, MagickFormat.Png);
			return result;
		}

		static List<HdPakTocItem> ReadToc(FileStream fs, int startOffset)
		{
			int tocSize = fs.ReadInt32(startOffset, SeekOrigin.Begin);

			var result = new List<HdPakTocItem>(tocSize);
			for (int i = 0; i < tocSize; i++)
			{
				var offset = fs.Position;

				var item = new HdPakTocItem();
				item.Name = fs.ReadString();

				fs.Seek(offset + 20, SeekOrigin.Begin);
				item.MetadataOffset = fs.ReadInt32();
				item.MetadataLength = fs.ReadInt32();

				item.SomeCount = fs.ReadInt32();
				item.CompressedDataSize = fs.ReadInt32();
				item.UncompressedDataSize = fs.ReadInt32();

				fs.Seek((item.SomeCount - 1) * 2 * 4, SeekOrigin.Current);
				fs.ReadInt32();

				var padSeek = (fs.Position - offset) % 8;
				padSeek = padSeek == 0 ? 0 : (8 - padSeek);
				fs.Seek(padSeek, SeekOrigin.Current);

				result.Add(item);
			}

			return result;
		}
	}
}
