using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Environment.Items
{
	public class CornItem : Item
	{
		public CornItem(Material material, Single quality, Single damage) : base(material, quality, damage)
		{

			this.material = material;
			this.quality = quality;
			this.weight = 0.100F;
			this.damage = damage;
			this.name = "corn";
			this.wooden = false;

		}
	}
}
