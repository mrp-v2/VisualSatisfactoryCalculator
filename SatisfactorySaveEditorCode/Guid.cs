using System;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class GuidStruct : IStructData
	{
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
				return "Guid";
			}
		}

		public Guid Data { get; set; }

		public GuidStruct(BinaryReader reader)
		{
			Data = new Guid(reader.ReadBytes(16));
		}
	}
}
