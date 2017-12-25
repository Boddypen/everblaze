using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Everblaze.Environment;
using Everblaze.Environment.Entities;
using Everblaze.Environment.Tiles;
using Everblaze.Gameplay.Actions;
using Everblaze.Interface;
using Everblaze.Miscellaneous;
using Everblaze.Resources;

using Lidgren.Network;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


//IDEAS: So the inventory is always hidden, only shows when pressing TAB to bring up the big inventory screen.
//IDEAS: When items are added into the inventory, display a message in the top-right corner, which fades away. e.g. "Corn added"
//IDEAS: When skill milestones are reached, e.g. Foraging 25, Digging 50, Masonry 75, etc: Display a message notifying the player they have reached a new tier of that skill. These could be called 'Novice', 'Apprentice', etc like in skyrim or oblivion.

//TODO: Jumping
//TODO: Sprinting

//SHIT: Large worlds crash the UDP system due to a single, large message (split up the world into small sections?)


namespace Everblaze
{

	/// 
	/// <summary>
	///		Everblaze - An MMORPG Sandbox game.
	/// </summary>
	/// 
	/// <remarks>
	///		Created by Marcus Kirkwood, 2017
	///		Software Design and Development
	///		Major HSC Project
	/// </remarks>
	/// 
	/// <seealso cref="Microsoft.Xna.Framework.Game" />
	/// 
	public class Game : Microsoft.Xna.Framework.Game
	{

		#region Constant Things

		/// <summary>
		///		The name of the game.
		/// </summary>
		public const String GAME_NAME = "Everblaze";

		/// <summary>
		///		The game's current version.
		/// </summary>
		public const String GAME_VERSION = "d0.10";

		/// <summary>
		///		Credits of the game.
		/// </summary>
		public const String GAME_CREDITS = "Made by Marcus Kirkwood";

		/// <summary>
		///		The graphics-wise scale of the game.
		/// </summary>
		public const int SCALE = 1;

		
		public const int
			NETWORK_REFRESH_REQUEST = 0,
			NETWORK_REFRESH_RESPONSE = 1,
			NETWORK_TILE_UPDATE_REQUEST = 2,
			NETWORK_TILE_UPDATE_RESPONSE = 3;

		#endregion


		#region Enumerables

		/// <summary>
		///		The state in which the game is current running in.
		/// </summary>
		public enum GameState
		{

			/// <summary>
			///		The game is running normally.
			/// </summary>
			Running,

			/// <summary>
			///		The inventory/crafting screen is open.
			/// </summary>
			RunningInventory,

			/// <summary>
			///		The game wants to close.
			/// </summary>
			Quitting,

			/// <summary>
			///		The game is currently <i>not</i> the active application,
			///		and shouldn't respond to user input.
			/// </summary>
			Unfocused,
			
			ConnectionFailed,
			Connecting,
			ConnectingLoading

		}

		// The game's GameState enum.
		public GameState gameState = GameState.Connecting;

		#endregion


		#region Input

		// The keyboard and mouse states.
		public KeyboardState k, kOld;
		public MouseState m, mOld;


		// The sensitivity of looking around in the 3D environment.
		public static float lookSensitivity = 0.50F;

		// Whether or not the mouse cursor is locked to the centre of the screen.
		public Boolean mouseLocked = true;

		#endregion


		#region XNA

		// The game's graphics device manager
		public GraphicsDeviceManager graphics;

		// SpriteBatch
		public SpriteBatch spriteBatch;

		// A basic effect
		public BasicEffect basicEffect;

		#endregion


		#region 3D

		// Test floor tile.
		public VertexPositionTexture[] tileVertices;


		// The camera
		public Camera camera;

		#endregion


		#region Data

		// RNG
		private Random random;

		/// <summary>
		///		The game world.
		/// </summary>
		public World world;


		/// <summary>
		///		The list of game notifications.
		/// </summary>
		public List<Notification> notifications;


		// Indicator thing at the top of the screen.
		public float indicatorOffset = 0.0F;


		/// <summary>
		///		The action the player is currently performing.
		///		May be null to indicate no current action.
		/// </summary>
		public TimedAction currentAction = null;


