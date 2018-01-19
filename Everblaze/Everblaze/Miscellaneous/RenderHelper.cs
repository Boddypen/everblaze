using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Miscellaneous
{
	public class RenderHelper
	{

		/// 
		/// <summary>
		///		Sets up the view, projection, and world properties of the supplied <see cref="BasicEffect"/> object (reference).
		/// </summary>
		/// 
		/// <param name="effect">The <see cref="BasicEffect"/> to modify.</param>
		/// <param name="camera">The game camera.</param>
		/// <param name="renderScale">The amount to scale the rendered object.</param>
		/// <param name="renderPosition">The location of the rendered object.</param>
		/// 
		public static void setupEffect(
			ref BasicEffect effect,
			Camera camera,
			Single renderScale,
			Vector3 renderPosition)
		{

			// Effect View
			effect.View = Matrix.CreateLookAt(
				camera.position,
				camera.target,
				Vector3.Up);

			// Effect Projection
			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(90.0F),
				1920.0F / 1080.0F,
				0.1F,
				2000.0F);

			// Effect World
			effect.World =
				  Matrix.CreateScale(renderScale)
				* Matrix.CreateTranslation(renderPosition);

		}

	}
}
