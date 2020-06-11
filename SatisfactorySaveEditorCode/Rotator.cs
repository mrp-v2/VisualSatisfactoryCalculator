using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class Rotator : Vector
	{
		public new string Type
		{
			get
			{
				return "Rotator";
			}
		}

		public Rotator(BinaryReader reader) : base(reader)
		{
		}
	}
}
