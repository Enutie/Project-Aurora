using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis
{
    public class Player
    {
        public int Life { get; private set; }
        public IEnumerable<Card> Library { get; private set; }
        public IList<Card> Hand { get; }
        public IList<Card> Battlefield { get; }

        public Player(int initialLife, IEnumerable<Card> library)
        {
            Life = initialLife;
            Library = library;
            Hand = new List<Card>();
            Battlefield = new List<Card>();
        }
        public void DrawCard()
        {
            if (Library.Any())
            {
                Card drawnCard = Library.First();
                Hand.Add(drawnCard);
                Library = Library.Skip(1);
            }
        }

        public void PlayCard(Card card)
        {
            if (Hand.Contains(card))
            {
                Hand.Remove(card);
                Battlefield.Add(card);
            }
        }
    }
}
