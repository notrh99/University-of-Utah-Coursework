using System.Net;
using Communications;
using Microsoft.Extensions.Logging;

namespace ChatServer
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
    /// Class that contains all of the UI logic for the ChatServer. Also persists the Networking objects to communicate back to the clients.
    /// </summary>
    public partial class ChatServer : Form
    {
        /// <summary>
        /// The Server Networking object. 
        /// </summary>
        private readonly Networking _networking;
        private readonly int Port = 11000;

        /// <summary>
        /// Client Networking Objects.
        /// </summary>
        private List<Networking> _clients = new List<Networking>();

        private bool _serverStarted = false;
        /// <summary>
        /// Public Constructor, used by the Dependency Injection Framework to create the Logger.
        /// </summary>
        /// <param name="logger"></param>
        public ChatServer(ILogger<ChatServer> logger)
        {
            InitializeComponent();

            _networking = new Networking(logger, OnConnect, OnDisconnect, ReportMessageArrived, '\n');  

            SetupDefaults();
        }

        /// <summary>
        /// Set Defaults inside some of the Text Fields. I.e. The IP Address, the Port, and the HostName. Also starts the background service.
        /// </summary>
        private void SetupDefaults()
        {
            ServerPort_Input.Text = Port.ToString();
            ServerHostName_Input.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].MapToIPv4().ToString();
            ServerName_Input.Text = _networking.ID;

            var backgroundThread = new Thread(StartServer);
            backgroundThread.Start();
        }


        /// <summary>
        /// Button Click event for the Shutdown button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerShutDown_Button_Click(object sender, EventArgs e)
        {

            if (!_serverStarted)
            {
                ServerShutDown_Button.Text = "Shutdown Server";

                StartServer();
            }
            else
            {
                SendServerSideChatToAllClients("** Shutting Server Down.");

                _networking.StopWaitingForClients();

                var clients = new List<Networking>();

                lock (_clients)
                {
                    clients.AddRange(_clients);
                }

                foreach (var client in clients)
                {
                    client.Disconnect();
                }

                _serverStarted = false;
                ServerShutDown_Button.Text = "Start Server";
            }
        }

        /// <summary>
        /// Turns the Server back on.
        /// </summary>
        private void StartServer()
        {
            _serverStarted = true;

            _networking.WaitForClients(Port, true);
        }

        /// <summary>
        /// OnClick delegate - Handles when a Client Connects.
        /// </summary>
        /// <param name="channel">The Network channel that spawned the event.</param>
        private void OnConnect(Networking channel)
        {
            lock (_clients)
            {
                _clients.Add(channel);
            }

            SendServerSideChatToAllClients($"** {channel.ID} has Connected.");

            RefreshParticipants();
        }

        /// <summary>
        /// Event that handles when a Client disconnects.
        /// </summary>
        /// <param name="channel">The Network channel that spawned the event.</param>
        private void OnDisconnect(Networking channel)
        {
            lock (_clients)
            {
                var participant = _clients.First(x => x.ID == channel.ID);

                _clients.Remove(participant);
            }

            SendServerSideChatToAllClients($"** {channel.ID} has Disconnected.");

            RefreshParticipants();
        }

        /// <summary>
        /// Event that handles when a message is received by the server.
        /// </summary>
        /// <param name="channel">The Network channel that spawned the event.</param>
        /// <param name="message">The string message sent across the network.</param>
        private void ReportMessageArrived(Networking channel, string message)
        {
            if (!HandleCommandMessage(channel, message))
            {
                string messageText = $"{channel.ID} - {message}";

                SendServerSideChatToAllClients(messageText);
            }
        }

        /// <summary>
        /// Handles command messages when they come across the server. Handles sending the participant list back, or when a name change is requested.
        /// </summary>
        /// <param name="channel">Networking channel that sent the message</param>
        /// <param name="message">The message to be parsed as a command.</param>
        /// <returns>A bool that indicates if the message was a command message.</returns>
        private bool HandleCommandMessage(Networking channel, string message)
        {
            var result = false;

            if (message.StartsWith("Command"))
            {
                var commandArguments = message.Split(" ");

                if (commandArguments.Length == 2 && commandArguments[1] == "Participants")
                {
                    channel.Send(GetParticipantsCommandMessage());
                }
                else if(commandArguments.Length == 3 && commandArguments[1] == "Name")
                {
                    string newName = commandArguments[2];

                    var copiedClients = new List<Networking>();

                    lock (_clients)
                    {
                        copiedClients.AddRange(_clients);
                    }

                    if (copiedClients.All(x => x.ID != newName))
                    {
                        HandleNameChange(channel.ID, newName);
                    }
                    else
                    {
                        channel.Send($"** Someone is already Named {newName}, please choose something else.");
                    }
                }

                result = true;
            }

            return result;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        private string GetParticipantsCommandMessage()
        {
            string participants = string.Join(",", GetParticipants());

            return $"Command Participants,{participants}";
        }

        /// <summary>
        /// Sends a message from the server to all clients. Used for sending 'X has connected!' type messages - anything that gets spawned from the server, not from a client.
        /// </summary>
        /// <param name="message">/param>
        private void SendServerSideChatToAllClients(string message)
        {
            ChatBox_History.Invoke(() =>
            {
                ChatBox_History.AppendText($"{message}{Environment.NewLine}");
            });

            SendMessageToAllClients(message);
        }

        /// <summary>
        /// Sends any message to all clients. This is a helper method used to send new messages from other chatters to clients, or if someone changes their name.
        /// </summary>
        /// <param name="message"></param>
        private void SendMessageToAllClients(string message)
        {
            var copiedClients = new List<Networking>();

            lock (_clients)
            {
                copiedClients.AddRange(_clients);
            }

            foreach (var client in copiedClients)
            {
                client.Send(message);
            }
        }

        /// <summary>
        /// Handles a Name change. Updates the Client ID to contain the name - Sends the new name to all clients.
        /// </summary>
        /// <param name="channelId">The old ChannelId to update.</param>
        /// <param name="newName">The new Name to set as the channelId on the network object</param>
        private void HandleNameChange(string channelId, string newName)
        {
            lock (_clients)
            {
                var participant = _clients.First(x => x.ID == channelId);

                participant.ID = newName;
            }

            RefreshParticipants();

            SendServerSideChatToAllClients($"** {channelId} set their name to: {newName}");

            var updatedParticipants = GetParticipantsCommandMessage();

            SendMessageToAllClients(updatedParticipants);
        }

        /// <summary>
        /// Returns the list of current participants
        /// </summary>
        /// <returns>the list of current participants</returns>
        private IEnumerable<string> GetParticipants()
        {
            lock (_clients)
            {
                return _clients.Select(x => x.ID);
            }
        }

        /// <summary>
        /// Updates the Participants in the Server UI, and sends an updated list to all clients.
        /// </summary>
        private void RefreshParticipants()
        {
            this.Invoke(() =>
            {
                Participants_History.Clear();

                var participants = GetParticipants();

                Participants_History.AppendText($"{string.Join(Environment.NewLine, participants)}");
            });

            string participantsCommandMessage = GetParticipantsCommandMessage();

            SendMessageToAllClients(participantsCommandMessage);
        }
    }
}