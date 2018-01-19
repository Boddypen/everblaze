using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Resources
{

	public class TextureResources
	{

		public static Texture2D
			tileSlabTexture,
			tileGrassTexture,
			tileDirtTexture,
			tileFarmlandTexture,
			tileSandTexture,
			tileWaterTexture;

		public static Texture2D
			itemsSpritesheet;

		public static Texture2D
			tileCursorTexture,
			cursorTexture;

		public static Texture2D
			cloudsTexture;

		public static Texture2D
			contextMenuTexture,
			inventoryWindowTexture,
			selectedRowTexture;

		public static Texture2D
			whiteTexture;
		


		/// 
		/// <summary>
		///		Loads all texture content into the game.
		/// </summary>
		/// 
		/// <param name="content">The game's content manager.</param>
		/// 
		public static void loadContent(ContentManager content)
		{

			// Load tile textures.
			tileSlabTexture			= content.Load<Texture2D>("png/environment/tiles/slab");
			tileGrassTexture		= content.Load<Texture2D>("png/environment/tiles/grass");
			tileDirtTexture			= content.Load<Texture2D>("png/environment/tiles/dirt");
			tileFarmlandTexture		= content.Load<Texture2D>("png/environment/tiles/farmland");
			tileSandTexture			= content.Load<Texture2D>("png/environment/tiles/sand");
			tileWaterTexture		= content.Load<Texture2D>("png/environment/tiles/water");

			itemsSpritesheet		= content.Load<Texture2D>("png/items/items");

			tileCursorTexture		= content.Load<Texture2D>("png/environment/tiles/cursor");
			cursorTexture			= content.Load<Texture2D>("png/interface/cursor");

			cloudsTexture			= content.Load<Texture2D>("png/environment/sky/clouds");

			contextMenuTexture		= content.Load<Texture2D>("png/interface/action_menu");
			inventoryWindowTexture	= content.Load<Texture2D>("png/interface/inventory");
			selectedRowTexture		= content.Load<Texture2D>("png/interface/selected_row");

			whiteTexture			= content.Load<Texture2D>("png/interface/white");

		}

	}

}