		/// <summary>
		///		Whether or not the player can toggle between
		///		<see cref="GameState.Running"/> and <see cref="GameState.RunningInventory"/>.
		/// </summary>
		public Boolean canToggleInventory = true;
		
		#endregion


		#region Tile Cursor

		/// <summary>
		///		Whether or not the tile cursor is hovering over a tile.
		/// </summary>
		public Boolean tileCursorEnabled = false;

		/// <summary>
		///		The position of the tile cursor.
		/// </summary>
		public Point tileCursorPosition;

		/// <summary>
		///		Whether or not the player can initiate a context menu.
		/// </summary>
		public Boolean canOpenContextMenu = false;

		public ContextMenu contextMenu = null;

		#endregion


		#region Networking

		/// <summary>
		///		The network configuration for this client.
		/// </summary>
		public NetPeerConfiguration netConfig;

		/// <summary>
		///		This game instance's network client object.
		/// </summary>
		public NetClient client;

		/// <summary>
		///		The latency to the game server.
		/// </summary>
		public float ping = 0.0F;

		public String serverIP;

		public int connectionAttemptTimer = 0;
		public int connectionAttempts = 0;
		
		#endregion

		/// 
		/// <summary>
		///		Initializes a new instance of the <see cref="Game"/> class.
		/// </summary>
		/// 
		/// <param name="IP">The IP address of the server to attempt connection with.</param>
		/// 
		public Game(String IP)
		{

			// Create the most important thing in the entire game - the RNG.
			random = new Random();


			// Set up the graphics device manager.
			graphics = new GraphicsDeviceManager(this);

			// Set the initial window size to a 4:3 window.
			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
			graphics.IsFullScreen = false;


			// Set the content root directory.
			Content.RootDirectory = "data";

			// Make sure the cursor is visible...
			this.IsMouseVisible = true;


			#region Networking Setup

			// Set up the networking configuration.
			netConfig = new NetPeerConfiguration("Everblaze");
			netConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

			// Set up the actual client object.
			client = new NetClient(netConfig);
			client.Start();

			this.serverIP = IP;
			
			#endregion
			
		}


		/// 
		/// <summary>
		///		Handles all incoming network messages which may be
		///		queued at the client.
		/// </summary>
		/// 
		public void handleNetworkMessages()
		{
			try
			{
				ping = client.ServerConnection.AverageRoundtripTime * 1000.0F;
			}
			catch
			{
				ping = 0.0F;
			}

			NetIncomingMessage message;
			while ((message = client.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.DiscoveryResponse:
						Console.WriteLine("Connected to server: " + message.SenderEndPoint.Address + ":" + message.SenderEndPoint.Port);

						NetOutgoingMessage hailMessage = client.CreateMessage();
						hailMessage.Write(System.Environment.UserName);

						client.Connect(message.SenderEndPoint, hailMessage);
						break;

					case NetIncomingMessageType.Data:

						switch (message.ReadInt32())
						{
							// World refresh
							case NETWORK_REFRESH_RESPONSE:
								
								int worldWidth = message.ReadInt32();
								int worldHeight = message.ReadInt32();


								// If possible, copy the old player data.
								Player existingPlayer = null;
								if (world != null)
									existingPlayer = world.player;


								world = new World(random, worldWidth, worldHeight);

								for(int x = 0; x < worldWidth; x++)
								{
									for(int z = 0; z < worldHeight; z++)
									{
										world.tiles[x, z] = Tile.readFromNetwork(random, ref message);
									}
								}

								if(existingPlayer != null)
									world.player = existingPlayer;

								break;


							// Tile update
							case NETWORK_TILE_UPDATE_RESPONSE:

								int tileX = message.ReadInt32();
								int tileZ = message.ReadInt32();

								world.tiles[tileX, tileZ] = Tile.readFromNetwork(random, ref message);

								break;
						}

						break;

					case NetIncomingMessageType.VerboseDebugMessage:
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.WarningMessage:
					case NetIncomingMessageType.ErrorMessage:

						Console.WriteLine(message.ReadString());

						break;

					default:
						Console.WriteLine("Unhandled message: " + message.MessageType);
						break;
				}

				client.Recycle(message);
			}

			return;
		}


