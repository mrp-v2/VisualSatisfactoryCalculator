﻿using System;
using System.Collections.Generic;

using VisualSatisfactoryCalculator.code.DataStorage;
using VisualSatisfactoryCalculator.code.Extensions;
using VisualSatisfactoryCalculator.code.Interfaces;
using VisualSatisfactoryCalculator.code.Numbers;
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class Plan
	{
		public readonly HashSet<Step> Steps;
		public readonly CachedValue<ProcessedPlan> ProcessedPlan;

		public Plan()
		{
			Steps = new HashSet<Step>();
			ProcessedPlan = new CachedValue<ProcessedPlan>(() => new ProcessedPlan(this));
			ProcessedPlan.AddValueCalculatedCallback((sender, args) => VerifyPlan());
		}

		private void VerifyPlan()
		{
			foreach (Connection connection in ProcessedPlan.Get().GetAllConnections())
			{
				connection.VerifyConnection();
			}
		}

		public Dictionary<string, RationalNumber> GetNetRates(Encodings encodings)
		{
			return GetProductRates().Subtract(GetIngredientRates(encodings));
		}

		public Dictionary<string, RationalNumber> GetProductRates()
		{
			Dictionary<string, RationalNumber> rates = new Dictionary<string, RationalNumber>();
			foreach (Step step in Steps)
			{
				foreach (ItemRate itemCount in step.Recipe.Products.Values)
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

		public RationalNumber GetPowerDraw(Encodings encodings)
		{
			decimal powerDraw = 0;
			foreach (Step step in Steps)
			{
				powerDraw += step.GetPowerDraw(encodings);
			}
			return powerDraw;
		}

		public Dictionary<string, RationalNumber> GetIngredientRates(Encodings encodings)
		{
			Dictionary<string, RationalNumber> rates = new Dictionary<string, RationalNumber>()
			{
				{ Constants.MWItem.ID, GetPowerDraw(encodings) }
			};
			foreach (Step step in Steps)
			{
				foreach (ItemRate itemCount in step.Recipe.Ingredients.Values)
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
			foreach (Step step in Steps)
			{
				if (!totalMachines.ContainsKey(step.Recipe.MachineUID))
				{
					totalMachines.Add(step.Recipe.MachineUID, step.CalculateMachineCount());
				}
				else
				{
					totalMachines[step.Recipe.MachineUID] += step.CalculateMachineCount();
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
			Dictionary<string, RationalNumber> netRates = GetNetRates(encodings);
			bool first = true;
			foreach (string itemUID in netRates.Keys)
			{
				RationalNumber rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(rate.ToDecimal());
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
			Dictionary<string, RationalNumber> rates = GetProductRates();
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
				str += item.ToString(rates[itemUID].ToDecimal()) + " " + item.DisplayName;
			}
			return str;
		}

		public string GetIngredientsString(Encodings encodings)
		{
			string str = "All Ingredients: ";
			Dictionary<string, RationalNumber> rates = GetIngredientRates(encodings);
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
				str += item.ToString(rates[itemUID].ToDecimal()) + " " + item.DisplayName;
			}
			str += "\nNet Ingredients: ";
			Dictionary<string, RationalNumber> netRates = GetNetRates(encodings);
			first = true;
			foreach (string itemUID in netRates.Keys)
			{
				RationalNumber rate = netRates[itemUID];
				IItem item = encodings[itemUID] as IItem;
				string rateStr = item.ToString(-rate.ToDecimal());
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
