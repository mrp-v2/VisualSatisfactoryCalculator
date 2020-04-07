using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class TextProperty : SerializedProperty
	{
		public const string TypeName = nameof(TextProperty);
		public override string PropertyType => TypeName;
		public override int SerializedLength
		{
			get
			{
				if (Unknown5 == 255) return 5;

				var size = 9 + Unknown8.GetSerializedLength() + Value.GetSerializedLength() + FormatData.Sum(d => d.SerializedLength);
				if (Unknown5 == 3)
					size += 9;

				return size;
			}
		}

		public int Unknown4 { get; set; }
		public byte Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public byte Unknown7 { get; set; }
		public string Unknown8 { get; set; }
		public string Value { get; set; }

		public List<TextFormatData> FormatData { get; } = new List<TextFormatData>();

		public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"text";
		}

		public static TextProperty Parse(string propertyName, int index, BinaryReader reader, bool inArray = false)
		{
			var result = new TextProperty(propertyName, index);

			if (!inArray)
			{
				var unk3 = reader.ReadByte();
				Trace.Assert(unk3 == 0);
			}

			result.Unknown4 = reader.ReadInt32();

			result.Unknown5 = reader.ReadByte();
			if (result.Unknown5 == 3)
			{
				result.Unknown6 = reader.ReadInt32();
				result.Unknown7 = reader.ReadByte();
			}
			else if (result.Unknown5 == 255)
			{
				return result;
			}

			var unk5 = reader.ReadInt32();
			Trace.Assert(unk5 == 0);

			result.Unknown8 = reader.ReadLengthPrefixedString();

			result.Value = reader.ReadLengthPrefixedString();

			if (result.Unknown5 == 3)
			{
				var count = reader.ReadInt32();
				for (var i = 0; i < count; i++)
				{
					result.FormatData.Add(new TextFormatData()
					{
						Name = reader.ReadLengthPrefixedString(),
						Unknown1 = reader.ReadByte(),
						Unknown2 = reader.ReadInt32(),
						Unknown3 = reader.ReadInt32(),
						Unknown4 = reader.ReadInt32(),
						Unknown5 = reader.ReadByte(),
						Data = reader.ReadLengthPrefixedString()
					});
				}
			}

			return result;
		}
	}

	public class TextFormatData
	{
		public int SerializedLength => Name.GetSerializedLength() + 14 + Data.GetSerializedLength();

		public string Name { get; set; }
		public byte Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public byte Unknown5 { get; set; }
		public string Data { get; set; }
	}
}
