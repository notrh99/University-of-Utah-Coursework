using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer.Services;

namespace WebServer.Tests
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
    /// Tests the Repository Class
    /// </summary>
    [TestClass]
    public class RepositoryClientTests
    {
        private readonly Repository _systemUnderTest;

        /// <summary>
        /// Sets up the Secrets Json, builds the Connection string from the Config.
        /// </summary>
        public RepositoryClientTests()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();

            IConfigurationRoot Configuration = builder.Build();

            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = Configuration["ServerURL"],
                InitialCatalog = Configuration["DBName"],
                UserID = Configuration["UserName"],
                Password = Configuration["DBPassword"],
                ConnectTimeout = 15
            }.ConnectionString;

            _systemUnderTest = new Repository(connectionString);

        }

        /// <summary>
        /// Tests a Player is inserted, selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void InsertAndSelectPlayer()
        {
            var expected = new Player
            {
                PlayerName = Guid.NewGuid().ToString().Substring(1,10),
                InsertDate = DateTime.Today,
            };

            expected.PlayerId = _systemUnderTest.InsertPlayer(expected);

            var actual = _systemUnderTest.SelectPlayer(expected.PlayerName);

            Assert.AreEqual(expected.PlayerName, actual?.PlayerName);
            Assert.AreEqual(expected.PlayerId, actual?.PlayerId);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
        }

        /// <summary>
        /// Tests a Game is inserted, selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void InsertAndSelectGame()
        {
            var playerName = Guid.NewGuid().ToString();
            var playerId = _systemUnderTest.InsertPlayer(new Player
            {
                PlayerName = playerName,
                InsertDate = DateTime.Today,
            });

            var expected = new Game
            {
                PlayerId = playerId,
                BornTime = DateTime.Today.AddDays(-1),
                MaxSize = 100.15,
                MaxSizeTime = DateTime.Today.AddDays(1),
                InsertDate = DateTime.Today.AddDays(-2),
                DeathTime = DateTime.Today.AddDays(2),
                PlayerName = playerName
            };

            expected.GameId = _systemUnderTest.InsertGame(expected);

            var games = _systemUnderTest.SelectGames();

            var actual = games.First(x => x.GameId == expected.GameId);

            Assert.AreEqual(expected.PlayerId, actual?.PlayerId);
            Assert.AreEqual(expected.GameId, actual?.GameId);
            Assert.AreEqual(expected.BornTime, actual?.BornTime);
            Assert.AreEqual(expected.MaxSize, actual?.MaxSize);
            Assert.AreEqual(expected.MaxSizeTime, actual?.MaxSizeTime);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
            Assert.AreEqual(expected.DeathTime, actual?.DeathTime);
        }

        /// <summary>
        /// Tests a Game is inserted, selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void InsertAndSelectGame_WITH_Null()
        {
            var playerName = Guid.NewGuid().ToString();
            var playerId = _systemUnderTest.InsertPlayer(new Player
            {
                PlayerName = playerName,
                InsertDate = DateTime.Today,
            });

            var expected = new Game
            {
                PlayerId = playerId,
                BornTime = DateTime.Today.AddDays(-1),
                MaxSize = null,
                MaxSizeTime = null,
                InsertDate = DateTime.Today.AddDays(-2),
                DeathTime = null,
                PlayerName = playerName
            };

            expected.GameId = _systemUnderTest.InsertGame(expected);

            var games = _systemUnderTest.SelectGames();

            var actual = games.First(x => x.GameId == expected.GameId);

            Assert.AreEqual(expected.PlayerId, actual?.PlayerId);
            Assert.AreEqual(expected.GameId, actual?.GameId);
            Assert.AreEqual(expected.BornTime, actual?.BornTime);
            Assert.AreEqual(expected.MaxSize, actual?.MaxSize);
            Assert.AreEqual(expected.MaxSizeTime, actual?.MaxSizeTime);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
            Assert.AreEqual(expected.DeathTime, actual?.DeathTime);
        }

        /// <summary>
        /// Tests a Game is inserted, UPDATES IT, then selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void UpdateAndSelectGame()
        {
            var playerId = _systemUnderTest.InsertPlayer(new Player
            {
                PlayerName = Guid.NewGuid().ToString(),
                InsertDate = DateTime.Today,
            });

            var expected = new Game
            {
                PlayerId = playerId,
                BornTime = DateTime.Today.AddDays(-1),
                MaxSize = 100,
                MaxSizeTime = DateTime.Today.AddDays(1),
                InsertDate = DateTime.Today.AddDays(-2),
                DeathTime = DateTime.Today.AddDays(2)
            };

            expected.GameId = _systemUnderTest.InsertGame(expected);

            expected.MaxSize = 200;
            expected.DeathTime = DateTime.Today.AddHours(12);
            expected.MaxSizeTime = DateTime.Today.AddHours(5);

            _systemUnderTest.UpdateGame(expected);

            var games = _systemUnderTest.SelectGames();

            var actual = games.First(x => x.GameId == expected.GameId);

            Assert.AreEqual(expected.PlayerId, actual?.PlayerId);
            Assert.AreEqual(expected.GameId, actual?.GameId);
            Assert.AreEqual(expected.BornTime, actual?.BornTime);
            Assert.AreEqual(expected.MaxSize, actual?.MaxSize);
            Assert.AreEqual(expected.MaxSizeTime, actual?.MaxSizeTime);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
            Assert.AreEqual(expected.DeathTime, actual?.DeathTime);
        }

        /// <summary>
        /// Tests a PlayerSize is inserted, selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void UpdateAndSelectPlayerSize()
        {
            var playerId = _systemUnderTest.InsertPlayer(new Player
            {
                PlayerName = Guid.NewGuid().ToString(),
                InsertDate = DateTime.Today,
            });

            var gameId = _systemUnderTest.InsertGame(new Game
            {
                PlayerId = playerId,
                BornTime = DateTime.Today.AddDays(-1),
                MaxSize = 100,
                MaxSizeTime = DateTime.Today.AddDays(1),
                InsertDate = DateTime.Today.AddDays(-2),
                DeathTime = DateTime.Today.AddDays(2)
            });

            var expected = new PlayerSize
            {
                GameId = gameId,
                Size = 150.25,
                InsertDate = DateTime.Today.AddHours(5),
            };

            expected.PlayerSizeId = _systemUnderTest.InsertPlayerSize(expected);

            var actual = _systemUnderTest.SelectPlayerSize(expected.PlayerSizeId);

            Assert.AreEqual(expected.PlayerSizeId, actual?.PlayerSizeId);
            Assert.AreEqual(expected.GameId, actual?.GameId);
            Assert.AreEqual(expected.Size, actual?.Size);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
        }


        /// <summary>
        /// Tests a Game is inserted, UPDATES IT, then selects it out and checks the params are correct.
        /// </summary>
        [TestMethod]
        public void UpdateAndSelectGame_WHEN_null()
        {
            var playerId = _systemUnderTest.InsertPlayer(new Player
            {
                PlayerName = Guid.NewGuid().ToString(),
                InsertDate = DateTime.Today,
            });

            var expected = new Game
            {
                PlayerId = playerId,
                BornTime = DateTime.Today.AddDays(-1),
                MaxSize = 100,
                MaxSizeTime = DateTime.Today.AddDays(1),
                InsertDate = DateTime.Today.AddDays(-2),
                DeathTime = DateTime.Today.AddDays(2)
            };

            expected.GameId = _systemUnderTest.InsertGame(expected);

            expected.MaxSize = 2050.0;
            expected.DeathTime = null;
            expected.MaxSizeTime = null;

            _systemUnderTest.UpdateGame(expected);

            var games = _systemUnderTest.SelectGames();

            var actual = games.First(x => x.GameId == expected.GameId);

            Assert.AreEqual(expected.PlayerId, actual?.PlayerId);
            Assert.AreEqual(expected.GameId, actual?.GameId);
            Assert.AreEqual(expected.BornTime, actual?.BornTime);
            Assert.AreEqual(expected.MaxSize, actual?.MaxSize);
            Assert.AreEqual(expected.MaxSizeTime, actual?.MaxSizeTime);
            Assert.AreEqual(expected.InsertDate, actual?.InsertDate);
            Assert.AreEqual(expected.DeathTime, actual?.DeathTime);
        }

        /// <summary>
        /// Tests the Setup is correct. Commented out, because it will fail if the tables already exist.
        /// </summary>
        //[TestMethod]
        //public void SetUpTableTest()
        //{
        //    _systemUnderTest.SetupTablesAndSeedRandomRecords();
        //}
    }
}