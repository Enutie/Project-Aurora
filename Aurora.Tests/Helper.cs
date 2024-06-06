using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Tests
{
    public static class Helper
    {
        public static Deck GetDeck
            ()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 24; i++)
            {
                deck.Add(new Land(LandType.Plains));
            }
            for (int i = 0; i < 36; i++)
            {
                deck.Add(new Creature("Devoted Hero", new[] { Mana.White }, 1, 2));
            }
            return new Deck(deck);
        }
    }
}
