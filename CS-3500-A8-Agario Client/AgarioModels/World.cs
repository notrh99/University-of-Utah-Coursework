using Microsoft.Extensions.Logging;

namespace AgarioModels
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
    /// Contains a core World - That handles the players, and their positions defined by the server.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Stores the IDs of the Food and Player objects, for easy access.
        /// </summary>
        private Dictionary<long, Player> _players = new Dictionary<long, Player>();
        private Dictionary<long, Food> _food = new Dictionary<long, Food>();

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        //Constructor to pass in the logger
        public World(ILogger logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Convenience method to AddFood, Handles multi threaded applications.
        /// </summary>
        /// <param name="food"></param>
        public void AddFood(Food food)
        {
            lock (_food)
            {
                if (_food.ContainsKey(food.ID))
                {
                    _food[food.ID] = food;

                }
                else
                {
                    _food.Add(food.ID, food);
                }
            }
        }
        /// <summary>
        /// Convenience method to GetFood, Handles multi threaded applications.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Food> GetFood()
        {
            lock (_food)
            {
                var result = new List<Food>(_food.Values.ToList());

                return result;
            }
        }
        /// <summary>
        /// Convenience method to RemoveFood, Handles multi threaded applications.
        /// </summary>
        /// <param name="foodId"></param>
        public void RemoveFood(long foodId)
        {
            lock (_food)
            {
                if (_food.ContainsKey(foodId))
                {
                    _food.Remove(foodId);
                }
            }
        }
        /// <summary>
        /// Convenience method to GetPlayers, Handles multi threaded applications.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Player> GetPlayers()
        {
            lock (_players)
            {
                var result = new List<Player>(_players.Values.ToList());

                return result;
            }
        }
        /// <summary>
        /// Convenience method to remove a single player, Handles multi threaded applications.
        /// </summary>
        /// <param name="playerId"></param>
        public void RemovePlayer(long playerId)
        {
            lock (_players)
            {
                if (_players.ContainsKey(playerId))
                {
                    _players.Remove(playerId);
                }
            }
        }
        /// <summary>
        /// Convenience method to add a single player, Handles multi threaded applications.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            lock (_food)
            {
                if (_players.ContainsKey(player.ID))
                {
                    _players[player.ID] = player;

                }
                else
                {
                    _players.Add(player.ID, player);
                }
            }
        }
        /// <summary>
        /// Convenience method to get a single player, Handles multi threaded applications.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Player? GetPlayer(long Id)
        {
            lock (_players)
            {
                Player? result = null;

                if (_players.ContainsKey(Id))
                {
                    result = _players[Id];
;               }

                return result;
            }
        }

        /// <summary>
        /// Returns the count of food in the game.
        /// </summary>
        /// <returns></returns>
        public int GetFoodCount()
        {
            lock (_food)
            {
                return _food.Count;
            }
        }
    }
}