		/// 
		/// <summary>
		///		Called after the Game and GraphicsDevice are created, but before LoadContent.
		/// </summary>
		/// 
		protected override void Initialize()
		{

			// Set up the camera.
			camera = new Camera(
				new Vector3(2.0F, 1.7F, 6.0F),
				new Vector3(2.0F, 0.0F, 2.0F));
			
			// Set up the tile cursor.
			tileCursorEnabled = false;
			tileCursorPosition = new Point(0, 0);


			// Set up the BasicEffect
			basicEffect = new BasicEffect(GraphicsDevice);
			

			// Create the list of notifications.
			notifications = new List<Notification>();
			notifications.Add(new Notification("Welcome to " + GAME_NAME + "."));


			// Detect the initial keyboard and mouse states.
			k = Keyboard.GetState();
			m = Mouse.GetState();
			kOld = k;
			mOld = m;


			base.Initialize();
		}


		/// 
		/// <summary>
		///		Loads all game content.
		/// </summary>
		/// 
		protected override void LoadContent()
		{

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load all font resources.
			FontResources.loadContent(Content);

			// Load all texture resources.
			TextureResources.loadContent(Content);

			// Load all model resources.
			ModelResources.loadContent(Content);

		}


		/// 
		/// <summary>
		///		Performs an update tick on the game object.
		/// </summary>
		/// 
		/// <param name="gameTime">Time passed since the last call to Update.</param>
		/// 
		protected override void Update(GameTime gameTime)
		{
			
			// Get the keyboard/mouse states.
			k = Keyboard.GetState();
			m = Mouse.GetState();
			
			// Make sure the game switches to an unfocused state when not active.
			
			if (!this.IsActive)
			{
				if (this.gameState.Equals(GameState.Running))
				{
					this.gameState = GameState.Unfocused;
				}

				this.IsMouseVisible = true;
			}
			else if(gameState.Equals(GameState.RunningInventory))
			{
				this.IsMouseVisible = true;
			}
			else
			{
				this.IsMouseVisible = false;
			}


			handleNetworkMessages();

			switch (this.gameState)
			{

				// The client and server has connected successfully, now waiting for the game data to be loaded over.
				case GameState.ConnectingLoading:

					if(world != null)
					{
						gameState = GameState.Running;
					}

					break;


				// After many attempts, the connection to the server has failed.
				case GameState.ConnectionFailed: break;


				// Attempts are being made to connect to the server.
				case GameState.Connecting:
					
					if (client.ConnectionStatus.Equals(NetConnectionStatus.Connected))
					{
						Thread.Sleep(750);

						// Get the world from the server.
						NetOutgoingMessage refreshRequest = client.CreateMessage();
						refreshRequest.Write(NETWORK_REFRESH_REQUEST);
						client.SendMessage(refreshRequest, NetDeliveryMethod.ReliableOrdered);

						gameState = GameState.ConnectingLoading;
					}
					else
					{
						if (connectionAttemptTimer >= 70)
						{
							// Discover the server...
							Console.WriteLine("Attempting connection with server at {0}:{1}...", serverIP, 1945, client.ConnectionStatus, client.Status);
							client.DiscoverKnownPeer(serverIP, 1945);
							handleNetworkMessages();

							connectionAttempts++;


							if (connectionAttempts >= 10)
							{
								gameState = GameState.ConnectionFailed;
							}

							connectionAttemptTimer = 0;
						}
						else
						{
							connectionAttemptTimer++;
						}
					}
					
					break;


				// The game is running normally.
				case GameState.Running:
				case GameState.RunningInventory:
					
					switch(gameState)
					{

						case GameState.Running:
						case GameState.RunningInventory:
							
							// Find out how far the mouse has moved since the last upate.
							Vector2 lookMovement = Vector2.Zero;
							if (mouseLocked)
							{
								lookMovement = new Vector2(
									m.X - (graphics.PreferredBackBufferWidth / 2),
									m.Y - (graphics.PreferredBackBufferHeight / 2));
							}


							// Update the world.
							if (world != null)
							{
								world.update(random, k, lookMovement);
							}

							// Update the current action.
							if (currentAction != null)
							{
								if (currentAction.tick())
								{
									currentAction.perform(random, client, world, world.player.skills, ref notifications);
									currentAction = null;
								}
							}

							break;
						}


					#region 3D View

					// Move the camera with the player.
					if (world != null)
					{
						camera.position = world.player.position + new Vector3(0.0F, world.player.height * 0.90F, 0.0F);
						camera.target = world.player.lookTarget;
					}

					#endregion


					#region Tile Cursor

					if (contextMenu == null)
					{
						// Determine if there are any tile cursors.
						Vector3 rayStart = camera.position;
						Vector3 rayDirection = camera.target - camera.position;
						rayDirection.Normalize();
						Point? currentTile = world.getSelectedTile(camera);

						if (currentTile.HasValue)
						{
							tileCursorEnabled = true;
							tileCursorPosition = currentTile.Value;
						}
						else
						{
							tileCursorEnabled = false;
						}
					}

					// Move the tile name indicator back & forth.
					if (tileCursorEnabled)
					{
						indicatorOffset += (50.0F - indicatorOffset) / 10.0F;
					}
					else
					{
						indicatorOffset += (0.0F - indicatorOffset) / 10.0F;
					}


					// Allow the context menu to be opened
					if (m.RightButton.Equals(ButtonState.Pressed)
						&& canOpenContextMenu
						&& tileCursorEnabled)
					{
						contextMenu = new ContextMenu(world.player.skills, tileCursorPosition, world, null);

						mouseLocked = false;
						canOpenContextMenu = false;
					}

					if (contextMenu == null
						&& m.RightButton.Equals(ButtonState.Released))
					{
						canOpenContextMenu = true;
					}


					// Allow the context menu to be closed, if clicked away from.
					// Also, update the context menu.
					if (contextMenu != null)
					{
						// Update the context menu.
						int result = contextMenu.update(m, graphics);

						if (result >= 0)
						{
							if (currentAction == null)
							{
								currentAction = new TimedAction(contextMenu.actionList[result], world.player.skills);

								contextMenu = null;
								mouseLocked = true;
							}
						}
						else
						{
							// Then allow the menu to be closed.
							Rectangle contextMenuRectangle = contextMenu.calculateSize(FontResources.interfaceFont, graphics);
							if (m.LeftButton.Equals(ButtonState.Pressed)
								&&
								(
									m.X < contextMenuRectangle.Left ||
									m.X > contextMenuRectangle.Right ||
									m.Y < contextMenuRectangle.Top ||
									m.Y > contextMenuRectangle.Bottom
								))
							{
								contextMenu = null;
								mouseLocked = true;
							}
						}
					}

					#endregion


					#region Mouse Looking

					// Reset the mouse to the middle of the screen.
					if (mouseLocked)
					{
						Mouse.SetPosition(
						graphics.PreferredBackBufferWidth / 2,
						graphics.PreferredBackBufferHeight / 2);

						IsMouseVisible = false;
					}
					else
					{
						IsMouseVisible = true;
					}

					#endregion


					#region Notifications

					// Update the first notification.
					if (notifications.Count >= 1)
					{
						if (notifications[0].update())
							notifications.RemoveAt(0);
					}

					#endregion

					break;

				
				// The game is not the active window.
				case GameState.Unfocused:

					// Allow the player to resume the game.
					if (this.IsActive && m.LeftButton.Equals(ButtonState.Pressed))
						this.gameState = GameState.Running;

					break;


				// The game wants to quit.
				case GameState.Quitting:

					// Quit the game.
					Console.WriteLine("Game is currently in 'Quitting' state; closing.");

					client.Disconnect("Game closed");

					this.Exit();

					break;


				// Where the current Game State is unknown.
				default:

					Console.WriteLine("Error! - Unhandled Game State: " + this.gameState.ToString());
					this.Exit();

					break;

			}



			// Allow the inventory to be toggled.
			#region Inventory Toggling

			if (gameState.Equals(GameState.RunningInventory)
				|| gameState.Equals(GameState.Running))
			{
				if (canToggleInventory && k.IsKeyDown(Keys.Tab))
				{
					gameState = (gameState.Equals(GameState.Running) ? GameState.RunningInventory : GameState.Running);

					Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

					canToggleInventory = false;
				}
				else if (!canToggleInventory && k.IsKeyUp(Keys.Tab))
				{
					canToggleInventory = true;
				}
			}

			#endregion

			// Allow the game to exit.
			if (k.IsKeyDown(Keys.Escape)) gameState = GameState.Quitting;


			// Update the 'old' mouse and keyboard states.
			mOld = m;
			kOld = k;


			base.Update(gameTime);
		}


