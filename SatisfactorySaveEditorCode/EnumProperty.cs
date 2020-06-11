using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class EnumProperty : SerializedProperty
	{
		public const string TypeName = nameof(EnumProperty);
		public override string PropertyType
		{
			get
			{
				return TypeName;
			}
		}

		public override int SerializedLength
		{
			get
			{
				return Name.GetSerializedLength();
			}
		}

		public string Name { get; set; }
		public string Type { get; set; }

		public EnumProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}
		public override string ToString()
		{
			return $"enum: {Name}";
		}

		public static EnumProperty Parse(string propertyName, int index, BinaryReader reader, out int overhead)
		{
			EnumProperty result = new EnumProperty(propertyName, index)
			{
				Type = reader.ReadLengthPrefixedString()
			};

			overhead = result.Type.Length + 6;

			byte unk4 = reader.ReadByte();
			Trace.Assert(unk4 == 0);

			result.Name = reader.ReadLengthPrefixedString();
			//result.Value = reader.ReadInt32();

			return result;
		}
	}
}
