using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

			this.material = material;
			this.quality = quality;
			this.weight = 20.0F;
			this.damage = damage;
			this.wooden = false;
			this.name = "dirt";

		}

	}
}
