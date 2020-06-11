using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class StructProperty : SerializedProperty
	{
		public const string TypeName = nameof(StructProperty);
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
				return Data.SerializedLength;
			}
		}

		public string Type
		{
			get
			{
				return Data.Type;
			}
		}

		public int Unk1 { get; set; }
		public int Unk2 { get; set; }
		public int Unk3 { get; set; }
		public int Unk4 { get; set; }
		public byte Unk5 { get; set; }

		public IStructData Data { get; set; }

		public StructProperty(string propertyName, int index = 0) : base(propertyName, index)
		{
		}

		public override string ToString()
		{
			return $"struct {Type}";
		}

		public static int GetSerializedArrayLength(StructProperty[] properties)
		{
			int size = 4;

			StructProperty first = properties[0];

			size += first.PropertyName.GetSerializedLength();
			size += TypeName.GetSerializedLength();

			size += 8;

			size += first.Data.Type.GetSerializedLength();

			size += 17;

			size += properties.Sum(p => p.Data.SerializedLength);

			return size;
		}

		private static IStructData ParseStructData(BinaryReader reader, string type)
		{
			switch (type)
			{
				case "LinearColor":
					return new LinearColor(reader);
				case "Color":
					return new Color(reader);
				case "Rotator":
					return new Rotator(reader);
				case "Vector":
					return new Vector(reader);
				case "Box":
					return new Box(reader);
				case "Quat":
					return new Quat(reader);
				case "InventoryItem":
					return new InventoryItem(reader);
				case "RailroadTrackPosition":
					return new RailroadTrackPosition(reader);
				case "Guid":
					return new GuidStruct(reader);
				case "FluidBox":
					return new FluidBox(reader);
				/*
                case "InventoryStack":
                case "InventoryItem":
                case "PhaseCost":
                case "ItemAmount":
                case "ResearchCost":
                case "CompletedResearch":
                case "ResearchRecipeReward":
                case "ItemFoundData":
                case "RecipeAmountStruct":
                case "MessageData":
                case "SplinePointData":
                    return new DynamicStructData(reader);
                */
				default:
					return new DynamicStructData(reader, type);
					//throw new NotImplementedException($"Can't deserialize struct {type}");
			}
		}

		public static StructProperty[] ParseArray(BinaryReader reader)
		{
			int count = reader.ReadInt32();
			StructProperty[] result = new StructProperty[count];

			string name = reader.ReadLengthPrefixedString();

			string propertyType = reader.ReadLengthPrefixedString();
			Trace.Assert(propertyType == "StructProperty");

#pragma warning disable IDE0059 // Unnecessary assignment of a value
			int size = reader.ReadInt32();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			int index = reader.ReadInt32();

			string structType = reader.ReadLengthPrefixedString();


			int unk1 = reader.ReadInt32();
			//Trace.Assert(unk1 == 0);

			int unk2 = reader.ReadInt32();
			//Trace.Assert(unk2 == 0);

			int unk3 = reader.ReadInt32();
			//Trace.Assert(unk3 == 0);

			int unk4 = reader.ReadInt32();
			//Trace.Assert(unk4 == 0);

			byte unk5 = reader.ReadByte();
			Trace.Assert(unk5 == 0);

			for (int i = 0; i < count; i++)
			{
				result[i] = new StructProperty(name, index)
				{
					Unk1 = unk1,
					Unk2 = unk2,
					Unk3 = unk3,
					Unk4 = unk4,
					Unk5 = unk5,
					Data = ParseStructData(reader, structType)
				};
			}


			return result;
		}

		public static StructProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
		{
			StructProperty result = new StructProperty(propertyName, index);
			string type = reader.ReadLengthPrefixedString();

			overhead = type.Length + 22;

			result.Unk1 = reader.ReadInt32();
			Trace.Assert(result.Unk1 == 0);

			result.Unk2 = reader.ReadInt32();
			Trace.Assert(result.Unk2 == 0);

			result.Unk3 = reader.ReadInt32();
			Trace.Assert(result.Unk3 == 0);

			result.Unk4 = reader.ReadInt32();
			Trace.Assert(result.Unk4 == 0);

			result.Unk5 = reader.ReadByte();
			Trace.Assert(result.Unk5 == 0);

			long before = reader.BaseStream.Position;
			result.Data = ParseStructData(reader, type);
			long after = reader.BaseStream.Position;

			if (before + size != after)
			{
				throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
			}

			return result;
		}
	}
}
