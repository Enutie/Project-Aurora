﻿using Aurora.Exceptions;
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
        public Phase CurrentPhase { get; private set; }

        private Player _attackingPlayer;
        private Player _defendingPlayer;
        private List<Creature> _attackingCreatures;
        private Dictionary<Creature, Creature> _blockingCreatures;

        public Game(List<Player> players)
        {
            Players = players;

            foreach (Player player in Players)
            {
                player.Deck.Shuffle();
                DrawStartingHand(player);
            }
        }

        public void StartBeginningPhase()
        {
            CurrentPhase = Phase.Beginning;
            _landsPlayedThisTurn[GetCurrentPlayer()] = 0;
            _hasAttackedThisTurn = false;
            UntapPermanents(GetCurrentPlayer());
            DrawCard(GetCurrentPlayer());
        }

        public void StartMainPhase1()
        {
            CurrentPhase = Phase.MainPhase1;
        }

        public void StartCombatPhase()
        {
            CurrentPhase = Phase.Combat;
        }

        public void StartMainPhase2()
        {
            CurrentPhase = Phase.MainPhase2;
        }
        public void StartEndPhase()
        {
            CurrentPhase = Phase.Ending;
        }

        public void AdvanceToNextPhase()
        {
            switch (CurrentPhase)
            {
                case Phase.Beginning:
                    StartMainPhase1();
                    break;
                case Phase.MainPhase1:
                    StartCombatPhase();
                    break;
                case Phase.Combat:
                    StartMainPhase2();
                    break;
                case Phase.MainPhase2:
                    StartEndPhase();
                    break;
                case Phase.Ending:
                    SwitchTurn();
                    break;
                default:
                    throw new InvalidOperationException("Unknown phase.");
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

        public bool SwitchTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
            var currentPlayer = GetCurrentPlayer();
            StartBeginningPhase();

            CheckWinConditions();
            if (currentPlayer == Players[1])
            {
                return TakeAITurn();
            }
            return false;
        }

        private static void UntapPermanents(Player currentPlayer)
        {
            foreach (var land in currentPlayer.Battlefield.OfType<Land>())
            {
                land.IsTapped = false;
            }

            foreach (var creature in currentPlayer.Battlefield.OfType<Creature>())
            {
                creature.IsTapped = false; // Untap all creatures for the current player
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
            if (CurrentPhase == Phase.MainPhase1 || CurrentPhase == Phase.MainPhase2)
            {

                if (GetCurrentPlayer() == player && CanPlayLand(player))
                {
                    player.PlayLand(land);
                    _landsPlayedThisTurn[player]++;
                }
                else
                {
                    throw new AlreadyPlayedALandExpection("Player cannot play a land at this time.");
                }
            }
            else
            {
                throw new InvalidPhaseException("Can only play a land in first or second main phase");
            }
        }

        public void CastCreature(Player player, Creature creature)
        {
            if (CurrentPhase == Phase.MainPhase1 || CurrentPhase == Phase.MainPhase2)
            {
                var creatureToRemove = player.Hand.FirstOrDefault(c => c.Id == creature.Id);
                if (creatureToRemove == null)
                {
                    throw new InvalidMoveException("The creature is not in the player's hand.");
                }

                if (player.ManaPool.CanAfford(creature.ManaCost))
                {
                    player.ManaPool.Spend(creature.ManaCost);

                    player.Hand.Remove(creatureToRemove);
                    player.Battlefield.Add(creature);
                }
                else
                {
                    throw new InvalidMoveException("Player does not have enough mana to cast the creature.");
                }
            }
            else
            {
                throw new InvalidPhaseException("Can only play a creature on first or second main phase");
            }
        }

        public bool TakeAITurn()
        {
            Player aiPlayer = Players[1];
            if (GetCurrentPlayer() == aiPlayer)
            {
                AdvanceToNextPhase();
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
                AdvanceToNextPhase();
                if (AIAttackAction())
                {
                    return true; // Return true to indicate that the AI has declared attackers

                }
                else
                {
                    return PostCombatAITurn();
                }
            }
            else
            {
                throw new InvalidOperationException($"It's not the {aiPlayer.Name}'s Turn");
            }
        }

        public bool PostCombatAITurn()
        {
            AdvanceToNextPhase();
            //Do secondmain stuff
            AdvanceToNextPhase();

            SwitchTurn();
            return false;
        }

        public void DeclareAttackers(Player attackingPlayer, List<Creature> attackingCreatures)
        {
            if (CurrentPhase == Phase.Combat)
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
                if (GetCurrentPlayer() == Players[0])
                {
                    AIDefendingAction();
                }
            }
            else
            {
                throw new InvalidPhaseException("Can only declare attackers in the combat phase.");
            }
        }

        private bool AIAttackAction()
        {
            Player aiPlayer = Players[1];
            List<Creature> attackingCreatures = aiPlayer.Battlefield.OfType<Creature>().Where(c => !c.IsTapped).ToList();

            if (attackingCreatures.Any())
            {
                DeclareAttackers(aiPlayer, attackingCreatures);
                return true; // Return true to indicate that the AI has declared attackers
            }

            return false;
        }

        private void AIDefendingAction()
        {
            Player aiPlayer = Players[1];
            Dictionary<Creature, Creature> blockingCreatures = [];

            foreach (var attackingCreature in _attackingCreatures)
            {
                var availableBlockers = aiPlayer.Battlefield.OfType<Creature>()
                    .Where(c => !c.IsTapped && !blockingCreatures.ContainsValue(c))
                    .ToList();

                if (availableBlockers.Any())
                {
                    var blocker = availableBlockers.First();
                    blockingCreatures[attackingCreature] = blocker;
                }
            }

            AssignBlockers(aiPlayer, blockingCreatures);
        }

        public void AssignBlockers(Player defendingPlayer, Dictionary<Creature, Creature> blockingCreatures)
        {
            if (CurrentPhase == Phase.Combat)
            {

                _defendingPlayer = defendingPlayer;
                _blockingCreatures = blockingCreatures;

                ResolveCombat();
            }
            else
            {
                throw new InvalidPhaseException("Can only assign blockers in the combat phase.");
            }
        }

        private void ResolveCombat()
        {
            if (CurrentPhase == Phase.Combat)
            {

                foreach (var attackingCreature in _attackingCreatures)
                {
                    if (_blockingCreatures.TryGetValue(attackingCreature, out var blockingCreature))
                    {
                        attackingCreature.DealDamage(blockingCreature);
                        blockingCreature.DealDamage(attackingCreature);
                        if (attackingCreature.Toughness <= 0)
                        {
                            _attackingPlayer.Battlefield.Remove(attackingCreature);
                            _attackingPlayer.Graveyard.Add(attackingCreature);
                        }

                        if (blockingCreature.Toughness <= 0)
                        {
                            _defendingPlayer.Battlefield.Remove(blockingCreature);
                            _defendingPlayer.Graveyard.Add(blockingCreature);
                        }
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
            else
            {
                throw new InvalidPhaseException("Can only resolve combat in the combat phase");
            }
        }
    }
}
