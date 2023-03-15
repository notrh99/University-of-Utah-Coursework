using System.Text.Json;
using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;

namespace ClientGUI
{
    /// <summary> 
    /// Author:    Tyler DeBruin and Rayyan Hamid
    /// Partner:   None
    /// Date:      4-9-2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// Contains the Client Logic. Communicates between the World, and the Network - abstracted away from the UI specific drawing logic.
    /// </summary>
    public class ClientLogic
    {
        /// <summary>
        /// Port used by the server.
        /// </summary>
        private const int _gamePort = 11000;


        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Contains the World, Networking, and ClientId to manage the client.
        /// </summary>
        public readonly World World;
        /// <summary>
        /// ClientID - Given by the server. Allows the user to 'follow' themselves around.
        /// </summary>
        public long ClientID;
        /// <summary>
        /// Networking class to communicate with the server.
        /// </summary>
        private readonly Networking _networking;


        //Counts total messages recieved by the server.
        private int _totalPacketCount = 0;

        /// <summary>
        /// Getter for the total messages receieved.
        /// </summary>
        /// <returns></returns>
        public int GetPacketCount()
        {
            return _totalPacketCount;
        }

        /// <summary>
        /// Delegates used by this Logic class, to communicate back to the UI. Saves a reference passed from the constructor.
        /// </summary>
        private readonly OnConnectCallback _onConnectCallback;
        private readonly OnDisconnectCallback _onDisconnectCallback;
        private readonly OnHeartbeat _onHeartbeat;

        /// <summary>
        /// Constructor to build the logic class. Uses delegates to abstract away the client's game logic from the client's paint logic in the UI.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="onConnectCallback"></param>
        /// <param name="onDisconnectCallback"></param>
        /// <param name="onHeartbeat"></param>
        public ClientLogic(ILogger logger,
            OnConnectCallback onConnectCallback,
            OnDisconnectCallback onDisconnectCallback,
            OnHeartbeat onHeartbeat)
        {
            _logger = logger;
            World = new World(logger);

            _onConnectCallback = onConnectCallback;
            _onDisconnectCallback = onDisconnectCallback;
            _onHeartbeat = onHeartbeat;

            _networking = new Networking(logger, ReportConnectionEstablished, ReportDisconnect, ReportMessageArrived, '\n');
        }


        /// <summary>
        /// Networking Delegate to handle communication from the server.
        ///
        /// When a request from the server comes in, its handled by the Protocol here.
        ///
        /// The world object  describes the world, and the UI paints it. This creates the communication between the world and the network.
        /// </summary>
        /// <param name="channel">Server's network channel.</param>
        /// <param name="message">Message from the server.</param>
        private void ReportMessageArrived(Networking channel, string message)
        {
            _totalPacketCount++;

            _logger.Log(LogLevel.Information, message);

            if (message.StartsWith(Protocols.CMD_Food))
            {
                string json = message[Protocols.CMD_Food.Length..];

                List<Food>? foodList = JsonSerializer.Deserialize<List<Food>>(json);

                if (foodList != null)
                {
                    foreach (var food in foodList)
                    {
                        World.AddFood(food);
                    }
                }
            }
            else if (message.StartsWith(Protocols.CMD_Player_Object))
            {
                string playerId = message[Protocols.CMD_Player_Object.Length..];

                long clientId = long.Parse(playerId);

                ClientID = clientId;
            }
            else if (message.StartsWith(Protocols.CMD_Update_Players))
            {
                string json = message[Protocols.CMD_Update_Players.Length..];

                List<Player>? playerList = JsonSerializer.Deserialize<List<Player>>(json);

                if (playerList != null)
                {
                    foreach (var player in playerList)
                    {
                        if (player.ID == ClientID)
                        {
                            _isClientAlive = false;
                        }

                        World.AddPlayer(player);
                    }
                }
            }
            else if (message.StartsWith(Protocols.CMD_Eaten_Food))
            {
                string json = message[Protocols.CMD_Eaten_Food.Length..];

                List<long>? foodEatenList = JsonSerializer.Deserialize<List<long>>(json);

                if (foodEatenList != null)
                {
                    foreach (var foodId in foodEatenList)
                    {
                        World.RemoveFood(foodId);
                    }
                }
            }
            else if (message.StartsWith(Protocols.CMD_Dead_Players))
            {
                string json = message[Protocols.CMD_Dead_Players.Length..];

                List<long>? playerDeadList = JsonSerializer.Deserialize<List<long>>(json);

                if (playerDeadList != null)
                {
                    foreach (var playerId in playerDeadList)
                    {
                        if (playerId == ClientID)
                        {
                            _isClientAlive = false;
                        }

                        World.RemovePlayer(playerId);
                    }
                }
            }
            else if (message.StartsWith(Protocols.CMD_HeartBeat))
            {
                string heartbeat = message[Protocols.CMD_HeartBeat.Length..];

                long heartBeat = long.Parse(heartbeat);

                _onHeartbeat(heartBeat);
            }
        }

