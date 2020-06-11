using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class NameProperty : SerializedProperty
	{
		public const string TypeName = nameof(NameProperty);
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

		public NameProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"name: {Value}";
		}

		public static NameProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			NameProperty result = new NameProperty(propertyName, index);

			byte unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			result.Value = reader.ReadLengthPrefixedString();

			return result;
		}
	}
}
