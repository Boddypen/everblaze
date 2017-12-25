using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Everblaze.Interface;

namespace Everblaze.Gameplay.Skills
{

	/// 
	/// <summary>
	///		Represents a single skill which the player can improve over time.
	/// </summary>
	/// 
	public class Skill
	{

		/// <summary>
		///		The multiplier for all skillgain.
		/// </summary>
		public const float MULTIPLIER = 5.0F;



		/// <summary>
		///		The level of the skill, clamped between 0.0F and 100.0F.
		/// </summary>
		public float level;

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Skill"/> class.
		/// </summary>
		/// 
		/// <param name="startingLevel">The starting level of the skill.</param>
		/// 
		public Skill(float startingLevel)
		{

			if (startingLevel < 0.0F)
				this.level = 0.0F;
			else if (startingLevel > 100.0F)
				this.level = 100.0F;
			else
				this.level = startingLevel;
			
		}


		/// 
		/// <summary>
		///		Gets the time multiplier which the skill will affect a <see cref="TimedAction"/> by.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="float"/> representing the multiplier.
		/// </returns>
		/// 
		public virtual float getTimeMultiplier()
		{
			return 1.0F - (this.level * 0.0075F);
		}


		/// 
		/// <summary>
		///		Increases the skill slightly.
		/// </summary>
		/// 
		public virtual void increase(Random random, List<Notification> notifications)
		{

			float skillgain = (0.1F / ((level + 100.0F) / 100.0F) + (float)(random.NextDouble() * 0.01F)) * Skill.MULTIPLIER;
			float previousLevel = this.level;

			level += skillgain;

			if (level > 100.0F) level = 100.0F;
			if (level < 1.0F) level = 1.0F;


			// Notify the player of skill advancements.

			if (previousLevel < 25.0F && this.level >= 25.0F)
				notifications.Add(new Notification("You have reached apprentice level in " + this.getName() + "."));

			if(previousLevel < 50.0F && this.level >= 50.0F)
				notifications.Add(new Notification("You have reached adept level in " + this.getName() + "."));

			if(previousLevel < 75.0F && this.level >= 75.0F)
				notifications.Add(new Notification("You have reached expert level in " + this.getName() + "."));

			if(previousLevel < 100.0F && this.level >= 100.0F)
				notifications.Add(new Notification("You have become a master in " + this.getName() + "!"));

		}


		/// 
		/// <summary>
		///		Gets the readable name of the skill.
		/// </summary>
		/// 
		public virtual String getName()
		{
			return "Blank Skill";
		}

	}

}
