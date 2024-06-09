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
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ManaPool ManaPool { get; } = new ManaPool();
        public int Life { get; set; } = 20;
        public void TakeDamage(int damage)
        {
            Life -= damage;
        }

        public void PlayLand(Land land)
        {
            Hand.Remove(land);
            Battlefield.Add(land);
            ManaPool.Add(land.ProducedMana, land);
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

        public List<Creature> AssignBlockers(List<Creature> attackingCreatures)
        {
            var blockingCreatures = new List<Creature>();

            foreach (var attacker in attackingCreatures)
            {
                var blocker = Battlefield.OfType<Creature>().FirstOrDefault(c => !c.IsAttacking);
                if (blocker != null)
                {
                    attacker.IsBlocked = true;
                    attacker.BlockedBy = blocker;
                    blockingCreatures.Add(blocker);
                }
            }

            return blockingCreatures;
        }
    }
}
