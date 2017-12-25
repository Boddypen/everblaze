using System;
using System.Collections.Generic;

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

	public class GrassTile : Tile
	{
		
		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="GrassTile"/> class.
		/// </summary>
		/// 
		public GrassTile(Random random) : base(random)
		{
			this.networkID = NETWORK_GRASS;


			this.slipperiness = 0.72F;

			// Make all grass tiles have a random chance of starting foragable.
			this.fruitful = (random.Next(2) == 0);
		}


		/// 
		/// <summary>
		///		Writes any custom properties to a <see cref="NetOutgoingMessage" />.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetOutgoingMessage" /> to send.</param>
		/// 
		public override void writeCustomProperties(ref NetOutgoingMessage message)
		{
			message.Write(this.fruitful);

			base.writeCustomProperties(ref message);
		}
		public override void readCustomProperties(ref NetIncomingMessage message)
		{
			this.fruitful = message.ReadBoolean();

			base.readCustomProperties(ref message);
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
			int tileX,
			int tileZ)
		{

			Tile.renderTile(
				graphics,
				effect,
				camera,
				tileX,
				tileZ,
				this.heights,
				TextureResources.tileGrassTexture,
				0.0F);

		}


		/// 
		/// <summary>
		///		Adds available actions to the specified list.
		/// </summary>
		/// 
		/// <param name="actionList">The action list to be appended to.</param>
		/// <param name="tilePosition">The position of the tile, in the world.</param>
		/// <param name="skills">The skills of the player.</param>
		/// <param name="tool">The <see cref="Item" /> currently equipped by the player when activating the tile.</param>
		/// 
		public override void addActions(ref List<Gameplay.Actions.Action> actionList, Point tilePosition, SkillSet skills, Item tool)
		{
			if (tool is ShovelItem)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Cultivate, skills, tilePosition));
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Dig, skills, tilePosition));
			}

			if (this.fruitful)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Forage, skills, tilePosition));
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
			int tileX,
			int tileZ)
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
			return "Wild, lush grass grows here.";
		}


		/// 
		/// <summary>
		///		Gets the name of the tile.
		/// </summary>
		/// 
		public override String getName()
		{
			return "grass";
		}
		
	}

}
