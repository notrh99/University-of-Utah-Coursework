using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Timers;
using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using Timer = System.Windows.Forms.Timer;

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
    /// Contains the logic for Painting the game, the actual display of the form, and for handling all user input.
    /// </summary>
    public partial class AgarioClient : Form
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// THe core client logic of the game, communicating through the network protocol.
        /// </summary>
        private readonly ClientLogic _clientLogic;

        /// <summary>
        /// Constructor for the Form. Initializes the logic class, and sets a timer to calculate the Frames Per second.
        /// </summary>
        /// <param name="logger"></param>
        public AgarioClient(ILogger<AgarioClient> logger)
        {
            _logger = logger;
            InitializeComponent();

            _clientLogic = new ClientLogic(_logger, OnConnectCallback, OnDisconnectCallback, OnHeartbeat);

            SetFramesPerSecond();
        }

        /// <summary>
        /// Starts two timers - One to track the Heartbeat, and one to track the Frames Per second.
        /// </summary>
        private void SetFramesPerSecond()
        {
            //Subscribe to the Painting events.
            this.DoubleBuffered = true;
            this.Paint += Draw_Scene;

            //Begin the Timer to Count frames.
            var invalidate = new System.Timers.Timer(1000 / 30);
            invalidate.Elapsed += UpdateFpsCounter;
            invalidate.Start();

            //Updating the FPS and HPS, once a second - and displays the update on the UI.
            var fps = new System.Timers.Timer(1000);
            fps.Elapsed += UpdateMetricsPerSecond;
            fps.Start();
        }

        /// <summary>
        /// Repaints the screen everytime the count updates(30 times a second).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFpsCounter(object? sender, ElapsedEventArgs e)
        {
            this.framesCount++;

            this.Invalidate();
        }

        /// <summary>
        /// Callback from the Client Logic to show a heartbeat is still occuring. Increments the heartbeat counter.
        /// </summary>
        /// <param name="heartbeat"></param>
        private void OnHeartbeat(long heartbeat)
        {
            lock (_hpsLockObject)
            {
                heartbeatCount++;
            }
        }



        /// <summary>
        /// Method utilized by the UI to calculate the FPS and HPS to display on the UI.
        ///
        /// Counts the Frames, Until it gets reset by the timer.
        /// Counts the HeartBeats until reset by the timer.
        ///
        /// Locks on separate lock objects to avoid race conditions. In theory, it shouldn't ever happen unless the timer is firing off twice at once.
        /// </summary>
        private int framesCount = 0;
        private int heartbeatCount = 0;
        private object _fpsLockObject = new object();
        private object _hpsLockObject = new object();


        /// <summary>
        /// Updates the FPS/HPS Metrics once a second has passed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateMetricsPerSecond(object? sender, EventArgs e)
        {
            lock (_fpsLockObject)
            {
                FPS_Label.Invoke(() =>
                {
                    FPS_Label.Text = framesCount.ToString();
                });

                framesCount = 0;
            }

            lock (_hpsLockObject)
            {
                HPS_Label.Invoke(() =>
                {
                    HPS_Label.Text = heartbeatCount.ToString();
                });

                heartbeatCount = 0;
            }
        }

        /// <summary>
        /// When the server or client disconnect. The way the app is built, this shouldn't happen.
        /// </summary>
        /// <param name="host"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnDisconnectCallback(string host)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Callback for when the app connects to the server.
        /// </summary>
        /// <param name="host">dns name for the connected host.</param>
        private void OnConnectCallback(string host)
        {
            this.Invoke(() =>
            {
                ErrorLabel.Text = $"Connected to {host}";

                PlayerNameInput.Enabled = false;
                PlayerNameInput.Visible = false;

                HostnameInput.Enabled = false;
                HostnameInput.Visible = false;

                PlayerNameLabel.Visible = false;
                HostnameLabel.Visible = false;


                PlayerNameInput_Connected.Text = PlayerNameInput.Text;
                PlayerNameInput_Connected.Visible = true;
                PlayerNameLabel_Connected.Visible = true;
            });
        }

        /// <summary>
        /// GUI Thread Validation.
        /// Validates the PlayerName is not empty.
        ///
        /// If it is, returns false, and sets the Error on the GUI.
        ///
        /// This takes a TextBox form, because there are multiple forms on the UI. After initially connecting, the old one is removed.
        /// This allows us to reuse the code for both.
        /// </summary>
        /// <param name="playerNameTextBox"></param>
        /// <returns>True if Valid, False if Invalid.</returns>
        private bool ValidateInput(TextBox playerNameTextBox)
        {
            var result = true;

            string playerName = playerNameTextBox.Text;

            if (string.IsNullOrEmpty(playerName) || playerName.Length > 10)
            {
                string error = "Invalid Name, cannot be empty, and must be less than 10 characters.";

                ErrorProvider_InputValidation.SetError(playerNameTextBox, error);
                ErrorLabel.Text = error;

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Connection, with Validation on the Hostname and Port.
        /// Also Clears the errors, and tries the inputs again. If they fail again, the errors will be reset on the GUI.
        /// Handled on GUI Thread.
        /// </summary>
        /// <param name="playerNameTextBox">TextBox for the Host to Connect to. SHould be an IP, or DNS Name.</param>
        /// <param name="hostNameTextBox">Textbox for the PlayerName, cannot be left empty.</param>
        private void InitializeConnection(TextBox playerNameTextBox, TextBox hostNameTextBox)
        {
            ErrorProvider_InputValidation.Clear();

            if (!ValidateInput(playerNameTextBox))
                return;

            string playerName = playerNameTextBox.Text;
            string hostName = hostNameTextBox.Text;

            try
            {
                _clientLogic.StartGame(playerName, hostName);
            }
            catch (Exception)
            {
                string error = $"Unable to Connect to Host: {hostName}:11000";
                ErrorProvider_InputValidation.SetError(hostNameTextBox, error);
                ErrorLabel.Text = error;
            }
        }

        /// <summary>
        /// HostName Input Enter Key event. Starts the game, after validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HostnameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                InitializeConnection(PlayerNameInput, HostnameInput);
            }
        }

        /// <summary>
        /// PlayerName Input Enter Key event. Starts the game, after validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerNameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                InitializeConnection(PlayerNameInput, HostnameInput);
            }
        }

        /// <summary>
        /// Handles Constants of the UI. 
        /// </summary>
        private const int screenXOffset = 10;
        private const int screenYOffset = 10;
        private const int viewWidth = 700;
        private const int viewHeight = 700;

        /// <summary>
        /// Font for the PlayerNames.
        /// </summary>
        private readonly Font font = new Font(FontFamily.GenericSerif, 16F);
        /// <summary>
        /// Saves the Player's key positions after every heartbeat, so if the player dies - we don't change the camera view. 
        /// </summary>
        private Vector2 _lastPlayerLocation = new Vector2();
        private Vector2 _lastLocalLocation = new Vector2();
        private Vector2 _lastMoveToLocation = new Vector2();

        private int _scaleCharacterToView = 1;
        private void AdjustScale(double clientRadius)
        {
            var minimumScale = Math.Round((viewWidth / 16) / clientRadius, MidpointRounding.AwayFromZero);;

            var scale = Math.Min(4, minimumScale);

            if (_scaleCharacterToView < scale)
            {
                _scaleCharacterToView += 1;
            }
            else if (_scaleCharacterToView > scale)
            {
                _scaleCharacterToView -= 1;
            }
        }


        /// <summary>
        /// Draw Sce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw_Scene(object? sender, PaintEventArgs e)
        {
            if (_clientLogic.IsConnectedToServer())
            {
                var client = _clientLogic.World.GetPlayer(_clientLogic.ClientID);

                //Create a Window in the screen.
                var viewWindow = new Rectangle(screenXOffset, screenYOffset, viewWidth, viewHeight);
                e.Graphics.SetClip(viewWindow);
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), viewWindow);

                //Update the Metrics we're tracking.
                Mass_Label.Text = client?.Mass.ToString() ?? "0";
                Position_Label.Text = client == null ? "0,0" : $"{client?.X.ToString("0000")},{client?.Y.ToString("0000")}";
                Food_Label.Text = _clientLogic.World.GetFoodCount().ToString();
                IncPacket_Label.Text = _clientLogic.GetPacketCount().ToString();

                if (client != null)
                {
                    //Scale
                    AdjustScale(client.GetRadius());

                    //Controls Movement and Camera when the player is alive.

                    ErrorLabel.Text = "Good luck!";
                    PlayerNameInput_Connected.Enabled = false;

                    _lastPlayerLocation.X = (client.X * _scaleCharacterToView);
                    _lastPlayerLocation.Y = (client.Y * _scaleCharacterToView);

                    var clientGraphic = new SolidBrush(Color.FromArgb(client.ARGBColor));

                    var clientDiameter = client.GetDiameter() * _scaleCharacterToView;
                    var clientRadius = client.GetRadius() * _scaleCharacterToView;

                    float clientLocalPosition_X = (float)(((viewWidth / 2) + screenXOffset));
                    float clientLocalPosition_Y = (float)(((viewHeight / 2) + screenYOffset));

                    _lastLocalLocation.X = clientLocalPosition_X;
                    _lastLocalLocation.Y = clientLocalPosition_Y;

                    e.Graphics.FillEllipse(clientGraphic, new Rectangle((int)_lastLocalLocation.X - (int)clientRadius, (int)_lastLocalLocation.Y - (int)clientRadius, (int)clientDiameter, (int)clientDiameter));

                    var clientFontSize = e.Graphics.MeasureString(client.Name, font);

                    e.Graphics.DrawString(client.Name, font, Brushes.Black,
                        (int)(_lastLocalLocation.X) - (clientFontSize.Width / 2),
                        (int)(_lastLocalLocation.Y) - (clientFontSize.Height / 2));


                    //Grab the Mouse Positions on the screen, and find the delta between them, and the center player.
                    int move_OffsetX = (int)((this.PointToClient(MousePosition).X + screenXOffset) - _lastLocalLocation.X - clientRadius);
                    int move_OffsetY = (int)((this.PointToClient(MousePosition).Y + screenYOffset) - _lastLocalLocation.Y - clientRadius);

                    //Add those values to the center player's position.
                    _lastMoveToLocation.X = (int)client.X + move_OffsetX;
                    _lastMoveToLocation.Y = (int)client.Y + move_OffsetY;

                    _logger.Log(LogLevel.Debug, JsonSerializer.Serialize(new
                    {
                        Client = JsonSerializer.Serialize(client),

                        XOffset = move_OffsetX,
                        YOffset = move_OffsetY,

                        MoveToPosition_X = _lastMoveToLocation.X,
                        MoveToPosition_Y = _lastMoveToLocation.Y
                    }));

                    ErrorLabel.Text = $"Moving to: {move_OffsetX.ToString("0000")},{move_OffsetY.ToString("0000")}";

                    MousePosition_Label.Text = $"{_lastMoveToLocation.X.ToString("0000")},{_lastMoveToLocation.Y.ToString("0000")}";

                    _clientLogic.SendMoveRequest((int)_lastMoveToLocation.X, (int)_lastMoveToLocation.Y);
                }
                else if (!_clientLogic.IsClientAlive())
                {
                    //Handles when the Player Dies.

                    ErrorLabel.Text = "Hit the Enter key to start over.";
                    PlayerNameInput_Connected.Enabled = true;

                    string deathMessage = "You have died.";

                    var fontSize = e.Graphics.MeasureString(deathMessage, new Font(FontFamily.GenericSerif, 24F));

                    e.Graphics.DrawString(deathMessage, font, Brushes.Red,
                        (int)((viewWidth / 2) + screenXOffset) - (fontSize.Width / 2),
                        (int)((viewHeight / 2) + screenYOffset) - (fontSize.Height / 2));
                }

                //Set the Position of all Food, in relation to the Player.
                foreach (var food in _clientLogic.World.GetFood())
                {
                    var foodGraphic = new SolidBrush(Color.FromArgb(food.ARGBColor));

                    var foodDiameter = food.GetDiameter() * _scaleCharacterToView;
                    var foodRadius = food.GetRadius() * _scaleCharacterToView;

                    float localFoodPosition_X = (((food.X * _scaleCharacterToView)) - (_lastPlayerLocation.X - _lastLocalLocation.X));
                    float localFoodPosition_Y = (((food.Y * _scaleCharacterToView)) - (_lastPlayerLocation.Y - _lastLocalLocation.Y));

                    if (localFoodPosition_X + foodRadius > screenXOffset &&
                        localFoodPosition_Y + foodRadius > screenYOffset &&
                        localFoodPosition_X - foodRadius < (screenXOffset + viewWidth) &&
                        localFoodPosition_Y - foodRadius < (screenYOffset + viewHeight))
                    {
                        e.Graphics.FillEllipse(foodGraphic, new Rectangle((int)(localFoodPosition_X - foodRadius), (int)(localFoodPosition_Y - foodRadius), (int)foodDiameter, (int)foodDiameter));


                        //var fontSize = e.Graphics.MeasureString($"{food.X},{food.Y}", font);

                        //e.Graphics.DrawString($"{food.X},{food.Y}", font, Brushes.Black,
                        //    (int)(localFoodPosition_X + foodRadius - (fontSize.Width / 2)),
                        //    (int)(localFoodPosition_Y + foodRadius - (fontSize.Height / 2)));
                    }
                }

                //Set Position of Players, in relation to the Player.
                foreach (var player in _clientLogic.World.GetPlayers())
                {
                    if (player.ID != _clientLogic.ClientID)
                    {
                        var playerGraphic = new SolidBrush(Color.FromArgb(player.ARGBColor));

                        var playerDiameter = player.GetDiameter() * _scaleCharacterToView;
                        var playerRadius = player.GetRadius() * _scaleCharacterToView;

                        float localPosition_X = (((player.X * _scaleCharacterToView)) - (_lastPlayerLocation.X - _lastLocalLocation.X));
                        float localPosition_Y = (((player.Y * _scaleCharacterToView)) - (_lastPlayerLocation.Y - _lastLocalLocation.Y));

                        if (localPosition_X + playerRadius > screenXOffset &&
                            localPosition_Y + playerRadius > screenYOffset &&
                            localPosition_X - playerRadius < (screenXOffset + viewWidth) &&
                            localPosition_Y - playerRadius < (screenYOffset + viewHeight))
                        {
                            e.Graphics.FillEllipse(playerGraphic, new Rectangle((int)(localPosition_X - playerRadius), (int)(localPosition_Y - playerRadius), (int)playerDiameter, (int)playerDiameter));

                            var fontSize = e.Graphics.MeasureString(player.Name, font);

                            e.Graphics.DrawString(player.Name, font, Brushes.Black,
                                (int)(localPosition_X - (fontSize.Width / 2)),
                                (int)(localPosition_Y - (fontSize.Height / 2)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles when a Player hits the Enter Key on the second playerName input after the player dies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerNameInput_Connected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                InitializeConnection(PlayerNameInput_Connected, HostnameInput);
            }
        }

        /// <summary>
        /// Handles when the Player is in a game, and hits the space bar - splitting the character. Can only be handled when
        /// the player is connected, and playing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgarioClient_KeyDown(object sender, KeyEventArgs e)
        {
            if ( _clientLogic.IsConnectedToServer() && e.KeyCode == Keys.Space)
            {
                var client = _clientLogic.World.GetPlayer(_clientLogic.ClientID);

                if (client != null)
                {
                    e.Handled = true;

                    _clientLogic.SendSplitRequest((int)_lastMoveToLocation.X, (int)_lastMoveToLocation.Y);
                }
            }
        }
    }
}