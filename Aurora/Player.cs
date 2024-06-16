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
        public List<Card> Graveyard { get; set; } = new List<Card>();
        public Deck Deck { get; set; }
        public string Name { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ManaPool ManaPool { get; } = new ManaPool();
        public int Life { get; set; } = 20;
        public void TakeDamage(int damage)
        {
            Life -= damage;
        }

        public Player(string name)
        {
            Name = name;
            Deck = DeckFactory.MonoGreenVanilla();
        }

        public Player()
        {
            
        }

        public void PlayLand(Land land)
        {
            Land landToRemove = (Land)Hand.FirstOrDefault(l => l.Id == land.Id);
            if (landToRemove != null)
            {
                Hand.Remove(landToRemove);
                Battlefield.Add(landToRemove);
                ManaPool.Add(landToRemove.ProducedMana, landToRemove);
            }
        }

        public void DrawCard()
        {
            if (Deck.Cards.Count == 0)
            {
                throw new InvalidOperationException("Player has no more cards to draw.");
            }

            Card drawnCard = Deck.Cards.First();
            Deck.Cards.RemoveAt(0);
            Hand.Add(drawnCard);
        }

        public Dictionary<string, string> AssignBlockers(List<string> attackingCreatureIds)
        {
            var blockingAssignments = new Dictionary<string, string>();

            foreach (var attackerId in attackingCreatureIds)
            {
                var blocker = Battlefield.OfType<Creature>().FirstOrDefault(c => !c.IsAttacking && !blockingAssignments.ContainsValue(c.Id));
                if (blocker != null)
                {
                    blockingAssignments[attackerId] = blocker.Id;
                }
            }

            return blockingAssignments;
        }
    }
}
