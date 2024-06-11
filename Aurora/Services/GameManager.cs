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
	public class GameManager : IGameManager
	{
		private readonly ILogger<GameManager> _logger;
		private readonly IGameStorage _games;
        private readonly ICardConverter _cardConverter;

        public GameManager(ILogger<GameManager> logger, IGameStorage gameStorage, ICardConverter cardConverter) 
		{
			this._logger = logger;
			this._games = gameStorage;
            this._cardConverter = cardConverter;
        }

		public GameDTO CreateGame(string playerName)
		{
			var game = new Game(new List<Player>() {
		new Player(playerName),
		new Player("AI")
	});
			_games.AddGame(game);

			return new GameDTO
			{
				Id = game.Id,
				Players = game.Players.Select(p => new PlayerDTO
				{
					Id = p.Id,
					Name = p.Name,
					Hand = p.Hand.Select(_cardConverter.ConvertToCardDTO).ToList(),
					Battlefield = p.Battlefield.Select(_cardConverter.ConvertToCardDTO).ToList(),
					Life = p.Life
				}).ToList(),
				CurrentPlayerIndex = game.currentPlayerIndex,
				IsGameOver = game.IsGameOver,
				Winner = game.Winner != null ? new PlayerDTO
				{
					Id = game.Winner.Id,
					Name = game.Winner.Name,
					Life = game.Winner.Life
				} : null
			};
		}

		public GameDTO GetGameState(string gameId)
		{
			try
			{
				if (!_games.TryGetGame(gameId, out var game))
				{
					throw new GameNotFoundException($"Game with ID '{gameId}' not found.");
				}

				return new GameDTO
				{
					Id = game.Id,
					Players = game.Players.Select(p => new PlayerDTO
					{
						Id = p.Id,
						Name = p.Name,
						Hand = p.Hand.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
						Battlefield = p.Battlefield.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
						Life = p.Life
					}).ToList(),
					CurrentPlayerIndex = game.currentPlayerIndex,
					IsGameOver = game.IsGameOver,
					Winner = game.Winner != null ? new PlayerDTO
					{
						Id = game.Winner.Id,
						Name = game.Winner.Name,
						Life = game.Winner.Life
					} : null
				};
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

				var game = new Game(players);
				_games.AddGame(game);

				return new GameDTO
				{
					Id = game.Id,
					Players = game.Players.Select(p => new PlayerDTO
					{
						Id = p.Id,
						Name = p.Name,
						Hand = p.Hand.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
						Battlefield = p.Battlefield.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
						Life = p.Life
					}).ToList(),
					CurrentPlayerIndex = game.currentPlayerIndex,
					IsGameOver = game.IsGameOver,
					Winner = game.Winner != null ? new PlayerDTO
					{
						Id = game.Winner.Id,
						Name = game.Winner.Name,
						Life = game.Winner.Life
					} : null
				};
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
	}
}
