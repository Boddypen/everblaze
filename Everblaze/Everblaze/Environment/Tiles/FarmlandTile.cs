using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Everblaze.Environment.Items;
using Everblaze.Gameplay.Actions;
using Everblaze.Gameplay.Skills;
using Everblaze.Miscellaneous;
using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Everblaze.Environment.Tiles
{
	public class FarmlandTile : Tile
	{

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="FarmlandTile"/> class.
		/// </summary>
		/// 
		public FarmlandTile(Random random) : base(random)
		{
			this.networkID = NETWORK_FARMLAND;


			this.slipperiness = 0.72F;
		}


		/// 
		/// <summary>
		///		Called to render the tile onto the screen.
		/// </summary>
		/// 
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
				TextureResources.tileFarmlandTexture,
				0.0F);

		}


		/// 
		/// <summary>
		///		Adds available actions to the specified list.
		/// </summary>
		/// 
		public override void addActions(
			ref List<Gameplay.Actions.Action> actionList,
			Point tilePosition,
			SkillSet skills,
			Item tool)
		{

			if (tool is ShovelItem)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Cultivate, skills, tilePosition));
			}

			base.addActions(ref actionList, tilePosition, skills, tool);
		}


		/// 
		/// <summary>
		///		Called when the tile is dug.
		/// </summary>
		/// 
		public override void onDig(
			Random random,
			SkillSet skills,
			World world,
			Int32 tileX,
			Int32 tileZ)
		{
			world.replaceTile(tileX, tileZ, new DirtTile(random));

			world.player.inventory.items.Add(new DirtItem(Item.Material.None, (float)random.NextDouble() * skills.digging.level, 0.0F));

			base.onDig(random, skills, world, tileX, tileZ);
		}


		/// 
		/// <summary>
		///		Gets a description of the tile's features.
		/// </summary>
		/// 
		public override String getDescription()
		{
			return "A section of land designed to grow crops.";
		}


		/// 
		/// <summary>
		///		Gets the name of the tile.
		/// </summary>
		/// 
		public override String getName()
		{
			return "farmland";
		}

	}
}
