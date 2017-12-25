using System;

namespace Everblaze.Gameplay.Skills
{
	public class DiggingSkill : Skill
	{
		
		public DiggingSkill(float startingLevel)
			: base(startingLevel)
		{
			
		}

		public override String getName()
		{
			return "Digging";
		}

	}
}
