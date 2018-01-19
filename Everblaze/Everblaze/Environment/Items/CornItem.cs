using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Items
{
	public class CornItem : Item
	{
		public CornItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_CORN;
			
			this.weight = 0.100F;
			this.volume = 0.300F;
			this.name = "corn";

		}

		public override String getDescription()
		{
			return "A yellow, leafy plant with many edible kernels.";
		}

		public override Model getModel()
		{
			return ModelResources.cornModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(1.0F, 1.0F, 1.0F);
		}
		
		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(33, 1, 15, 15);
		}
	}
}
