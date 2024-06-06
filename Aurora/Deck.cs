using Aurora;

public class Deck
{
    public List<Card> Cards { get; private set; }
    private readonly Random _random = new Random();

    public Deck(IEnumerable<(LandType Type, int Count)> landCounts)
    {
        Cards = new List<Card>();
        foreach (var landCount in landCounts)
        {
            for (int i = 0; i < landCount.Count; i++)
            {
                Cards.Add(new Land(landCount.Type));
            }
        }
    }

    public Deck(IEnumerable<Card> cards)
    {
        Cards = cards.ToList();
    }

    public void Shuffle()
    {
        Cards = Cards.OrderBy(x => _random.Next()).ToList();
    }
}