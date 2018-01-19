using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{
	public class SmallBarrelItem : StorageItem
	{
		public SmallBarrelItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_SMALL_BARREL;

			this.storage = new Container(40.0F, 100);

			this.volume = 60.0F;
			this.weight = 5.0F;
			this.wooden = true;
			this.name = "small barrel";

		}
		
		public override Model getModel()
		{
			return ModelResources.smallBarrelModel;
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(129, 1, 15, 15);
		}

	}
}
