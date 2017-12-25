using System;

namespace Everblaze.Gameplay.Skills
{
	public class FarmingSkill : Skill
	{

		public FarmingSkill(float startingLevel)
			: base(startingLevel)
		{

		}

		public override String getName()
		{
			return "Farming";
		}

	}
}
