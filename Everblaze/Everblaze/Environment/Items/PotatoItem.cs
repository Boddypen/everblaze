using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Environment.Items
{
	public class PotatoItem : Item
	{
		public PotatoItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.material = material;
			this.quality = quality;
			this.weight = 0.400F;
			this.damage = damage;
			this.name = "potato";
			this.wooden = false;

		}
	}
}
