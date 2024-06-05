using Aurora.Server.Requests;
using Aurora.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public IActionResult CreateGame([FromBody] CreateGameRequest request)
        {
            var game = _gameService.CreateGame(request.PlayerName);
            var response = new CreateGameResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    HandCount = p.Hand.Count,
                    BattlefieldCount = p.Battlefield.Count
                }),
                CurrentPlayer = game.GetCurrentPlayer().Id
            };
            return Ok(response);
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGameState(string gameId)
        {
            var game = _gameService.GetGame(gameId);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    HandCount = p.Hand.Count,
                    BattlefieldCount = p.Battlefield.Count
                }),
                CurrentPlayer = game.GetCurrentPlayer().Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/play")]
        public IActionResult PlayLand(string gameId, [FromBody] PlayLandRequest request)
        {
            var game = _gameService.PlayLand(gameId, request.PlayerId, request.LandIndex);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    HandCount = p.Hand.Count,
                    BattlefieldCount = p.Battlefield.Count
                }),
                CurrentPlayer = game.GetCurrentPlayer().Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/ai-play")]
        public IActionResult AIOpponentPlay(string gameId)
        {
            var game = _gameService.AIOpponentPlay(gameId);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    HandCount = p.Hand.Count,
                    BattlefieldCount = p.Battlefield.Count
                }),
                CurrentPlayer = game.GetCurrentPlayer().Id
            };
            return Ok(response);
        }
    }
}
