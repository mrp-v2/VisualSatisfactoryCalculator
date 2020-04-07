using SatisfactorySaveParser.PropertyTypes.Structs;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes
{
	public class StructProperty : SerializedProperty
	{
		public const string TypeName = nameof(StructProperty);
		public override string PropertyType => TypeName;
		public override int SerializedLength => Data.SerializedLength;

		public string Type => Data.Type;
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
			var size = 4;

			var first = properties[0];

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
			var count = reader.ReadInt32();
			StructProperty[] result = new StructProperty[count];

			var name = reader.ReadLengthPrefixedString();

			var propertyType = reader.ReadLengthPrefixedString();
			Trace.Assert(propertyType == "StructProperty");

#pragma warning disable IDE0059 // Unnecessary assignment of a value
			var size = reader.ReadInt32();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
			var index = reader.ReadInt32();

			var structType = reader.ReadLengthPrefixedString();


			var unk1 = reader.ReadInt32();
			//Trace.Assert(unk1 == 0);

			var unk2 = reader.ReadInt32();
			//Trace.Assert(unk2 == 0);

			var unk3 = reader.ReadInt32();
			//Trace.Assert(unk3 == 0);

			var unk4 = reader.ReadInt32();
			//Trace.Assert(unk4 == 0);

			var unk5 = reader.ReadByte();
			Trace.Assert(unk5 == 0);

			for (var i = 0; i < count; i++)
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
			var result = new StructProperty(propertyName, index);
			var type = reader.ReadLengthPrefixedString();

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

			var before = reader.BaseStream.Position;
			result.Data = ParseStructData(reader, type);
			var after = reader.BaseStream.Position;

			if (before + size != after)
			{
				throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
			}

			return result;
		}
	}
}
