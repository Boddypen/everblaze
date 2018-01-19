using System;
using System.Collections.Generic;
using System.Linq;

using Everblaze.Environment.Entities;
using Everblaze.Environment.Items;
using Everblaze.Environment.Tiles;
using Everblaze.Miscellaneous;
using Everblaze.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static Everblaze.Game;


namespace Everblaze.Environment
{

	/// 
	/// <summary>
	///		A class representing a collection of entities and a map of tiles.
	/// </summary>
	/// 
	public class World
	{

		/// <summary>
		///		The array of tiles which make up the world.
		/// </summary>
		public Tile[,] tiles;

		/// <summary>
		///		The width of the world in tiles, along the X axis.
		/// </summary>
		public int width;

		/// <summary>
		///		The height/depth of the world in tiles, along the Z axis.
		/// </summary>
		public int height;


		/// <summary>
		///		The list of items which are on the ground in the world.
		/// </summary>
		public List<Item> items;

		
		/// <summary>
		///		The counter which counts up to the nature tick.
		/// </summary>
		private int natureTickCounter = 0;


		/// <summary>
		///		The client-side player of the world.
		/// </summary>
		public Player player;


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="World"/> class.
		/// </summary>
		/// 
		/// <param name="width">The width of the world, in tiles, measured along the X axis.</param>
		/// <param name="height">The height of the world, in tiles, measured along the Z axis.</param>
		/// 
		public World(int width, int height)
		{
			
			this.tiles = new Tile[width, height];

			this.width = width;
			this.height = height;

			this.items = new List<Item>();

			this.player = new Player();
			this.player.position = new Vector3(
				(float)Program.random.NextDouble() * (Tile.TILE_WIDTH * width),
				0.0F,
				(float)Program.random.NextDouble() * (Tile.TILE_WIDTH * height));

		}


		/// 
		/// <summary>
		///		Updates the world.
		/// </summary>
		/// 
		/// <param name="k">The current keyboard state.</param>
		/// 
		public void update(
			GameSide gameSide,
			KeyboardState k,
			Vector2 lookMovement)
		{
			
			
			switch(gameSide)
			{
				case GameSide.Client:

					// Update the player.
					this.player.update(this, k, lookMovement);

					break;

				case GameSide.Server:
					
					// Perform nature ticks.
					if (natureTickCounter >= 54000)
					{
						natureTickCounter = 0;
						natureTick();
					}
					natureTickCounter++;

					break;
			}
			
		}


		/// 
		/// <summary>
		///		Performs a nature tick on the world.
		///		This is where trees grow, crops grow, and other cool stuff
		///		happens.
		/// </summary>
		/// 
		private void natureTick()
		{

			for(int x = 0; x < width; x++)
			{
				for(int z = 0; z < height; z++)
				{
					tiles[x, z].natureTick(this, x, z);
				}
			}

		}


		public Tile getTileUnder(Vector3 position)
		{
			if (position.X < 0.0F || position.X >= Tile.TILE_WIDTH * width
				|| position.Z < 0.0F || position.Z >= Tile.TILE_WIDTH * height)
				return new Tile();

			return this.tiles[
				(int)Math.Floor(position.X / Tile.TILE_WIDTH),
				(int)Math.Floor(position.Z / Tile.TILE_WIDTH)];
		}


		/// 
		/// <summary>
		///		Randomly generates the world.
		/// </summary>
		/// 
		public void generate()
		{

			// Generate a heightmap.
			float[,] heightmap = new float[width, height];
			
			// Generate a noise system to create the world.
			FastNoise f = new FastNoise();
			f.SetNoiseType(FastNoise.NoiseType.Simplex);
			f.SetFrequency(0.05F);
			f.SetFractalOctaves(15);
			f.SetFractalLacunarity(10.0F);
			f.SetFractalGain(1.0F);

			// For each tile int the world, set its height according to the noise generated previously.
			for(int x = 0; x < width; x++)
				for(int z = 0; z < height; z++)
					heightmap[x, z] = (f.GetPerlin((float)x, (float)z) + 0.4F) * 100.0F;


			// Generate tiles.
			for (int x = 0; x < width; x++)
				for (int z = 0; z < height; z++)
					this.tiles[x, z] = new GrassTile();


			// Randomise heights.
			for (int x = 1; x < width - 1; x++)
				for (int z = 1; z < height - 1; z++)
					this.changeHeight(x, z, Tile.TileCorner.BottomRight, (int)heightmap[x, z] + Program.random.Next(3));


			// Re-do tiles.
			for (int x = 0; x < width; x++)
			{
				for (int z = 0; z < height; z++)
				{
					float tileAverageHeight = this.tiles[x, z].getAverageHeight();

					if(tileAverageHeight <= 3.0F)
					{
						replaceTile(x, z, new SandTile());
					}


					if (tiles[x, z].GetType().Equals(typeof(GrassTile))
						&& Program.random.Next(10) == 0)
						replaceTile(x, z, new TreeTile());

				}
			}

		}


