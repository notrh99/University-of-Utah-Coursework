using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Communications
{
    /// <summary> 
    /// Author:    Tyler DeBruin and Rayyan Hamid
    /// Partner:   None
    /// Date:      3/28/2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// Class that contains all of the Networking logic between a Client and a Server. This abstracts away the core logic, so that the GUI can only contain 'View' logic.
    /// </summary>
    public class Networking
    {
        /// <summary>
        /// TCP Client used to 'Connect' to another instance with a listener.
        /// </summary>
        private TcpClient _tcpClient = new TcpClient();

        /// <summary>
        /// Private ID, used by the Getter of the ID property.
        /// </summary>
        private string _Id = string.Empty;

        /// <summary>
        /// Public Property ID, defaults to the Hostname of whatever spawned the Network connection. Either the Client, or the Server.
        /// </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_Id))
                {
                    _Id = Dns.GetHostName();
                }

                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        /// <summary>
        /// Delegate Fields - Saves off the Delegates, so that they can be called back by the methods of the class.
        /// </summary>
        private readonly ReportConnectionEstablished _onConnect;
        private readonly ReportDisconnect _onDisconnect;
        private readonly ReportMessageArrived _onMessage;
        private readonly char _terminationCharacter;

        /// <summary>
        /// Logging Field, so we can log stuff as neccessary.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Cancellation Token for canceling the  WaitForClients method, and closing connections. 
        /// </summary>
        private CancellationTokenSource _waitForCancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Helper property for if the Network is working as a Server.
        /// </summary>
        private bool _serverListening = false;

        /// <summary>
        /// Public Constructor for creating a Networking Object.
        ///
        /// 
        ///
        /// </summary>
        /// <param name="logger">Logger is an Interface from Microsoft's logging package.</param>
        /// <param name="onConnect">OnConnect Delegate - Will be called when a Connection is Established to a listening Network object, or when a Network Connects to a Server.</param>
        /// <param name="onDisconnect">OnDisconnect is called when a Client Disconnects.</param>
        /// <param name="onMessage">Called when a Message is sent from the Client, or from the Server.</param>
        /// <param name="terminationCharacter">Appended to the end of a message to indicate to the TcpClient that a message has 'stopped'.</param>
        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect onDisconnect, ReportMessageArrived onMessage, char terminationCharacter)
        {
            _logger = logger;
            _onConnect = onConnect;
            _onDisconnect = onDisconnect;
            _onMessage = onMessage;
            _terminationCharacter = terminationCharacter;
        }

        /// <summary>
        /// Used by the Server.
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="infinite"></param>
        public async void WaitForClients(int port, bool infinite)
        {
            if (!_serverListening)
            {
                _serverListening = true;

                var listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                _waitForCancellationTokenSource = new CancellationTokenSource();

                _logger.Log(LogLevel.Information, $"\nServer {listener.LocalEndpoint} Started.");

                try
                {
                    do
                    {
                        var tcpClientConnection = await listener.AcceptTcpClientAsync(_waitForCancellationTokenSource.Token);

                        BeginListeningForClient(tcpClientConnection);

                    } while (infinite);
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Debug, $"\nServer Terminated due to an Exception being thrown: {e.Message}");
                }

                listener.Stop();
                _serverListening = false;
                _logger.Log(LogLevel.Information, $"\nServer {listener.LocalEndpoint} Stopped.");
            }
        }

        /// <summary>
        /// Using the Cancellation Token, cancels awaiting for Client Connections. 
        /// </summary>
        public void StopWaitingForClients()
        {
            if (_serverListening)
            {
                _waitForCancellationTokenSource.Cancel();

                _logger.Log(LogLevel.Information, $"\nStopWaitingForClients called, Server Currently Listening.");
            }
            else
            {
                _logger.Log(LogLevel.Debug, $"\nStopWaitingForClients called, Server Currently not Listening.");
            }
        }

        /// <summary>
        /// Helper method to start to Listen to the client, on its own background thread.
        /// Calls OnConnect with a new Network Object that 
        /// </summary>
        /// <param name="tcpClient"></param>
        private void BeginListeningForClient(TcpClient tcpClient)
        {
            _logger.Log(LogLevel.Debug, $"\n ** New Connection ** Accepted From {tcpClient.Client.RemoteEndPoint} to {ID}\n");

            //Create a new Networking object, with the call backs passed in.
            var clientNetworking = new Networking(_logger, _onConnect, _onDisconnect, _onMessage, _terminationCharacter);

            //Grab the TCPClient accepted by the listener, set it on the new Networking object.
            clientNetworking._tcpClient = tcpClient;

            // Should not be unknown, but Endpoint is Nullabe.
            clientNetworking.ID = tcpClient.Client.LocalEndPoint?.ToString() ?? "Unknown";

            _logger.Log(LogLevel.Debug, $"{clientNetworking.ID} Connected, and Listening on Background Thread.");

            //Call OnConnect with this new Network Object.
            _onConnect(clientNetworking);

            var newClientThread = new Thread(() =>
            {
                //Subscribe to the messages on a new Thread.
                clientNetworking.ClientAwaitMessagesAsync();
            });

            //Start the Thread.
            newClientThread.Start();
        }


        /// <summary>
        /// Used by a ClientApplication to Connect to the Server.
        ///
        /// Creates a new TCP Client.
        ///
        /// Calls the OnConnect delegate passed into the constructor.
        /// </summary>
        /// <param name="host">Hostname or Ip to connect to.</param>
        /// <param name="port">Port to connect to, on that Host.</param>
        public void Connect(string host, int port)
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.Close();

                _logger.Log(LogLevel.Debug, $"Closed already active TCP Connection.");
            }

            _tcpClient = new TcpClient(host, port);

            _onConnect(this);
        }

        /// <summary>
        /// Calls Disconnect on the TCP client, closing the connection if one is open.
        ///
        /// Calls the OnDisconnect delegate passed into the constructor.
        /// </summary>
        public void Disconnect()
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.Close();

                _logger.Log(LogLevel.Debug, $"Closed active TCP Connection.");
            }

            _onDisconnect(this);
        }

        /// <summary>
        /// Sends a Message to the client subscribed to ClientAwaitMessagesAsync.
        ///
        /// A termination character is appended to the end of the message as it transmits it.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public async void Send(string text)
        {
            try
            {
                var stream = _tcpClient.GetStream();

                var textToSend = new StringBuilder($"{text}{_terminationCharacter}");

                using (var writer = new StreamWriter(stream, leaveOpen: true))
                {
                    _logger.Log(LogLevel.Information, $"Sending Message: {textToSend}");

                    await writer.WriteAsync(textToSend, CancellationToken.None);
                }
            }
            catch (Exception)
            {
                _tcpClient.Close();
                _onDisconnect(this);
            }
        }

        /// <summary>
        /// Method used by the Client end of a request to recieve data.
        ///
        /// The server uses this to listen to messages from the client.
        /// The Client uses this to listen to messages from the sever. 
        /// </summary>
        /// <param name="infinite">False to complete after recieving the first message. True to never complete, and constantly feed messages to the recipient.</param>
        public async void ClientAwaitMessagesAsync(bool infinite = true)
        {
            try
            {
                var dataBacklog = new StringBuilder();

                byte[] buffer = new byte[4096];

                var stream = _tcpClient.GetStream();

                do
                {
                    int total = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string current_data = Encoding.UTF8.GetString(buffer, 0, total);

                    dataBacklog.Append(current_data);

                    CheckForMessage(dataBacklog);
                } while (infinite);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Failed to Process Message");
            }
        }

        /// <summary>
        /// Reads the Stream until it gets to the end, denoted by the termination character. Calls the OnMethod callback passed into the constructor with that message.
        /// </summary>
        /// <param name="data">The Data being sent across the network stream.</param>
        private void CheckForMessage(StringBuilder data)
        {
            string allData = data.ToString();

            int terminator_position = allData.IndexOf(_terminationCharacter);

            bool foundOneMessage = false;

            while (terminator_position >= 0)
            {
                foundOneMessage = true;

                string message = allData.Substring(0, terminator_position);

                data.Remove(0, terminator_position + 1);

                allData = data.ToString();

                terminator_position = allData.IndexOf(_terminationCharacter);

                _onMessage(this, message);
            }

            _logger.Log(LogLevel.Information,
                !foundOneMessage ? "Message not found" : $"After Message: {data.Length} bytes unprocessed.");
        }


        /// <summary>
        /// Delegate declaration for ReportMessageArrived
        /// </summary>
        /// <param name="channel">The network that spawned the event. Either the client, or the server.</param>
        /// <param name="message">The message to send across the network. A termination character will be appended to the end prior to transit.</param>
        public delegate void ReportMessageArrived(Networking channel, string message);
        /// <summary>
        /// Delegate declaration for ReportDisconnect
        /// </summary>
        /// <param name="channel">The Channel that spawns the disconnect event, from the disconnect method, or by killing the tcp connection.</param>
        public delegate void ReportDisconnect(Networking channel);
        /// <summary>
        /// Delegate declaration for ReportConnectionEstablished
        /// </summary>
        /// <param name="channel">The Channel that spawns the connection.</param>
        public delegate void ReportConnectionEstablished(Networking channel);
    }
}
