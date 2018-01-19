using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{
	public class BranchItem : Item
	{
		public BranchItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.networkID = Item.NETWORK_BRANCH;

			this.weight = 0.250F;

			this.volume = 2.000F;

			this.wooden = true;
			this.name = "branch";

		}

		public override Model getModel()
		{
			return ModelResources.branchModel;
		}

		public override Vector3 getCustomScaling()
		{
			return new Vector3(0.3F, 0.3F, 1.2F);
		}

		public override Rectangle getSpriteRectangle()
		{
			return new Rectangle(17, 1, 15, 15);
		}
	}
}
