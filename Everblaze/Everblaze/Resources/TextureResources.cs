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
			tileFarmlandTexture;

		public static Texture2D
			tileCursorTexture,
			cursorTexture;

		public static Texture2D
			contextMenuTexture;

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
			tileSlabTexture = content.Load<Texture2D>("png/environment/tiles/slab");
			tileGrassTexture = content.Load<Texture2D>("png/environment/tiles/grass");
			tileDirtTexture = content.Load<Texture2D>("png/environment/tiles/dirt");
			tileFarmlandTexture = content.Load<Texture2D>("png/environment/tiles/farmland");

			tileCursorTexture = content.Load<Texture2D>("png/environment/tiles/cursor");
			cursorTexture = content.Load<Texture2D>("png/interface/cursor");

			contextMenuTexture = content.Load<Texture2D>("png/interface/action_menu");

			whiteTexture = content.Load<Texture2D>("png/interface/white");

		}

	}

}
