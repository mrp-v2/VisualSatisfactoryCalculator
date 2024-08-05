﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.model.production
{
	public class ItemRateCollection<ItemType> : Dictionary<ItemType, ItemRate<ItemType>> where ItemType : AbstractItem
	{

	}
}