using Aurora.Application.DTO;
using Aurora.Application.Interfaces;
using Aurora.Infrastructure.Interfaces;
using Aurora.Shared.Exceptions;
using Aurora.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Domain.Entities;
using Aurora.Domain.Enums;

namespace Aurora.Application.Services
{

    public class PlayerActionService : IPlayerActionService
    {
        private readonly ILogger<PlayerActionService> _logger;
        private readonly IGameStorage _games;
        private readonly IGameManager _gameManager;
        private readonly ICardConverter _cardConverter;

        public PlayerActionService(ILogger<PlayerActionService> logger, IGameStorage games, IGameManager gameManager, ICardConverter cardConverter)
        {
            _logger = logger;
            _games = games;
            _gameManager = gameManager;
            _cardConverter = cardConverter;
        }

        public GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments)
        {
            try
            {
                if (!_games.TryGetGame(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                game.AssignBlockers(defendingPlayerId, blockerAssignments);

                return game.GetGameState();
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

                game.DeclareAttackers(attackingPlayerId, attackingCreatureIds);

                return game.GetGameState();
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
            catch (InvalidPhaseException ex)
            {
                _logger.LogError(ex, "Can only attack in the combat phase");
                throw;
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

                game.CastCreature(playerId, creatureDTO);

                return game.GetGameState();
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
            catch (InvalidPhaseException ex)
            {
                _logger.LogError(ex, "Can only play creatures in first or second mainphase");
                throw;
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

                game.PlayLand(playerId, landDTO);

                return game.GetGameState();
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
            catch (InvalidPhaseException ex)
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
