using System;
using System.Collections.Generic;
using System.IO;
using Everblaze.Gameplay.Skills;
using Everblaze.Resources;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everblaze.Environment.Items
{

	/// 
	/// <summary>
	///		A class representing a single item in the game world.
	/// </summary>
	/// 
	public class Item
	{
		// Network identifiers for different types of items.
		public const int
			NETWORK_BRANCH = 1,
			NETWORK_CORN = 2,
			NETWORK_DIRT = 3,
			NETWORK_HATCHET = 4,
			NETWORK_PLANK = 5,
			NETWORK_POTATO = 6,
			NETWORK_SHOVEL = 7,
			NETWORK_SMALL_BARREL = 8,
			NETWORK_SAND = 9;

		public int networkID;

		public enum Material
		{
			OakWood,
			Rock,
			None
		}


		/// <summary>
		///		The damage the item has sustained, representing wear on a
		///		tool, or decay on a piece of food.
		///		Clamped between 0.0F and 100.0F.
		/// </summary>
		public float damage;

		/// <summary>
		///		The quality of the item.
		///		Clamped between 1.0F and 100.0F.
		/// </summary>
		public float quality;

		/// <summary>
		///		The weight of the item.
		///		Unit: <c>kg</c>
		/// </summary>
		public float weight;

		/// <summary>
		///		The volume of the item.
		///		Unit: <c>L</c>
		/// </summary>
		public float volume;

		/// <summary>
		///		The name of the item.
		/// </summary>
		public String name;


		/// <summary>
		///		Whether or not this item is wooden.
		/// </summary>
		public Boolean wooden;


		/// <summary>
		///		The material of which this item comprises of.
		/// </summary>
		public Material material;


		/// <summary>
		///		The location of the item, in the game world.
		///		Note: Only represents X & Z values. Y value will be determined by height of the tile.
		/// </summary>
		public Vector2 position;
		/// <summary>
		///		The rotation of the item, in the game world.
		/// </summary>
		public float rotation;



		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Item"/> class.
		/// </summary>
		/// 
		/// <param name="material">
		///		The material of which the item is made of.
		///		Can be <see cref="Material.None"/> to indicate no material.
		///	</param>
		/// 
		public Item(Material material, float quality, float damage)
		{

			// Assign default values.
			this.damage = damage;
			this.quality = quality;
			this.weight = 1.0F;

			this.wooden = false;
			
			this.volume = 0.0F;

			this.material = material;

			this.name = "unknown";

			this.rotation = (float)Program.random.NextDouble() * 359.0F;

		}
		

		/// 
		/// <summary>
		///		Generates a random item which can be obtained
		///		through <see cref="Gameplay.Actions.Action.Operation.Forage"/>.
		/// </summary>
		/// 
		/// <returns>
		///		A randomised <see cref="Item"/> which is obtainable through foraging.
		/// </returns>
		/// 
		public static Item generateForagedItem()
		{

			// Decide on a quality for the foraged item.
			float quality = 1.0F + (float)(Program.random.NextDouble() * 89.0F);


			List<Item> foragables = new List<Item>();
			foragables.Add(new PotatoItem(Material.None, quality, 0.0F));
			foragables.Add(new BranchItem(Material.None, quality, 0.0F));
			foragables.Add(new CornItem(Material.None, quality, 0.0F));


			int i = Program.random.Next(foragables.Count);
			return foragables[i];

		}


		#region Networking & File System

		/// 
		/// <summary>
		///		Reads an <see cref="Item"/> from a <see cref="NetIncomingMessage"/>.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetIncomingMessage"/>.</param>
		/// 
		/// <returns>
		///		A new <see cref="Item"/>, based on data received from the server.
		/// </returns>
		/// 
		public static Item readFromNetwork(ref NetIncomingMessage message)
		{
			Item item = Item.readFromID(message.ReadInt32());

			item.quality = message.ReadFloat();
			item.damage = message.ReadFloat();

			item.readCustomNetworkProperties(ref message);

			return item;
		}


		/// 
		/// <summary>
		///		Reads any custom properties from a <see cref="NetIncomingMessage"/>.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetIncomingMessage"/> received from the server.</param>
		/// 
		public virtual void readCustomNetworkProperties(ref NetIncomingMessage message)
		{
			return;
		}


		/// 
		/// <summary>
		///		Writes any custom properties to a <see cref="NetOutgoingMessage"/>.
		/// </summary>
		/// 
		/// <param name="message">The <see cref="NetOutgoingMessage"/> to be sent to the server.</param>
		/// 
		public virtual void writeCustomNetworkProperties(ref NetOutgoingMessage message)
		{
			return;
		}


		/// 
		/// <summary>
		///		Reads any custom properties from a <see cref="String"/> array, usually stored in a file.
		/// </summary>
		/// 
		/// <param name="customProperties">The raw custom properties data, in an array.</param>
		/// 
		public virtual void readCustomFileProperties(String[] customProperties)
		{
			return;
		}


		/// 
		/// <summary>
		///		Gets the <see cref="String"/> to write to the disk when saving item information.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="String"/>
		/// </returns>
		/// 
		public virtual String getCustomFileProperties()
		{
			return "";
		}


		/// 
		/// <summary>
		///		Parses a network ID (<see cref="Item.networkID"/>) into a <see cref="Item"/> instance.
		///		This is used when reading the network ID from a server of file, regarding items.
		/// </summary>
		/// 
		/// <param name="networkID">The network identifier to parse.</param>
		/// 
		/// <returns>
		///		An <see cref="Item"/> instance.
		/// </returns>
		/// 
		public static Item readFromID(int networkID)
		{
			// Default item, incase of errors:
			Item item = new Item(Material.None, 1.0F, 0.0F);

			// Determine what the item actually is.
			switch (networkID)
			{
				case NETWORK_BRANCH:
					item = new BranchItem(Material.OakWood, 1.0F, 0.0F);
					break;

				case NETWORK_CORN:
					item = new CornItem(Material.None, 1.0F, 0.0F);
					break;

				case NETWORK_DIRT:
					item = new DirtItem(Material.None, 1.0F, 0.0F);
					break;

				case NETWORK_HATCHET:
					item = new HatchetItem(Material.OakWood, 1.0F, 0.0F);
					break;

				case NETWORK_PLANK:
					item = new PlankItem(Material.OakWood, 1.0F, 0.0F);
					break;

				case NETWORK_POTATO:
					item = new PotatoItem(Material.None, 1.0F, 0.0F);
					break;

				case NETWORK_SHOVEL:
					item = new ShovelItem(Material.OakWood, 1.0F, 0.0F);
					break;

				case NETWORK_SMALL_BARREL:
					item = new SmallBarrelItem(Material.OakWood, 1.0F, 0.0F);
					break;

				case NETWORK_SAND:
					item = new SandItem(Material.None, 1.0F, 0.0F);
					break;
			}

			return item;
		}


		/// 
		/// <summary>
		///		Writes an item's data to a file.
		/// </summary>
		/// 
		/// <param name="fileName">Name of the file.</param>
		/// <param name="item">The item.</param>
		/// 
		public static void writeToFile(
			String fileName,
			Item item)
		{

			String[] itemData = new String[]
			{
				// Network ID
				item.networkID.ToString(),

				// Quality, Damage, Weight
				item.quality.ToString(),
				item.damage.ToString(),
				item.weight.ToString(), // <-- Weight must also be saved, as items may change weight in different situations.

				// Position & Rotation
				item.position.X.ToString() + (char)(0xFF) +
				item.position.Y.ToString() + (char)(0xFF) +
				item.rotation.ToString(),
				
				// Custom Properties
				item.getCustomFileProperties()
			};


			Directory.CreateDirectory(Path.GetDirectoryName(fileName));
			File.WriteAllLines(fileName, itemData);

		}


		/// 
		/// <summary>
		///		Reads item data from a file.
		/// </summary>
		/// 
		/// <param name="fileName">Name/path of the file.</param>
		/// 
		/// <returns>
		///		A <see cref="Item"/> instance, based on the data in the file.
		/// </returns>
		/// 
		public static Item readFromFile(
			String fileName)
		{

			// Get the file's data.
			String[] fileData = File.ReadAllLines(fileName);

			Item item = Item.readFromID(int.Parse(fileData[0]));
			

			// Quality, Damage, Weight
			item.quality = float.Parse(fileData[1]);
			item.damage = float.Parse(fileData[2]);
			item.weight = float.Parse(fileData[3]);


			// Position
			String[] positionData = fileData[4].Split((char)0xFF);
			item.position = new Vector2(
				float.Parse(positionData[0]),
				float.Parse(positionData[1]));

			// Rotation
			item.rotation = float.Parse(positionData[2]);


			// Custom Properties
			item.readCustomFileProperties(fileData[5].Split((char)0xFF));

			return item;
		}

		#endregion


		public virtual void addActions(
			ref List<Gameplay.Actions.Action> actionList,
			ref Item targetItem,
			SkillSet skills,
			Item tool)
		{

			// Examine
			actionList.Add(
				new Gameplay.Actions.Action(
					Gameplay.Actions.Action.Operation.Examine,
					skills,
					ref targetItem));

			return;
		}


		/// 
		/// <summary>
		///		Gets the name of the material of which this item is comprised of.
		/// </summary>
		/// 
		public String getMaterialName()
		{
			switch(this.material)
			{

				case Material.OakWood:
					return "oak";

				case Material.Rock:
					return "rock";

				case Material.None:
					return null;

				default:
					return null;

			}
		}


		/// 
		/// <summary>
		///		Gets the name of the item.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="String"/> representing the name of the item.
		/// </returns>
		/// 
		public virtual String getName()
		{
			// Get the base name of the item.
			String name = this.name;

			// If there is a modifier, add it to the end of the name.
			if(this.getMaterialName() != null)
				name = name + ", " + this.getMaterialName();

			// Return it.
			return name;
		}


		/// 
		/// <summary>
		///		Gets a short description of the item.
		/// </summary>
		/// 
		/// <returns>
		///		A short description of the item, in a <see cref="String"/>;
		/// </returns>
		/// 
		public virtual String getDescription()
		{
			return "A completely blank item, made of dark matter.";
		}


		/// 
		/// <summary>
		///		Gets the sprite's rectangle on the items spritesheet.
		///		Link: <see cref="TextureResources.itemsSpritesheet"/>
		/// </summary>
		/// 
		public virtual Rectangle getSpriteRectangle()
		{
			return new Rectangle(1, 1, 15, 15);
		}


		/// 
		/// <summary>
		///		Gets the <see cref="Model"/> to use when drawing the item in the game world.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="Model"/> instance.
		/// </returns>
		/// 
		public virtual Model getModel()
		{
			return null;
		}


		/// 
		/// <summary>
		///		This method can be overrided to allow for custom scaling options for the item's model.
		/// </summary>
		/// 
		/// <returns>
		///		A <see cref="Vector3"/> indicating the scaling of the item's model.
		/// </returns>
		/// 
		public virtual Vector3 getCustomScaling()
		{
			return Vector3.One;
		}

	}

}
