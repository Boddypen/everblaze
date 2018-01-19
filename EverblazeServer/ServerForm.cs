using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Everblaze;
using Everblaze.Environment;
using Everblaze.Environment.Items;
using Everblaze.Environment.Tiles;

using Lidgren.Network;

using Microsoft.Xna.Framework;

using static System.Windows.Forms.ListView;


namespace EverblazeServer
{
	public partial class ServerForm : Form
	{

		/// <summary>
		///		RNG.
		/// </summary>
		public static Random random;

		/// <summary>
		///		Whether or not the server is running.
		/// </summary>
		public Boolean serverRunning = false;

		/// <summary>
		///		The password which must be supplied to connect to the server.
		/// </summary>
		public String password = "foobar";


		#region Networking

		/// <summary>
		///		The network configuration.
		/// </summary>
		public NetPeerConfiguration netConfig;

		/// <summary>
		///		The server itself!
		/// </summary>
		public NetServer server;

		#endregion


		#region File System

		/// <summary>
		///		The directory to store the server data in, such
		///		as world information and player stats.
		/// </summary>
		public String serverDataDirectory;

		/// <summary>
		///		The server configuration.
		/// </summary>
		public ServerConfiguration serverConfiguration;

		#endregion


		#region Game

		/// <summary>
		///		The server game world, which all clients will follow.
		/// </summary>
		public World world;

		#endregion


		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="ServerForm"/> class.
		/// </summary>
		/// 
		/// <param name="autoStart">Whether or not to automatically start the server.</param>
		/// 
		public ServerForm(Boolean autoStart)
		{
			InitializeComponent();


			// Set up the file system.
			serverDataDirectory = Path.Combine(Environment.CurrentDirectory, "server_data");
			

			// Set up the RNG.
			random = new Random();


			#region Networking Setup

			// Set up the config.
			netConfig = new NetPeerConfiguration("Everblaze");
			netConfig.Port = 1945;
			netConfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
			netConfig.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			netConfig.PingInterval = 1.5F;
			netConfig.ConnectionTimeout = 8.0F;

			// Set up the server object.
			server = new NetServer(netConfig);

			ServerTimer.Interval = 17;

			#endregion


			#region Game Setup

			// Set up the server configuration.
			// This will automatically load existing server data or create new data.
			serverConfiguration = new ServerConfiguration(
				Path.Combine(serverDataDirectory, "main.cfg"),
				ref world);

			#endregion


			if (autoStart)
			{
				log("INFO", "Automatic startup command received. Starting automatically.");

				Thread.Sleep(750);
				StartMenuItem_Click(null, null);
			}

		}
		

		/// 
		/// <summary>
		///		Handles the Tick event of the ServerTimer control.
		///		This is where the server logic is performed.
		/// </summary>
		/// 
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		/// 
		private void ServerTimer_Tick(Object sender, EventArgs e)
		{

			// Update the clients list.
			ListViewItemCollection players = new ListViewItemCollection(PlayersView);
			players.Clear();
			foreach (NetConnection client in server.Connections)
			{
				players.Add(new ListViewItem(new String[] { client.Status.ToString(), client.RemoteEndPoint.Address.ToString() }));
			}

			// Read all incoming messages.
			NetIncomingMessage message;
			int messageCount = 0;
			
			while((message = server.ReadMessage()) != null)
			{
				messageCount++;

				switch (message.MessageType)
				{
					case NetIncomingMessageType.VerboseDebugMessage:
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.ErrorMessage:

						log("Info", message.ReadString());

						break;
						

					case NetIncomingMessageType.Data:

						switch(message.ReadUInt32())
						{

							// Client requested world refresh.
							case Game.NETWORK_REFRESH_REQUEST:
								sendWorldRefresh(message.SenderConnection);
								log("Info", "Sent a world refresh to " + message.SenderConnection);
								break;


							case Game.NETWORK_TILE_UPDATE_REQUEST:

								int tileX = message.ReadInt32();
								int tileZ = message.ReadInt32();

								Tile updatedTile = Tile.readFromNetwork(ref message);

								world.tiles[tileX, tileZ] = updatedTile;

								log("Info", "Received new tile data at " + tileX + ", " + tileZ + ". (" + world.tiles[tileX, tileZ].getName() + ")");

								foreach (NetConnection client in server.Connections)
								{
									NetOutgoingMessage tileUpdateResponse = server.CreateMessage();
									tileUpdateResponse.Write(Game.NETWORK_TILE_UPDATE_RESPONSE);
									tileUpdateResponse.Write(tileX);
									tileUpdateResponse.Write(tileZ);
									writeTileData(tileX, tileZ, ref tileUpdateResponse);

									server.SendMessage(tileUpdateResponse, client, NetDeliveryMethod.ReliableOrdered);

									//log("Info", "Relaying tile data to " + client.RemoteEndPoint.Address.ToString());

									//sendWorldRefresh(client);
								}
								break;


							case Game.NETWORK_NEW_ITEM_REQUEST:

								float itemX = message.ReadFloat();
								float itemZ = message.ReadFloat();

								Item newItem = Item.readFromNetwork(ref message);

								newItem.position = new Vector2(itemX, itemZ);

								world.items.Add(newItem);

								foreach(NetConnection client in server.Connections)
								{
									NetOutgoingMessage newItemResponse = server.CreateMessage();
									newItemResponse.Write(Game.NETWORK_NEW_ITEM_RESPONSE);
									newItemResponse.Write(itemX);
									newItemResponse.Write(itemZ);
									writeItemData(newItem, ref newItemResponse);

									server.SendMessage(newItemResponse, client, NetDeliveryMethod.ReliableOrdered);

									log("Info", "Relaying item data to " + client.RemoteEndPoint.Address.ToString());
								}

								break;


							default:
								log("Error", "Unknown message from client!");
								break;

						}

						break;

					case NetIncomingMessageType.ConnectionApproval:
						message.SenderConnection.Approve();
						break;

					case NetIncomingMessageType.DiscoveryRequest:

						NetOutgoingMessage response = server.CreateMessage();
						server.SendDiscoveryResponse(response, message.SenderEndPoint);

						break;

					case NetIncomingMessageType.StatusChanged:
						log("Net", "Server->Client status changed: " + message.SenderConnection.Status);
						break;

					default:
						log("Error", "Unhandled message type: " + message.MessageType);
						break;
				}

				server.Recycle(message);
			}

		}


