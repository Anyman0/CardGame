using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    internal class Table
    {
        public List<Card> TableCards;  
        public Table(List<Card> table) 
        {
            TableCards= table;
        }

        public void ClearTable(Table table)
        {

        }

        public void ClearTableToPlayer(Player player, Table table)
        {
            
        }

        public string DisplayTable(int tableSize, Card cardOnTop)
        {
            string visibleDeckSize;
            if(tableSize >= 10) visibleDeckSize = $"\nTABLE:        \n┌──┐\n│{tableSize}│\n└──┘     Card on top: {cardOnTop.Value}";  
            else visibleDeckSize = $"\nTABLE:   \n┌──┐\n│{tableSize} │\n└──┘     Card on top: {cardOnTop.Value}";
            return visibleDeckSize;
        }

    }
}
