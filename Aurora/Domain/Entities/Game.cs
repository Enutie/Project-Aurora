// Domain/Entities/Game.
using Aurora.Application.DTO;
using Aurora.Domain.Enums;
using Aurora.Domain.Services;
using Aurora.Shared.Exceptions;
using Aurora.Shared.Utils;

namespace Aurora.Domain.Entities
{
    public class Game
    {
        private readonly GameState _gameState;
        private readonly TurnManager _turnManager;
        private readonly CombatManager _combatManager;
        private readonly ICardConverter _cardConverter;

        public Game(List<Player> players, ICardConverter cardConverter)
        {
            _gameState = new GameState(players);
            _cardConverter = cardConverter;
            _turnManager = new TurnManager(_gameState, _cardConverter);
            _combatManager = new CombatManager(_gameState);
            StartGame();
        }

        public void StartGame()
        {
            _turnManager.StartGame();
        }

        // In Game.cs
        public void AdvanceToNextPhase()
        {
            _turnManager.AdvanceToNextPhase();
            CheckWinConditions();
            Console.WriteLine($"Advanced to phase: {_gameState.CurrentPhase}");
        }

        public void PlayLand(string playerId, LandDTO landDTO)
        {
            _turnManager.PlayLand(playerId, landDTO);
        }

        public void CastCreature(string playerId, CreatureDTO creatureDTO)
        {
            if (_gameState.CurrentPhase != Phase.MainPhase1 && _gameState.CurrentPhase != Phase.MainPhase2)
            {
                throw new InvalidPhaseException("Can only play a creature in first or second main phase");
            }

            var player = _gameState.GetPlayerById(playerId);
            var creature = _cardConverter.ConvertToCreature(creatureDTO);

            if (!player.Hand.Any(c => c.Id == creature.Id))
            {
                throw new InvalidMoveException("The creature is not in the player's hand.");
            }

            if (player.ManaPool.CanAfford(creature.ManaCost))
            {
                player.ManaPool.Spend(creature.ManaCost);
                player.Hand.RemoveAll(c => c.Id == creature.Id);
                player.Battlefield.Add(creature);
            }
            else
            {
                throw new InvalidMoveException("Player does not have enough mana to cast the creature.");
            }
        }

        public void DeclareAttackers(string attackingPlayerId, List<string> attackingCreatureIds)
        {
            var attackingPlayer = _gameState.GetPlayerById(attackingPlayerId);
            var attackingCreatures = attackingPlayer.Battlefield.OfType<Creature>()
                .Where(c => attackingCreatureIds.Contains(c.Id)).ToList();

            _combatManager.DeclareAttackers(attackingPlayer, attackingCreatures);
        }

        public void AssignBlockers(string defendingPlayerId, Dictionary<string, string> blockerAssignments)
        {
            var defendingPlayer = _gameState.GetPlayerById(defendingPlayerId);
            var blockingCreatures = new Dictionary<Creature, Creature>();

            foreach (var assignment in blockerAssignments)
            {
                var attackingCreature = _gameState.Players.SelectMany(p => p.Battlefield).OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Key);
                var blockingCreature = defendingPlayer.Battlefield.OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Value);

                if (attackingCreature != null && blockingCreature != null)
                {
                    blockingCreatures[attackingCreature] = blockingCreature;
                }
            }

            _combatManager.AssignBlockers(defendingPlayer, blockingCreatures);
        }
        public GameDTO GetGameState()
        {
            return new GameDTO
            {
                Id = _gameState.Id,
                Players = _gameState.Players.Select(_cardConverter.ConvertToPlayerDTO).ToList(),
                CurrentPlayerIndex = _gameState.CurrentPlayerIndex,
                IsGameOver = _gameState.IsGameOver,
                Winner = _gameState.Winner,
                CurrentPhase = _gameState.CurrentPhase.ToString(),
            };
        }

        public GameDTO SwitchTurn()
        {
            _turnManager.SwitchTurn();
            CheckWinConditions();
            return GetGameState();
        }

        private void CheckWinConditions()
        {
            foreach (var player in _gameState.Players)
            {
                if (player.Life <= 0 || player.Deck.Cards.Count == 0)
                {
                    _gameState.IsGameOver = true;
                    _gameState.Winner = _cardConverter.ConvertToPlayerDTO(_gameState.Players.FirstOrDefault(p => p != player));
                    break;
                }
            }
        }
    }
}