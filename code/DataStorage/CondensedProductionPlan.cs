using System;
using System.Collections.Generic;
using System.IO;
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
					IItem shared = myRecipe.GetItemCounts().ToItems().FindMatch(child.GetRecipe(recipes).GetItemCounts().ToItems());
					decimal rate = myRecipe.GetRateOf(shared) * -1;
					plan.AddRelatedStep(child.ToProductionStep(recipes, rate, shared));
				}
			}
			return plan;
		}

		public CondensedProductionPlan(ProductionPlan plan) : base(plan, null)
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

		public static readonly int BYTES_OF_LENGTH = 3;
		public static readonly byte byteMask = 0xFF;

		public static CondensedProductionPlan FromBytes(byte[] bytes)
		{
			int originalLength = 0;
			for (int i = 0; i < BYTES_OF_LENGTH; i++)
			{
				originalLength |= bytes[i] << (8 * (BYTES_OF_LENGTH - i - 1));
			}
			byte[] relevantBytes = new byte[originalLength];
			for (int i = 0; i < originalLength; i++)
			{
				relevantBytes[i] = bytes[i + BYTES_OF_LENGTH];
			}
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				ms.Write(relevantBytes, 0, relevantBytes.Length);
				try
				{
					return (CondensedProductionPlan)bf.Deserialize(ms);
				}
				catch (InvalidCastException e)
				{
					Console.Error.WriteLine(e.ToString());
					return default;
				}
			}
		}
	}
}
