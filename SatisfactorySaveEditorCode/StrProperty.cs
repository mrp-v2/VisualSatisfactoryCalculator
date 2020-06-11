using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class StrProperty : SerializedProperty
	{
		public const string TypeName = nameof(StrProperty);
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
				return Value.GetSerializedLength();
			}
		}

		public string Value { get; set; }

		public StrProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"str: {Value}";
		}

		public static StrProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			StrProperty result = new StrProperty(propertyName, index);

			byte unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			result.Value = reader.ReadLengthPrefixedString();

			return result;
		}
	}
}
