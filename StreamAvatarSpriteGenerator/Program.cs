using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StreamAvatarSpriteGenerator
{
	
	class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");

			//var generator = new SpriteGenerator();

			//var files = Directory.EnumerateFiles(@"C:\Games\h3tools\ExtractedDefs", "*.def");
			//foreach (var file in files)
			//{
			//	var def = generator.LoadDef(file);
				
			//	StringBuilder sb = new StringBuilder();
			//	foreach (var defGroup in def.Groups)
			//	{
			//		sb.AppendLine($"{(int)defGroup.Key} - {defGroup.Value.GetGroupName(def.Type)}:");
			//		foreach (var item in defGroup.Value.Items)
			//		{
			//			sb.AppendLine($"  {item.Name}");
			//			sb.AppendLine($"    Frame Width-Length: {item.FrameWidth} x {item.FrameHeight}");
			//			sb.AppendLine($"    Frame Left: {item.FrameLeft}");
			//			sb.AppendLine($"    Frame Top: {item.FrameTop}");
						
			//		}
			//		sb.AppendLine();
			//	}

			//	var txtFile = Path.ChangeExtension(file, ".txt");
			//	File.WriteAllText(txtFile, sb.ToString());
			//}

			//var defFilePath = @"C:\tmp\spec\ab\CABEHE.def";
			//var defFile = generator.LoadDef(defFilePath);
			//var root = Path.GetDirectoryName(defFilePath);

			//generator.LoadImages(Animation.Idle, root, defFile.Groups[DefAnimation.MouseOver]);
			//var image = generator.CreateSpriteSheet(Animation.Idle);
			//image.Write(@"C:\tmp\spec\AB_test.png");

			return;

		}
	}
}
