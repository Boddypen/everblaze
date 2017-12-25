using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Environment.Items
{
	public class BranchItem : Item
	{
		public BranchItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.material = material;
			this.quality = quality;
			this.weight = 0.250F;
			this.damage = damage;
			this.wooden = true;
			this.name = "branch";

		}
	}
}
