using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Interface
{
	public class Notification
	{

		/// <summary>
		///		The text which is displayed by the notification.
		/// </summary>
		public String text;
		
		// How long the notification has been displayed for.
		private int age;

		private float opacity = 1.0F;


		public Notification(String text)
		{
			this.text = text.Trim();
		}


		public Boolean update()
		{
			opacity = 1.0F - ((age - (60.0F * 4.0F)) / 60.0F);
			if (opacity < 0.0F) opacity = 0.0F;
			if (opacity > 1.0F) opacity = 1.0F;

			age++;
			if (age > 60 * 5)
				return true;

			return false;
		}


		public void draw(
			SpriteBatch spriteBatch,
			GraphicsDeviceManager graphics)
		{

			Vector2 size = FontResources.notificationFont.MeasureString(this.text);

			FontResources.drawStringWithShadow(
				spriteBatch,
				FontResources.notificationFont,
				this.text,
				new Vector2((graphics.PreferredBackBufferWidth / 2.0F) - (size.X / 2.0F),
							(graphics.PreferredBackBufferHeight / 2.0F) + 250.0F),
				Color.White * opacity);

		}

	}
}
