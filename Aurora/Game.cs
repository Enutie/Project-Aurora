using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Game
    {
        public List<Player> Players { get; } = new List<Player>();
        private int currentPlayerIndex = 0;
        public string Id { get; set; }
        private Dictionary<Player, int> _landsPlayedThisTurn = new Dictionary<Player, int>();
        public bool IsGameOver { get; private set; }
        public Player Winner { get; private set; }

        public Game(Deck deck)
        {
            Players = new List<Player>();

            var deck1 = deck;
            var deck2 = new Deck(deck.Cards);

            deck1.Shuffle();
            var player1 = new Player { Deck = deck1 };
            DrawStartingHand(player1);
            Players.Add(player1);

            deck2.Shuffle();
            var player2 = new Player { Deck = deck2 };
            DrawStartingHand(player2);
            Players.Add(player2);
        }
        public void CheckWinConditions()
        {
            foreach (var player in Players)
            {
                if (player.Life <= 0 || player.Deck.Cards.Count == 0)
                {
                    IsGameOver = true;
                    Winner = Players.FirstOrDefault(p => p != player);
                    break;
                }
            }
        }

        private void DrawStartingHand(Player player)
        {
            for (int i = 0; i < 7; i++)
            {
                player.DrawCard();
            }
        }

        public Player GetCurrentPlayer()
        {
            return Players[currentPlayerIndex];
        }

        public void DrawCard(Player player)
        {
            try
            {
                player.DrawCard();
            }
            catch (InvalidOperationException)
            {
                IsGameOver = true;
                Winner = Players.FirstOrDefault(p => p != player);
            }
        }

        public void SwitchTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
            _landsPlayedThisTurn[GetCurrentPlayer()] = 0;
            DrawCard(GetCurrentPlayer());
            CheckWinConditions();
        }

        public bool CanPlayLand(Player player)
        {
            if (!_landsPlayedThisTurn.ContainsKey(player))
            {
                _landsPlayedThisTurn[player] = 0;
            }
            return _landsPlayedThisTurn[player] == 0;
        }

        public void PlayLand(Player player, Land land)
        {
            if (GetCurrentPlayer() == player && CanPlayLand(player))
            {
                player.PlayLand(land);
                _landsPlayedThisTurn[player]++;
            }
            else
            {
                throw new InvalidOperationException("Player cannot play a land at this time.");
            }
        }

        public void CastCreature(Player player, Creature creature)
        {
            if (player.ManaPool.CanAfford(creature.ManaCost))
            {
                player.ManaPool.Spend(creature.ManaCost);
                player.Hand.Remove(creature);
                player.Battlefield.Add(creature);
            }
            else
            {
                throw new InvalidOperationException("Player does not have enough mana to cast the creature.");
            }
        }

        public void TakeAITurn()
        {
            Player aiPlayer = Players[1];
            if (GetCurrentPlayer() == aiPlayer)
            {
                if (aiPlayer.Hand.OfType<Land>().Any())
                {
                    Land landToPlay = aiPlayer.Hand.OfType<Land>().FirstOrDefault();

                    if (landToPlay != null && CanPlayLand(aiPlayer))
                    {
                        PlayLand(aiPlayer, landToPlay);
                    }
                }

                SwitchTurn();
            }
            else
            {
                throw new InvalidOperationException($"Its not the {aiPlayer.Name}'s Turn");
            }
        }

        public void Attack(Player attacker, Player defender, List<Creature> attackingCreatures)
        {
            // Assign the attacking creatures
            foreach (var creature in attackingCreatures)
            {
                creature.IsAttacking = true;
            }

            // Prompt the defender to assign blockers (this will be handled by the AI or the defending player)
            var blockingCreatures = defender.AssignBlockers(attackingCreatures);

            // Resolve combat damage
            foreach (var creature in attackingCreatures)
            {
                if (creature.IsBlocked)
                {
                    var blocker = creature.BlockedBy;
                    creature.DealDamage(blocker);
                    blocker.DealDamage(creature);
                }
                else
                {
                    defender.TakeDamage(creature.Power);
                }
            }

            // Clear the attacking/blocking status
            foreach (var creature in attackingCreatures)
            {
                creature.IsAttacking = false;
                creature.IsBlocked = false;
                creature.BlockedBy = null;
            }
        }
    }
}
