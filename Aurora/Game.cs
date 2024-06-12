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
        public bool _hasAttackedThisTurn = false;
        public bool IsGameOver { get; private set; }
        public Player Winner { get; private set; }

        private Player _attackingPlayer;
        private Player _defendingPlayer;
        private List<Creature> _attackingCreatures;
        private Dictionary<Creature, Creature> _blockingCreatures;

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

            foreach (var creature in currentPlayer.Battlefield.OfType<Creature>())
            {
                creature.IsTapped = false; // Untap all creatures for the current player
            }

            DrawCard(GetCurrentPlayer());
            CheckWinConditions();
            if (currentPlayer == Players[1])
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
            var creatureToRemove = player.Hand.FirstOrDefault(c => c.Id == creature.Id);
            if (creatureToRemove == null)
            {
                throw new InvalidOperationException("The creature is not in the player's hand.");
            }

            if (player.ManaPool.CanAfford(creature.ManaCost))
            {
                player.ManaPool.Spend(creature.ManaCost);

                player.Hand.Remove(creatureToRemove);
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
				// Play a land if possible
				if (aiPlayer.Hand.OfType<Land>().Any())
				{
					Land landToPlay = aiPlayer.Hand.OfType<Land>().FirstOrDefault();

					if (landToPlay != null && CanPlayLand(aiPlayer))
					{
						PlayLand(aiPlayer, landToPlay);
					}
				}

				// Play creatures if possible
				var availableMana = aiPlayer.ManaPool.AvailableMana();
				var creaturesInHand = aiPlayer.Hand.OfType<Creature>().ToList();

				foreach (var creature in creaturesInHand)
				{
					if (aiPlayer.ManaPool.CanAfford(creature.ManaCost))
					{
						CastCreature(aiPlayer, creature);
						break; // Play only one creature per turn for now
					}
				}

				SwitchTurn();
			}
			else
			{
				throw new InvalidOperationException($"It's not the {aiPlayer.Name}'s Turn");
			}
		}


		public void DeclareAttackers(Player attackingPlayer, List<Creature> attackingCreatures)
        {
            foreach (var creature in attackingCreatures)
            {
                if (!creature.IsTapped) // Check if the creature is not already tapped
                {
                    creature.IsAttacking = true;
                    creature.IsTapped = true; // Tap the creature when it attacks
                }
                else
                {
                    throw new InvalidOperationException("Cannot declare a tapped creature as an attacker.");
                }
            }
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
                attackingCreature.IsAttacking = false;
            }

            _attackingCreatures.Clear();
            _blockingCreatures.Clear();
            _hasAttackedThisTurn = true;

            CheckWinConditions();
        }
    }
}
