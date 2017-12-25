using System;
using System.Collections.Generic;
using Everblaze.Environment.Items;
using Everblaze.Gameplay.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Everblaze.Environment.Entities
{

	/// 
	/// <summary>
	///		A player.
	/// </summary>
	/// 
	/// <seealso cref="Everblaze.Environment.Entities.Entity" />
	/// 
	public class Player : Entity
	{

		/// <summary>
		///		The skillset of the player.
		/// </summary>
		public SkillSet skills;


		/// <summary>
		///		The list of items the player is holding.
		/// </summary>
		public Container inventory;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Player"/> class.
		/// </summary>
		/// 
		public Player()
		{

			this.height = 1.73F;

			this.skills = new SkillSet(1.0F);

			this.inventory = new Container(64);

		}


		/// 
		/// <summary>
		///		Performs an update tick on the entity.
		/// </summary>
		/// 
		public void update(
			Random random,
			World world,
			KeyboardState k,
			Vector2 lookMovement)
		{

			#region Player Movement

			// Update the player's movement.
			Vector2 movementDemand = Vector2.Zero;

			if (k.IsKeyDown(Keys.W))
				movementDemand.Y -= 1.0F;
			if (k.IsKeyDown(Keys.S))
				movementDemand.Y += 1.0F;
			if (k.IsKeyDown(Keys.A))
				movementDemand.X -= 1.0F;
			if (k.IsKeyDown(Keys.D))
				movementDemand.X += 1.0F;

			if(movementDemand.X != movementDemand.Y)
				movementDemand.Normalize();

			movementDemand *= 0.015F;
			
			this.velocity.X -= movementDemand.Y * (float)Math.Sin(MathHelper.ToRadians(yaw));
			this.velocity.Z += movementDemand.Y * (float)Math.Cos(MathHelper.ToRadians(yaw));
			this.velocity.Z += movementDemand.X * (float)Math.Sin(MathHelper.ToRadians(yaw));
			this.velocity.X -= movementDemand.X * -(float)Math.Cos(MathHelper.ToRadians(yaw));

			#endregion


			// Update the player's eyes
			this.pitch -= lookMovement.Y * Game.lookSensitivity;
			this.yaw += lookMovement.X * Game.lookSensitivity;

			
			base.update(random, world);
		}

	}

}
