using System.IO;
using System.Text;

namespace SASpriteGen.Model.Def
{
	public class DefHandler
	{
		public DefFile LoadDef(string name, byte[] data)
		{
			using var br = new BinaryReader(new MemoryStream(data));
			return LoadDef(name, br);
		}

		public DefFile LoadDef(string filePath)
		{
			using var br = new BinaryReader(new FileStream(filePath, FileMode.Open));
			return LoadDef(Path.GetFileNameWithoutExtension(filePath), br);
		}

		private DefFile LoadDef(string name, BinaryReader reader)
		{
			var result = new DefFile(name);
			result.Type = (DefType)reader.ReadInt32();
			result.Width = reader.ReadInt32();
			result.Height = reader.ReadInt32();
			result.GroupsCount = reader.ReadInt32();
			reader.ReadBytes(768); //skip this, don't care

			for (int i = 0; i < result.GroupsCount; i++)
			{
				var defGroup = new DefGroup();
				defGroup.GroupNum = (DefAnimation)reader.ReadInt32();
				result.Groups.Add(defGroup.GroupNum, defGroup);
				defGroup.ItemsCount = reader.ReadInt32();
				reader.ReadInt32(); //skip this, don't care
				reader.ReadInt32(); //skip this, don't care

				for (int j = 0; j < defGroup.ItemsCount; j++)
				{
					var item = new DefGroupItem();
					defGroup.Items.Add(item);

					var bytes = reader.ReadBytes(13);
					int nameLength = bytes.Length;
					for (int k = 0; k < bytes.Length; k++)
					{
						if (bytes[k] == '\0')
						{
							nameLength = k;
							break;
						}
					}
					item.FileName = Encoding.ASCII.GetString(bytes, 0, nameLength);
				}

				for (int j = 0; j < defGroup.ItemsCount; j++)
				{
					var item = defGroup.Items[j];
					item.Offset = reader.ReadInt32();
				}

				var streamPosition = reader.BaseStream.Position;
				for (int j = 0; j < defGroup.ItemsCount; j++)
				{
					var item = defGroup.Items[j];
					reader.BaseStream.Seek(item.Offset, SeekOrigin.Begin);
					item.FileSize = reader.ReadInt32();
					item.Compression = reader.ReadInt32();
					item.Width = reader.ReadInt32();
					item.Height = reader.ReadInt32();
					item.FrameWidth = reader.ReadInt32();
					item.FrameHeight = reader.ReadInt32();
					item.FrameLeft = reader.ReadInt32();
					item.FrameTop = reader.ReadInt32();
				}

				reader.BaseStream.Seek(streamPosition, SeekOrigin.Begin);
			}
			return result;
		}
	}
}
