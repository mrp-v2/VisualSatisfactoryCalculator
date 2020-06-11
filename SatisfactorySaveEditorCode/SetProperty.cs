using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class SetProperty : SerializedProperty
	{
		public const string TypeName = nameof(SetProperty);
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
				return 0;
			}
		}

		public string Type { get; set; }
		public List<SerializedProperty> Elements { get; set; } = new List<SerializedProperty>();

		public SetProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"set: ";
		}

		public static SetProperty Parse(string propertyName, int index, BinaryReader reader, out int overhead)
		{
			SetProperty result = new SetProperty(propertyName, index)
			{
				Type = reader.ReadLengthPrefixedString()
			};

			overhead = result.Type.Length + 6;

			byte unk = reader.ReadByte();
			Trace.Assert(unk == 0);

			int unk2 = reader.ReadInt32();
			Trace.Assert(unk2 == 0);

			switch (result.Type)
			{
				case NameProperty.TypeName:
				{
					int count = reader.ReadInt32();
					for (int i = 0; i < count; i++)
					{
						string value = reader.ReadLengthPrefixedString();
						result.Elements.Add(new NameProperty($"Element {i}") { Value = value });
					}
				}
				break;
				default:
					throw new NotImplementedException($"Parsing an array of {result.Type} is not yet supported.");
			}


			return result;
		}
	}
}
