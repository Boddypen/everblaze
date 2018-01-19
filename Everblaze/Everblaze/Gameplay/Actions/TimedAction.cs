using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Everblaze.Environment;
using Everblaze.Gameplay.Skills;
using Everblaze.Gameplay.Actions;
using Everblaze.Environment.Tiles;
using Everblaze.Environment.Items;
using Everblaze.Interface;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Everblaze.Miscellaneous;

namespace Everblaze.Gameplay.Actions
{
	public class TimedAction
	{

		public Action action;

		public float lifetime = 0.0F;
		public float age = 0.0F;

		public TimedAction(Action action,
						   SkillSet skills)
		{

			this.action = action;

			calculateTime(skills);

		}


		/// 
		/// <summary>
		///		Performs the action (finally!).
		/// </summary>
		/// 
		/// <param name="client">The network client that represents the game.</param>
		/// <param name="world">The world object.</param>
		/// <param name="skills">The skillset of the player performing the action.</param>
		/// 
		public void perform(
			NetClient client,
			World world,
			SkillSet skills,
			ref List<Notification> notifications)
		{

			Boolean fail = Program.random.NextDouble() > action.chance;
			
			
			switch(action.operation)
			{

				case Action.Operation.Examine:

					if (fail) break;

					switch (action.targetType)
					{
						case Action.TargetType.Tile:
							notifications.Add(new Notification(world.tiles[action.tileTarget.X, action.tileTarget.Y].getDescription()));
							break;

						case Action.TargetType.Item:
							notifications.Add(new Notification(action.targetItem.getDescription()));
							break;
					}

					break;
				

				case Action.Operation.Dig:

					skills.digging.increase(notifications);

					// Determine the tile the point is on top of.
					//TODO: Move this outside of the switch, so all 'cases' have access to it.
					int tileX = (int)(world.player.position.X / Tile.TILE_WIDTH);
					int tileZ = (int)(world.player.position.Z / Tile.TILE_WIDTH);

					// Force the player to fail if they're digging on a tile which doesn't allow it.
					fail = !world.tiles[tileX, tileZ].diggable;

					if (fail)
					{
						notifications.Add(new Notification("The ground is too hard to dig."));
						break;
					}
					
					// Now that we know the tile we're on, we need to find the quadrant.
					float tilePositionX = (world.player.position.X - (float)(tileX * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;
					float tilePositionZ = (world.player.position.Z - (float)(tileZ * Tile.TILE_WIDTH)) / Tile.TILE_WIDTH;

					Tile.TileCorner currentCorner = Tile.TileCorner.TopLeft;
					if (tilePositionX < 0.5F && tilePositionZ < 0.5F) currentCorner = Tile.TileCorner.TopLeft;
					else if (tilePositionX >= 0.5F && tilePositionZ < 0.5F) currentCorner = Tile.TileCorner.TopRight;
					else if (tilePositionX < 0.5F && tilePositionZ >= 0.5F) currentCorner = Tile.TileCorner.BottomLeft;
					else if (tilePositionX >= 0.5F && tilePositionZ >= 0.5F) currentCorner = Tile.TileCorner.BottomRight;

					world.tiles[tileX, tileZ].onDig(skills, world, tileX, tileZ);

					world.changeHeight(tileX, tileZ, currentCorner, -1);

					int minX = action.tileTarget.X - 1;
					int minZ = action.tileTarget.Y - 1;
					int maxX = action.tileTarget.X + 1;
					int maxZ = action.tileTarget.Y + 1;
					if (minX < 0) minX = 0;
					if (minZ < 0) minZ = 0;
					if (maxX >= world.width) maxX = world.width - 1;
					if (maxZ >= world.height) maxZ = world.height - 1;
					for (int x = minX; x <= maxX; x++)
						for (int z = minZ; z <= maxZ; z++)
							updateTile(world, client, x, z);

					notifications.Add(new Notification("You excavate some " + world.tiles[tileX, tileZ].getName() + "."));

					break;


				case Action.Operation.Forage:

					skills.foraging.increase(notifications);

					if (fail)
					{
						notifications.Add(new Notification("You fail to find anything of use."));
						break;
					}

					Item foragedItem = Item.generateForagedItem();

					world.player.inventory.store(foragedItem);
					world.tiles[action.tileTarget.X, action.tileTarget.Y].fruitful = false;
					updateTile(world, client, action.tileTarget.X, action.tileTarget.Y);

					notifications.Add(new Notification("You find a " + foragedItem.getName() + "!"));

					break;
					

				case Action.Operation.Cultivate:

					skills.farming.increase(notifications);
					skills.digging.increase(notifications);

					if (fail) break;

					world.replaceTile(action.tileTarget.X, action.tileTarget.Y, new DirtTile());
					updateTile(world, client, action.tileTarget.X, action.tileTarget.Y);

					notifications.Add(new Notification("The tile is now cultivated."));

					break;


				case Action.Operation.CutDown:

					skills.woodcutting.increase(notifications);

					if (fail) break;

					world.replaceTile(action.tileTarget.X, action.tileTarget.Y, new GrassTile());

					notifications.Add(new Notification("You cut down the tree."));

					break;


				default:
					Console.WriteLine("Unhandled action: '" + action.operation.ToString() + "'");
					return;
			}

		}


		/// 
		/// <summary>
		///		Sends the most current data from a tile in the world to the server, who will then
		///		update it correspondingly to the rest of the players.
		/// </summary>
		/// 
		/// <param name="world">The world.</param>
		/// <param name="client">The client.</param>
		/// <param name="tileX">The tile's X position.</param>
		/// <param name="tileZ">The tile's Z position.</param>
		/// 
		public void updateTile(World world, NetClient client, int tileX, int tileZ)
		{
			NetOutgoingMessage tileUpdate = client.CreateMessage();
			tileUpdate.Write(Game.NETWORK_TILE_UPDATE_REQUEST);
			NetworkHelper.writeTileData(world, tileX, tileZ, ref tileUpdate);

			client.SendMessage(tileUpdate, NetDeliveryMethod.ReliableOrdered);
		}
		

		public void calculateTime(SkillSet skills)
		{

			switch(action.operation)
			{
				case Action.Operation.Examine:
					lifetime = 0.0F;
					break;

				case Action.Operation.Dig:
					lifetime = 1.0F * skills.digging.getTimeMultiplier();
					break;

				case Action.Operation.Cultivate:
					lifetime = 4.0F * skills.farming.getTimeMultiplier();
					break;

				case Action.Operation.CutDown:
					lifetime = 7.5F * skills.woodcutting.getTimeMultiplier();
					break;

				case Action.Operation.Forage:
					lifetime = 5.0F * skills.foraging.getTimeMultiplier();
					break;

				default:
					lifetime = 0.0F;
					break;
			}

		}


		public float getPercentageCompleted()
		{
			return age / lifetime;
		}


		public Boolean tick()
		{

			// Increase the age of the action over time.
			age += 1.0F / 60.0F;

			// Once the age has reached the lifetime point, return true,
			// indicating that the action has been initiated.
			if (age >= lifetime)
			{
				return true;
			}

			return false;
		}

	}
}
