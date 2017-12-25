using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Gameplay.Skills
{
	public class ForagingSkill : Skill
	{
		public ForagingSkill(Single startingLevel) : base(startingLevel)
		{
			this.level = startingLevel;
		}

		public override String getName()
		{
			return "Foraging";
		}
	}
}