		/// 
		/// <summary>
		///		Logs a message to the server output box.
		/// </summary>
		/// 
		/// <param name="text">The text to log.</param>
		/// 
		public void log(String type, String text)
		{
			ServerOutputBox.AppendText(Environment.NewLine + "[" + DateTime.Now.ToShortTimeString() + "]  [" + type.ToUpper() + "] "+ text);
		}


		/// 
		/// <summary>
		///		Sends the entire world's data to a client.
		///		This would normally be done during an initial connection.
		/// </summary>
		/// 
		/// <param name="client">The <see cref="NetConnection"/> who is requesting the refresh.</param>
		/// 
		public void sendWorldRefresh(NetConnection client)
		{

			// Create the message.
			NetOutgoingMessage refresh = server.CreateMessage();

			refresh.Write(Game.NETWORK_REFRESH_RESPONSE);

			// Determine the world width & height.
			refresh.Write(world.width);
			refresh.Write(world.height);

			// Write all tile data into the refresh.
			for(int x = 0; x < world.width; x++)
			{
				for(int z = 0; z < world.height; z++)
				{
					writeTileData(x, z, ref refresh);
				}
			}
			
			// Finally, send the message.
			server.SendMessage(refresh, client, NetDeliveryMethod.ReliableOrdered);

			foreach(Item item in world.items)
			{
				NetOutgoingMessage itemRefresh = server.CreateMessage();
				itemRefresh.Write(Game.NETWORK_NEW_ITEM_RESPONSE);
				itemRefresh.Write(item.position.X);
				itemRefresh.Write(item.position.Y);
				writeItemData(item, ref itemRefresh);

				server.SendMessage(itemRefresh, client, NetDeliveryMethod.ReliableOrdered);
			}
		}


		public void writeItemData(Item item, ref NetOutgoingMessage message)
		{
			message.Write(item.networkID);

			message.Write(item.quality);
			message.Write(item.damage);

			item.writeCustomNetworkProperties(ref message);
		}


		public void writeTileData(int tileX, int tileZ, ref NetOutgoingMessage message)
		{
			message.Write(world.tiles[tileX, tileZ].networkID);

			int[] heights = world.tiles[tileX, tileZ].heights;
			message.Write(heights[0]);
			message.Write(heights[1]);
			message.Write(heights[2]);
			message.Write(heights[3]);

			world.tiles[tileX, tileZ].writeCustomProperties(ref message);
		}

		private void QuitMenuItem_Click(Object sender, EventArgs e)
		{
			//TODO: Close confirmation.
			this.Close();
		}

		private void StartMenuItem_Click(Object sender, EventArgs e)
		{
			// Start the server.
			if (serverRunning) return;

			server.Start();
			serverRunning = true;

			ServerTimer.Start();

			log("Info", "Server starting...");

			StartMenuItem.Enabled = false;
			StopMenuItem.Enabled = true;
		}

		private void StopMenuItem_Click_1(Object sender, EventArgs e)
		{
			server.Shutdown("Server closed");

			StartMenuItem.Enabled = true;
			StopMenuItem.Enabled = false;

			serverRunning = false;
		}

		private void clearServerLogToolStripMenuItem_Click(Object sender, EventArgs e)
		{
			ServerOutputBox.Clear();
		}


		/// 
		/// <summary>
		///		Handles the FormClosing event of the ServerForm control.
		///		The server world will be saved here.
		/// </summary>
		/// 
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
		/// 
		private void ServerForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			// Save the tiles.
			serverConfiguration.writeWorldTiles(ref world.tiles);

			// Save the items.
			serverConfiguration.writeWorldItems(ref world.items);
		}
	}
}
