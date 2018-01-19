using System;

using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Items
{
	public class SandItem : Item
	{

		public SandItem(
			Material material,
			Single quality,
			Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_SAND;

			this.volume = 50.0F;
			this.weight = 20.0F;

			this.wooden = false;
			this.name = "sand";

		}

		public override Model getModel()
		{
			return ModelResources.sandPileModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(1.25F, 0.70F, 1.25F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(145, 1, 15, 15);
		}

	}
}
