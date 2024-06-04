using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Player
    {
        public List<Card> Library { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Battlefield { get; set; }
        public string Name { get; set; }
        public Player(string name, List<Card> library)
        {
            Name = name;
            Library = library;
            Hand = new List<Card>();
            Battlefield = new List<Card>();
        }

        public void DrawStartingHand()
        {
            for (int i = 0; i < 7; i++)
            {
                DrawCard();
            }
        }

        public void DrawCard()
        {
            if (Library.Count > 0)
            {
                Hand.Add(Library.First());
                Library.RemoveAt(0);
            }
        }

        public void Play(Card card)
        {
            if (Hand.Contains(card))
            {
                Hand.Remove(card);
                Battlefield.Add(card);
            }
        }

        public virtual void PassTurn() { }
    }
}
