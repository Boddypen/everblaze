using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Gameplay.Skills
{

	/// 
	/// <summary>
	///		An array of skills which are assigned to a character.
	/// </summary>
	/// 
	public class SkillSet
	{

		public DiggingSkill digging;
		public FarmingSkill farming;
		public WoodcuttingSkill woodcutting;
		public ForagingSkill foraging;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="SkillSet"/> class.
		/// </summary>
		/// 
		/// <param name="baseLevel">The base level to initialise all the skills at.</param>
		/// 
		public SkillSet(float baseLevel)
		{
			
			// Create the skillset.
			digging = new DiggingSkill(baseLevel);
			farming = new FarmingSkill(baseLevel);
			woodcutting = new WoodcuttingSkill(baseLevel);
			foraging = new ForagingSkill(baseLevel);

		}

	}
}
