using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Everblaze.Resources;


namespace Everblaze.Interface
{
	public class InterfaceHelper
	{

		public static readonly Rectangle REGION_ARROW = new Rectangle(0, 0, 16, 32);

		public static readonly Rectangle REGION_TOP_LEFT = new Rectangle(16, 0, 5, 5);
		public static readonly Rectangle REGION_TOP_RIGHT = new Rectangle(22, 0, 7, 5);
		public static readonly Rectangle REGION_BOTTOM_LEFT = new Rectangle(16, 6, 5, 7);
		public static readonly Rectangle REGION_BOTTOM_RIGHT = new Rectangle(22, 6, 7, 7);

		public static readonly Rectangle REGION_TOP = new Rectangle(21, 0, 1, 5);
		public static readonly Rectangle REGION_BOTTOM = new Rectangle(21, 6, 1, 7);
		public static readonly Rectangle REGION_LEFT = new Rectangle(16, 5, 5, 1);
		public static readonly Rectangle REGION_RIGHT = new Rectangle(22, 5, 7, 1);

		public static readonly Rectangle REGION_BAR = new Rectangle(0, 32, 1, 5);
		public static readonly Rectangle REGION_BAR_GREYSCALE = new Rectangle(1, 32, 1, 5);


		public static void drawWindow(
			SpriteBatch spriteBatch,
			Rectangle window,
			float opacity,
			float depth)
		{

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X, window.Y, 5, 5),
				REGION_TOP_LEFT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.Right - 5, window.Y, 5, 5),
				REGION_TOP_RIGHT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X, window.Bottom - 5, 5, 5),
				REGION_BOTTOM_LEFT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.Right - 5, window.Bottom - 5, 5, 5),
				REGION_BOTTOM_RIGHT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X + 5, window.Y, window.Width - 10, 5),
				REGION_TOP,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X + 5, window.Bottom - 5, window.Width - 10, 5),
				REGION_BOTTOM,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X, window.Y + 5, 5, window.Height - 10),
				REGION_LEFT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.Right - 5, window.Y + 5, 5, window.Height - 10),
				REGION_RIGHT,
				Color.White * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

			spriteBatch.Draw(
				TextureResources.whiteTexture,
				new Rectangle(window.X + 5, window.Y + 5, window.Width - 10, window.Height - 10),
				null,
				Color.LightGray * opacity,
				0.0F,
				Vector2.Zero,
				SpriteEffects.None,
				depth);

		}


		public static void drawWindowWithArrow(
			SpriteBatch spriteBatch,
			Rectangle window,
			float opacity,
			float depth)
		{

			drawWindow(spriteBatch, window, opacity, depth);

			spriteBatch.Draw(
				TextureResources.contextMenuTexture,
				new Rectangle(window.X - 16, window.Top + (window.Height / 2) - 16, 16, 32),
				REGION_ARROW,
				Color.White * opacity);

		}

	}
}
