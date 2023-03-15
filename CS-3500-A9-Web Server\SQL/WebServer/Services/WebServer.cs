using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Communications;
using Microsoft.Extensions.Logging;

namespace WebServer.Services
{
    /// <summary> 
    /// Author:    Tyler DeBruin and Rayyan Hamid & H. James de St. Germain
    /// Partner:   None
    /// Date:      4-27-2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// Starter Code for a simple web server
    ///
    /// Modified by Tyler Debruin and Rayyan Hamid to match the specifications of Assignment 9.
    /// Listens on a specified Port for HTTP Requests.
    ///
    /// <para>
    ///     The following messages are actionable:
    ///   </para>
    ///   <para>
    ///      get highscore - respond with a highscore page
    ///   </para>
    ///   <para>
    ///      get favicon - don't do anything (we don't support this)
    ///   </para>
    ///   <para>
    ///      get scores - along with a name, respond with a list of scores for the particular user
    ///   </para>
    ///   <para>
    ///     create - contact the DB and create the required tables and seed them with some dummy data
    ///   </para>
    ///   <para>
    ///     get index (or "", or "/") - send a happy home page back
    ///   </para>
    ///   <para>
    ///     get css/styles.css?v=1.0  - send your sites css file data back
    ///   </para>
    ///   <para>
    ///     otherwise send a page not found error
    ///   </para>
    /// </summary>
    public class WebServer
    {
        private readonly ILogger<WebServer> _logger;
        private readonly Repository _repository;

        /// <summary>
        /// Public constructor utilized by the Dependency Injection framework.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public WebServer(ILogger<WebServer> logger, Repository repository)
        {
            _logger = logger;
            _repository = repository;
            _networking = new Networking(logger, OnClientConnect, OnDisconnect, onMessage, '\n');
        }

        /// <summary>
        /// Start the Server
        /// </summary>
        /// <param name="port"></param>
        public void StartServer(int port)
        {
            _networking.WaitForClients(port, true);
        }

        /// <summary>
        /// Stop the Server
        /// </summary>
        /// <param name="port"></param>
        public void StopServer()
        {
            _networking.StopWaitingForClients();
        }

        /// <summary>
        /// keep track of how many requests have come in.  Just used
        /// for display purposes.
        /// </summary>
        private int _counter = 1;

        /// <summary>
        /// Networking Class, used to start the server.
        /// </summary>
        private readonly Networking _networking;

        /// <summary>
        ///   <para>
        ///     When a request comes in (from a browser) this method will
        ///     be called by the Networking code.  Each line of the HTTP request
        ///     will come as a separate message.  The "line" we are interested in
        ///     is a PUT or GET request.  
        ///   </para>
        ///   <para>
        ///     The following messages are actionable:
        ///   </para>
        ///   <para>
        ///      get highscore - respond with a highscore page
        ///   </para>
        ///   <para>
        ///      get favicon - don't do anything (we don't support this)
        ///   </para>
        ///   <para>
        ///      get scores - along with a name, respond with a list of scores for the particular user
        ///   </para>
        ///   <para>
        ///     create - contact the DB and create the required tables and seed them with some dummy data
        ///   </para>
        ///   <para>
        ///     get index (or "", or "/") - send a happy home page back
        ///   </para>
        ///   <para>
        ///     get css/styles.css?v=1.0  - send your sites css file data back
        ///   </para>
        ///   <para>
        ///     otherwise send a page not found error
        ///   </para>
        ///   <para>
        ///     Warning: when you send a response, the web browser is going to expect the message to
        ///     be line by line (new line separated) but we use new line as a special character in our
        ///     networking object.  Thus, you have to send _every line of your response_ as a new Send message.
        ///   </para>
        /// </summary>
        /// <param name="network_message_state"> provided by the Networking code, contains socket and message</param>
        internal void onMessage(Networking channel, string message)
        {
            _logger.Log(LogLevel.Information, $"{channel.ID}: {message}");
            if (message.StartsWith("GET"))
            {
                channel.Send("HTTP / 1.1 200 OK");
                channel.Send("Connection: close");
                channel.Send("Content-Type: text/html; charset=UTF-8");
                channel.Send("");

                var scoresRegex = new Regex("GET /scores/([^\\s]+) HTTP/1\\.1");
                var updateScoresRegex = new Regex("GET /scores/([^/]+)/([\\d]+)/([\\d]+)/([\\d]+)/([\\d]+) HTTP/1.1");

                if (new[] { "GET / HTTP/1.1", "GET /index HTTP/1.1", "GET /index.html HTTP/1.1", "GET  HTTP/1.1" }.Contains(message))
                {
                    channel.Send(LoadHtmlPage(Page.Index));

                    _counter++;
                }
                else if (new[] { "GET /highscores HTTP/1.1" }.Contains(message))
                {
                    channel.Send(LoadHtmlPage(Page.HighScores));
                }
                else if (new[] { "GET /create HTTP/1.1" }.Contains(message))
                {
                    channel.Send(LoadHtmlPage(Page.CreateDatabase));
                }
                else if (new[] { "GET /fancy HTTP/1.1" }.Contains(message))
                {
                    channel.Send(LoadHtmlPage(Page.Fancy));
                }
                else if(updateScoresRegex.IsMatch(message))
                {
                    var match = updateScoresRegex.Match(message);
                    var name = match.Groups[1].Value;
                    var massString = match.Groups[2].Value;
                    var rankString = match.Groups[3].Value;
                    var startTimeString = match.Groups[4].Value;
                    var endTimeString = match.Groups[5].Value;

                    var urlDecodedName = name.Replace("%20", " ");
                    int mass = int.Parse(massString);
                    int rank = int.Parse(rankString);
                    int startTime = int.Parse(startTimeString);
                    int endTime = int.Parse(endTimeString);

                    channel.Send(LoadHtmlPage(Page.UpdatePlayerScore, urlDecodedName, mass, startTime, endTime));
                }
                else if (scoresRegex.IsMatch(message))
                {
                    var match = scoresRegex.Match(message);
                    var name = match.Groups[1].Value;

                    var urlDecodeName = name.Replace("%20", " ");

                    channel.Send(LoadHtmlPage(Page.PlayerScore, urlDecodeName));
                }
                else
                {
                    channel.Send(LoadHtmlPage(Page.PageNotFound));
                }
            }

            channel.Disconnect();
        }

