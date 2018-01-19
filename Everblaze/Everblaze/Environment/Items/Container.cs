using System;
using System.Collections.Generic;
using System.Linq;
using Everblaze.Gameplay.Actions;
using Everblaze.Interface;
using Everblaze.Miscellaneous;
using Everblaze.Resources;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Everblaze.Environment.Items
{
	
	/// 
	/// <summary>
	///		An object which can store a fixed number of items.
	/// </summary>
	/// 
	public class Container
	{

		public enum WindowStyle
		{
			Inventory,
			ItemDisplay
		}


		/// <summary>
		///		The list of items which are currently stored in this container.
		/// </summary>
		public List<Item> items;


		/// <summary>
		///		The maxmimum volume of items (including items of the same type) which
		///		can be stored in this container.
		/// </summary>
		public float capacity = 0;

		/// <summary>
		///		The maximum number of items which can be stored in this container.
		/// </summary>
		public int maximumItems = 0;
		
		/// <summary>
		///		How many rows down the container has been scrolled.
		/// </summary>
		public int scroll = 0;
		private int lastScrollStep = 0;		

		public int hoverIndex = -1;
		public int selectIndex = -1;

		public float slideOffset = 650.0F;
		

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Container"/> class.
		/// </summary>
		/// 
		/// <param name="capacity">The maximum volume of items the container can hold.</param>
		/// 
		public Container(float capacity, int maximumItems)
		{
			// Set properties.
			this.items = new List<Item>();
			this.capacity = capacity;
			this.maximumItems = maximumItems;
			
		}


		/// 
		/// <summary>
		///		Gets the currently selected item in the container.
		/// </summary>
		/// 
		/// <returns>
		///		An <see cref="Item"/> which may be <c>null</c> if there is no item selected.
		/// </returns>
		/// 
		public Item getSelectedItem()
		{
			if(selectIndex >= 0)
			{
				if(items[selectIndex] != null)
				{
					return items[selectIndex];
				}
			}

			return null;
		}


		/// 
		/// <summary>
		///		Stores an item in the container.
		/// </summary>
		/// 
		/// <param name="item">The <see cref="Item"/> to be stored.</param>
		/// 
		/// <returns>
		///		A <see cref="Boolean"/> indicating whether or not the storage was successful.
		/// </returns>
		/// 
		public Boolean store(Item item)
		{
			// If there are too many items
			if (items.Count >= maximumItems) return false;

			// If there is too much volume of items
			float totalVolume = 0.0F;
			foreach (Item i in items)
				totalVolume += i.volume;

			if(totalVolume + item.volume > capacity)
				return false;
			
			// Otherwise, add the item.
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


		public void update(
			WindowStyle windowStyle,
			GraphicsDeviceManager graphics,
			KeyboardState k,
			MouseState m,
			ref Boolean canSelectItem,
			World world,
			NetClient client,
			ref ContextMenu contextMenu)
		{

			// Determine the window's rectangle.
			Rectangle rectangle = Container.getWindowRectangle(windowStyle, graphics);

			// Determine if the mouse cursor is within the item selection window.
			switch (windowStyle)
			{
				case WindowStyle.Inventory:
					
					if(    m.X >= rectangle.Left + 32
						&& m.Y >= rectangle.Top + 416
						&& m.X < rectangle.Right - 16
						&& m.Y < rectangle.Bottom - 31
						&& this.items.Count >= 1)
					{

						// Allow for scrolling.
						if (m.ScrollWheelValue > lastScrollStep) scroll++;
						if (m.ScrollWheelValue < lastScrollStep) scroll--;

						if(items.Count <= 8)
						{
							scroll = 0;
						}
						else
						{
							if (scroll < 0) scroll = 0;
							if (scroll > items.Count - 8) scroll = items.Count - 8;
						}
						

						int verticalPostiion = m.Y - rectangle.Top - 416;
						
						hoverIndex = (int)(verticalPostiion / 44.0F);

						// Make sure the player can't select beyond the amount of items in the container.
						if (hoverIndex >= this.items.Count)
							hoverIndex = -1;

					}
					else
					{
						hoverIndex = -1;
					}

					break;
			}
			

			// Make sure the selected item remains correct.
			if(selectIndex < 0 || selectIndex >= items.Count || items[selectIndex] == null)
			{
				selectIndex = -1;
			}


			// Allow the selection of items.
			if(hoverIndex >= 0)
			{
				if(canSelectItem)
				{
					if(m.RightButton.Equals(ButtonState.Pressed)
						&& k.IsKeyUp(Keys.LeftShift))
					{
						// Open the context menu, based on the selected item.

						canSelectItem = false;

						Item selectedItem = this.items[hoverIndex];

						contextMenu = new ContextMenu(
							world.player.skills,
							ref selectedItem,
							world,
							world.player.inventory.getSelectedItem(),
							new Point(m.X, m.Y));

					}


					if(m.LeftButton.Equals(ButtonState.Pressed)
						&& k.IsKeyDown(Keys.LeftShift))
					{
						canSelectItem = false;

						Item itemToDrop = this.items[hoverIndex];
						itemToDrop.position = new Vector2(world.player.position.X, world.player.position.Z);
						NetworkHelper.addNewItem(itemToDrop, client);

						Console.WriteLine("Dropping a " + itemToDrop.ToString());

						this.items.RemoveAt(hoverIndex);

						if (hoverIndex == selectIndex)
							selectIndex = -1;

						hoverIndex = -1;
					}


					if (m.LeftButton.Equals(ButtonState.Pressed)
						&& k.IsKeyUp(Keys.LeftShift)
						&& contextMenu == null)
					{
						canSelectItem = false;

						if (hoverIndex == selectIndex)
						{
							selectIndex = -1;
						}
						else
						{
							selectIndex = hoverIndex;
						}
					}
				}
			}

			
			lastScrollStep = m.ScrollWheelValue;

		}


		/// 
		/// <summary>
		///		Gets the correct window position and size to use for
		///		the specified type of container window.
		/// </summary>
		/// 
		/// <param name="windowStyle">The type of window to be drawn.</param>
		/// <param name="graphics">The game's graphics device manager.</param>
		/// 
		/// <returns>A <see cref="Rectangle"/>.</returns>
		/// 
		private static Rectangle getWindowRectangle(
			WindowStyle windowStyle,
			GraphicsDeviceManager graphics)
		{
			switch(windowStyle)
			{
				case WindowStyle.Inventory:
					return new Rectangle(
						graphics.PreferredBackBufferWidth - 650,
						(graphics.PreferredBackBufferHeight / 2) - (800 / 2),
						650,
						800);

				case WindowStyle.ItemDisplay:
					return new Rectangle(
						5,
						(graphics.PreferredBackBufferHeight / 2) - (800 / 2),
						500,
						800);

				default:
					return new Rectangle();
			}
		}


		/// 
		/// <summary>
		///		Draws a container window onto the screen.
		/// </summary>
		/// 
		/// <param name="windowStyle">The type of window (inventory screen, item display, etc.)</param>
		/// <param name="container">The container to draw.</param>
		/// <param name="graphics">The game's graphics device manager.</param>
		/// <param name="spriteBatch">The game's spritebatch object.</param>
		/// <param name="windowTitle">The title to be drawn at the top of the window.</param>
		/// 
		public static void draw(
			WindowStyle windowStyle,
			ref Container container,
			GraphicsDeviceManager graphics,
			SpriteBatch spriteBatch,
			String windowTitle)
		{
			
			// Determine the window location and size.
			Rectangle windowRectangle = Container.getWindowRectangle(windowStyle, graphics);

			windowRectangle.X += (int)container.slideOffset;

			// Draw the window.
			switch(windowStyle)
			{
				case WindowStyle.Inventory:

					spriteBatch.Draw(
						TextureResources.inventoryWindowTexture,
						windowRectangle,
						null,
						Color.White,
						0.0F,
						new Vector2(0.0F, 0.0F),
						SpriteEffects.None,
						0.5F);

					break;

				default:
					return;
			}


			// Draw the contained items.
			switch(windowStyle)
			{
				case WindowStyle.Inventory:

					for(int i = container.scroll; i < container.scroll + 8; i++)
					{

						// Break out of the loop if there are no more items to draw.
						if (i >= container.items.Count) break;

						// Draw the selection background, if the item is selected.
						if(container.hoverIndex >= 0)
						{
							if(container.hoverIndex == i)
							{
								spriteBatch.Draw(
									TextureResources.selectedRowTexture,
									new Rectangle(windowRectangle.Left + 32,
												  windowRectangle.Top + 417 + (i * 44) - (container.scroll * 44),
												  602,
												  43),
									null,
									Color.White,
									0.0F,
									new Vector2(0.0F, 0.0F),
									SpriteEffects.None,
									0.4F);
							}
						}

						// Draw the item...
						drawItemLine(
							spriteBatch,
							container.items[i],
							new Rectangle(
								windowRectangle.Left + 33,
								windowRectangle.Top + 417 + (i * 44) - (container.scroll * 44),
								600,
								43),
							i == container.selectIndex);
					}

					break;
			}

		}


		private static void drawItemLine(
			SpriteBatch spriteBatch,
			Item item,
			Rectangle rectangle,
			Boolean isSelectedItem)
		{

			// Item Icon
			spriteBatch.Draw(
				TextureResources.itemsSpritesheet,
				new Rectangle(
					rectangle.X + 7,
					rectangle.Y + 7,
					30, 30),
				item.getSpriteRectangle(),
				Color.White);


			// Item Name
			String itemName = item.getName().Substring(0, 1).ToUpper() + item.getName().Substring(1);

			if (isSelectedItem)
			{
				itemName = "> " + itemName;
			}

			spriteBatch.DrawString(
				FontResources.interfaceFont,
				itemName,
				new Vector2(rectangle.X + 45, rectangle.Y + 14),
				Color.Black,
				0.0F,
				Vector2.Zero,
				1.0F,
				SpriteEffects.None,
				0.3F);


			// Item Quality
			String itemQuality = item.quality.ToString("0.00");
			spriteBatch.DrawString(
				FontResources.interfaceFont,
				itemQuality,
				new Vector2(rectangle.Right - 270, rectangle.Y + 14),
				Color.Black,
				0.0F,
				Vector2.Zero,
				1.0F,
				SpriteEffects.None,
				0.3F);


			// Item Damage
			String itemDamage = item.damage.ToString("0.00");
			spriteBatch.DrawString(
				FontResources.interfaceFont,
				itemDamage,
				new Vector2(rectangle.Right - 170, rectangle.Y + 14),
				Color.Black,
				0.0F,
				Vector2.Zero,
				1.0F,
				SpriteEffects.None,
				0.3F); //TODO: Different colours depending on damage level.


			// Item Weight
			String itemWeight = item.weight.ToString("0.00");
			spriteBatch.DrawString(
				FontResources.interfaceFont,
				itemWeight,
				new Vector2(rectangle.Right - 70, rectangle.Y + 14),
				Color.Black,
				0.0F,
				Vector2.Zero,
				1.0F,
				SpriteEffects.None,
				0.3F);
			
		}

	}
}
