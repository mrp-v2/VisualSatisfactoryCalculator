using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class ByteProperty : SerializedProperty
	{
		public const string TypeName = nameof(ByteProperty);
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
				return Type == "None" ? 1 : Value.GetSerializedLength();
			}
		}

		public string Type { get; set; }
		public string Value { get; set; }

		public ByteProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"byte";
		}

		public static ByteProperty Parse(string propertyName, int index, BinaryReader reader, out int overhead)
		{
			ByteProperty result = new ByteProperty(propertyName, index)
			{
				Type = reader.ReadLengthPrefixedString()
			};

			byte unk = reader.ReadByte();
			Trace.Assert(unk == 0);

			if (result.Type == "None")
			{
				result.Value = reader.ReadByte().ToString();
			}
			else
			{
				result.Value = reader.ReadLengthPrefixedString();
			}

			overhead = result.Type.Length + 6;

			return result;
		}
	}
}
