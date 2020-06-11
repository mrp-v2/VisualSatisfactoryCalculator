using System;
using System.Diagnostics;
using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class ObjectProperty : SerializedProperty, IObjectReference
	{
		public const string TypeName = nameof(ObjectProperty);
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
				return LevelName.GetSerializedLength() + PathName.GetSerializedLength();
			}
		}

		public string LevelName { get; set; }
		public string PathName { get; set; }
		public SaveObject ReferencedObject
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public ObjectProperty(string propertyName, string root = null, string name = null, int index = 0) : base(propertyName, index)
		{
			LevelName = root;
			PathName = name;
		}

		public ObjectProperty(string propertyName, int index) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"obj: {PathName}";
		}

		public static ObjectProperty Parse(string propertyName, int index, BinaryReader reader)
		{
			ObjectProperty result = new ObjectProperty(propertyName, index);

			byte unk3 = reader.ReadByte();
			Trace.Assert(unk3 == 0);

			result.LevelName = reader.ReadLengthPrefixedString();
			result.PathName = reader.ReadLengthPrefixedString();

			return result;
		}
	}
}
