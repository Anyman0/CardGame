using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Deck
    {
        private const int DeckSize = 52;
        private const int SameCardAmount = 4;   
        public Card[] CardDeck;
        public Deck(Card[] deck)
        { 
            CardDeck= deck;
        }

        public void DrawCard(Card card) 
        { 

        }

        public Card[] CreateDeck()
        {
            for (int i = 0; i < DeckSize / SameCardAmount; i++)
            {
                for(int j = 0; j < SameCardAmount; j++)
                {
                    Card card = new Card(i + 2);
                    CardDeck.Append(card);
                    Console.WriteLine(card.Value);
                }
            }
            return CardDeck;
        }

       
    }
}
