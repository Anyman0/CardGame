using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Hand : Player
    {
        private const int _maxDrawValue = 5;
        public int MaxDrawValue = _maxDrawValue;
        
        public Hand(string name, List<Card> cards) : base(name, cards)
        { 
            
        }        
    }
}