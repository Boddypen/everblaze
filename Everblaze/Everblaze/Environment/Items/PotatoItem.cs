using System;

using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Items
{
	public class PotatoItem : Item
	{
		public PotatoItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_POTATO;
			
			this.weight = 0.400F;
			this.volume = 0.300F;
			this.name = "potato";

		}

		public override String getDescription()
		{
			return "A small, starchy vegetable used in cooking.";
		}

		public override Model getModel()
		{
			return ModelResources.potatoModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(0.1F, 0.1F, 0.1F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(97, 1, 15, 15);
		}
	}
}
