using System;
using System.Collections.Generic;


namespace Everblaze.Environment.Items
{

	/// 
	/// <summary>
	///		A class representing a single item in the game world.
	/// </summary>
	/// 
	public class Item
	{

		public enum Material
		{
			OakWood,
			Rock,
			None
		}


		/// <summary>
		///		The damage the item has sustained, representing wear on a
		///		tool, or decay on a piece of food.
		///		Clamped between 0.0F and 100.0F.
		/// </summary>
		public float damage;

		/// <summary>
		///		The quality of the item.
		///		Clamped between 1.0F and 100.0F.
		/// </summary>
		public float quality;

		/// <summary>
		///		The weight of the item.
		///		Unit: <c>kg</c>
		/// </summary>
		public float weight;

		/// <summary>
		///		The name of the item.
		/// </summary>
		public String name;


		/// <summary>
		///		Whether or not this item is wooden.
		/// </summary>
		public Boolean wooden;


		/// <summary>
		///		The material of which this item comprises of.
		/// </summary>
		public Material material;



		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Item"/> class.
		/// </summary>
		/// 
		/// <param name="material">
		///		The material of which the item is made of.
		///		Can be <see cref="Material.None"/> to indicate no material.
		///	</param>
		/// 
		public Item(Material material, float quality, float damage)
		{

			// Assign default values.
			this.damage = damage;
			this.quality = quality;
			this.weight = 1.0F;

			this.wooden = false;

			this.material = material;

			this.name = "unknown";

		}


		/// 
		/// <summary>
		///		Generates a random item which can be obtained
		///		through <see cref="Gameplay.Actions.Action.Operation.Forage"/>.
		/// </summary>
		/// 
		/// <returns>
		///		A randomised <see cref="Item"/> which is obtainable through foraging.
		/// </returns>
		/// 
		public static Item generateForagedItem(Random random)
		{

			// Decide on a quality for the foraged item.
			float quality = 1.0F + (float)(random.NextDouble() * 89.0F);


			List<Item> foragables = new List<Item>();
			foragables.Add(new PotatoItem(Material.None, quality, 0.0F));
			foragables.Add(new BranchItem(Material.None, quality, 0.0F));
			foragables.Add(new CornItem(Material.None, quality, 0.0F));


			int i = random.Next(foragables.Count);
			return foragables[i];

		}


		/// 
		/// <summary>
		///		Gets the name of the material of which this item is comprised of.
		/// </summary>
		/// 
		public String getMaterialName()
		{
			switch(this.material)
			{

				case Material.OakWood:
					return "oak";

				case Material.Rock:
					return "rock";

				case Material.None:
					return null;

				default:
					return null;

			}
		}


		/// 
		/// <summary>
		///		Gets the name of the item.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="String"/> representing the name of the item.
		/// </returns>
		/// 
		public virtual String getName()
		{
			// Get the base name of the item.
			String name = this.name;

			// If there is a modifier, add it to the end of the name.
			if(this.getMaterialName() != null)
				name = name + ", " + this.getMaterialName();

			// Return it.
			return name;
		}

	}

}
