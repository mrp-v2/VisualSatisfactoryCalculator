using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class Quat : IStructData
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float W { get; set; }

		public int SerializedLength
		{
			get
			{
				return 16;
			}
		}

		public string Type
		{
			get
			{
				return "Quat";
			}
		}

		public Quat(BinaryReader reader)
		{
			X = reader.ReadSingle();
			Y = reader.ReadSingle();
			Z = reader.ReadSingle();
			W = reader.ReadSingle();
		}
	}
}
