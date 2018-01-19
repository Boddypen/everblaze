using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{
	
	public class PlankItem : Item
	{

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="PlankItem"/> class.
		/// </summary>
		/// 
		public PlankItem(
			Material material,
			float quality,
			float damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_PLANK;

			this.quality = quality;
			this.weight = 2.0F;
			this.volume = 3.0F;
			this.damage = damage;

			this.name = "plank";

			this.material = material;

			this.wooden = true;
		}


		public override Model getModel()
		{
			return ModelResources.plankModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(1.0F, 0.05F, 0.25F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(81, 1, 15, 15);
		}

	}
}
