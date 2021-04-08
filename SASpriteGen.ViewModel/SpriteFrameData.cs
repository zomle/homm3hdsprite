using ImageMagick;
using System;
using System.IO;

namespace SASpriteGen.ViewModel
{
	public class SpriteFrameData : ViewModel
	{
		public int FrameIndex { get; set; }

		public MagickImage Image { get; }

		private Stream imageStream;
		public Stream ImageStream
		{
			get
			{
				if (imageStream == null)
				{
					imageStream = new MemoryStream(Image.ToByteArray());
				}
				imageStream.Position = 0;
				return imageStream;
			}
			set { throw new NotImplementedException(); }
		}

		public int Width { get { return Image.Width; } }
		public int Height { get { return Image.Height; } }

		public double OriginalOffsetX { get; set; }
		public double OriginalOffsetY { get; set; }

		private double offsetX;
		public double OffsetX
		{
			get { return offsetX; }
			set
			{
				if (offsetX != value)
				{
					offsetX = value;
					NotifyPropertyChanged();
				}
			}
		}

		private double offsetY;
		public double OffsetY
		{
			get { return offsetY; }
			set
			{
				if (offsetY != value)
				{
					offsetY = value;
					NotifyPropertyChanged();
				}
			}
		}

		public double HighResScaleX { get; set; }
		public double HighResScaleY { get; set; }

		private double scalex;
		public double ScaleX
		{
			get
			{
				return scalex;
			}
			set
			{
				if (scalex != value)
				{
					scalex = Math.Round(value, 2);
					NotifyPropertyChanged();
				}
			}
		}

		private double scaley;
		public double ScaleY
		{
			get
			{
				return scaley;
			}
			set
			{
				if (scaley != value)
				{
					scaley = Math.Round(value, 2);
					NotifyPropertyChanged();
				}
			}
		}

		private bool currentPreviewFrame;

		public SpriteFrameData(int frameIndex, MagickImage image, double offsetX, double offsetY, double highResScaleX, double highResScaleY, double scaleX, double scaleY)
		{
			FrameIndex = frameIndex;
			Image = image;
			OriginalOffsetX = offsetX;
			OriginalOffsetY = offsetY;
			HighResScaleX = highResScaleX;
			HighResScaleY = highResScaleY;
			OffsetX = offsetX;
			OffsetY = offsetY;
			ScaleX = scaleX;
			ScaleY = scaleY;
		}

		public bool CurrentPreviewFrame
		{
			get { return currentPreviewFrame; }
			set
			{
				if (currentPreviewFrame != value)
				{
					currentPreviewFrame = value;
					NotifyPropertyChanged();
				}
			}
		}
	}
}
