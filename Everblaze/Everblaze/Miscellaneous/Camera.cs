using System;

using Microsoft.Xna.Framework;


namespace Everblaze.Miscellaneous
{

	/// 
	/// <summary>
	///		An object representing a game camera.
	/// </summary>
	/// 
	public class Camera
	{
		
		public Vector3 position;
		public Vector3 target;

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Camera"/> class.
		/// </summary>
		/// 
		/// <param name="startingPosition">The starting position of the camera.</param>
		/// <param name="startingTarget">The starting target which the camera will be facing.</param>
		/// 
		public Camera(Vector3 startingPosition, Vector3 startingTarget)
		{
			this.position = startingPosition;
			this.target = startingTarget;
		}


		/// 
		/// <summary>
		///		Moves the camera forward in the direction of its target.
		/// </summary>
		/// 
		/// <param name="distance">The distance to move the camera by.</param>
		/// 
		public void moveForward(float distance)
		{

			Vector3 direction = this.target - this.position;
			direction.Normalize();
			direction *= distance;

			this.position += direction;
			this.target += direction;

		}

	}
}
