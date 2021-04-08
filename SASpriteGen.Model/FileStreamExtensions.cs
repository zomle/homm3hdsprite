using System.IO;
using System.Text;

namespace SASpriteGen.Model
{
	internal static class FileStreamExtensions
	{
		public static byte[] ReadBytes(this FileStream fs, int length)
		{
			var result = new byte[length];
			fs.Read(result, 0, result.Length);
			return result;
		}

		public static int ReadInt32(this FileStream fs, int offset = 0, SeekOrigin origin = SeekOrigin.Current)
		{
			if (offset != 0 || origin != SeekOrigin.Current)
			{
				fs.Seek(offset, origin);
			}
			int result = 0;
			result |= fs.ReadByte();
			result |= (fs.ReadByte() << 8);
			result |= (fs.ReadByte() << 16);
			result |= (fs.ReadByte() << 24);

			return result;
		}

		public static string ReadString(this FileStream fs, int length, Encoding encoding, int offset = 0, SeekOrigin origin = SeekOrigin.Current)
		{
			if (offset != 0 || origin != SeekOrigin.Current)
			{
				fs.Seek(offset, origin);
			}

			var data = new byte[length];
			fs.Read(data, 0, length);
			return encoding.GetString(data);
		}

		public static string ReadString(this FileStream fs, int offset = 0, SeekOrigin origin = SeekOrigin.Current)
		{
			var sb = new StringBuilder();

			if (offset != 0 || origin != SeekOrigin.Current)
			{
				fs.Seek(offset, origin);
			}

			char chr;
			do
			{
				chr = (char)fs.ReadByte();
				if (chr != 0)
				{
					sb.Append(chr);
				}
			} while (chr != '\0');
			fs.Seek(-1, SeekOrigin.Current);
			return sb.ToString();
		}
	}
}
