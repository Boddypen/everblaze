using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Everblaze.Environment.Items;
using Everblaze.Gameplay.Actions;
using Everblaze.Gameplay.Skills;
using Everblaze.Miscellaneous;
using Everblaze.Resources;

using Lidgren.Network;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Tiles
{
	public class SandTile : Tile
	{

		public SandTile() : base()
		{

			this.networkID = NETWORK_SAND;

			this.slipperiness = 0.71F;

			this.fruitful = false;

		}

		public override void draw(
			GraphicsDeviceManager graphics,
			BasicEffect effect,
			Camera camera,
			World world,
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
				TextureResources.tileSandTexture,
				0.0F);

		}

		public override void addActions(
			ref List<Gameplay.Actions.Action> actionList,
			Point tilePosition,
			SkillSet skills,
			Item tool)
		{
			if(tool is ShovelItem)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Dig, skills, tilePosition));
			}

			base.addActions(ref actionList, tilePosition, skills, tool);
		}

		public override void onDig(
			SkillSet skills,
			World world,
			Int32 tileX,
			Int32 tileZ)
		{
			world.player.inventory.store(
				new SandItem(
					Item.Material.None,
					(float)Program.random.NextDouble() * skills.digging.level,
					0.0F));

			base.onDig(skills, world, tileX, tileZ);
		}

		public override String getDescription()
		{
			return "Coarse sand, littered with rocks and shells.";
		}

		public override String getName()
		{
			return "sand";
		}

	}
}
