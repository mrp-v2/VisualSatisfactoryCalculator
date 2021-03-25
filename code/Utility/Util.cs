using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Utility
{
	class Util
	{
		// example single item lists:
		// (BlueprintGeneratedClass'\"/Game/FactoryGame/Resource/RawResources/CrudeOil/Desc_LiquidOil.Desc_LiquidOil_C\"')
		// example 2 item list:
		// (/Game/FactoryGame/Resource/Parts/NuclearFuelRod/Desc_NuclearFuelRod.Desc_NuclearFuelRod_C,/Game/FactoryGame/Resource/Parts/PlutoniumFuelRods/Desc_PlutoniumFuelRod.Desc_PlutoniumFuelRod_C)
		// example 3 item list:
		// (/Game/FactoryGame/Resource/RawResources/Coal/Desc_Coal.Desc_Coal_C,/Game/FactoryGame/Resource/Parts/CompactedCoal/Desc_CompactedCoal.Desc_CompactedCoal_C,/Game/FactoryGame/Resource/Parts/PetroleumCoke/Desc_PetroleumCoke.Desc_PetroleumCoke_C)
		public static string[] ParseUIDList(string itemList)
		{
			string[] items = itemList.TrimStart('(').TrimEnd(')').Replace("'", "").Replace("\"", "").Split(',');
			for (int i = 0; i < items.Length; i++)
			{
				string item = items[i];
				items[i] = item.Substring(item.IndexOf('.') + 1);
			}
			return items;
		}

		public class CompareByCount<T, V> : IComparer<T> where T : ICollection<V>
		{
			public int Compare(T x, T y)
			{
				return x.Count - y.Count;
			}
		}
	}
}
