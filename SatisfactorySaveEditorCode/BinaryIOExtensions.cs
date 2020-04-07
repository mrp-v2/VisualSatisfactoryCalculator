using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Structures;
using System.IO;
using System.Linq;
using System.Text;

namespace SatisfactorySaveParser
{
	public static class BinaryIOExtensions
	{
		public static char[] ReadCharArray(this BinaryReader reader)
		{
			var count = reader.ReadInt32();
			if (count >= 0)
			{
				return reader.ReadChars(count);
			}
			else
			{
				var bytes = reader.ReadBytes(count * -2);
				return Encoding.Unicode.GetChars(bytes);
			}
		}

		public static string ReadLengthPrefixedString(this BinaryReader reader)
		{
			return new string(reader.ReadCharArray()).TrimEnd('\0');
		}

		public static int GetSerializedLength(this string str)
		{
			if (str == null || str.Length == 0) return 4;

			if (str.Any(c => c > 127))
			{
				return str.Length * 2 + 6;
			}

			return str.Length + 5;
		}

		public static Vector3 ReadVector3(this BinaryReader reader)
		{
			return new Vector3()
			{
				X = reader.ReadSingle(),
				Y = reader.ReadSingle(),
				Z = reader.ReadSingle()
			};
		}

		public static Vector4 ReadVector4(this BinaryReader reader)
		{
			return new Vector4()
			{
				X = reader.ReadSingle(),
				Y = reader.ReadSingle(),
				Z = reader.ReadSingle(),
				W = reader.ReadSingle()
			};
		}

		public static ChunkInfo ReadChunkInfo(this BinaryReader reader)
		{
			return new ChunkInfo()
			{
				CompressedSize = reader.ReadInt64(),
				UncompressedSize = reader.ReadInt64()
			};
		}
	}
}
