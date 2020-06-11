using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class IntProperty : SerializedProperty
	{
		public const string TypeName = nameof(IntProperty);
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

		public int Value { get; set; }

		public IntProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"int: {Value}";
		}

		public static IntProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			byte unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			return new IntProperty(propertyName, index)
			{
				Value = reader.ReadInt32()
			};
		}
	}
}
