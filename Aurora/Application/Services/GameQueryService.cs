using Aurora.Application.DTO;
using Aurora.Application.Interfaces;
using Aurora.Domain.Entities;
using Aurora.Infrastructure.Interfaces;
using Aurora.Shared.Exceptions;
using Aurora.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Application.Services
{
    public class GameQueryService : IGameQueryService
    {
        private readonly ILogger<GameQueryService> _logger;
        private readonly IGameStorage _games;
        private readonly ICardConverter _cardConverter;

        public GameQueryService(ILogger<GameQueryService> _logger, IGameStorage _games, ICardConverter cardConverter)
        {
            this._logger = _logger;
            this._games = _games;
            _cardConverter = cardConverter;
        }

        public CreatureDTO GetCreatureById(string creatureId)
        {
            _logger.LogInformation($"Searching for creature with ID: {creatureId}");
            var games = _games.GetAllGames();
            _logger.LogInformation($"Number of games: {games.Count()}");

            foreach (var game in games)
            {
                var gameState = game.GetGameState();
                _logger.LogInformation($"Checking game: {gameState.Id}");

                if (gameState.Players == null)
                {
                    _logger.LogWarning($"Players list is null for game {gameState.Id}");
                    continue;
                }

                foreach (var player in gameState.Players)
                {
                    if (player == null)
                    {
                        _logger.LogWarning($"Encountered a null player in game {gameState.Id}");
                        continue;
                    }

                    _logger.LogInformation($"Checking player: {player.Id}");
                    _logger.LogInformation($"Cards in hand: {player.Hand?.Count ?? 0}, Cards on battlefield: {player.Battlefield?.Count ?? 0}");

                    // Check in hand
                    var creatureInHand = player.Hand?.OfType<CreatureDTO>().FirstOrDefault(c => c.Id == creatureId);
                    if (creatureInHand != null)
                    {
                        _logger.LogInformation($"Found creature in player's hand: {creatureInHand.Id}");
                        return creatureInHand;
                    }

                    // Check on battlefield
                    var creatureOnBattlefield = player.Battlefield?.OfType<CreatureDTO>().FirstOrDefault(c => c.Id == creatureId);
                    if (creatureOnBattlefield != null)
                    {
                        _logger.LogInformation($"Found creature on player's battlefield: {creatureOnBattlefield.Id}");
                        return creatureOnBattlefield;
                    }

                    // Optionally, check in graveyard if you're tracking that
                    if (player.Graveyard != null)
                    {
                        var creatureInGraveyard = player.Graveyard.OfType<CreatureDTO>().FirstOrDefault(c => c.Id == creatureId);
                        if (creatureInGraveyard != null)
                        {
                            _logger.LogInformation($"Found creature in player's graveyard: {creatureInGraveyard.Id}");
                            return creatureInGraveyard;
                        }
                    }
                }
            }

            _logger.LogWarning($"Creature with ID {creatureId} not found in any game or player");
            return null;
        }

        public LandDTO GetLandById(string landId)
        {
            _logger.LogInformation($"Searching for land with ID: {landId}");
            var games = _games.GetAllGames();
            _logger.LogInformation($"Number of games: {games.Count()}");

            foreach (var game in games)
            {
                var gameState = game.GetGameState();
                _logger.LogInformation($"Checking game: {gameState.Id}");

                if (gameState.Players == null)
                {
                    _logger.LogWarning($"Players list is null for game {gameState.Id}");
                    continue;
                }

                foreach (var player in gameState.Players)
                {
                    if (player == null)
                    {
                        _logger.LogWarning($"Encountered a null player in game {gameState.Id}");
                        continue;
                    }

                    _logger.LogInformation($"Checking player: {player.Id}");
                    _logger.LogInformation($"Cards in hand: {player.Hand?.Count ?? 0}, Cards on battlefield: {player.Battlefield?.Count ?? 0}");

                    // Check in hand
                    var landInHand = player.Hand?.OfType<LandDTO>().FirstOrDefault(l => l.Id == landId);
                    if (landInHand != null)
                    {
                        _logger.LogInformation($"Found land in player's hand: {landInHand.Id}");
                        return landInHand;
                    }

                    // Check on battlefield
                    var landOnBattlefield = player.Battlefield?.OfType<LandDTO>().FirstOrDefault(l => l.Id == landId);
                    if (landOnBattlefield != null)
                    {
                        _logger.LogInformation($"Found land on player's battlefield: {landOnBattlefield.Id}");
                        return landOnBattlefield;
                    }
                }
            }

            _logger.LogWarning($"Land with ID {landId} not found in any game or player");
            return null;
        }

        public PlayerDTO GetPlayerInfo(string gameId, string playerId)
        {
            try
            {
                if (!_games.TryGetGame(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                var player = game.GetGameState().Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    throw new PlayerNotFoundException($"Player with ID '{playerId}' not found in the game.");
                }

                return new PlayerDTO
                {
                    Id = player.Id,
                    Name = player.Name,
                    Hand = player.Hand,
                    Battlefield = player.Battlefield,
                    Life = player.Life
                };
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving player information.");
                throw new InvalidOperationException("An error occurred while retrieving player information.", ex);
            }
        }
    }


}
