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
        public int currentPlayerIndex { get; set; } = 0;
        public string Id { get; set; } = Guid.NewGuid().ToString();
        private Dictionary<Player, int> _landsPlayedThisTurn = new Dictionary<Player, int>();
        private bool _hasAttackedThisTurn = false;
        public bool IsGameOver { get; private set; }
        public Player Winner { get; private set; }

        private Player _attackingPlayer;
        private Player _defendingPlayer;
        private List<Creature> _attackingCreatures;
        private Dictionary<Creature, Creature> _blockingCreatures;

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

        public Game(List<Player> players)
        {
            Players = players;

            foreach( Player player in Players)
            {
                player.Deck.Shuffle();
                DrawStartingHand(player);
            }


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
            var currentPlayer = GetCurrentPlayer();
            _landsPlayedThisTurn[GetCurrentPlayer()] = 0;
            _hasAttackedThisTurn = false;

            foreach (var land in currentPlayer.Battlefield.OfType<Land>())
            {
                land.IsTapped = false;
            }

            DrawCard(GetCurrentPlayer());
            CheckWinConditions();
            if(currentPlayer == Players[1])
            {
                TakeAITurn();
            }
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
            var availableLands = player.Battlefield.OfType<Land>().Where(l => !l.IsTapped).ToList();
            var availableMana = availableLands.Select(l => l.ProducedMana).ToList();

            if (CanAfford(availableMana, creature.ManaCost))
            {
                PayMana(availableLands, creature.ManaCost);
                player.Hand.Remove(creature);
                player.Battlefield.Add(creature);
            }
            else
            {
                throw new InvalidOperationException("Player does not have enough mana to cast the creature.");
            }
        }

        private void PayMana(List<Land> availableLands, IEnumerable<Mana> cost)
        {
            var remainingCost = cost.GroupBy(m => m).ToDictionary(g => g.Key, g => g.Count());

            // Handle specific mana costs
            foreach (var mana in cost.Where(m => m != Mana.Colorless))
            {
                var land = availableLands.FirstOrDefault(l => l.ProducedMana == mana);
                if (land != null)
                {
                    land.IsTapped = true;
                    availableLands.Remove(land);
                    remainingCost[mana]--;
                    if (remainingCost[mana] == 0)
                    {
                        remainingCost.Remove(mana);
                    }
                }
            }

            // Handle colorless mana costs
            if (remainingCost.ContainsKey(Mana.Colorless))
            {
                int colorlessCost = remainingCost[Mana.Colorless];
                remainingCost.Remove(Mana.Colorless);

                foreach (var land in availableLands.ToList())
                {
                    if (colorlessCost == 0)
                        break;

                    land.IsTapped = true;
                    availableLands.Remove(land);
                    colorlessCost--;
                }
            }
        }


        private bool CanAfford(List<Mana> availableMana, IEnumerable<Mana> cost)
        {
            var remainingCost = cost.GroupBy(m => m).ToDictionary(g => g.Key, g => g.Count());
            int colorlessCost = remainingCost.ContainsKey(Mana.Colorless) ? remainingCost[Mana.Colorless] : 0;

            if (remainingCost.ContainsKey(Mana.Colorless))
            {
                remainingCost.Remove(Mana.Colorless);
            }

            foreach (var mana in availableMana)
            {
                if (remainingCost.ContainsKey(mana))
                {
                    remainingCost[mana]--;
                    if (remainingCost[mana] == 0)
                    {
                        remainingCost.Remove(mana);
                    }
                }
                else if (colorlessCost > 0)
                {
                    colorlessCost--;
                }
            }

            return !remainingCost.Any() && colorlessCost == 0;
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


        public void DeclareAttackers(Player attackingPlayer, List<Creature> attackingCreatures)
        {
            _attackingPlayer = attackingPlayer;
            _attackingCreatures = attackingCreatures;
        }

        public void AssignBlockers(Player defendingPlayer, Dictionary<Creature, Creature> blockingCreatures)
        {
            _defendingPlayer = defendingPlayer;
            _blockingCreatures = blockingCreatures;

            ResolveCombat();
        }

        private void ResolveCombat()
        {
            foreach (var attackingCreature in _attackingCreatures)
            {
                if (_blockingCreatures.TryGetValue(attackingCreature, out var blockingCreature))
                {
                    attackingCreature.DealDamage(blockingCreature);
                    blockingCreature.DealDamage(attackingCreature);
                }
                else
                {
                    _defendingPlayer.TakeDamage(attackingCreature.Power);
                }
            }

            _attackingCreatures.Clear();
            _blockingCreatures.Clear();

            CheckWinConditions();
        }

        public void Attack(Player attacker, Player defender, List<Creature> attackingCreatures)
        {
            if (attacker != GetCurrentPlayer())
            {
                throw new InvalidOperationException("Only the current player can attack.");
            }

            if (_hasAttackedThisTurn)
            {
                throw new InvalidOperationException("Player has already attacked this turn.");
            }

            foreach (var creature in attackingCreatures)
            {
                if (!attacker.Battlefield.Contains(creature))
                {
                    throw new InvalidOperationException("Cannot attack with a creature that is not on the attacker's battlefield.");
                }
            }

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

            _hasAttackedThisTurn = true;
        }
    }
}
