using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{
	public class DirtItem : Item
	{

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="DirtItem"/> class.
		/// </summary>
		/// 
		public DirtItem(
			Material material,
			Single quality,
			Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_DIRT;

			this.volume = 50.0F;

			this.material = material;
			this.quality = quality;
			this.weight = 20.0F;
			this.damage = damage;
			this.wooden = false;
			this.name = "dirt";

		}

		public override Model getModel()
		{
			return ModelResources.dirtPileModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(1.25F, 0.70F, 1.25F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(49, 1, 15, 15);
		}

	}
}
