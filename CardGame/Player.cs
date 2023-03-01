using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Player
    {
        public readonly string Name;
        public List<Card> PlayerHand;
        public Player(string name, List<Card> playerHand) 
        {
            Name = name;
            PlayerHand = playerHand;
        }
    }
}
