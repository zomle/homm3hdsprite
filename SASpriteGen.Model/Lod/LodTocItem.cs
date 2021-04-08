namespace SASpriteGen.Model
{
	public class LodTocItem
	{
		public string Name { get; set; }
		public int DataOffset { get; set; }
		public int CompressedDataSize { get; set; }
		public int FileType { get; set; }
		public int UncompressedDataSize { get; set; }
	}
}
