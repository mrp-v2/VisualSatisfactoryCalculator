using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class FloatProperty : SerializedProperty
	{
		public const string TypeName = nameof(FloatProperty);
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
				return 4;
			}
		}

		public float Value { get; set; }

		public FloatProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"float: {Value}";
		}

		public static FloatProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			FloatProperty result = new FloatProperty(propertyName, index);

			byte unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			result.Value = reader.ReadSingle();

			return result;
		}
	}
}
