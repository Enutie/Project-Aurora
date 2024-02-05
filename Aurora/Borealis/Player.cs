using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis
{
    public class Player
    {
        public List<int> Deck;
        public Player() 
        {
            Deck = new List<int>();
            Deck.Add(1);
        }
    }
}
