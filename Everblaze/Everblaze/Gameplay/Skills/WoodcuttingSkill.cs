using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Everblaze.Gameplay.Skills
{
	public class WoodcuttingSkill : Skill
	{
		public WoodcuttingSkill(Single startingLevel) : base(startingLevel)
		{
			
		}

		public override String getName()
		{
			return "Woodcutting";
		}
	}
}
