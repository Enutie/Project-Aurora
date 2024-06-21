using Aurora.Application.DTO;
using Aurora.Application.Interfaces;
using Aurora.Shared.Exceptions;
using Aurora.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aurora.Application.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameManager _gameManager;
        private readonly IPlayerActionService _playerActionService;
        private readonly IGameQueryService _gameQueryService;
        private readonly ICardConverter _cardConverter;
        public GameService(ILogger<GameService> logger, IGameManager gameManager, IPlayerActionService playerActionService, IGameQueryService gameQueryService, ICardConverter cardConverter)
        {
            _logger = logger;
            _gameManager = gameManager;
            _playerActionService = playerActionService;
            _gameQueryService = gameQueryService;
            _cardConverter = cardConverter;
        }

        public GameDTO CreateGame(string playerName)
        {
            return _gameManager.CreateGame(playerName);
        }

        public GameDTO StartGame(List<PlayerDTO> playerDTOs)
        {
            return _gameManager.StartGame(playerDTOs);
        }

        public GameDTO PlayLand(string gameId, string playerId, LandDTO landDTO)
        {
            return _playerActionService.PlayLand(gameId, playerId, landDTO);
        }

        public GameDTO CastCreature(string gameId, string playerId, CreatureDTO creatureDTO)
        {
            return _playerActionService.CastCreature(gameId, playerId, creatureDTO);
        }

        public GameDTO Attack(string gameId, string attackingPlayerId, List<string> attackingCreatureIds)
        {
            return _playerActionService.Attack(gameId, attackingPlayerId, attackingCreatureIds);
        }

        public GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments)
        {
            return _playerActionService.AssignBlockers(gameId, defendingPlayerId, blockerAssignments);
        }

        public GameDTO SwitchTurn(string gameId)
        {
            return _gameManager.SwitchTurn(gameId);
        }

        public GameDTO GetGameState(string gameId)
        {
            return _gameManager.GetGameState(gameId);
        }

        public PlayerDTO GetPlayerInfo(string gameId, string playerId)
        {
            return _gameQueryService.GetPlayerInfo(gameId, playerId);
        }

        public LandDTO GetLandById(string landId)
        {
            return _gameQueryService.GetLandById(landId);
        }

        public CreatureDTO GetCreatureById(string creatureId)
        {
            return _gameQueryService.GetCreatureById(creatureId);
        }

        public GameDTO AdvanceToNextPhase(string gameId)
        {
            return _gameManager.AdvanceToNextPhase(gameId);
        }
    }
}