        /// <summary>
        /// The UI calls into the logic to communicate moving with the Network class.
        ///
        /// The Grid is 5000 x 5000 units, where 0,0 is in the top left of the gameboard.
        /// As you move down, you move 'up' the Y axis.
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        public void SendMoveRequest(int xPosition, int yPosition)
        {
            _networking.Send(string.Format(Protocols.CMD_Move, xPosition, yPosition));
        }

        /// <summary>
        /// The UI calls into the logic to communicate splitting with the Network class.
        ///
        /// The Grid is 5000 x 5000 units, where 0,0 is in the top left of the gameboard.
        /// As you move down, you move 'up' the Y axis. The split will 'fire' the second cell off towards the vector you supply.
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        public void SendSplitRequest(int xPosition, int yPosition)
        {
            _networking.Send(string.Format(Protocols.CMD_Split, xPosition, yPosition));
        }


        /// <summary>
        /// Tracks if the player is connected, and alive.
        /// </summary>
        /// <returns></returns>
        private bool _isClientAlive = true;

        /// <summary>
        /// Getter for _isClientAlive
        /// </summary>
        /// <returns></returns>
        public bool IsClientAlive()
        {
            return _isClientAlive;
        }

        /// <summary>
        /// Bool to track when we're connected to the server.
        /// </summary>
        private bool _connectedToServer;

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns></returns>
        public bool IsConnectedToServer()
        {
            return _connectedToServer;
        }

        /// <summary>
        /// Starts the game, by sending a name to the server. If the server has already started, this is used to change you name when you die.
        /// </summary>
        /// <param name="name">Name of your cell.</param>
        /// <param name="host">DNS or IP Address</param>
        public void StartGame(string name, string host)
        {
            if (!_connectedToServer)
            {
                _networking.Connect(host, _gamePort);

                new Thread(() =>
                {
                    _networking.ClientAwaitMessagesAsync();
                }).Start();
            }

            _networking.Send(string.Format(Protocols.CMD_Start_Game, name));
        }

        /// <summary>
        /// Delegate for when the Server disconnects - so a callback can be made back to the UI.
        /// </summary>
        /// <param name="channel">Server channel.</param>
        private void ReportDisconnect(Networking channel)
        {
            _connectedToServer = false;

            _onDisconnectCallback(channel.ID);
        }

        /// <summary>
        /// Delegate for when the Network successfully connects, calls the callback to the UI to start the game.
        /// </summary>
        /// <param name="channel"></param>
        private void ReportConnectionEstablished(Networking channel)
        {
            _connectedToServer = true;

            _onConnectCallback(channel.ID);
        }

        /// <summary>
        /// These are the defined delegates for this class. They communicate the heartbeat, when the server connects, and when the server disconnects; back to the UI.
        /// </summary>
        /// <param name="host"></param>
        public delegate void OnConnectCallback(string host);
        public delegate void OnDisconnectCallback(string host);
        public delegate void OnHeartbeat(long heartbeat);
    }
}
