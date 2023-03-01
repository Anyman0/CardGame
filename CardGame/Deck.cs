using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Deck : Table
    {
        private const int DeckSize = 52;
        private const int SameCardAmount = 4;   
        public List<Card> CardDeck;
        public Deck(List<Card> deck) : base (deck)
        { 
            CardDeck= deck;
        }

        public void DrawCard(Card card, Player player) 
        {
            player.PlayerHand.Add(card);
        }

        public List<Card> CreateDeck()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < DeckSize / SameCardAmount; i++)
            {
                for(int j = 0; j < SameCardAmount; j++)
                {
                    Card card = new Card(i + 2);
                    cards.Add(card);                   
                }
            }
            return cards;
        }

        // Below method shuffles the given deck
        public List<Card> ShuffleDeck(List<Card> deck)
        {            
            Card[] shuffledDeck = deck.ToArray();
            Random rnd = new Random();
            shuffledDeck = deck.OrderBy(x => rnd.Next()).ToArray();           
            
            return shuffledDeck.ToList();
        } 

        public string DisplayDeck(int deckSize)
        {
            string visibleDeckSize;
            if(deckSize >= 10) visibleDeckSize = $"\nDECK:   \n┌──┐\n│{deckSize}│\n└──┘";
            else visibleDeckSize = $"\nDECK:   \n┌──┐\n│{deckSize} │\n└──┘";
            return visibleDeckSize;
        }

       
    }
}
