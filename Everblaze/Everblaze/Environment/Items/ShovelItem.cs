using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{
	public class ShovelItem : Item
	{
		public ShovelItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_SHOVEL;

			this.material = material;
			this.quality = quality;
			this.weight = 1.500F;
			this.volume = 3.0F;
			this.damage = damage;
			this.wooden = false;
			this.name = "shovel";

		}

		public override String getDescription()
		{
			return "A useful tool for digging and terraforming.";
		}

		public override Model getModel()
		{
			return ModelResources.shovelModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(0.15F, 0.15F, 2.5F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(113, 1, 15, 15);
		}
	}
}