		/// 
		/// <summary>
		///		Gets the height of a particular tile's corner.
		/// </summary>
		/// 
		/// <param name="tileX">The tile's X position.</param>
		/// <param name="tileZ">The tile's Z position.</param>
		/// <param name="corner">The corner in question.</param>
		/// 
		public int getHeight(int tileX, int tileZ, Tile.TileCorner corner)
		{
			try
			{
				Tile tileQuery = this.tiles[tileX, tileZ];

				if (tileQuery == null)
					return 0;

				// Get the height.
				return tileQuery.getHeight(corner);
			}
			catch
			{
				return 0;
			}
		}


		/// 
		/// <summary>
		///		Replaces a tile in the world with a new one.
		/// </summary>
		/// 
		/// <param name="tileX">The X location.</param>
		/// <param name="tileZ">The Z location.</param>
		/// <param name="newTile">The new tile to replace the old one.</param>
		/// 
		public void replaceTile(int tileX, int tileZ, Tile newTile)
		{
			// Remember the old height data from the existing tile.
			int[] oldHeights = tiles[tileX, tileZ].heights;

			tiles[tileX, tileZ] = newTile;
			tiles[tileX, tileZ].heights = oldHeights;
		}


		/// 
		/// <summary>
		///		Gets the location of the tile at which the provided ray cast is pointing at.
		/// </summary>
		/// 
		/// <param name="camera">The camera of the entity.</param>
		/// 
		/// <returns>
		///		The location of the tile, in a <see cref="Point"/>.
		///		The result may be null if there are no selected tiles.
		/// </returns>
		/// 
		public Point? getSelectedTile(Camera camera)
		{

			Point? selectedTile = null;
			Camera raycast = new Camera(camera.position, camera.target);
			
			for(int i = 0; i < 150; i++)
			{
				// Get the tile the camera is over.
				Point currentTile = new Point((int)Math.Floor(raycast.position.X / Tile.TILE_WIDTH),
											  (int)Math.Floor(raycast.position.Z / Tile.TILE_WIDTH));

				if(currentTile.X < 0 || currentTile.X >= width ||
				   currentTile.Y < 0 || currentTile.Y >= height)
				{
					return null;
				}

				// Determine if the camera is in the ground.
				if (raycast.position.Y <= getHeightAtPoint(raycast.position.X, raycast.position.Z))
				{
					selectedTile = currentTile;

					if (selectedTile.Value.X < 0 || selectedTile.Value.Y < 0 || selectedTile.Value.X > width || selectedTile.Value.Y > height)
					{
						raycast.moveForward(0.05F);
						continue;
					}

					return selectedTile;
				}
				else
				{
					raycast.moveForward(0.05F);
				}
			}
			
			return null;
		}


