namespace SASpriteGen.Model.Pak
{
	public class HdPakTocItem
	{
		public string Name { get; set; }
		public int MetadataOffset { get; set; }
		public int MetadataLength { get; set; }

		public int SomeCount { get; set; }
		public int CompressedDataSize { get; set; }
		public int UncompressedDataSize { get; set; }
	}
}
