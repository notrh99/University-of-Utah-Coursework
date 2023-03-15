using System.Data;
using System.Data.SqlClient;

namespace WebServer.Services
{
    /// <summary> 
    /// Author:    Tyler DeBruin
    /// Partner:   Rayyan Hamid
    /// Date:      4-27-2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    ///
    /// Repository Class for interacting with the database. Performs CRUD Operations - no fancy logic. That should be handled above in the WebServer, or the Client GUI.
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Injected by the DI Framework.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Creates a Repository object, with a connection string passed from the DI Framework. Ultimately resovlves it from the secret.json. See the Program.cs for more info on the setup.
        /// </summary>
        /// <param name="connectionString"></param>
        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Sets up and Seeds the Database tables.
        /// The Tables are created one at a time, and 10 records for each datapoint are created. The names are random guids.
        ///
        /// Helps to easily 'get started' - when you don't have anything setup prior.
        /// </summary>
        public void SetupTablesAndSeedRandomRecords()
        {
            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var playerTable = con.CreateCommand();
                playerTable.CommandText = DatabaseConstants.CreatePlayersTable;
                playerTable.ExecuteScalar();

                for (int i = 1; i < 11; i++)
                {
                    InsertPlayer(new Player
                    {
                        InsertDate = DateTime.Now,
                        PlayerName = Guid.NewGuid().ToString().Substring(0,10)
                    });
                }

                var gameTable = con.CreateCommand();
                gameTable.CommandText = DatabaseConstants.CreateGameTable;
                gameTable.ExecuteScalar();

                for (int i = 1; i < 11; i++)
                {
                    var newPlayerDateTime = DateTime.Now.AddHours(-1 * i);

                    InsertGame(new Game
                    {
                        PlayerId = i,
                        MaxSizeTime = newPlayerDateTime.AddMinutes(5),
                        DeathTime = newPlayerDateTime.AddMinutes(7),
                        MaxSize = i * 100,
                        BornTime = newPlayerDateTime,
                        InsertDate = newPlayerDateTime,
                    });
                }

                var playerSizeTable = con.CreateCommand();
                playerSizeTable.CommandText = DatabaseConstants.CreatePlayerSizeTable;
                playerSizeTable.ExecuteScalar();

                for (int i = 1; i < 100; i++)
                {
                    InsertPlayerSize(new PlayerSize
                    {
                        Size = i, 
                        GameId = 1,
                        InsertDate = DateTime.Now.AddMinutes(-1 * i),
                    });
                }
            }
        }

        /// <summary>
        /// Creates a Player record in the Database. The client is responsible for making sure the Name doesn't already exist.
        /// </summary>
        /// <param name="player">The player to create</param>
        /// <returns>The ID created from the database.</returns>
        /// <exception cref="Exception"></exception>
        public int InsertPlayer(Player player)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.InsertPlayer;

                sqlCommand.Parameters.AddWithValue("@PlayerName", player.PlayerName);
                sqlCommand.Parameters.AddWithValue("@InsertDate", player.InsertDate);

                var playerId = sqlCommand.ExecuteScalar();

                if (playerId != null && int.TryParse(playerId.ToString(), out int idresult))
                {
                    return idresult;
                }

                throw new Exception("Unable to Insert");
            }
        }

        /// <summary>
        /// Selects the first player record with the playerName provided.
        ///
        /// If the Id doesn't exist, returns a null object.
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>If the Id doesn't exist, returns a null object.</returns>
        public Player? SelectPlayer(string playerName)
        {
            Player? result = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.SelectPlayer;

                var queryParameter = sqlCommand.Parameters.Add("@PlayerName", SqlDbType.VarChar, 10);
                queryParameter.Value = playerName;

                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        var playerNameString = sqlReader["PlayerName"].ToString();
                        var dateTimeString = sqlReader["InsertDate"].ToString();
                        var playerIdString = sqlReader["PlayerId"].ToString();

                        if (!string.IsNullOrEmpty(playerNameString) &&
                            !string.IsNullOrEmpty(dateTimeString) &&
                            !string.IsNullOrEmpty(playerIdString))
                        {
                            result = new Player
                            {
                                PlayerName = playerNameString,
                                InsertDate = DateTime.Parse(dateTimeString),
                                PlayerId = int.Parse(playerIdString)
                            };
                        }


                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Inserts a Game Object - The client will insert, store the ID - and Update when a max size is obtained.
        /// </summary>
        /// <param name="game"></param>
        /// <returns>The ID created from the database.</returns>
        /// <exception cref="Exception"></exception>
        public int InsertGame(Game game)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.InsertGame;

                if(game.DeathTime.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@DeathTime", game.DeathTime);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@DeathTime", DBNull.Value);
                }

                if (game.MaxSizeTime.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSizeTime", game.MaxSizeTime);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSizeTime", DBNull.Value);
                }

                if (game.MaxSize.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSize", game.MaxSize);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSize", DBNull.Value);
                }

                sqlCommand.Parameters.AddWithValue("@PlayerId", game.PlayerId);
                sqlCommand.Parameters.AddWithValue("@BornTime", game.BornTime);
                sqlCommand.Parameters.AddWithValue("@InsertDate", game.InsertDate);

                var gameId = sqlCommand.ExecuteScalar();

                if (gameId != null && int.TryParse(gameId.ToString(), out int idresult))
                {
                    return idresult;
                }

                throw new Exception("Unable to Insert");
            }
        }

        /// <summary>
        /// Updates a Game Object in the database.
        /// Only allows a client to Update: DeathTime, MaxSize, MaxSizeTime.
        /// </summary>
        /// <param name="game"></param>
        public void UpdateGame(Game game)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.UpdateGame;

                if (game.DeathTime.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@DeathTime", game.DeathTime);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@DeathTime", DBNull.Value);
                }

                if (game.MaxSizeTime.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSizeTime", game.MaxSizeTime);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSizeTime", DBNull.Value);
                }

                if (game.MaxSize.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSize", game.MaxSize);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@MaxSize", DBNull.Value);
                }

                sqlCommand.Parameters.AddWithValue("@GameId", game.GameId);

                sqlCommand.ExecuteScalar();
            }
        }

        /// <summary>
        /// Select all of the Games from the database.
        ///
        /// </summary>
        /// <returns>Returns an Empty List if no games exist.</returns>
        public List<Game> SelectGames()
        {
            var result = new List<Game>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.SelectGame;

                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        var gameId = sqlReader["GameId"].ToString();
                        var playerIdString = sqlReader["PlayerId"].ToString();
                        var playerName = sqlReader["PlayerName"].ToString();

                        var bornTime = sqlReader["BornTime"].ToString();
                        var deathTime = sqlReader["DeathTime"].ToString();
                        var maxSize = sqlReader["MaxSize"].ToString();
                        var maxSizeTime = sqlReader["MaxSizeTime"].ToString();
                        var insertDateTime = sqlReader["InsertDate"].ToString();

                        if (!string.IsNullOrEmpty(gameId) &&
                            !string.IsNullOrEmpty(playerIdString) &&
                            !string.IsNullOrEmpty(insertDateTime) &&
                            !string.IsNullOrEmpty(bornTime))
                        {
                            result.Add(new Game
                            {
                                GameId = int.Parse(gameId),
                                PlayerId = int.Parse(playerIdString),
                                BornTime = DateTime.Parse(bornTime),
                                InsertDate = DateTime.Parse(insertDateTime),
                                MaxSize = string.IsNullOrEmpty(maxSize) ? null : double.Parse(maxSize),
                                MaxSizeTime = string.IsNullOrEmpty(maxSizeTime) ? null : DateTime.Parse(maxSizeTime),
                                DeathTime = string.IsNullOrEmpty(deathTime) ? null : DateTime.Parse(deathTime),
                                PlayerName = playerName
                            });
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Inserts a PlayerSize object - Records the player as they grow in the game.
        /// </summary>
        /// <param name="playerSize"></param>
        /// <returns>The ID created from the database.</returns>
        /// <exception cref="Exception"></exception>
        public int InsertPlayerSize(PlayerSize playerSize)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.InsertPlayerSize;

                sqlCommand.Parameters.AddWithValue("@GameId", playerSize.GameId);
                sqlCommand.Parameters.AddWithValue("@Size", playerSize.Size);
                sqlCommand.Parameters.AddWithValue("@InsertDate", playerSize.InsertDate);

                var id = sqlCommand.ExecuteScalar();

                if (id != null && int.TryParse(id.ToString(), out int idresult))
                {
                    return idresult;
                }

                throw new Exception("Unable to Insert");
            }
        }

        /// <summary>
        /// Selects the PlayerSize with the given id. Exists to test that the insert works correctly.
        ///
        /// </summary>
        /// <param name="playerSizeId"></param>
        /// <returns>If the Id doesn't exist, returns a null object.</returns>
        public PlayerSize? SelectPlayerSize(int playerSizeId)
        {
            PlayerSize? result = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var sqlCommand = con.CreateCommand();

                sqlCommand.CommandText = DatabaseConstants.SelectPlayerSize;

                var queryParameter = sqlCommand.Parameters.Add("@PlayerSizeId", SqlDbType.VarChar, 10);
                queryParameter.Value = playerSizeId;

                using (var sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        var gameId = sqlReader["GameId"].ToString();
                        var playerSizeIdString = sqlReader["PlayerSizeID"].ToString();
                        var size = sqlReader["Size"].ToString();
                        var insertDateString = sqlReader["InsertDate"].ToString();

                        if (!string.IsNullOrEmpty(gameId) &&
                            !string.IsNullOrEmpty(playerSizeIdString) &&
                            !string.IsNullOrEmpty(insertDateString) &&
                            !string.IsNullOrEmpty(size))
                        {
                            result = new PlayerSize
                            {
                                GameId = int.Parse(gameId),
                                Size = double.Parse(size),
                                PlayerSizeId = int.Parse(playerSizeIdString),
                                InsertDate = DateTime.Parse(insertDateString),
                            };
                        }
                    }
                }

                return result;
            }
        }
    }
}