		/// 
		/// <summary>
		///		Gets the exact height of the point specified in the world.
		/// </summary>
		/// 
		/// <param name="positionX">The X position.</param>
		/// <param name="positionZ">The Z position.</param>
		/// 
		/// <returns>
		///		The height of the point.
		/// </returns>
		/// 
		public float getHeightAtPoint(float positionX, float positionZ)
		{

			if(positionX < 0.0F || positionX > Tile.TILE_WIDTH * this.width
				|| positionZ < 0.0F || positionZ > Tile.TILE_WIDTH * this.height)
			{
				return 0.0F;
			}


			float[] heightmap = new float[4];
			Boolean flipped = getQuadrantHeightmap(positionX, positionZ, ref heightmap);

			int tileX = (int)(positionX / Tile.TILE_WIDTH);
			int tileZ = (int)(positionZ / Tile.TILE_WIDTH);

			float tilePositionX = (positionX - (float)(tileX * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;
			float tilePositionZ = (positionZ - (float)(tileZ * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;

			// Determine the height of the point.
			float height = 0.0F;

			float sqX = (tilePositionX < 0.5F ? tilePositionX * 2.0F : ((tilePositionX - 0.5F) * 2.0F));
			float sqZ = (tilePositionZ < 0.5F ? tilePositionZ * 2.0F : ((tilePositionZ - 0.5F) * 2.0F));
			
			if(flipped)
			{
				if(sqX < sqZ)
				{
					height = heightmap[2];

					height += (heightmap[3] - heightmap[2]) * sqX;
					height += (heightmap[0] - heightmap[2]) * (1.0F - sqZ);
				}
				else
				{
					height = heightmap[1];

					height += (heightmap[0] - heightmap[1]) * (1.0F - sqX);
					height += (heightmap[3] - heightmap[1]) * sqZ;
				}
			}
			else
			{
				if (sqX + sqZ < 1.0F)
				{
					height = heightmap[0];

					height += (heightmap[1] - heightmap[0]) * sqX;
					height += (heightmap[2] - heightmap[0]) * sqZ;
				}
				else
				{
					height = heightmap[3];

					height += (heightmap[1] - heightmap[3]) * (1.0F - sqZ);
					height += (heightmap[2] - heightmap[3]) * (1.0F - sqX);
				}
			}

			return height;

		}

		//public Vector3 getNormalAtPoint(
		//	float positionX,
		//	float positionZ)
		//{

		//	// Get the heightmap of the point.
		//	float[] heightmap = new float[4];
		//	Boolean flipped = this.getQuadrantHeightmap(positionX, positionZ, ref heightmap);

		//	// Calculate position on the tile...
		//	float tilePositionX = (positionX - (float)((int)(positionX / Tile.TILE_WIDTH) * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;
		//	float tilePositionZ = (positionZ - (float)((int)(positionZ / Tile.TILE_WIDTH) * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;


		//	float sqX = (tilePositionX < 0.5F ? tilePositionX * 2.0F : ((tilePositionX - 0.5F) * 2.0F));
		//	float sqZ = (tilePositionZ < 0.5F ? tilePositionZ * 2.0F : ((tilePositionZ - 0.5F) * 2.0F));

		//	float averageHeight = this.getHeightAtPoint(
		//		(int)(positionX / Tile.TILE_WIDTH) + (Tile.TILE_WIDTH / 2.0F),
		//		(int)(positionZ / Tile.TILE_WIDTH) + (Tile.TILE_WIDTH / 2.0F));

		//	VertexPositionNormalTexture[] vpnt = Tile.calculateTileVertices(this.tiles[tileX, tileZ].heights);

		//	if (flipped)
		//	{
		//		if (sqX < sqZ)
		//		{
		//			// 0, 2, 3
		//			return vpnt
		//		}
		//		else
		//		{
		//			// 0, 1, 3
		//		}
		//	}
		//	else
		//	{
		//		if (sqX + sqZ < 1.0F)
		//		{
		//			// 0, 1, 2
		//		}
		//		else
		//		{
		//			// 1, 2, 3
		//		}
		//	}
			
		//}


		/// 
		/// <summary>
		///		Gets the quadrant heightmap.
		/// </summary>
		/// 
		/// <param name="positionX">The position x.</param>
		/// <param name="positionZ">The position z.</param>
		/// <param name="flipped">if set to <c>true</c> [flipped].</param>
		/// <param name="tilePositionX">The tile position x.</param>
		/// <param name="tilePositionZ">The tile position z.</param>
		/// 
		/// <returns>
		///		Whether or not the quadrant is 'flipped'.
		/// </returns>
		/// 
		private Boolean getQuadrantHeightmap(
			float positionX,
			float positionZ,
			ref float[] heightmap)
		{
			
			// Generate the float array.
			heightmap = new float[4] { 0.0F, 0.0F, 0.0F, 0.0F };
			

			// If the position is outside of the world...
			if(    positionX < 0.0F
				|| positionZ < 0.0F
				|| positionX >= Tile.TILE_WIDTH * this.width
				|| positionZ >= Tile.TILE_WIDTH * this.height)
			{
				// Return the hightmap as it is, with no important values.
				return false;
			}


			// Determine the tile the point is on top of.
			int tileX = (int)(positionX / Tile.TILE_WIDTH);
			int tileZ = (int)(positionZ / Tile.TILE_WIDTH);


			// Now that we know the tile we're on, we need to find the quadrant.
			float tilePositionX = (positionX - (float)(tileX * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;
			float tilePositionZ = (positionZ - (float)(tileZ * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;

			Tile.TileCorner currentCorner = Tile.TileCorner.TopLeft;
			if (tilePositionX < 0.5F && tilePositionZ < 0.5F) currentCorner = Tile.TileCorner.TopLeft;
			else if (tilePositionX >= 0.5F && tilePositionZ < 0.5F) currentCorner = Tile.TileCorner.TopRight;
			else if (tilePositionX < 0.5F && tilePositionZ >= 0.5F) currentCorner = Tile.TileCorner.BottomLeft;
			else if (tilePositionX >= 0.5F && tilePositionZ >= 0.5F) currentCorner = Tile.TileCorner.BottomRight;


			// Now we need to find the heightmap of the quadrant we're on.
			switch (currentCorner)
			{
				case Tile.TileCorner.TopLeft:
					heightmap[0] = getHeight(tileX, tileZ, Tile.TileCorner.TopLeft) * Tile.HEIGHT_STEP;
					heightmap[1] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopLeft) + getHeight(tileX, tileZ, Tile.TileCorner.TopRight)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[2] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopLeft) + getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[3] = tiles[tileX, tileZ].getAverageHeight() * Tile.HEIGHT_STEP;
					return true;

				case Tile.TileCorner.TopRight:
					heightmap[0] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopLeft) + getHeight(tileX, tileZ, Tile.TileCorner.TopRight)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[1] = getHeight(tileX, tileZ, Tile.TileCorner.TopRight) * Tile.HEIGHT_STEP;
					heightmap[2] = tiles[tileX, tileZ].getAverageHeight() * Tile.HEIGHT_STEP;
					heightmap[3] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopRight) + getHeight(tileX, tileZ, Tile.TileCorner.BottomRight)) / 2.0F) * Tile.HEIGHT_STEP;
					return false;

				case Tile.TileCorner.BottomLeft:
					heightmap[0] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopLeft) + getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[1] = tiles[tileX, tileZ].getAverageHeight() * Tile.HEIGHT_STEP;
					heightmap[2] = getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft) * Tile.HEIGHT_STEP;
					heightmap[3] = ((getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft) + getHeight(tileX, tileZ, Tile.TileCorner.BottomRight)) / 2.0F) * Tile.HEIGHT_STEP;
					return false;

				case Tile.TileCorner.BottomRight:
					heightmap[0] = tiles[tileX, tileZ].getAverageHeight() * Tile.HEIGHT_STEP;
					heightmap[1] = ((getHeight(tileX, tileZ, Tile.TileCorner.TopRight) + getHeight(tileX, tileZ, Tile.TileCorner.BottomRight)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[2] = ((getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft) + getHeight(tileX, tileZ, Tile.TileCorner.BottomRight)) / 2.0F) * Tile.HEIGHT_STEP;
					heightmap[3] = getHeight(tileX, tileZ, Tile.TileCorner.BottomRight) * Tile.HEIGHT_STEP;
					return true;

				default:
					return false;
			}

		}


