using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Everblaze.Environment.Items
{
	public class HatchetItem : Item
	{
		public HatchetItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_HATCHET;

			this.material = material;
			this.quality = quality;
			this.weight = 1.500F;
			this.volume = 2.5F;
			this.damage = damage;
			this.wooden = false;
			this.name = "hatchet";

		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(65, 1, 15, 15);
		}
	}
}
