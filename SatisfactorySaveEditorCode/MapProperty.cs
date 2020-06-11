﻿using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class MapProperty : SerializedProperty
	{
		public const string TypeName = nameof(MapProperty);
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
				return Data.Length + 4;
			}
		}

		public string KeyType { get; set; }
		public string ValueType { get; set; }

		//public Dictionary<int, (string name, string type, ArrayProperty array)> Values { get; set; } = new Dictionary<int, (string, string, ArrayProperty)>();
		public byte[] Data { get; set; }

		public MapProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public static MapProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
		{
			MapProperty result = new MapProperty(propertyName, index)
			{
				KeyType = reader.ReadLengthPrefixedString(),
				ValueType = reader.ReadLengthPrefixedString()
			};

			overhead = result.KeyType.Length + result.ValueType.Length + 11;

			byte unk = reader.ReadByte();
			Trace.Assert(unk == 0);

			int unk1 = reader.ReadInt32();
			Trace.Assert(unk1 == 0);

			result.Data = reader.ReadBytes(size - 4);

			/*
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.ReadInt32();
                var name = reader.ReadLengthPrefixedString();
                var type = reader.ReadLengthPrefixedString();

                var arr_size = reader.ReadInt32();
                var unk4 = reader.ReadInt32();
                Trace.Assert(unk4 == 0);

                var parsed = ArrayProperty.Parse(null, reader, arr_size, out int _);

                var unk5 = reader.ReadLengthPrefixedString();
                Trace.Assert(unk5 == "None");

                result.Values[key] = (name, type, parsed);
            }
            */

			return result;
		}
	}
}