		/// 
		/// <summary>
		///		Changes the height of a particular tile's corner.
		/// </summary>
		/// 
		/// <param name="tileX">The tile's X position.</param>
		/// <param name="tileZ">The tile's Z position.</param>
		/// <param name="corner">The corner to change.</param>
		/// <param name="change">The amount to change the height by (positive or negative integer).</param>
		/// 
		public void changeHeight(int tileX, int tileZ, Tile.TileCorner corner, int change)
		{

			int displacementX = 0;
			int displacementZ = 0;

			switch(corner)
			{
				case Tile.TileCorner.TopLeft:
					displacementX = -1;
					displacementZ = -1;
					break;

				case Tile.TileCorner.TopRight:
					displacementX = 1;
					displacementZ = -1;
					break;

				case Tile.TileCorner.BottomLeft:
					displacementX = -1;
					displacementZ = 1;
					break;

				case Tile.TileCorner.BottomRight:
					displacementX = 1;
					displacementZ = 1;
					break;
			}

			try
			{
				this.tiles[tileX, tileZ].changeHeight(corner, change);
			}
			catch { }

			try
			{
				this.tiles[tileX + displacementX, tileZ].changeHeight(Tile.swapHorizontally(corner), change);
			}
			catch { }

			try
			{
				this.tiles[tileX, tileZ + displacementZ].changeHeight(Tile.swapVertically(corner), change);
			}
			catch { }

			try
			{
				this.tiles[tileX + displacementX, tileZ + displacementZ].changeHeight(Tile.swapVertically(Tile.swapHorizontally(corner)), change);
			}
			catch { }

		}


