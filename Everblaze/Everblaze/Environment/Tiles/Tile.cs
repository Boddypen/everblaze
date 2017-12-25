using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Everblaze.Miscellaneous;
using Everblaze.Gameplay.Actions;
using Everblaze.Gameplay.Skills;
using Everblaze.Environment.Items;
using Lidgren.Network;

namespace Everblaze.Environment.Tiles
{

	/// 
	/// <summary>
	///		A class representing a single tile in the game world.
	///		The player will be able to interact with the tile in
	///		various ways.
	/// </summary>
	/// 
	/// <remarks>
	///		Created by Marcus Kirkwood, 2017
	///		Software Design and Development
	///		Major HSC Project
	/// </remarks>
	/// 
	public class Tile
	{

		/// <summary>
		///		The on-edge width of all tiles.
		///		Unit: <c>m</c>
		/// </summary>
		public const float TILE_WIDTH = 4.0F;

		/// <summary>
		///		How much each 'height' measurement of a tile's corner represents.
		///		Unit: <c>m</c>
		/// </summary>
		public const float HEIGHT_STEP = 0.20F;
		
		// Network identifiers for different types of tiles.
		public const int
			NETWORK_DIRT = 1,
			NETWORK_GRASS = 2,
			NETWORK_TREE = 3,
			NETWORK_FARMLAND = 4;

		public int networkID;


		/// <summary>
		///		Represents a single corner of a tile.
		/// </summary>
		public enum TileCorner
		{
			/// <summary>
			///		The corner at position 0, 0
			/// </summary>
			TopLeft,

			/// <summary>
			///		The corner at position 1, 0
			/// </summary>
			TopRight,

			/// <summary>
			///		The corner at position 0, 1
			/// </summary>
			BottomLeft,

			/// <summary>
			///		The corner at position 1, 1
			/// </summary>
			BottomRight
		}


		/// <summary>
		///		Represents a side along a tile.
		/// </summary>
		public enum TileSide
		{

			/// <summary>
			///		The side towards negative-X.
			/// </summary>
			West,

			/// <summary>
			///		The side towards positive-X.
			/// </summary>
			East,

			/// <summary>
			///		The side towards negative-Z.
			/// </summary>
			North,

			/// <summary>
			///		The side towards positive-Z.
			/// </summary>
			South

		}


		#region Corner Swapping

		/// 
		/// <summary>
		///		Return the corner which is vertically opposite from the specified corner.
		/// </summary>
		/// 
		/// <param name="corner">The corner to swap.</param>
		/// 
		public static TileCorner swapVertically(TileCorner corner)
		{
			switch(corner)
			{
				case TileCorner.TopLeft: return TileCorner.BottomLeft;
				case TileCorner.BottomLeft: return TileCorner.TopLeft;
				case TileCorner.TopRight: return TileCorner.BottomRight;
				case TileCorner.BottomRight: return TileCorner.TopRight;

				default: return TileCorner.TopLeft;
			}
			
		}

		/// 
		/// <summary>
		///		Return the corner which is horizontally opposite from the specified corner.
		/// </summary>
		/// 
		/// <param name="corner">The corner to swap.</param>
		/// 
		public static TileCorner swapHorizontally(TileCorner corner)
		{
			switch (corner)
			{
				case TileCorner.TopLeft: return TileCorner.TopRight;
				case TileCorner.BottomLeft: return TileCorner.BottomRight;
				case TileCorner.TopRight: return TileCorner.TopLeft;
				case TileCorner.BottomRight: return TileCorner.BottomLeft;

				default: return TileCorner.TopLeft;
			}
		}

		#endregion


		/// <summary>
		///		The heights of the four corners of the tile.
		/// </summary>
		public int[] heights;

		/// <summary>
		///		How slippery the tile is. Clamped between 0.0 and 1.0
		///		The closer to 1.0, the more slippery the tile is.
		/// </summary>
		public float slipperiness;


		/// <summary>
		///		Whether or not foragables have grown on the tile.
		///		Does not affect non-foragable tiles.
		/// </summary>
		public Boolean fruitful = true;

		/// <summary>
		///		Whether or not the tile is allowed to be affected by
		///		a digging action.
		/// </summary>
		public Boolean diggable = true;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Tile"/> class.
		/// </summary>
		/// 
		/// <param name="random">A random number generator.</param>
		/// 
		public Tile(Random random)
		{

			this.heights = new int[4];
			heights[0] = 0;
			heights[1] = 0;
			heights[2] = 0;
			heights[3] = 0;

			this.slipperiness = 0.80F;
			
		}


