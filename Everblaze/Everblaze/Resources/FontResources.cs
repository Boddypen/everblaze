using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Resources
{

	/// 
	/// <summary>
	///		A class which contains all the resources regarding spritefonts for the game.
	/// </summary>
	/// 
	public class FontResources
	{

		// Fonts
		public static SpriteFont interfaceFont;
		public static SpriteFont notificationFont;


		/// 
		/// <summary>
		///		Loads all font resources.
		/// </summary>
		/// 
		/// <param name="content">The game's content manager.</param>
		/// 
		public static void loadContent(ContentManager content)
		{

			// Load fonts.
			interfaceFont = content.Load<SpriteFont>("spritefont/interface");
			notificationFont = content.Load<SpriteFont>("spritefont/notification");
			
		}


		/// 
		/// <summary>
		///		Draws a string onto the screen with a drop shadow.
		/// </summary>
		/// 
		/// <param name="spriteBatch">The game's sprite batch.</param>
		/// <param name="spriteFont">The sprite font to use.</param>
		/// <param name="text">The string to draw.</param>
		/// <param name="position">The location of the string.</param>
		/// <param name="color">The color to use.</param>
		/// 
		public static void drawStringWithShadow(
			SpriteBatch spriteBatch,
			SpriteFont spriteFont,
			String text,
			Vector2 position,
			Color color)
		{

			// Calculate the position of the drop shadow.
			Vector2 shadowDisplacement = position + new Vector2(2.0F, 2.0F);

			// Draw the black shadow.
			spriteBatch.DrawString(spriteFont, text, shadowDisplacement, Color.Black * color.ToVector4().W, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.01F);

			// Draw the coloured string.
			spriteBatch.DrawString(spriteFont, text, position, color, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.00F);

		}

	}

}
