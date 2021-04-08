using System;

namespace SASpriteGen.ViewModel
{
	public class SpriteFrameData : ViewModel
	{
		public int FrameIndex { get; set; }

		public FramedImageViewModel FramedImage { get; set; }

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

		public Command<string> ChangeXOffset { get; set; }
		public Command<string> ChangeYOffset { get; set; }

		private bool currentPreviewFrame;
		private bool disposedValue;

		public SpriteFrameData(int frameIndex, FramedImageViewModel image)
		{
			FramedImage = image;
			FrameIndex = frameIndex;

			ChangeXOffset = new Command<string>((arg) =>
			{
				var dx = Convert.ToInt32(arg);
				FramedImage.ManualOffsetX += dx;
			});

			ChangeYOffset = new Command<string>((arg) =>
			{
				var dy = Convert.ToInt32(arg);
				FramedImage.ManualOffsetY += dy;
			});
		}

		internal void ResetOffsetsToDefault()
		{
			FramedImage.ResetOffsetsToDefault();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					FramedImage?.Dispose();
				}

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
