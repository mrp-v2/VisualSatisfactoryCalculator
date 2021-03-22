using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Plan : HashSet<Connection>
	{
		public ProcessedPlan ProcessedPlan { get; private set; }

		public HashSet<Step> GetSteps()
		{
			HashSet<Step> steps = new HashSet<Step>();
			foreach (Connection connection in this)
			{
				steps.UnionWith(connection.GetSteps());
			}
			return steps;
		}

		new public bool Add(Connection item)
		{
			bool baseAdd = base.Add(item);
			PlanChanged();
			return baseAdd;
		}

		private void PlanChanged()
		{
			ProcessedPlan = new ProcessedPlan(this);
		}

		public Plan() : base()
		{

		}

		public Dictionary<string, decimal> GetNetRates(Encodings encodings)
		{
			return GetProductRates().Subtract(GetIngredientRates(encodings));
		}

		public Dictionary<string, decimal> GetProductRates()
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
			foreach (Step step in GetSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().Products.Values)
				{
					if (rates.ContainsKey(itemCount.ItemUID))
					{
						rates[itemCount.ItemUID] += step.GetItemRate(itemCount.ItemUID, true);
					}
					else
					{
						rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, true));
					}
				}
			}
			return rates;
		}

		public decimal GetPowerDraw(Encodings encodings)
		{
			decimal powerDraw = 0;
			foreach (Step step in GetSteps())
			{
				powerDraw += step.GetPowerDraw(encodings);
			}
			return powerDraw;
		}

		public Dictionary<string, decimal> GetIngredientRates(Encodings encodings)
		{
			Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
			{
				{ Constants.MWItem.UID, GetPowerDraw(encodings) }
			};
			foreach (Step step in GetSteps())
			{
				foreach (ItemCount itemCount in step.GetRecipe().Ingredients.Values)
				{
					if (rates.ContainsKey(itemCount.ItemUID))
					{
						rates[itemCount.ItemUID] += step.GetItemRate(itemCount.ItemUID, false);
					}
					else
					{
						rates.Add(itemCount.ItemUID, step.GetItemRate(itemCount.ItemUID, false));
					}
				}
			}
			return rates;
		}

		public Dictionary<string, int> MachineCount()
		{
			Dictionary<string, int> totalMachines = new Dictionary<string, int>();
			foreach (Step step in GetSteps())
			{
				if (!totalMachines.ContainsKey(step.GetRecipe().MachineUID))
				{
					totalMachines.Add(step.GetRecipe().MachineUID, step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.GetRecipe().MachineUID] += step.CalculateMachineCount();
				}
			}
			return totalMachines;
		}

		public string GetMachinesString(Encodings encodings)
		{
			string total = "";
			Dictionary<string, int> machines = MachineCount();
			foreach (string machineUID in machines.Keys)
			{
				total += machines[machineUID] + " " + encodings[machineUID].DisplayName + "\n";
			}
			return total;
		}

		public string GetProductsString(Encodings encodings)
		{
			string str = "Net Products: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			bool first = true;
			foreach (string itemUID in netRates.Keys)
			{
				decimal rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(rate);
				if (rate > 0 && decimal.Parse(rateStr) != 0)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.DisplayName;
				}
			}
			str += "\nAll Products: ";
			Dictionary<string, decimal> rates = GetProductRates();
			first = true;
			foreach (string itemUID in rates.Keys)
			{
				IItem item = encodings[itemUID] as IItem;
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += item.ToString(rates[itemUID]) + " " + item.DisplayName;
			}
			return str;
		}

		public string GetIngredientsString(Encodings encodings)
		{
			string str = "All Ingredients: ";
			Dictionary<string, decimal> rates = GetIngredientRates(encodings);
			bool first = true;
			foreach (string itemUID in rates.Keys)
			{
				IItem item = encodings[itemUID] as IItem;
				if (first)
				{
					first = false;
				}
				else
				{
					str += ", ";
				}
				str += item.ToString(rates[itemUID]) + " " + item.DisplayName;
			}
			str += "\nNet Ingredients: ";
			Dictionary<string, decimal> netRates = GetNetRates(encodings);
			first = true;
			foreach (string itemUID in netRates.Keys)
			{
				decimal rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(-rate);
				if (rate < 0 && decimal.Parse(rateStr) != 0)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						str += ", ";
					}
					str += rateStr + " " + item.DisplayName;
				}
			}
			return str;
		}
	}
}
