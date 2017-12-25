﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Environment.Items
{
	public class ShovelItem : Item
	{
		public ShovelItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.material = material;
			this.quality = quality;
			this.weight = 1.500F;
			this.damage = damage;
			this.wooden = false;
			this.name = "shovel";

		}
	}
}