		/// 
		/// <summary>
		///		Gets the <i>average</i> height along a side of a tile.
		/// </summary>
		/// 
		/// <param name="tileX">The tile's X location in the world.</param>
		/// <param name="tileZ">The tile's Z location in the world.</param>
		/// <param name="side">The side to measure.</param>
		/// 
		/// <returns>
		///		The height at the halfway point of a side of a tile.
		/// </returns>
		/// 
		public float getSideHeight(int tileX, int tileZ, Tile.TileSide side)
		{

			float height = 0.0F;

			switch(side)
			{
				case Tile.TileSide.West:

					height += getHeight(tileX, tileZ, Tile.TileCorner.TopLeft);
					height += getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft);
					height /= 2.0F;

					break;


				case Tile.TileSide.East:

					height += getHeight(tileX, tileZ, Tile.TileCorner.TopRight);
					height += getHeight(tileX, tileZ, Tile.TileCorner.BottomRight);
					height /= 2.0F;

					break;


				case Tile.TileSide.North:

					height += getHeight(tileX, tileZ, Tile.TileCorner.TopLeft);
					height += getHeight(tileX, tileZ, Tile.TileCorner.TopRight);
					height /= 2.0F;

					break;


				case Tile.TileSide.South:

					height += getHeight(tileX, tileZ, Tile.TileCorner.BottomLeft);
					height += getHeight(tileX, tileZ, Tile.TileCorner.BottomRight);
					height /= 2.0F;

					break;
			}

			return height;

		}


		/// 
		/// <summary>
		///		Renders the entire world onto the screen.
		/// </summary>
		/// 
		/// <param name="graphics">The graphics device manager of the game.</param>
		/// <param name="effect">A BasicEffect instance.</param>
		/// <param name="camera">The camera.</param>
		/// 
		public void draw(
			GraphicsDeviceManager graphics,
			BasicEffect effect,
			Camera camera)
		{

			int range = 32;

			int minX = (int)((player.position.X / Tile.TILE_WIDTH) - range);
			int minZ = (int)((player.position.Z / Tile.TILE_WIDTH) - range);
			int maxX = (int)((player.position.X / Tile.TILE_WIDTH) + range);
			int maxZ = (int)((player.position.Z / Tile.TILE_WIDTH) + range);

			if (minX < 0) minX = 0;
			if (minZ < 0) minZ = 0;
			if (maxX >= width) maxX = width - 1;
			if (maxZ >= height) maxZ = height - 1;

			for (int x = minX; x <= maxX; x++)
				for (int z = minZ; z <= maxZ; z++)
				{
					this.tiles[x, z].draw(
						graphics,
						effect,
						camera,
						this,
						x,
						z);
				}


			// Draw the items in the world.
			foreach(Item item in items)
			{
				// Don't draw the item if no model is returned...
				if (item.getModel() == null) continue;


				Vector3 itemPosition = new Vector3(
					item.position.X,
					this.getHeightAtPoint(item.position.X, item.position.Y),
					item.position.Y);

				//TODO: Rotate the item based on the position normal.

				ModelResources.renderModel(
					item.getModel(),
					camera,
					itemPosition,
					new Vector3(0.0F, item.rotation, 0.0F),
					item.getCustomScaling());

			}


			// Draw the clouds
			Tile.renderClouds(graphics, effect, camera);

		}

	}

}
