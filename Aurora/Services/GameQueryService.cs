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
            var games = _games.GetAllGames();
			var game = games.FirstOrDefault(g => g.Players.Any(p => p.Hand.Any(c => c.Id == creatureId && c is Creature)));

			if (game == null)
			{
				return null;
			}

			var creature = game.Players.SelectMany(p => p.Hand).OfType<Creature>().FirstOrDefault(c => c.Id == creatureId);

			if (creature == null)
			{
				return null;
			}

			return new CreatureDTO
			{
				Id = creature.Id,
				Name = creature.Name,
				ManaCost = creature.ManaCost.Select(m => m.ToString()).ToList(),
				Power = creature.Power,
				Toughness = creature.Toughness
			};
		}

        public LandDTO GetLandById(string landId)
        {
            var games = _games.GetAllGames();
            var game = games.FirstOrDefault(g => g.Players.Any(p => p.Hand.Any(c => c.Id == landId && c is Land)));

            if (game == null)
            {
                return null;
            }

            var land = game.Players.SelectMany(p => p.Hand).OfType<Land>().FirstOrDefault(l => l.Id == landId);

            if (land == null)
            {
                return null;
            }

            return new LandDTO
            {
                Id = land.Id,
                LandType = land.Type.ToString()
            };
        }

        public PlayerDTO GetPlayerInfo(string gameId, string playerId)
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

                return new PlayerDTO
                {
                    Id = player.Id,
                    Name = player.Name,
                    Hand = player.Hand.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
                    Battlefield = player.Battlefield.Select(c => _cardConverter.ConvertToCardDTO(c)).ToList(),
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
