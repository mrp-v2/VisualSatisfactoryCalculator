using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class Vector : IStructData
	{
		public int SerializedLength
		{
			get
			{
				return 12;
			}
		}

		public string Type
		{
			get
			{
				return "Vector";
			}
		}

		public Vector3 Data { get; set; }

		public Vector(BinaryReader reader)
		{
			Data = new Vector3()
			{
				X = reader.ReadSingle(),
				Y = reader.ReadSingle(),
				Z = reader.ReadSingle()
			};
		}
	}
}
