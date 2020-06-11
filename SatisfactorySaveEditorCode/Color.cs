using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class Color : IStructData
	{
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
		public byte A { get; set; }

		public int SerializedLength
		{
			get
			{
				return 4;
			}
		}

		public string Type
		{
			get
			{
				return "Color";
			}
		}

		public Color(BinaryReader reader)
		{
			B = reader.ReadByte();
			G = reader.ReadByte();
			R = reader.ReadByte();
			A = reader.ReadByte();
		}
	}
}
