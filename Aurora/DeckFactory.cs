using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public static class DeckFactory
    {
        public static Deck CreateDeck(LandType landType)
        {
            List<Card> cards = new List<Card>();
            for(int i = 0; i < 60; i++)
            {
                cards.Add(new Land(landType));
            }

            return new Deck(cards);
        }
    }
}
