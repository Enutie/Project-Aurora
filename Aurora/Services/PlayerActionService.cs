using Aurora.DTO;
using Aurora.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
	public class PlayerActionService : IPlayerActionService
	{
		private readonly ILogger<PlayerActionService> _logger;
        private readonly IGameStorage _games;
        private readonly IGameManager _gameManager;

        public PlayerActionService(ILogger<PlayerActionService> _logger, IGameStorage _games, IGameManager _gameManager)
		{
			this._logger = _logger;
            this._games = _games;
            this._gameManager = _gameManager;
        }
		public GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments)
		{
			try
			{
				if (!_games.TryGetGame(gameId, out var game))
				{
					throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
				}

				var defendingPlayer = game.Players.FirstOrDefault(p => p.Id == defendingPlayerId);
				if (defendingPlayer == null)
				{
					throw new PlayerNotFoundException($"Defending player with ID '{defendingPlayerId}' not found in the game.");
				}

				var blockingCreatures = new Dictionary<Creature, Creature>();
				foreach (var assignment in blockerAssignments)
				{
					var attackingCreature = game.Players.SelectMany(p => p.Battlefield).OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Key);
					var blockingCreature = defendingPlayer.Battlefield.OfType<Creature>().FirstOrDefault(c => c.Id == assignment.Value);

					if (attackingCreature != null && blockingCreature != null)
					{
						blockingCreatures[attackingCreature] = blockingCreature;
					}
				}

				game.AssignBlockers(defendingPlayer, blockingCreatures);

				return _gameManager.GetGameState(gameId);
			}
			catch (GameNotFoundException ex)
			{
				_logger.LogError(ex, "Game not found");
				throw;
			}
			catch (PlayerNotFoundException ex)
			{
				_logger.LogError(ex, "Player not found");
				throw;
			}
		}

		public GameDTO Attack(string gameId, string attackingPlayerId, List<string> attackingCreatureIds)
		{
			try
			{
				if (!_games.TryGetGame(gameId, out var game))
				{
					throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
				}

				var attackingPlayer = game.Players.FirstOrDefault(p => p.Id == attackingPlayerId);
				if (attackingPlayer == null)
				{
					throw new PlayerNotFoundException($"Attacking player with ID '{attackingPlayerId}' not found in the game.");
				}

				if (game._hasAttackedThisTurn)
				{
					throw new InvalidMoveException($"{game.GetCurrentPlayer().Name} has already attacked this turn");
				}

				var attackingCreatures = attackingPlayer.Battlefield.OfType<Creature>()
					.Where(c => attackingCreatureIds.Contains(c.Id)).ToList();

				game.DeclareAttackers(attackingPlayer, attackingCreatures);

				return _gameManager.GetGameState(gameId);
			}
			catch (GameNotFoundException ex)
			{
				_logger.LogError(ex, "Game not Found");
				throw;
			}
			catch (PlayerNotFoundException ex)
			{
				_logger.LogError(ex, "Player not Found");
				throw;
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogError(ex, "Invalid move. " + ex.Message);
				throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while attacking.");
				throw new InvalidOperationException("An error occurred while attacking.", ex);
			}
		}

		public GameDTO CastCreature(string gameId, string playerId, CreatureDTO creatureDTO)
		{
			try
			{
				if (!_games.TryGetGame(gameId, out var game))
				{
					throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
				}

				var player = game.Players.FirstOrDefault(p => p.Id == playerId);
				if (player == null)
				{
					throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
				}

				var creature = new Creature(creatureDTO.Name,
					creatureDTO.ManaCost.Select(mana => (Mana)Enum.Parse(typeof(Mana), mana)).ToList(),
					creatureDTO.Power, creatureDTO.Toughness)
				{
					Id = creatureDTO.Id,
					IsAttacking = creatureDTO.IsAttacking,
					IsBlocked = creatureDTO.IsBlocked,
					BlockedBy = creatureDTO.BlockedBy != null ? new Creature(creatureDTO.BlockedBy.Name,
					creatureDTO.BlockedBy.ManaCost.Select(mana => (Mana)Enum.Parse(typeof(Mana), mana)).ToList(),
					creatureDTO.BlockedBy.Power, creatureDTO.BlockedBy.Toughness)
					{
						Id = creatureDTO.BlockedBy.Id
					} : null
				};

				game.CastCreature(player, creature);
				return _gameManager.GetGameState(gameId);
			}
			catch (GameNotFoundException ex)
			{
				_logger.LogError(ex, "Game not Found");
				throw;
			}
			catch (PlayerNotFoundException ex)
			{
				_logger.LogError(ex, "Player not Found");
				throw;
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogError(ex, "Invalid Move" + ex.Message);
				throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while casting a creature.");
				throw new InvalidOperationException("An error occurred while casting a creature.", ex);
			}
		}

		public GameDTO PlayLand(string gameId, string playerId, LandDTO landDTO)
		{
			try
			{
				if (!_games.TryGetGame(gameId, out var game))
				{
					throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
				}

				var player = game.Players.FirstOrDefault(p => p.Id == playerId);
				if (player == null)
				{
					throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
				}

				var land = new Land((LandType)Enum.Parse(typeof(LandType), landDTO.LandType.ToString()))
				{
					Id = landDTO.Id,
					IsTapped = landDTO.IsTapped
				};

				game.PlayLand(player, land);
				return _gameManager.GetGameState(gameId);
			}
			catch (GameNotFoundException ex)
			{
				_logger.LogError(ex, $"Game with ID '{gameId}' not found.");
				throw;
			}
			catch (PlayerNotFoundException ex)
			{
				_logger.LogError(ex, $"Player with ID '{playerId}' not found in the game.");
				throw;
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogError(ex, "Invalid move. " + ex.Message);
				throw new InvalidMoveException("Invalid move. " + ex.Message, ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while playing a land.");
				throw new InvalidOperationException("An error occurred while playing a land.", ex);
			}
		}
	}
}
