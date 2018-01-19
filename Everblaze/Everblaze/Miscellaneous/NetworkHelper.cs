using System;
using System.Collections.Generic;
using Everblaze.Environment;
using Everblaze.Environment.Items;
using Lidgren.Network;

namespace Everblaze.Miscellaneous
{
	public class NetworkHelper
	{

		/// 
		/// <summary>
		///		Writes the correct tile data to a <see cref="NetOutgoingMessage"/>
		///		to be sent to a game server.
		/// </summary>
		/// 
		/// <param name="world">The game world.</param>
		/// <param name="tileX">The tile's X position.</param>
		/// <param name="tileZ">The tile's Z position.</param>
		/// <param name="message">The <see cref="NetOutgoingMessage"/> to add to.</param>
		/// 
		public static void writeTileData(
			World world,
			int tileX,
			int tileZ,
			ref NetOutgoingMessage message)
		{

			message.Write(tileX);
			message.Write(tileZ);

			message.Write(world.tiles[tileX, tileZ].networkID);

			int[] heights = world.tiles[tileX, tileZ].heights;
			message.Write(heights[0]);
			message.Write(heights[1]);
			message.Write(heights[2]);
			message.Write(heights[3]);

			world.tiles[tileX, tileZ].writeCustomProperties(ref message);

		}


		/// 
		/// <summary>
		///		Creates a new item on the server's end.
		/// </summary>
		/// 
		/// <param name="item">The item to add.</param>
		/// <param name="client">
		///		The local <see cref="NetClient"/> object which is used to correctly send the network message.
		///	</param>
		/// 
		public static void addNewItem(
			Item item,
			NetClient client)
		{

			NetOutgoingMessage message = client.CreateMessage();

			message.Write(Game.NETWORK_NEW_ITEM_REQUEST);

			message.Write(item.position.X);
			message.Write(item.position.Y);

			message.Write(item.networkID);

			message.Write(item.quality);
			message.Write(item.damage);

			item.writeCustomNetworkProperties(ref message);

			client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);

		}

	}
}
