using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			this.quality = quality;
			this.weight = 2.0F;
			this.damage = damage;

			this.name = "plank";

			this.material = material;

			this.wooden = true;
		}
		
	}
}
