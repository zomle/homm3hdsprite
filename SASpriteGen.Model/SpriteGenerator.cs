using ImageMagick;
using SASpriteGen.Model.Def;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SASpriteGen.Model
{
	public class SpriteGenerator
	{
		Dictionary<Animation, AnimationData> Animations;

		public SpriteGenerator()
		{
			Animations = new Dictionary<Animation, AnimationData>();
		}

		

		public void LoadImages(Animation animation, IEnumerable<string> filePaths)
		{
			var frameList = new AnimationData();

			Animations[animation] = frameList;
		}


		public void LoadImages(Animation animation, string root, DefGroup defGroup)
		{
			var data = new AnimationData();

			Animations[animation] = data;

			int minLeft = int.MaxValue;
			int minTop = int.MaxValue;

			foreach (var item in defGroup.Items)
			{
				var frameData = new FrameData();
				var frameList = data.Frames;
				frameData.NewImage = new MagickImage(Path.Combine(root, Path.GetFileNameWithoutExtension(item.FileName) + ".png"));
				frameData.OldWidth = item.Width;
				frameData.OldHeight = item.Height;
				frameData.OldFrameWidth = item.FrameWidth;
				frameData.OldFrameHeight = item.FrameHeight;
				frameData.OldFrameLeft = item.FrameLeft;
				frameData.OldFrameTop = item.FrameTop;

				minLeft = Math.Min(minLeft, item.FrameLeft);
				minTop = Math.Min(minTop, item.FrameTop);

				frameList.Add(frameData);
			}

			foreach (var frame in data.Frames)
			{
				double widthRatio = (double)frame.NewImage.Width / frame.OldFrameWidth;
				double heightRatio = (double)frame.NewImage.Height / frame.OldFrameHeight;
				var xOffset = (int)((frame.OldFrameLeft - minLeft) * widthRatio);
				var yOffset = (int)((frame.OldFrameTop - minTop) * heightRatio);

				data.MaxWidth = Math.Max(frame.NewImage.Width + xOffset, data.MaxWidth);
				data.MaxHeight = Math.Max(frame.NewImage.Height + yOffset, data.MaxHeight);

				frame.AdjustedXOffset = xOffset;
				frame.AdjustedYOffset = yOffset;
			}

		}

		public (int, int) ReadSizes()
		{
			int minWidth = int.MaxValue;
			int minHeight = int.MaxValue;

			int maxWidth = int.MinValue;
			int maxHeight = int.MinValue;

			foreach (var animation in Animations)
			{
				foreach (var frame in animation.Value.Frames)
				{
					minWidth = frame.NewImage.Width < minWidth ? frame.NewImage.Width : minWidth;
					maxWidth = frame.NewImage.Width > maxWidth ? frame.NewImage.Width : maxWidth;

					minHeight = frame.NewImage.Height < minHeight ? frame.NewImage.Height : minHeight;
					maxHeight = frame.NewImage.Height > maxHeight ? frame.NewImage.Height : maxHeight;
				}
			}

			return (maxWidth, maxHeight);
		}

		public (int, int) ReadMaxSizes(Animation animation)
		{
			int minWidth = int.MaxValue;
			int minHeight = int.MaxValue;

			int maxWidth = int.MinValue;
			int maxHeight = int.MinValue;

			foreach (var frame in Animations[animation].Frames)
			{
				minWidth = frame.NewImage.Width < minWidth ? frame.NewImage.Width : minWidth;
				maxWidth = frame.NewImage.Width > maxWidth ? frame.NewImage.Width : maxWidth;

				minHeight = frame.NewImage.Height < minHeight ? frame.NewImage.Height : minHeight;
				maxHeight = frame.NewImage.Height > maxHeight ? frame.NewImage.Height : maxHeight;
			}
			return (maxWidth, maxHeight);
		}

		public MagickImage CreateSpriteSheet(Animation animation)
		{
			var animationData = Animations[animation];
			int frameCount = animationData.Frames.Count;

			var result = new MagickImage(MagickColors.Transparent, frameCount * animationData.MaxWidth, animationData.MaxHeight);

			for (int i = 0; i < frameCount; ++i)
			{
				var frame = animationData.Frames[i];
				result.Composite(frame.NewImage, animationData.MaxWidth * i + frame.AdjustedXOffset, frame.AdjustedYOffset, CompositeOperator.Copy);
			}

			Console.WriteLine($"Frame count: {frameCount}; Frame width: {animationData.MaxWidth}; Frame height: {animationData.MaxHeight}");
			return result;
		}
	}
}
