using System;
using System.Collections.Generic;
using System.IO;

using Everblaze.Environment;
using Everblaze.Environment.Items;
using Everblaze.Environment.Tiles;


namespace EverblazeServer
{
	public class ServerConfiguration
	{
		public const int
			CONFIG_WORLD_WIDTH = 0,
			CONFIG_WORLD_HEIGHT = 1;


		/// <summary>
		///		The path to the configuration profile's configuration profile file.
		/// </summary>
		public String configurationFilePath;

		/// <summary>
		///		The width of the world, in tiles.
		/// </summary>
		public int worldWidth;
		/// <summary>
		///		The height of the world, in tiles.
		/// </summary>
		public int worldHeight;


		public ServerConfiguration(
			String configurationFilePath,
			ref World world)
		{
			this.configurationFilePath = configurationFilePath;


			if (File.Exists(configurationFilePath))
			{
				// Attempt to load the existing configuration.

				updateFromConfiguration();

				loadWorld(ref world);
			}
			else
			{
				// Generate a new configuration.

				createNewConfiguration();
				updateFromConfiguration();

				createWorld(ref world);
			}

		}


		public void updateFromConfiguration()
		{

			String[] configData = File.ReadAllLines(configurationFilePath);

			this.worldWidth = int.Parse(configData[CONFIG_WORLD_WIDTH]);
			this.worldHeight = int.Parse(configData[CONFIG_WORLD_HEIGHT]);

		}


		public void createNewConfiguration()
		{

			String[] configData = new String[64];

			configData[CONFIG_WORLD_WIDTH] = 64.ToString();
			configData[CONFIG_WORLD_HEIGHT] = 64.ToString();

			Directory.CreateDirectory(Path.GetDirectoryName(configurationFilePath));
			File.WriteAllLines(configurationFilePath, configData);
			
		}


		public void saveConfiguration()
		{
			String[] configData = new String[64];

			// Write the config data.
			configData[CONFIG_WORLD_WIDTH] = this.worldWidth.ToString();
			configData[CONFIG_WORLD_HEIGHT] = this.worldHeight.ToString();

			// Then write the compiled data to the file.
			Directory.CreateDirectory(Path.GetDirectoryName(configurationFilePath));
			File.WriteAllLines(configurationFilePath, configData);
		}


		/// 
		/// <summary>
		///		Uses the configuration to load existing world data from the file system.
		/// </summary>
		/// 
		/// <param name="world">The world.</param>
		/// 
		public void loadWorld(ref World world)
		{
			
			this.updateFromConfiguration();

			// Create the world object.
			world = new World(this.worldWidth, this.worldHeight);

			// Load the world tiles.
			loadWorldTiles(ref world.tiles);

			// Load the world items.
			loadWorldItems(ref world.items);

		}


		public void createWorld(ref World world)
		{
			this.updateFromConfiguration();

			// Create the world object.
			world = new World(this.worldWidth, this.worldHeight);

			world.generate();

			writeWorldTiles(ref world.tiles);
		}

		public void loadWorldTiles(ref Tile[,] tiles)
		{
			
			int index = 0;

			for(int x = 0; x < worldWidth; x++)
			{
				for(int z = 0; z < worldHeight; z++)
				{
					String tileFileName = "t" + index.ToString("x") + ".dat";

					tiles[x, z] = Tile.readFromFile(Path.Combine(Path.GetDirectoryName(configurationFilePath), "tiles", tileFileName));

					index++;
				}
			}

		}


		public void writeWorldTiles(ref Tile[,] tiles)
		{
			int index = 0;

			Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(configurationFilePath), "tiles"));

			for (int x = 0; x < worldWidth; x++)
			{
				for (int z = 0; z < worldHeight; z++)
				{
					String tileFileName = "t" + index.ToString("x") + ".dat";
					
					Tile.writeToFile(Path.Combine(Path.GetDirectoryName(configurationFilePath), "tiles", tileFileName), tiles[x, z]);

					index++;
				}
			}
		}

		public void loadWorldItems(ref List<Item> items)
		{

			// Start off by clearing the list, we dont' want duplicates.
			items.Clear();

			if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(configurationFilePath), "items")))
				return;


			// Go through every file in the items directory and add it to the list of items.
			foreach(String file in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(configurationFilePath), "items")))
			{
				try
				{
					items.Add(Item.readFromFile(file));
				}
				catch { }
			}
		}

		public void writeWorldItems(ref List<Item> items)
		{

			for(int i = 0; i < items.Count; i++)
			{
				Item item = items[i];

				// Perform a null check in-case something scary happens :S
				if(item != null)
				{
					String itemFileName = "i" + i.ToString("x") + ".dat";

					Item.writeToFile(Path.Combine(Path.GetDirectoryName(configurationFilePath), "items", itemFileName), item);
				}
			}

		}

	}
}
