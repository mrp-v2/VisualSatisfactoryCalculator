using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class NameProperty : SerializedProperty
	{
		public const string TypeName = nameof(NameProperty);
		public override string PropertyType => TypeName;
		public override int SerializedLength => Value.GetSerializedLength();

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
			var result = new NameProperty(propertyName, index);

			var unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			result.Value = reader.ReadLengthPrefixedString();

			return result;
		}
	}
}