		/// 
		/// <summary>
		///		Performs a draw tick on the game object.
		/// </summary>
		/// 
		/// <param name="gameTime">Time passed since the last call to Draw.</param>
		/// 
		protected override void Draw(GameTime gameTime)
		{

			// Clear the screen.
			GraphicsDevice.Clear(new Color(126, 192, 238));

			// Begin the spritebatch, getting ready for the scaled sprites to be drawn onto the screen.
			spriteBatch.Begin(
				SpriteSortMode.BackToFront,
				BlendState.AlphaBlend,
				SamplerState.PointClamp,
				DepthStencilState.Default,
				RasterizerState.CullCounterClockwise);



			if (gameState.Equals(GameState.Running)
				|| gameState.Equals(GameState.RunningInventory))
			{
				// Draw the world!!
				if (world != null)
				{
					world.draw(graphics, basicEffect, camera);
				}

			}

			if (gameState.Equals(GameState.Running))
			{
				
				// Draw the tile cursor.
				if (tileCursorEnabled)
					Tile.renderTile(
						graphics,
						basicEffect,
						camera,
						tileCursorPosition.X,
						tileCursorPosition.Y,
						world.tiles[tileCursorPosition.X, tileCursorPosition.Y].heights,
						TextureResources.tileCursorTexture,
						Tile.HEIGHT_STEP / 2.0F);

				// Draw the mouse cursor.
				spriteBatch.Draw(TextureResources.cursorTexture,
									new Vector2(graphics.PreferredBackBufferWidth / 2.0F, graphics.PreferredBackBufferHeight / 2.0F),
									null,
									Color.White,
									0.0F,
									new Vector2(16.0F),
									1.0F,
									SpriteEffects.None,
									0.0F);

			}

			if (gameState.Equals(GameState.Running))
			{
				// Draw the context menu
				if (contextMenu != null)
					contextMenu.draw(spriteBatch, FontResources.interfaceFont, graphics);

				// Draw the little menu thing at the top of the screen which shows the current tile.
				{
					InterfaceHelper.drawWindow(
						spriteBatch,
						new Rectangle((graphics.PreferredBackBufferWidth / 2) - 100,
										-40 + (int)indicatorOffset,
										200,
										30),
						1.0F);

					if (tileCursorEnabled)
					{
						String tileName = world.tiles[tileCursorPosition.X, tileCursorPosition.Y].getName();
						Vector2 tileNameSize = FontResources.interfaceFont.MeasureString(tileName);

						spriteBatch.DrawString(FontResources.interfaceFont,
							tileName,
							new Vector2((graphics.PreferredBackBufferWidth / 2.0F) - (tileNameSize.X / 2.0F),
							(-40.0F + indicatorOffset) + 6.0F),
							Color.Black);
					}
				}
			}

			if (gameState.Equals(GameState.Running)
				|| gameState.Equals(GameState.RunningInventory))
			{
				// Draw the notifications.
				if (notifications.Count >= 1)
				{
					notifications[0].draw(spriteBatch, graphics);
				}


				// Draw the action window.
				{
					InterfaceHelper.drawWindow(
						spriteBatch,
						new Rectangle(5, graphics.PreferredBackBufferHeight - 105, 300, 100),
						0.80F);

					spriteBatch.Draw(
							TextureResources.contextMenuTexture,
							new Rectangle(15, graphics.PreferredBackBufferHeight - 40, 280, 25),
							InterfaceHelper.REGION_BAR_GREYSCALE,
							Color.White,
							0.0F,
							Vector2.Zero,
							SpriteEffects.None,
							0.4F);

					if (currentAction == null)
					{
						spriteBatch.DrawString(
							FontResources.interfaceFont,
							System.Environment.UserName,
							new Vector2(20.0F, graphics.PreferredBackBufferHeight - 90.0F),
							Color.Black);
					}

					if (currentAction != null)
					{

						spriteBatch.DrawString(
							FontResources.interfaceFont,
							currentAction.action.getName() + "...",
							new Vector2(20.0F, graphics.PreferredBackBufferHeight - 90.0F),
							Color.Black,
							0.0F,
							Vector2.Zero,
							1.0F,
							SpriteEffects.None,
							0.0F);

						spriteBatch.Draw(
							TextureResources.contextMenuTexture,
							new Rectangle(15, graphics.PreferredBackBufferHeight - 40, (int)(currentAction.getPercentageCompleted() * 280.0F), 25),
							InterfaceHelper.REGION_BAR,
							Color.White,
							0.0F,
							Vector2.Zero,
							SpriteEffects.None,
							0.25F);

						String percentageString = (int)Math.Round(currentAction.getPercentageCompleted() * 100.0F) + "%";
						Vector2 percentageStringSize = FontResources.interfaceFont.MeasureString(percentageString);

						spriteBatch.DrawString(
							FontResources.interfaceFont,
							percentageString,
							new Vector2(15.0F + (280.0F / 2.0F) - (percentageStringSize.X / 2.0F),
										graphics.PreferredBackBufferHeight - 35.0F),
							Color.Black);
					}
				}
			}


			if (gameState.Equals(GameState.Unfocused))
			{
				FontResources.drawStringWithShadow(spriteBatch, FontResources.interfaceFont, "Click to focus!", new Vector2(20.0F, 20.0F), Color.White);
			}


			if(gameState.Equals(GameState.RunningInventory))
			{

				Rectangle inventoryRectangle = new Rectangle(
					(graphics.PreferredBackBufferWidth / 2) - 400,
					(graphics.PreferredBackBufferHeight / 2) - 300,
					800,
					600);


				InterfaceHelper.drawWindow(
					spriteBatch,
					inventoryRectangle,
					0.85F);

			}
			

			if(gameState.Equals(GameState.Connecting))
			{
				String connectingString = "";
				switch(client.ConnectionStatus)
				{
					case NetConnectionStatus.None:
					case NetConnectionStatus.Disconnected:
						connectingString = "Looking for the server." + new String('.', connectionAttempts);
						break;

					case NetConnectionStatus.InitiatedConnect:
						connectingString = "Establishing a connection with the server." + new String('.', connectionAttempts);
						break;

					case NetConnectionStatus.Connected:
						connectingString = "Connected to the server!";
						break;

					default:
						connectingString = "Please wait." + new String('.', connectionAttempts);
						break;
				}

				drawCentralNotification(connectingString);
			}

			if(gameState.Equals(GameState.ConnectionFailed))
			{
				drawCentralNotification("Connection failed after " + connectionAttempts + " attempts :(");
			}


			// Debug String.
			FontResources.drawStringWithShadow(spriteBatch, FontResources.interfaceFont, GAME_NAME + " " + GAME_VERSION, new Vector2(3.0F, 3.0F), Color.White);
			FontResources.drawStringWithShadow(spriteBatch, FontResources.interfaceFont, Math.Round(ping, 1) + " ms", new Vector2(3.0F, 33.0F), Color.White);


			// End the spritebatch.
			spriteBatch.End();
			
			
			base.Draw(gameTime);
		}


		/// 
		/// <summary>
		///		Draws a large, central notification in the middle of the screen with a message.
		/// </summary>
		/// 
		/// <param name="message">The message to be displayed.</param>
		/// 
		public void drawCentralNotification(String message)
		{
			Vector2 messageSize = FontResources.notificationFont.MeasureString(message);

			InterfaceHelper.drawWindow(
				spriteBatch,
				new Rectangle((graphics.PreferredBackBufferWidth / 2) - (int)((messageSize.X + 100) / 2),
								  (graphics.PreferredBackBufferHeight / 2) - 40,
								  (int)messageSize.X + 100,
								  80),
				1.0F);

			spriteBatch.DrawString(FontResources.notificationFont,
					message,
					new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2) - (messageSize / 2.0F),
					Color.Black);
		}

	}

}
