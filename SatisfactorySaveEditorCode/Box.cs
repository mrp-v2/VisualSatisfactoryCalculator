using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class Box : IStructData
	{
		public Vector3 Min { get; set; }
		public Vector3 Max { get; set; }

		public byte UnknownByte { get; set; }

		public int SerializedLength
		{
			get
			{
				return 25;
			}
		}

		public string Type
		{
			get
			{
				return "Box";
			}
		}

		public Box(BinaryReader reader)
		{
			Min = reader.ReadVector3();
			Max = reader.ReadVector3();

			UnknownByte = reader.ReadByte();
		}
	}
}
