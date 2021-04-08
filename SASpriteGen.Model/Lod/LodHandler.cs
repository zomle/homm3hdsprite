using SASpriteGen.Model.Def;
using System;
using System.Collections.Generic;
using System.IO;

namespace SASpriteGen.Model
{
	public class LodHandler
	{
		public int TocItemCount { get { return TableOfContents.Count; } }

		private List<LodTocItem> TableOfContents { get; set; }
		private string LodPath { get; set; }

		public LodHandler(string lodPath)
		{
			TableOfContents = new List<LodTocItem>();
			LodPath = lodPath;
		}

		public void LoadTableOfContents(Predicate<LodTocItem> loadPredicate)
		{
			using var fs = new FileStream(LodPath, FileMode.Open, FileAccess.Read);
			TableOfContents = ReadToc(fs, 8, loadPredicate);
		}

		public IEnumerable<DefFile> LoadDefFiles()
		{
			using var fs = new FileStream(LodPath, FileMode.Open, FileAccess.Read);
			var defHandler = new DefHandler();

			foreach (var item in TableOfContents)
			{
				if (!string.Equals(Path.GetExtension(item.Name), ".def", StringComparison.InvariantCultureIgnoreCase))
				{
					yield return null;
					continue;
				}
				
				fs.Seek(item.DataOffset, SeekOrigin.Begin);

				byte[] defData;
				if (item.CompressedDataSize != 0)
				{
					var compressed = new byte[item.CompressedDataSize];
					fs.Read(compressed, 0, compressed.Length);

					defData = Decompress(compressed, item.UncompressedDataSize);
				}
				else
				{
					defData = new byte[item.UncompressedDataSize];
					fs.Read(defData, 0, defData.Length);
				}
				var result = defHandler.LoadDef(Path.GetFileNameWithoutExtension(item.Name), defData);
				yield return result;
			}
		}

		private byte[] Decompress(byte[] compData, int uncompressedSize)
		{
			using var ds = new Ionic.Zlib.ZlibStream(new MemoryStream(compData), Ionic.Zlib.CompressionMode.Decompress);
			using var decompressed = new MemoryStream(uncompressedSize);
			ds.CopyTo(decompressed);

			return decompressed.ToArray();
		}

		static List<LodTocItem> ReadToc(FileStream fs, int startOffset, Predicate<LodTocItem> loadPredicate)
		{
			int tocSize = fs.ReadInt32(startOffset, SeekOrigin.Begin);
			fs.Seek(80, SeekOrigin.Current);

			var result = new List<LodTocItem>(tocSize);
			for (int i = 0; i < tocSize; i++)
			{
				var offset = fs.Position;

				var item = new LodTocItem();
				item.Name = fs.ReadString().ToUpperInvariant();

				fs.Seek(offset + 16, SeekOrigin.Begin);
				item.DataOffset = fs.ReadInt32();
				item.UncompressedDataSize = fs.ReadInt32();
				item.FileType = fs.ReadInt32();
				item.CompressedDataSize = fs.ReadInt32();

				if (loadPredicate(item))
				{
					result.Add(item);
				}
			}

			return result;
		}
	}
}
