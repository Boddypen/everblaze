using System;
using System.Collections.Generic;
using System.Linq;

using Everblaze.Interface;
using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Items
{

	/// 
	/// <summary>
	///		An object which can store a fixed number of items.
	/// </summary>
	/// 
	public class Container
	{

		/// <summary>
		///		The list of items which are currently stored in this container.
		/// </summary>
		public List<Item> items;

		/// <summary>
		///		The maxmimum number of items (including items of the same type) which
		///		can be stored in this container.
		/// </summary>
		public int capacity = 0;
		
		/// <summary>
		///		The <see cref="Rectangle"/> object which represents the container
		///		window's position and size. This can be modified to move and
		///		resize the window.
		/// </summary>
		public Rectangle rectangle;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Container"/> class.
		/// </summary>
		/// 
		/// <param name="capacity">The maximum number of items the container can hold.</param>
		/// 
		public Container(int capacity)
		{
			// Set properties.
			this.items = new List<Item>();
			this.capacity = capacity;


			// Set up a default window position/size.
			this.rectangle = new Rectangle(200, 200, 200, 350);

		}


		/// 
		/// <summary>
		///		Stores an item in the container.
		/// </summary>
		/// 
		/// <param name="item">The item to be stored.</param>
		/// 
		/// <returns>
		///		Whether or not the storage was successful.
		/// </returns>
		/// 
		public Boolean store(Item item)
		{
			if(items.Count >= capacity)
				return false;
			
			items.Add(item);
			return true;
		}


		/// 
		/// <summary>
		///		Gets the total number of items in the container.
		/// </summary>
		/// 
		/// <returns>
		///		An <see cref="int"/> representing the amount of
		///		items currently in the container.
		/// </returns>
		/// 
		public int getTotalItems()
		{
			return items.Count;
		}


		/// 
		/// <summary>
		///		Draws a window representing the container, showing all of its
		///		contents and the currently selected item.
		/// </summary>
		/// 
		public void draw(
			SpriteBatch spriteBatch)
		{

			// Draw the window.
			InterfaceHelper.drawWindow(
				spriteBatch,
				this.rectangle,
				0.80F);


			var q = from x in items
					group x by x into g
					let count = g.Count()
					orderby count descending
					select new {Value = g.Key, Count = count};

			// Draw the items within the window.
			int i = 0;
			foreach(var x in q)
			{

				// Get the name of the item.
				String itemName = x.Value.getName().Trim();

				// Add a count to the name if there is more than 1 item.
				if (x.Count > 1)
					itemName = itemName + " (x" + x.Count + ")";

				// Get the size of the string to be drawn.
				Vector2 itemStringSize = FontResources.interfaceFont.MeasureString(itemName);

				// Finally, draw the item.
				spriteBatch.DrawString(
					FontResources.interfaceFont,
					itemName,
					new Vector2(this.rectangle.Left + 15.0F,
								this.rectangle.Top + 15.0F + (i * 30.0F)),
					Color.Black);

				i++;

			}

		}

	}
}
