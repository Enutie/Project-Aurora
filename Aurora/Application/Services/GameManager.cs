using Aurora.Application.DTO;
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
    public class GameManager : IGameManager
    {
        private readonly ILogger<GameManager> _logger;
        private readonly IGameStorage _games;
        private readonly ICardConverter _cardConverter;

        public GameManager(ILogger<GameManager> logger, IGameStorage gameStorage, ICardConverter cardConverter)
        {
            _logger = logger;
            _games = gameStorage;
            _cardConverter = cardConverter;
        }

        public GameDTO AdvanceToNextPhase(string gameId)
        {
            try
            {
                if (!_games.TryGetGame(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }
                game.AdvanceToNextPhase();
                return CreateGameDTOFromGame(game);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while advancing to the next phase.");
                throw new InvalidOperationException("An error occurred while advancing to the next phase.", ex);
            }
        }

        public GameDTO CreateGame(string playerName)
        {
            var game = new Game(new List<Player>() {
        new Player(playerName),
        new Player("AI")
    }, _cardConverter);
            _games.AddGame(game);

            return CreateGameDTOFromGame(game);
        }

        public GameDTO GetGameState(string gameId)
        {
            try
            {
                if (!_games.TryGetGame(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }
                return CreateGameDTOFromGame(game);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the game state.");
                throw new InvalidOperationException("An error occurred while retrieving the game state.", ex);
            }
        }

        public GameDTO StartGame(List<PlayerDTO> playerDTOs)
        {
            try
            {
                var players = playerDTOs.Select(p => new Player
                {
                    Id = p.Id,
                    Name = p.Name,
                    Life = p.Life
                }).ToList();

                var game = new Game(players, _cardConverter);
                _games.AddGame(game);

                return CreateGameDTOFromGame(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start the game.");
                throw new InvalidOperationException("Failed to start the game.", ex);
            }
        }

        public GameDTO SwitchTurn(string gameId)
        {
            try
            {
                if (!_games.TryGetGame(gameId, out var game))
                {
                    throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
                }

                game.SwitchTurn();
                return GetGameState(gameId);
            }
            catch (GameNotFoundException ex)
            {
                _logger.LogError(ex, "Game not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while switching turns.");
                throw new InvalidOperationException("An error occurred while switching turns.", ex);
            }
        }

        private GameDTO CreateGameDTOFromGame(Game game)
        {
            var gameState = game.GetGameState();
            return new GameDTO
            {
                Id = gameState.Id,
                Players = gameState.Players.Select(p => new PlayerDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Hand = p.Hand.Cast<CardDTO>().ToList(),
                    Battlefield = p.Battlefield.Cast<CardDTO>().ToList(),
                    Life = p.Life
                }).ToList(),
                CurrentPlayerIndex = gameState.CurrentPlayerIndex,
                IsGameOver = gameState.IsGameOver,
                Winner = gameState.Winner,
                CurrentPhase = gameState.CurrentPhase.ToString(),
            };
        }
    }
}
