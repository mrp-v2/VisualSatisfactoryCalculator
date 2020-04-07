using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class BoolProperty : SerializedProperty
	{
		public const string TypeName = nameof(BoolProperty);
		public override string PropertyType => TypeName;
		public override int SerializedLength => 0;

		public bool Value { get; set; }

		public BoolProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"bool: {Value}";
		}

		public static BoolProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			var result = new BoolProperty(propertyName, index)
			{
				Value = reader.ReadByte() > 0
			};

			Trace.Assert(reader.ReadByte() == 0);

			return result;
		}
	}
}
