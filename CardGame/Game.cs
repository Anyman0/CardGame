using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Game
    {
        private const int _playerAmount = 4;        
        private static Player[] _AIPlayers = new Player[] { new Player("Dave", new List<Card>()), new Player("Mia", new List<Card >()), new Player("Jack", new List < Card >()), 
                                             new Player("Lola", new List < Card >()), new Player("Billy", new List < Card >()),  new Player("Nina", new List < Card >()), new Player("Zack", new List < Card >()),
                                             new Player("Phil", new List < Card >()) };         
        public Player[] Players;         
        public Game(Player[] players) 
        {
            Players = players;
        }

        public Player[] CreateGame(Player player)
        {
            List<Player> players = new List<Player>();
            Random rnd = new Random();
            Player[] shuffledPlayers = _AIPlayers.OrderBy(x => rnd.Next()).ToArray();
            for(int i = 0; i < _playerAmount - 1; i++)
            {
                players.Add(shuffledPlayers[i]);
            }
            players.Add(player);
            Players = players.ToArray();
            return Players;
        }

        public string Greeting(string name)
        {
            string greeting = $"Welcome to play simple card game, {name} :)";
            string opponents = $"\nYou will be playing against {Players[0].Name}, {Players[1].Name} and {Players[2].Name}.";
            return greeting + opponents;
        }        

        public bool CheckGameState(Player[] players)
        {
            bool state = false;
            foreach(Player player in players)
            {
                if(player.PlayerHand.Count == 0) 
                {
                    state = true;
                    Console.WriteLine($"Game has ended! The winner is: {player.Name.ToUpper()}!");
                }
            }
            return state;
        }
    }
}
