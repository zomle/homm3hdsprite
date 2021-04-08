namespace SASpriteGen.Model.Pak
{
	public class ImageSliceInfo
	{
		public string Name { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Rotation { get; set; }

		public int Scaling { get; set; }

		public string GetDebugString()
		{
			return $"X: {X}; Y: {Y}; Width: {Width}; Height: {Height}; Rotation: {Rotation}; Scale: {Scaling}";
		}
	}
}
