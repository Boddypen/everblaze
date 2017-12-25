using System;
using System.Collections.Generic;
using Everblaze.Environment.Items;
using Everblaze.Gameplay.Actions;
using Everblaze.Gameplay.Skills;
using Everblaze.Miscellaneous;
using Everblaze.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Tiles
{
	public class DirtTile : Tile
	{

		public DirtTile(Random random) : base(random)
		{
			this.networkID = NETWORK_DIRT;


			this.slipperiness = 0.77F;
		}
		

		public override void draw(
			GraphicsDeviceManager graphics,
			BasicEffect effect,
			Camera camera,
			Int32 tileX,
			Int32 tileZ)
		{

			Tile.renderTile(
				graphics,
				effect,
				camera,
				tileX,
				tileZ,
				this.heights,
				TextureResources.tileDirtTexture,
				0.0F);

		}

		public override void addActions(ref List<Gameplay.Actions.Action> actionList, Point tilePosition, SkillSet skills, Item tool)
		{
			if (tool is ShovelItem)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Dig, skills, tilePosition));
			}

			base.addActions(ref actionList, tilePosition, skills, tool);
		}

		public override void onDig(Random random, SkillSet skills, World world, Int32 tileX, Int32 tileZ)
		{
			world.player.inventory.items.Add(new DirtItem(Item.Material.None, (float)random.NextDouble() * skills.digging.level, 0.0F));

			base.onDig(random, skills, world, tileX, tileZ);
		}

		public override String getDescription()
		{
			return "Moist soil, great for growing crops on.";
		}

		public override String getName()
		{
			return "dirt";
		}

	}
}
