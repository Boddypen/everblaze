using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Environment.Items
{
	public class StorageItem : Item
	{

		/// <summary>
		///		The item's internal storage container.
		/// </summary>
		public Container storage;

		/// <summary>
		///		Whether or not to show the storage window.
		/// </summary>
		public Boolean showStorage = false;


		public StorageItem(Material material, float quality, float damage) : base(material, quality, damage)
		{
			this.storage = new Container(0.0F, 0);
		}

	}
}
