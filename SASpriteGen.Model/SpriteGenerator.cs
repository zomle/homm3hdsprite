using ImageMagick;
using System;
using System.Text;

namespace SASpriteGen.Model
{
	public class SpriteGenerator
	{
		public MagickImage CreateSpriteSheet(SpriteSheet spriteSheet)
		{
			var result = new MagickImage(MagickColors.Transparent, spriteSheet.TotalWidth, spriteSheet.TotalHeight);

			var currentYOffset = 0;
			
			foreach (var animation in spriteSheet.Animations)
			{
				var currentXOffset = 0;
				foreach (var frame in animation.Frames)
				{
					using (var tmpImage = frame.SourceImage.Clone())
					{
						tmpImage.Resize(new Percentage(frame.Scale * 100));
						int imgOffsetX = frame.OffsetX < 0 ? -frame.OffsetX : 0;
						int imgOffsetY = frame.OffsetY < 0 ? -frame.OffsetY : 0;

						tmpImage.Crop(new MagickGeometry(imgOffsetX, imgOffsetY, frame.FrameWidth, frame.FrameHeight));
						tmpImage.RePage();

						result.Composite(tmpImage, currentXOffset + Math.Max(0, frame.OffsetX), currentYOffset + Math.Max(0, frame.OffsetY), CompositeOperator.Copy);
					}
					currentXOffset += frame.FrameWidth;
				}
				currentYOffset += animation.TotalHeight;
			}

			return result;
		}

		public string CreateSpriteSheetInfo(SpriteSheet spriteSheet)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"Source creature id: {spriteSheet.SourceId}");
			sb.AppendLine($"Source graphics scaling: {Math.Round(spriteSheet.Animations[0].Frames[0].Scale, 2)}");
			sb.AppendLine($"Sprite sheet size: width: {spriteSheet.TotalWidth} x height: {spriteSheet.TotalHeight}");
			sb.AppendLine($"Individual frame size: width: {spriteSheet.Animations[0].Frames[0].FrameWidth} x height: {spriteSheet.Animations[0].Frames[0].FrameHeight}");
			sb.AppendLine();
			foreach (var animation in spriteSheet.Animations)
			{
				sb.AppendLine($"Animation {animation.Name} (source: {animation.Source}):");
				for (int i = 0; i < animation.Frames.Count; i++)
				{
					Frame frame = animation.Frames[i];
					sb.AppendLine($"  Frame {i + 1}: Offset X: {frame.OffsetX}; Offset Y: {frame.OffsetY}");
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
