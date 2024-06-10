namespace Borealis.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public List<Card> Library { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Battlefield { get; set; }

        public Player(string _name)
        {
            Name = _name;
            Life = 20;
            Library = new List<Card>();
            Hand = new List<Card>();
            Battlefield = new List<Card>();
        }

        public void DrawOpeningHand()
        {
            for (int i = 0; i < 7; i++)
            {
                DrawCard();
            }
        }

        public void DrawCard()
        {
            if (Library.Count == 0)
            {
                // Can't draw from an empty library
                return;
            }

            var card = Library[0];
            Library.RemoveAt(0);
            Hand.Add(card);
        }
    }
}