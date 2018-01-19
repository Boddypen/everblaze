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
	public class TreeTile : Tile
	{

		/// <summary>
		///		The quadrant of the tile where the tree is to be
		///		rendered. This is generated randomly when the
		///		tile is created.
		/// </summary>
		private TileCorner treeCorner;

		private double treeRotation = 0.0F;

		/// <summary>
		///		The growth level of the tree.
		///		Clamped between 0 and 5.
		/// </summary>
		public int growthLevel = 0;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="TreeTile"/> class.
		/// </summary>
		/// 
		public TreeTile() : base()
		{
			this.networkID = NETWORK_TREE;


			this.slipperiness = 0.71F;


			// Make all tree tiles have a random chance of starting foragable.
			this.fruitful = (Program.random.Next(2) == 0);

			this.diggable = false;

			// Start the tree's growth at 0.
			this.growthLevel = 0;
			

			// Randomise the tree's position on the tile.
			this.treeCorner = Tile.getRandomCorner();
			this.treeRotation = MathHelper.ToRadians((float)Program.random.NextDouble() * 360.0F);

		}


		#region Networking & File Storage

		public override void readCustomFileProperties(String[] customProperties)
		{
			this.fruitful = Boolean.Parse(customProperties[0]);

			this.treeRotation = float.Parse(customProperties[1]);

			switch (int.Parse(customProperties[2]))
			{
				case 0:
					this.treeCorner = TileCorner.TopLeft;
					break;

				case 1:
					this.treeCorner = TileCorner.TopRight;
					break;

				case 2:
					this.treeCorner = TileCorner.BottomLeft;
					break;

				case 3:
					this.treeCorner = TileCorner.BottomRight;
					break;
			}


			base.readCustomFileProperties(customProperties);
		}

		public override String getCustomFileProperties()
		{
			int treeCornerInteger = 0;
			switch (this.treeCorner)
			{
				case TileCorner.TopLeft: treeCornerInteger = 0; break;
				case TileCorner.TopRight: treeCornerInteger = 1; break;
				case TileCorner.BottomLeft: treeCornerInteger = 2; break;
				case TileCorner.BottomRight: treeCornerInteger = 3; break;
			}

			return this.fruitful.ToString() + (char)0xFF
				 + this.treeRotation.ToString() + (char)0xFF
				 + treeCornerInteger;
		}


		public override void readCustomNetworkProperties(ref NetIncomingMessage message)
		{
			this.fruitful = message.ReadBoolean();

			this.treeRotation = message.ReadDouble();
			
			switch(message.ReadInt32())
			{
				case 0:
					this.treeCorner = TileCorner.TopLeft;
					break;

				case 1:
					this.treeCorner = TileCorner.TopRight;
					break;

				case 2:
					this.treeCorner = TileCorner.BottomLeft;
					break;

				case 3:
					this.treeCorner = TileCorner.BottomRight;
					break;
			}

			base.readCustomNetworkProperties(ref message);
		}


		public override void writeCustomProperties(ref NetOutgoingMessage message)
		{
			message.Write(this.fruitful);

			message.Write(this.treeRotation);
			
			switch(this.treeCorner)
			{
				case TileCorner.TopLeft:
					message.Write((Int32)0);
					break;

				case TileCorner.TopRight:
					message.Write((Int32)1);
					break;

				case TileCorner.BottomLeft:
					message.Write((Int32)2);
					break;

				case TileCorner.BottomRight:
					message.Write((Int32)3);
					break;
			}

			base.writeCustomProperties(ref message);
		}

		#endregion


		/// 
		/// <summary>
		///		Called to render the tile onto the screen.
		/// </summary>
		/// 
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
				TextureResources.tileGrassTexture,
				0.0F);


			Vector2 treePosition = new Vector2(0.0F, 0.0F);
			switch(this.treeCorner)
			{
				case TileCorner.TopLeft:
					treePosition = new Vector2(-0.25F, -0.25F);
					break;

				case TileCorner.TopRight:
					treePosition = new Vector2(0.25F, -0.25F);
					break;

				case TileCorner.BottomLeft:
					treePosition = new Vector2(-0.25F, 0.25F);
					break;

				case TileCorner.BottomRight:
					treePosition = new Vector2(0.25F, 0.25F);
					break;
			}
			treePosition *= Tile.TILE_WIDTH;
			

			ModelResources.renderModel(
				ModelResources.treeModel,
				camera,
				new Vector3((tileX * Tile.TILE_WIDTH) + (Tile.TILE_WIDTH / 2.0F) + treePosition.X,
							getAverageHeight() * Tile.HEIGHT_STEP,
							(tileZ * Tile.TILE_WIDTH) + (Tile.TILE_WIDTH / 2.0F) + treePosition.Y),
				new Vector3(0.0F, (float)treeRotation, 0.0F),
				new Vector3(1.0F));

		}


		/// 
		/// <summary>
		///		Called when a nature tick occurs (roughly every 15 minutes).
		/// </summary>
		/// 
		/// <param name="world">The world.</param>
		/// <param name="tileX">The tile's X position in the world.</param>
		/// <param name="tileZ">The tile's Z position in the world.</param>
		/// 
		public override void natureTick(
			World world,
			int tileX,
			int tileZ)
		{

			// Increase the growth level of the tree.
			growthLevel++;
			if (growthLevel > 5)
				growthLevel = 5;


			base.natureTick(world, tileX, tileZ);
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

			if (tool is HatchetItem)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.CutDown, skills, tilePosition));
			}

			if (this.fruitful)
			{
				actionList.Add(new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Forage, skills, tilePosition));
			}

			base.addActions(ref actionList, tilePosition, skills, tool);
		}


		/// 
		/// <summary>
		///		Gets a description of the tile's features.
		/// </summary>
		/// 
		public override String getDescription()
		{
			return "Wild, lush grass grows here. A tree is also here.";
		}


		public override String getName()
		{
			String growthDisplay;
			switch(growthLevel)
			{
				case 0:
					growthDisplay = "sprouting ";
					break;

				case 1:
					growthDisplay = "young ";
					break;

				case 2:
					growthDisplay = "mature ";
					break;

				case 3:
					growthDisplay = "old ";
					break;

				case 4:
					growthDisplay = "very old ";
					break;

				case 5:
					growthDisplay = "dead ";
					break;

				default:
					growthDisplay = "";
					break;
			}

			return growthDisplay + "tree";
		}

	}
}
