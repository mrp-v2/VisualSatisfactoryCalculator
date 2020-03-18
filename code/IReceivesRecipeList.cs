﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code
{
	public interface IReceivesRecipeList
	{
		void SendRecipeList(List<Recipe> recipes, string purpose = null);
	}
}