		/// 
		/// <summary>
		///		Creates a <see cref="Tile"/> object based on the data from a <see cref="NetIncomingMessage"/>.
		/// </summary>
		/// 
		/// <param name="random">A random number generator, for use with tile creation.</param>
		/// <param name="message">The <see cref="NetIncomingMessage"/>.</param>
		/// 
		/// <returns>
		///		A <see cref="Tile"/>.
		/// </returns>
		/// 
		public static Tile readFromNetwork(
			Random random,
			ref NetIncomingMessage message)
		{

			// Create a default tile.
			Tile tile = new Tile(random);
			
			// Determine what the tile is.
			switch(message.ReadInt32())
			{
				case NETWORK_GRASS:
					tile = new GrassTile(random);
					break;

				case NETWORK_DIRT:
					tile = new DirtTile(random);
					break;

				case NETWORK_TREE:
					tile = new TreeTile(random);
					break;

				case NETWORK_FARMLAND:
					tile = new FarmlandTile(random);
					break;
			}

			Console.WriteLine("RECEIVING A {0} TILE.", tile.getName());

			// Set the correct heights for the tile.
			tile.heights = new int[]
					{
						message.ReadInt32(),
						message.ReadInt32(),
						message.ReadInt32(),
						message.ReadInt32()
					};


			// Load custom properties.
			tile.readCustomProperties(ref message);


			return tile;
		}


		/// 
		/// <summary>
		///		Reads any custom properties from a <see cref="NetIncomingMessage"/>.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetIncomingMessage"/> reveived.</param>
		/// 
		public virtual void readCustomProperties(ref NetIncomingMessage message)
		{
			return;
		}


		/// 
		/// <summary>
		///		Writes any custom properties to a <see cref="NetOutgoingMessage"/>.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetOutgoingMessage"/> to send.</param>
		/// 
		public virtual void writeCustomProperties(ref NetOutgoingMessage message)
		{
			return;
		}
		

		/// 
		/// <summary>
		///		Adds available actions to the specified list.
		/// </summary>
		/// 
		/// <param name="actionList">The action list to be appended to.</param>
		/// <param name="tilePosition">The position of the tile, in the world.</param>
		/// <param name="skills">The skills of the player.</param>
		/// <param name="tool">The <see cref="Item"/> currently equipped by the player when activating the tile.</param>
		///
		public virtual void addActions(
			ref List<Everblaze.Gameplay.Actions.Action> actionList,
			Point tilePosition,
			SkillSet skills,
			Item tool)
		{

			// Examine
			actionList.Add(
				new Gameplay.Actions.Action(Gameplay.Actions.Action.Operation.Examine,
											skills,
											tilePosition));

			return;
		}
		

		/// 
		/// <summary>
		///		Gets a description of the tile's features.
		/// </summary>
		/// 
		public virtual String getDescription()
		{
			return "A completely blank tile, made of dark matter.";
		}

		/// 
		/// <summary>
		///		Gets the name of the tile.
		/// </summary>
		/// 
		public virtual String getName()
		{
			return "blank tile";
		}


		/// 
		/// <summary>
		///		Changes the height of a corner of the tile.
		/// </summary>
		/// 
		/// <param name="corner">The corner to change.</param>
		/// <param name="change">The amount to change the corner by (positive or negative integer.</param>
		/// 
		public virtual void changeHeight(TileCorner corner, int change)
		{
			switch(corner)
			{
				case TileCorner.TopLeft:
					heights[1] += change;
					return;

				case TileCorner.TopRight:
					heights[2] += change;
					return;

				case TileCorner.BottomLeft:
					heights[0] += change;
					return;

				case TileCorner.BottomRight:
					heights[3] += change;
					return;

				default:
					return;
			}
		}


		/// 
		/// <summary>
		///		Gets the average height of all the height points of the tile.
		///		This is the height of the centre of the tile.
		/// </summary>
		/// 
		public virtual float getAverageHeight()
		{
			float heightsCombined = 0.0F;

			foreach (int height in this.heights)
				heightsCombined += (float)height;

			heightsCombined /= heights.Length;

			return heightsCombined;
		}
		

		/// 
		/// <summary>
		///		Gets the height of a particular corner of the tile.
		/// </summary>
		/// 
		/// <param name="corner">The corner in question.</param>
		/// 
		/// <returns>
		///		The height of the specified corner of the tile.
		/// </returns>
		/// 
		public int getHeight(TileCorner corner)
		{

			switch(corner)
			{
				case TileCorner.BottomLeft: return heights[0];
				case TileCorner.TopLeft: return heights[1];
				case TileCorner.TopRight: return heights[2];
				case TileCorner.BottomRight: return heights[3];

				default: return 0;
			}

		}


