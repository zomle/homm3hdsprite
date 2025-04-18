using ImageMagick;
using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using System;

namespace SASpriteGen.ViewModel
{
	public class FramedImageViewModel : ViewModel, IDisposable
	{
		public MagickImage Image { get; set; }
		public DefGroupItem DefSource { get; set; }

		private byte[] imageStream;
		public byte[] ImageStream
		{
			get
			{
				return imageStream;
			}
			set
			{
				imageStream = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(Width));
				NotifyPropertyChanged(nameof(Height));
			}
		}

		private uint frameWidth;
		public uint FrameWidth
		{
			get
			{
				return frameWidth;
			}
			set
			{
				if (frameWidth != value)
				{
					frameWidth = value;
					NotifyPropertyChanged();

					NotifyPropertyChanged(nameof(OffsetX));
					NotifyPropertyChanged(nameof(OffsetY));
				}
			}
		}

		private uint frameHeight;
		public uint FrameHeight
		{
			get
			{
				return frameHeight;
			}
			set
			{
				if (frameHeight != value)
				{
					frameHeight = value;
					NotifyPropertyChanged();

					NotifyPropertyChanged(nameof(OffsetX));
					NotifyPropertyChanged(nameof(OffsetY));
				}
			}
		}

		public int OffsetX
		{
			get
			{
				//adjust to first frame
				var firstImageOffset = (double)(DefSource.FrameLeft - FirstFrame.DefSource.FrameLeft) * ((double)Image.Width * Scale / (double)DefSource.FrameWidth);
				return (int)(FirstFrame.ImageCenterOffset + firstImageOffset + manualOffsetX);
			}

			set
			{
				throw new InvalidOperationException();
			}
		}

		public int OffsetY
		{
			get
			{
				var firstImageOffset = (double)(DefSource.FrameTop - FirstFrame.DefSource.FrameTop) * ((double)Image.Height * Scale / (double)DefSource.FrameHeight);
				return (int)(FirstFrame.ImageBottomOffset + firstImageOffset + manualOffsetY);
			}

			set
			{
				throw new InvalidOperationException();
			}
		}

		private double scale;
		public double Scale
		{
			get
			{
				return scale;
			}
			set
			{
				if (scale != value)
				{
					scale = Math.Round(value, 2);
					NotifyPropertyChanged();

					NotifyPropertyChanged(nameof(OffsetX));
					NotifyPropertyChanged(nameof(OffsetY));
				}
			}
		}

		public double Width { get { return Image.Width; } }
		public double Height { get { return Image.Height; } }

		private int manualOffsetX;
		public int ManualOffsetX
		{
			get { return manualOffsetX; }
			set
			{
				if (manualOffsetX != value)
				{
					manualOffsetX = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(OffsetX));
				}
			}
		}

		private int manualOffsetY;
		public int ManualOffsetY
		{
			get { return manualOffsetY; }
			set
			{
				if (manualOffsetY != value)
				{
					manualOffsetY = value;
					NotifyPropertyChanged();
					NotifyPropertyChanged(nameof(OffsetY));
				}
			}
		}

		private FramedImageViewModel FirstFrame { get; set; }

		private double ImageCenterOffset
		{
			get
			{
				return (FrameWidth - Image.Width * Scale) / 2.0;
			}
		}

		private double ImageBottomOffset
		{
			get
			{
				return FrameHeight - Image.Height * Scale;
			}
		}

		private bool disposedValue;

		public FramedImageViewModel(DefGroupItem defSource, MagickImage image, FramedImageViewModel firstFrame, double scale)
		{
			DefSource = defSource;
			Image = image;
			FirstFrame = firstFrame ?? this;

			FrameWidth = 200;
			FrameHeight = 200;

			ImageStream = Image.ToByteArray();

			Scale = scale;
		}

		internal Frame CreateFrame()
		{
			return new Frame(Image, FrameWidth, FrameHeight, OffsetX, OffsetY, Scale);
		}

		public void ResetOffsetsToDefault()
		{
			ManualOffsetX = 0;
			ManualOffsetY = 0;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					Image?.Dispose();
					Image = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				ImageStream = Resources.EmptyBitmap;
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
