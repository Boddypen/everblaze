using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Everblaze.Miscellaneous;
using Microsoft.Xna.Framework.Input;

namespace Everblaze.Environment.Entities
{

	/// 
	/// <summary>
	///		A class representing an entity within the game world.
	/// </summary>
	/// 
	public class Entity
	{

		/// <summary>
		///		The position of the entity in the game world.
		/// </summary>
		public Vector3 position;
		
		/// <summary>
		///		The entity's velocity.
		///		Unit: <c>m/s</c>
		/// </summary>
		public Vector3 velocity;

		/// <summary>
		///		The height of the entity, measured upwards from the floor.
		///		Unit: m
		/// </summary>
		public float height;

		/// <summary>
		///		The width of the entity, noting the fact that the shape of
		///		the entity is a cylinder.
		///		Unit: m
		/// </summary>
		public float width;

		/// <summary>
		///		The point at which the entity is looking at.
		/// </summary>
		public Vector3 lookTarget;

		public float yaw = 0.0F;
		public float pitch = 0.0F;


		public Boolean isDead = false;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Entity"/> class.
		/// </summary>
		/// 
		public Entity()
		{

			// Set default values.
			this.position = new Vector3(0.0F, 0.0F, 0.0F);
			this.velocity = Vector3.Zero;

			this.height = 1.0F;
			this.width = 0.4F;

			this.yaw = 0.0F;
			this.pitch = 0.0F;

			this.lookTarget = this.position + new Vector3(0.0F, 0.0F, -1.0F);

		}


		/// 
		/// <summary>
		///		Makes the entity look in the correct direction, based on its pitch/yaw settings.
		/// </summary>
		/// 
		public virtual void updateLookTarget()
		{
			// Quick bounds check...
			if (pitch < -89.0F) pitch = -89.0F;
			if (pitch > 89.0F) pitch = 89.0F;
			while (yaw < 0.0F) yaw += 360.0F;
			while (yaw >= 360.0F) yaw -= 360.0F;


			this.lookTarget = this.position + new Vector3(0.0F, this.height, 0.0F);

			this.lookTarget.Y += (float)Math.Sin(MathHelper.ToRadians(pitch));
			this.lookTarget.Z -= (float)Math.Cos(MathHelper.ToRadians(yaw)) * (float)Math.Cos(MathHelper.ToRadians(pitch));
			this.lookTarget.X += (float)Math.Sin(MathHelper.ToRadians(yaw)) * (float)Math.Cos(MathHelper.ToRadians(pitch));

		}


		/// 
		/// <summary>
		///		Performs an update tick on the entity.
		/// </summary>
		/// 
		public virtual void update(
			Random random,
			World world)
		{

			// Move the entity.
			position.X += velocity.X;
			position.Z += velocity.Z;
			position.Y = world.getHeightAtPoint(position.X, position.Z);

			velocity *= world.getTileUnder(random, this.position).slipperiness;
			

			// Update the entity's look target.
			this.updateLookTarget();
			

			// Allow the entity to be killed.
			if(isDead)
			{
				this.onDeath();
			}

		}
		

		/// 
		/// <summary>
		///		Called when the entity is killed.
		/// </summary>
		/// 
		public virtual void onDeath()
		{



		}


		/// 
		/// <summary>
		///		Renders the entity in the game world.
		/// </summary>
		/// 
		public virtual void draw(
			GraphicsDeviceManager graphics,
			Camera camera)
		{


			
		}

	}
}