		/// 
		/// <summary>
		///		Called on every game update tick (usually 60 times/second).
		/// </summary>
		/// 
		public virtual void update()
		{

		}


		/// 
		/// <summary>
		///		Called when an entity enters the tile.
		/// </summary>
		/// 
		public virtual void steppedOn()
		{

		}


		/// 
		/// <summary>
		///		Called when the tile is dug.
		/// </summary>
		/// 
		public virtual void onDig(
			Random random,
			SkillSet skills,
			World world,
			int tileX,
			int tileZ)
		{
			
		}


		/// 
		/// <summary>
		///		Called when a nature tick occurs (roughly every 15 minutes).
		/// </summary>
		/// <param name="world">The world.</param>
		/// <param name="tileX">The tile's X position in the world.</param>
		/// <param name="tileZ">The tile's Z position in the world.</param>
		public virtual void natureTick(
			World world,
			int tileX,
			int tileZ)
		{
			
		}


		/// 
		/// <summary>
		///		Called to render the tile onto the screen.
		/// </summary>
		/// 
		/// <param name="camera">The game camera.</param>
		/// 
		public virtual void draw(
			GraphicsDeviceManager graphics,
			BasicEffect effect,
			Camera camera,
			int tileX,
			int tileZ)
		{

		}


		/// 
		/// <summary>
		///		Returns a random tile corner.
		/// </summary>
		/// 
		/// <param name="random">A random number generator.</param>
		/// 
		/// <returns>
		///		A random corner for use with all world tiles.
		/// </returns>
		/// 
		public static TileCorner getRandomCorner(Random random)
		{
			// Create a random number.
			int randomNumber = random.Next(4);

			// Based on the random number, decide on a tile corner.
			switch(randomNumber)
			{
				case 0: return TileCorner.TopLeft;
				case 1: return TileCorner.TopRight;
				case 2: return TileCorner.BottomLeft;
				case 3: return TileCorner.BottomRight;

				// (If anything weird happens, return a default corner)
				default:
					return TileCorner.TopLeft;
			}
		}


		/// 
		/// <summary>
		///		Renders a tile onto the screen.
		/// </summary>
		/// 
		/// <param name="graphics">The game's <see cref="GraphicsDeviceManager"/>.</param>
		/// <param name="effect">The <see cref="Effect"/> to be used to draw the tile.</param>
		/// <param name="camera">The game <see cref="Camera"/>.</param>
		/// <param name="tileX">The tile's X position in the world.</param>
		/// <param name="tileZ">The tile's Z position in the world..</param>
		/// <param name="heights">The heightmap of the tile.</param>
		/// <param name="tileTexture">The <see cref="Texture2D"/> to use when drawing the top of the tile.</param>
		public static void renderTile(
			GraphicsDeviceManager graphics,
			BasicEffect effect,
			Camera camera,
			int tileX,
			int tileZ,
			int[] heights,
			Texture2D tileTexture,
			float heightOffset)
		{
			
			effect.View = Matrix.CreateLookAt(
				camera.position,
				camera.target,
				Vector3.Up);

			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(90.0F),
				1920.0F / 1080.0F,
				0.1F,
				100.0F);

			effect.World = Matrix.CreateTranslation(
				tileX * Tile.TILE_WIDTH,	// X
				heightOffset,				// Y
				tileZ * Tile.TILE_WIDTH);	// Z

			
			effect.TextureEnabled = true;
			effect.Texture = tileTexture;


			//// FOG
			//effect.FogEnabled = true;
			//effect.FogStart = 50.0F;
			//effect.FogEnd = 90.0F;
			//effect.FogColor = new Vector3(0.494F, 0.753F, 0.933F);
			
			effect.LightingEnabled = true;
			effect.PreferPerPixelLighting = false;
			effect.DirectionalLight0.Enabled = true;
			effect.DirectionalLight0.Direction = new Vector3(0.5F, 5.0F, 0.8F);
			effect.DirectionalLight0.DiffuseColor = new Vector3(0.80F);
			effect.DirectionalLight0.SpecularColor = new Vector3(0.1F, 0.09F, 0.08F);

			foreach(var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphics.GraphicsDevice.DrawUserPrimitives(
					PrimitiveType.TriangleList,
					calculateTileVertices(heights),
					0,
					4);
			}

		}


