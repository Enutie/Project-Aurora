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
		private readonly Dictionary<string, Game> _games;
		private readonly ILogger<GameManager> _logger;

		public GameManager(ILogger<GameManager> logger, Dictionary<string, Game> _games) 
		{
			this._logger = logger;
			this._games = _games;
		}

		public GameDTO CreateGame(string playerName)
		{
			var game = new Game(new List<Player>() {
		new Player(playerName),
		new Player("AI")
	});
			_games[game.Id] = game;

			return new GameDTO
			{
				Id = game.Id,
				Players = game.Players.Select(p => new PlayerDTO
				{
					Id = p.Id,
					Name = p.Name,
					Hand = p.Hand.Select(ConvertToCardDTO).ToList(),
					Battlefield = p.Battlefield.Select(ConvertToCardDTO).ToList(),
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
				if (!_games.TryGetValue(gameId, out var game))
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
						Hand = p.Hand.Select(c => ConvertToCardDTO(c)).ToList(),
						Battlefield = p.Battlefield.Select(c => ConvertToCardDTO(c)).ToList(),
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
				_games[game.Id] = game;

				return new GameDTO
				{
					Id = game.Id,
					Players = game.Players.Select(p => new PlayerDTO
					{
						Id = p.Id,
						Name = p.Name,
						Hand = p.Hand.Select(c => ConvertToCardDTO(c)).ToList(),
						Battlefield = p.Battlefield.Select(c => ConvertToCardDTO(c)).ToList(),
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
				if (!_games.TryGetValue(gameId, out var game))
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

		private CardDTO ConvertToCardDTO(Card card)
		{
			switch (card)
			{
				case Creature creature:
					return new CreatureDTO
					{
						Id = creature.Id,
						Name = creature.Name,
						Power = creature.Power,
						Toughness = creature.Toughness,
						ManaCost = creature.ManaCost.Select(m => m.ToString()).ToList(),
						IsAttacking = creature.IsAttacking,
						IsBlocked = creature.IsBlocked,
						BlockedBy = creature.BlockedBy != null ? ConvertToCardDTO(creature.BlockedBy) as CreatureDTO : null
					};

				case Land land:
					return new LandDTO
					{
						Id = land.Id,
						Name = land.Name,
						LandType = land.Type.ToString(),
						IsTapped = land.IsTapped
					};

				default:
					return new CardDTO
					{
						Id = card.Id,
						Name = card.Name
					};
			}
		}
	}
}
