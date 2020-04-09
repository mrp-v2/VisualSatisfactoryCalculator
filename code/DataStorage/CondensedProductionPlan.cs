using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;

namespace VisualSatisfactoryCalculator.code.DataStorage
{
	[Serializable]
	class CondensedProductionPlan : CondensedProductionStep
	{
		private readonly decimal multiplier;

		public ProductionPlan ToProductionPlan(List<IRecipe> recipes)
		{
			IRecipe myRecipe = GetRecipe(recipes);
			ProductionPlan plan = new ProductionPlan(myRecipe, multiplier);
			if (children != null && children.Count > 0)
			{
				foreach (CondensedProductionStep child in children)
				{
					ProductionStep childStep = child.ToProductionStep(recipes, plan);
					plan.AddChildStep(childStep);
				}
			}
			//plan.SetMultiplierAndRelated(multiplier);
			return plan;
		}

		public CondensedProductionPlan(ProductionPlan plan) : base(plan)
		{
			multiplier = plan.GetMultiplier();
		}

		public byte[] ToBytes()
		{
			byte[] originalBytes;
			using (MemoryStream ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, this);
				originalBytes = ms.ToArray();
			}
			if (originalBytes.Length > Math.Pow(2, BYTES_OF_LENGTH * 8 - 1))
			{
				throw new OverflowException("The length of the byte array is too large to represent with " + BYTES_OF_LENGTH + " bytes!");
			}
			byte[] finalBytes = new byte[BYTES_OF_LENGTH + originalBytes.Length];
			for (int i = 0; i < BYTES_OF_LENGTH; i++)
			{
				finalBytes[i] = (byte)((originalBytes.Length >> (8 * (BYTES_OF_LENGTH - i - 1))) & byteMask);
			}
			originalBytes.CopyTo(finalBytes, BYTES_OF_LENGTH);
			return finalBytes;
		}

		[NonSerialized]
		public static readonly int BYTES_OF_LENGTH = 3;
		[NonSerialized]
		public static readonly byte byteMask = 0xFF;

		public static CondensedProductionPlan FromBytes(byte[] bytes)
		{
			int originalLength = 0;
			for (int i = 0; i < BYTES_OF_LENGTH; i++)
			{
				originalLength |= bytes[i] << (8 * (BYTES_OF_LENGTH - i - 1));
			}
			if (bytes.Length != originalLength + BYTES_OF_LENGTH)
			{
				throw new ArgumentException("byte array is not the expected length!");
			}
			byte[] relevantBytes = bytes.SubArray(BYTES_OF_LENGTH, originalLength);
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream(relevantBytes))
			{
				try
				{
					return (CondensedProductionPlan)bf.Deserialize(ms);
				}
				catch (InvalidCastException e)
				{
					Console.Error.WriteLine(e.ToString());
					return default;
				} catch (SerializationException e)
				{
					Console.Error.WriteLine(e.ToString());
					return default;
				}
			}
		}

		public static int BytesToInt(byte[] bytes)
		{
			int number = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				number |= bytes[i] << (8 * (bytes.Length - i - 1));
			}
			return number;
		}
	}
}
