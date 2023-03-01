using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Card
    {
        private const int _maxCardValue = 14;
        private const int _minCardValue = 2;

        public readonly int Value;
        public Card(int value) 
        {
            Value = value;
        }     
        
        public string PresentCard(Card card)
        {
            string visibleCard = $"┌─┐\n│{card.Value}│\n└─┘";
            return visibleCard;
        }

    }
}
