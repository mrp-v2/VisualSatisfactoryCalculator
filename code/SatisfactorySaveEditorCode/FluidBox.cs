using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class FluidBox : IStructData
	{
		public float Unknown { get; set; }

		public int SerializedLength => 25;
		public string Type => "FluidBox";

		public FluidBox(BinaryReader reader)
		{
			Unknown = reader.ReadSingle();
		}
	}
}
