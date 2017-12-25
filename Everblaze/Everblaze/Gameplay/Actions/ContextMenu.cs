using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Everblaze.Interface;
using Everblaze.Resources;
using Everblaze.Gameplay.Skills;
using Everblaze.Environment;
using Everblaze.Environment.Items;


namespace Everblaze.Gameplay.Actions
{
	public class ContextMenu
	{

		/// <summary>
		///		The list of possible actions in the context menu.
		/// </summary>
		public List<Action> actionList;

		/// <summary>
		///		The currently selected index of the action list.
		///		Will be -1 when no item is selected.
		/// </summary>
		public int selectedItem = -1;

		/// <summary>
		///		The title displayed on the top of the context menu.
		/// </summary>
		public String title;


		public float age = 0.0F;

		
		public ContextMenu(SkillSet skills, Point tileTarget, World world, Item item)
		{

			actionList = new List<Action>();
			
			//HACK: Temporary ShovelItem override
			world.tiles[tileTarget.X, tileTarget.Y].addActions(ref actionList, tileTarget, skills, new ShovelItem(Item.Material.Rock, 50.0f, 0.0F));

			this.title = world.tiles[tileTarget.X, tileTarget.Y].getName().ToUpper();

			actionList.Reverse();

		}


		/// 
		/// <summary>
		///		Updates the context menu, allowing it to respond to any
		///		user input such as clicking or scrolling.
		/// </summary>
		/// 
		/// <param name="m">The current state of the mouse.</param>
		/// <param name="graphics">The game's graphics device manager.</param>
		/// 
		/// <returns>
		///		The selected item of the context menu, once pressed.
		///		If nothing is selected or pressed, a -1 will be returned.
		/// </returns>
		/// 
		public virtual int update(
			MouseState m,
			GraphicsDeviceManager graphics)
		{
			
			age += (1.0F - age) / 8.0F;
			if (age > 1.0F) age = 1.0F;

			// Remember the rectangle.
			Rectangle window = this.calculateSize(FontResources.interfaceFont, graphics);

			// Start off with the default selected item, -1.
			// A negative value indicates no items are selected.
			selectedItem = -1;

			// Determine if the cursor is above any selections.
			if(m.X >= window.Left + 5 &&
				m.X < window.Right - 5 &&
				m.Y > window.Top + 5 + 30 &&
				m.Y < window.Bottom - 5)
			{

				// Determine the index of the selection.
				selectedItem = (int)Math.Floor((m.Y - (window.Top + 5 + 30)) / 30.0F);

			}

			

			if(selectedItem >= 0 && m.LeftButton.Equals(ButtonState.Pressed))
			{
				return selectedItem;
			}
			else
			{
				return -1;
			}

		}


		public virtual Rectangle calculateSize(
			SpriteFont interfaceFont,
			GraphicsDeviceManager graphics)
		{

			Rectangle size = new Rectangle();

			size.X = (graphics.PreferredBackBufferWidth / 2) + 50;

			float maxWidth = 0.0F;
			foreach(Action action in actionList)
			{
				float width = interfaceFont.MeasureString(action.getFullString()).X + 50.0F;

				if (width > maxWidth)
					maxWidth = width;
			}
			float titleWidth = interfaceFont.MeasureString(title).X + 50.0F;
			if (titleWidth > maxWidth)
				maxWidth = titleWidth;
			

			size.Width = (int)Math.Ceiling(maxWidth);

			size.Height = (actionList.Count * 30) + 30 + 10;

			size.Y = (graphics.PreferredBackBufferHeight / 2) - (size.Height / 2);

			return size;

		}


		/// 
		/// <summary>
		///		Draw the context menu to the middle of the screen.
		/// </summary>
		/// 
		public virtual void draw(
			SpriteBatch spriteBatch,
			SpriteFont interfaceFont,
			GraphicsDeviceManager graphics)
		{

			Rectangle size = this.calculateSize(interfaceFont, graphics);
			size.Location = new Point(size.X - (int)((1.0F - age) * 50.0F), size.Y);

			InterfaceHelper.drawWindowWithArrow(spriteBatch, size, age);


			for(int i = 0; i < actionList.Count; i++)
			{
				Vector2 position = new Vector2(size.X + 20.0F, size.Y + 30.0F + 13.0F + (i * 30.0F));
				
				if(selectedItem >= 0 && i == selectedItem)
					spriteBatch.Draw(
						TextureResources.whiteTexture,
						new Rectangle(size.Left + 5, size.Y + 5 + 30 + (i * 30), size.Width - 10, 30),
						null,
						Color.White,
						0.0F,
						Vector2.Zero,
						SpriteEffects.None,
						0.1F);
				
				spriteBatch.DrawString(interfaceFont, actionList[i].getFullString(), position, Color.Black * age);
			}


			// Draw the title of the context menu.
			spriteBatch.Draw(
						TextureResources.whiteTexture,
						new Rectangle(size.Left + 5, size.Y + 5, size.Width - 10, 30),
						null,
						Color.Gray,
						0.0F,
						Vector2.Zero,
						SpriteEffects.None,
						0.1F);
			FontResources.drawStringWithShadow(spriteBatch, interfaceFont, title, new Vector2(size.X + 20.0F, size.Y + 13.0F), Color.White);

		}

	}
}