		/// 
		/// <summary>
		///		Calculates the tile vertices, after supplied with a heightmap.
		///		This needs to be used due to the funny-looking heightmaps which
		///		are utilised in this game.
		/// </summary>
		/// 
		/// <param name="heights">A tile's heightmap.</param>
		/// 
		/// <returns>
		///		The list of vertex positions, in a <see cref="VertexPositionNormalTexture"/> array.
		/// </returns>
		/// 
		public static VertexPositionNormalTexture[] calculateTileVertices(int[] heights)
		{
			
			// Calculate the average height of the corners, used for the centre of the tile.
			float averageHeight = 0.0F;
			foreach (float height in heights) averageHeight += height;
			averageHeight /= heights.Length;
			
			// Then calculate the vertices.

			VertexPositionNormalTexture[] tileVertices = new VertexPositionNormalTexture[12];
			tileVertices[0].Position = new Vector3(0.0F, heights[0] * Tile.HEIGHT_STEP, Tile.TILE_WIDTH);
			tileVertices[1].Position = new Vector3(0.0F, heights[1] * Tile.HEIGHT_STEP, 0.0F);
			tileVertices[2].Position = new Vector3(Tile.TILE_WIDTH / 2.0F, averageHeight * Tile.HEIGHT_STEP, Tile.TILE_WIDTH / 2.0F);

			tileVertices[3].Position = tileVertices[1].Position;
			tileVertices[4].Position = new Vector3(Tile.TILE_WIDTH, heights[2] * Tile.HEIGHT_STEP, 0.0F);
			tileVertices[5].Position = tileVertices[2].Position;

			tileVertices[6].Position = tileVertices[4].Position;
			tileVertices[7].Position = new Vector3(Tile.TILE_WIDTH, heights[3] * Tile.HEIGHT_STEP, Tile.TILE_WIDTH);
			tileVertices[8].Position = tileVertices[2].Position;

			tileVertices[9].Position = tileVertices[7].Position;
			tileVertices[10].Position = tileVertices[0].Position;
			tileVertices[11].Position = tileVertices[2].Position;


			Plane aPlane = new Plane(tileVertices[0].Position, tileVertices[1].Position, tileVertices[2].Position);
			Plane bPlane = new Plane(tileVertices[3].Position, tileVertices[4].Position, tileVertices[5].Position);
			Plane cPlane = new Plane(tileVertices[6].Position, tileVertices[7].Position, tileVertices[8].Position);
			Plane dPlane = new Plane(tileVertices[9].Position, tileVertices[10].Position, tileVertices[11].Position);

			tileVertices[0].Normal = aPlane.Normal;
			tileVertices[1].Normal = tileVertices[0].Normal;
			tileVertices[2].Normal = tileVertices[0].Normal;

			tileVertices[3].Normal = bPlane.Normal;
			tileVertices[4].Normal = tileVertices[3].Normal;
			tileVertices[5].Normal = tileVertices[3].Normal;

			tileVertices[6].Normal = cPlane.Normal;
			tileVertices[7].Normal = tileVertices[6].Normal;
			tileVertices[8].Normal = tileVertices[6].Normal;

			tileVertices[9].Normal = dPlane.Normal;
			tileVertices[10].Normal = tileVertices[9].Normal;
			tileVertices[11].Normal = tileVertices[9].Normal;


			tileVertices[0].TextureCoordinate = new Vector2(0.0F, 1.0F);
			tileVertices[1].TextureCoordinate = new Vector2(0.0F, 0.0F);
			tileVertices[2].TextureCoordinate = new Vector2(0.5F, 0.5F);

			tileVertices[3].TextureCoordinate = tileVertices[1].TextureCoordinate;
			tileVertices[4].TextureCoordinate = new Vector2(1.0F, 0.0F);
			tileVertices[5].TextureCoordinate = tileVertices[2].TextureCoordinate;

			tileVertices[6].TextureCoordinate = tileVertices[4].TextureCoordinate;
			tileVertices[7].TextureCoordinate = new Vector2(1.0F, 1.0F);
			tileVertices[8].TextureCoordinate = tileVertices[2].TextureCoordinate;

			tileVertices[9].TextureCoordinate = tileVertices[7].TextureCoordinate;
			tileVertices[10].TextureCoordinate = tileVertices[0].TextureCoordinate;
			tileVertices[11].TextureCoordinate = tileVertices[2].TextureCoordinate;


			// Finally, return the vertex positions.
			return tileVertices;
		}

	}
}
