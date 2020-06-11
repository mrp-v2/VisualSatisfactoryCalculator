using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class FluidBox : IStructData
	{
		public float Unknown { get; set; }

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
				return "FluidBox";
			}
		}

		public FluidBox(BinaryReader reader)
		{
			Unknown = reader.ReadSingle();
		}
	}
}
