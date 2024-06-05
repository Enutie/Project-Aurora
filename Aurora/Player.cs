using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Player
    {
        public List<Card> Hand { get; } = new List<Card>();
        public List<Card> Battlefield { get; } = new List<Card>();
        public Deck Deck { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public void DrawCard()
        {
            if (Deck.Cards.Count > 0)
            {
                Card drawnCard = Deck.Cards.First();
                Deck.Cards.RemoveAt(0);
                Hand.Add(drawnCard);
            }
        }
    }
}
