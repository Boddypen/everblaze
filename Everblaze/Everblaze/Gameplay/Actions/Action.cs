using System;

using Microsoft.Xna.Framework;

using Everblaze.Environment.Tiles;
using Everblaze.Gameplay.Skills;
using Everblaze.Environment.Items;

namespace Everblaze.Gameplay.Actions
{
	public class Action
	{

		/// <summary>
		///		A result which may be achieved upon the completion of an action.
		/// </summary>
		public enum Operation
		{

			/// <summary>
			///		Observes the target object.
			/// </summary>
			Examine,

			/// <summary>
			///		Lowers the nearest corner of a diggable tile.
			/// </summary>
			Dig,

			/// <summary>
			///		Converts a <see cref="GrassTile"/> into a <see cref="DirtTile"/>.
			/// </summary>
			Cultivate,

			/// <summary>
			///		Converts a <see cref="TreeTile"/> into a <see cref="GrassTile"/>.
			/// </summary>
			CutDown,

			/// <summary>
			///		Searches a grass (or similar) tile for resources such as food and herbs.
			/// </summary>
			Forage

		}


		public enum TargetType
		{

			/// <summary>
			///		The action is targeting a <see cref="Tile"/>, and therefore should
			///		utilise the <see cref="tileTarget"/> property.
			/// </summary>
			Tile,

			/// <summary>
			///		The action is targeting an <see cref="Item"/>.
			/// </summary>
			Item

		}

		
		/// <summary>
		///		The <see cref="Operation"/> which the action will achieve once completed.
		/// </summary>
		public Operation operation;

		/// <summary>
		///		The type of target the action is targeting.
		/// </summary>
		public TargetType targetType;
		
		/// <summary>
		///		The chance that the action will succeed upon completion.
		/// </summary>
		public float chance;

		/// <summary>
		///		If applicable, the tile in the world which this action targets.
		/// </summary>
		public Point tileTarget;

		/// <summary>
		///		If applicable, the target item's hash code.
		/// </summary>
		public Item targetItem;

		

		public Action(Operation operation, SkillSet skills, Point tileTarget)
		{
			this.operation = operation;
			this.targetType = TargetType.Tile;

			calculateChance(skills);

			this.tileTarget = tileTarget;
		}

		public Action(Operation operation, SkillSet skills, ref Item targetItem)
		{
			this.operation = operation;
			this.targetType = TargetType.Item;

			calculateChance(skills);

			this.targetItem = targetItem;
		}


		/// 
		/// <summary>
		///		Gets the full <see cref="String"/> of the action, including chance, etc.
		/// </summary>
		/// 
		public String getFullString()
		{

			String toReturn;
			switch(this.operation)
			{
				case Operation.Cultivate:
					toReturn = "Cultivate";
					break;

				case Operation.CutDown:
					toReturn = "Cut Down";
					break;

				case Operation.Dig:
					toReturn = "Dig";
					break;

				case Operation.Examine:
					toReturn = "Examine";
					break;

				case Operation.Forage:
					toReturn = "Forage";
					break;

				default:
					toReturn = "???";
					break;
			}
			
			if(chance < 1.0F)
			{
				toReturn = toReturn + " (" + (int)Math.Round(chance * 100.0F) + "%)";
			}

			return toReturn;

		}


		/// 
		/// <summary>
		///		Gets the word which represents the verb, in-action.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="String"/> representing the verb in present tense.
		/// </returns>
		/// 
		public String getName()
		{
			switch(this.operation)
			{
				case Operation.Examine:
					return "examining";

				case Operation.Dig:
					return "digging";

				case Operation.Cultivate:
					return "cultivating";

				case Operation.CutDown:
					return "cutting down";

				case Operation.Forage:
					return "foraging";

				default:
					return "nulling";
			}
		}


		/// 
		/// <summary>
		///		Calculates the chance that the action will succeed, and
		///		updates the <see cref="chance"/> property as required.
		/// </summary>
		/// 
		/// <param name="skills">The skillset of the character performing the action.</param>
		/// 
		public void calculateChance(SkillSet skills)
		{
			switch(this.operation)
			{
				case Operation.Examine:
					chance = 1.0F;
					break;

				case Operation.Dig:
					chance = 1.0F;
					break;

				case Operation.Cultivate:
					chance = 1.0F;
					break;

				case Operation.CutDown:
					chance = 1.0F;
					break;

				case Operation.Forage:
					chance = 0.5F + (skills.foraging.level * 0.004F);
					break;

				default:
					chance = 0.0F;
					break;
			}
		}

	}
}
