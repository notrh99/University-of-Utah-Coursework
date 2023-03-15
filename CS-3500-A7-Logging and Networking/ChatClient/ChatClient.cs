using Communications;
using Microsoft.Extensions.Logging;

namespace ChatClient
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
    /// Class that contains all of the UI logic for the ChatClient.
    /// </summary>
    public partial class ChatClient : Form
    {
        /// <summary>
        /// Used by the Networking object.
        /// </summary>
        private readonly Networking _networking;
        private int _port = 11000;

        /// <summary>
        /// Public Constructor, used by the Dependency Injection Framework to create the Logger.
        /// </summary>
        /// <param name="logger"></param>
        public ChatClient(ILogger<ChatClient> logger)
        {
            InitializeComponent();

            _networking = new Networking(logger, OnConnect, OnDisconnect, ReportMessageArrived, '\n');
        }

        /// <summary>
        /// Event method for hitting enter on the send message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatBox_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                var input = ChatBox_Input.Text;

                _networking.Send($"{input}");

                ChatBox_Input.Clear();
            }
        }

        /// <summary>
        /// Callback delegate when the client connects to the server.
        /// </summary>
        /// <param name="channel">Called when the Client connects.</param>
        private void OnConnect(Networking channel)
        {
            this.Invoke(() =>
            {
                ChatBox_Input.Enabled = true;
                Name_Input.Enabled = false;
                ServerAddress_Input.Enabled = false;
                Participants_Button.Enabled = true;

                ChatHistory_Display.AppendText($"Connected to Server.{Environment.NewLine}");
            });
        }

        /// <summary>
        ///  Callback delegate when the client disconnects from the server.
        /// </summary>
        /// <param name="channel"></param>
        private void OnDisconnect(Networking channel)
        {
            this.Invoke(() =>
            {
                Participants_Button.Enabled = false;
                Participants_History.Text = "";

                ChatBox_Input.Enabled = false;
                ChatBox_Input.Text = "";

                Name_Input.Enabled = true;
                ServerAddress_Input.Enabled = true;

                ChatHistory_Display.AppendText($"Disconnected.{Environment.NewLine}");
            });
        }

        /// <summary>
        ///  Callback delegate when the client recieves a message from the server.
        /// </summary>
        /// <param name="channel"></param>
        private void ReportMessageArrived(Networking channel, string message)
        {
            if (message.Contains("Command Participants"))
            {
                var participants = message.Split(",").Skip(1);

                this.Invoke(() =>
                {
                    Participants_History.Clear();
                    Participants_History.AppendText($"{string.Join(Environment.NewLine, participants)}");
                });
            }
            else
            {
                this.Invoke(() =>
                {
                    ChatHistory_Display.AppendText($"{message}{Environment.NewLine}");
                });
            }
        }

        /// <summary>
        /// Connect Button delegate. Handles the server being unavailable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Button_Click(object sender, EventArgs e)
        {
            string host = ServerAddress_Input.Text;
            int port = _port;

            try
            {
                _networking.Connect(host, port);

                var backgroundThread = new Thread(() => _networking.ClientAwaitMessagesAsync());
                backgroundThread.Start();

                if (!string.IsNullOrEmpty(Name_Input.Text))
                {
                    _networking.Send($"Command Name {Name_Input.Text}");
                }
            }
            catch (Exception)
            {
                var errorMessage = $"Error Connecting to Server: {host}:{port}{Environment.NewLine}";

                ChatHistory_Display.AppendText(errorMessage);
            }

        }

        /// <summary>
        /// The command to retrieve participants.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Participants_Button_Click(object sender, EventArgs e)
        {
            _networking.Send($"Command Participants");
        }
    }
}