        /// <summary>
        /// Enum to specify the page, utilized by private functions inside the web server.
        /// </summary>
        private enum Page
        {
            Index = 0,
            HighScores = 1,
            PlayerScore = 2,
            PageNotFound,
            CreateDatabase,
            UpdatePlayerScore,
            Fancy
        }

        /// <summary>
        /// Main Function to Handle Loading the HTML - and Performing various Actions.
        /// The Page Enum is a private member of this class, that tells the server with HTML Page to load.
        ///
        /// The Page is loaded from the Views folder. This allows you to modify it in Visual Studio - and the template is then modified by this function to
        /// create additional functionality. The Optional Parameters are really only utilized by the GET Method that creates data, according to the assignment specifications.
        ///
        /// A lock is created every time an html page loads, to ensure the file can be opened correctly across different threads.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="playerName"></param>
        /// <param name="mass"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private string LoadHtmlPage(Page page, string? playerName = null, int? mass = null, int? startTime = null, int? endTime = null)
        {
            string result = "";

            if (page == Page.Index)
            {
                lock (this)
                {
                    var htmlFile = File.ReadAllText("./view/index.html");

                    result = htmlFile.Replace("{{VisitorCount}}", _counter.ToString());
                }
            }
            else if (page == Page.HighScores)
            {
                //Get all the Games
                var games = _repository.SelectGames();

                var completyedGames =
                    games.Where(x => x.MaxSize != null && x.MaxSizeTime != null && x.DeathTime != null);

                //Create an HTML Template, but list out the stars we're interested in.
                var highScores = completyedGames.OrderByDescending(x => x.MaxSize).Select(x =>
                {
                    string calculatedMaxSizeSeconds = x.MaxSizeTime.HasValue ? (x.MaxSizeTime - x.BornTime).Value.TotalSeconds.ToString() : "N/A";
                    string calculatedDeathSecondTime = x.MaxSizeTime.HasValue && x.DeathTime.HasValue ? (x.DeathTime - x.MaxSizeTime).Value.TotalSeconds.ToString() : "N/A";

                    return $"<tr><td><a href=\"/scores/{x.PlayerName}\">{x.PlayerName}</a></ td ><td>{x.MaxSize}</td><td>{calculatedMaxSizeSeconds}</td><td>{calculatedDeathSecondTime}</td></tr >";
                });

                lock (this)
                {
                    var html = File.ReadAllText("./view/highscores.html");

                    result = html.Replace("{{PlayerScores}}", string.Join("\n", highScores));
                }
            }
            else if (page == Page.Fancy)
            {
                var games = _repository.SelectGames();

                var completyedGames =
                    games.Where(x => x.MaxSize != null && x.MaxSizeTime != null && x.DeathTime != null);

                var totalTimes = completyedGames.GroupBy(x => x.PlayerName).Select(x =>
                {
                    var bestScore = x.OrderByDescending(y => y.DeathTime.HasValue ? (y.DeathTime - y.BornTime).Value.TotalSeconds :  0).First();

                    var score = bestScore.DeathTime.HasValue
                        ? (bestScore.DeathTime - bestScore.BornTime).Value.TotalSeconds.ToString()
                        : "N/A";

                    return $"['{bestScore.PlayerName}', {score}],";
                }).Distinct();

                lock (this)
                { 
                    var html = File.ReadAllText("./view/fancy.html");

                    result = html.Replace("{{NameAndAliveSeconds}}", string.Join("\n", string.Join('\n', totalTimes)));
                }
            }
            else if (page == Page.UpdatePlayerScore)
            {
                ///Create the Data First - Then tell the client to go to the Player's score page.
                if (!string.IsNullOrEmpty(playerName) &&
                    startTime != null &&
                    endTime != null &&
                    mass != null)
                {
                    var player = _repository.SelectPlayer(playerName);

                    int playerId = 0;

                    if (player == null)
                    {
                        playerId = _repository.InsertPlayer(new Player
                        {
                            PlayerName = playerName,
                            InsertDate = DateTime.Now
                        });
                    }
                    else
                    {
                        playerId = player.PlayerId;
                    }

                    _repository.InsertGame(new Game
                    {
                        InsertDate = DateTime.Now,
                        MaxSizeTime = DateTime.UnixEpoch.AddSeconds(endTime.Value),
                        DeathTime = DateTime.UnixEpoch.AddSeconds(endTime.Value),
                        MaxSize = mass.Value,
                        BornTime = DateTime.UnixEpoch.AddSeconds(startTime.Value),
                        PlayerId = playerId
                    });
                }

                lock (this)
                {
                    var html = File.ReadAllText("./view/updateplayerscore.html");

                    result = html.Replace("{{PlayerName}}", playerName);
                }
            }
            else if (page == Page.PlayerScore)
            {
                // Lists all the data for a player.
                var games = _repository.SelectGames();

                var allPlayerGames = games.Where(x => x.PlayerName == playerName && x.DeathTime != null && x.MaxSize != null && x.MaxSizeTime != null).Select(x =>
                {

                    string calculatedMaxSizeSeconds = x.MaxSizeTime.HasValue ? (x.MaxSizeTime - x.BornTime).Value.TotalSeconds.ToString() : "N/A";
                    string calculatedDeathSecondTime = x.MaxSizeTime.HasValue && x.DeathTime.HasValue ? (x.DeathTime - x.MaxSizeTime).Value.TotalSeconds.ToString() : "N/A";

                    return
                        $"<tr><td>{x.BornTime}</td><td>{x.DeathTime}</td><td>{x.MaxSize}</td><td>{calculatedMaxSizeSeconds}</td><td>{calculatedDeathSecondTime}</td></tr >";
                });

                lock (this)
                {
                    var html = File.ReadAllText("./view/playerscore.html");

                    result = html.Replace("{{PlayerScores}}", string.Join("\n", allPlayerGames)).Replace("{{PlayerName}}", playerName);
                }
            }
            else if (page == Page.CreateDatabase)
            {
                lock (this)
                {
                    string setupOutcome = "Successfully setup, and seeded database tables.";

                    try
                    {
                        _repository.SetupTablesAndSeedRandomRecords();
                    }
                    catch (Exception)
                    {
                        setupOutcome = "Database Tables already exist.";
                    }

                    var html = File.ReadAllText("./view/createdatabaseoutcome.html");

                    result = html.Replace("{{DatabaseOutcome}}", setupOutcome);
                }
            }
            else if (page == Page.PageNotFound)
            {
                lock (this)
                {
                    result = File.ReadAllText("./view/pagenotfound.html");
                }
            }

            return result;
        }

        /// <summary>
        /// OnDisconnect Delegate used by the Networking class.
        /// </summary>
        /// <param name="channel">Channel of client disconnecting.</param>
        internal void OnDisconnect( Networking channel )
        {
            _logger.Log(LogLevel.Information, $"{channel.ID}: **Disconnected from WebServer.**");
        }

        /// <summary>
        /// Basic connect handler - i.e., a browser has connected!
        /// Print an information message
        /// </summary>
        /// <param name="channel"> the Networking connection</param>
        internal void OnClientConnect(Networking channel)
        {
            _logger.Log(LogLevel.Information, $"{channel.ID}: **Connected to WebServer.**");
        }
    }
}

