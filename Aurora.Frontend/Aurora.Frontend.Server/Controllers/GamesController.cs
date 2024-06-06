using Microsoft.AspNetCore.Mvc;
using Aurora.Server.Requests;
using Aurora.Server.Services;

namespace Aurora.Controllers
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
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count
                }).ToList()
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
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count,
                    Battlefield = p.Battlefield.Select(c => new CardResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Power = (c as Creature)?.Power,
                        Toughness = (c as Creature)?.Toughness
                    }).ToList()
                }).ToList(),
                CurrentPlayer = game.GetCurrentPlayer().Id,
                IsGameOver = game.IsGameOver,
                Winner = game.Winner?.Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/play-land")]
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
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count,
                    Battlefield = p.Battlefield.Select(c => new CardResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Power = (c as Creature)?.Power,
                        Toughness = (c as Creature)?.Toughness
                    }).ToList()
                }).ToList(),
                CurrentPlayer = game.GetCurrentPlayer().Id,
                IsGameOver = game.IsGameOver,
                Winner = game.Winner?.Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/cast-creature")]
        public IActionResult CastCreature(string gameId, [FromBody] CastCreatureRequest request)
        {
            var game = _gameService.CastCreature(gameId, request.PlayerId, request.CreatureIndex);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count,
                    Battlefield = p.Battlefield.Select(c => new CardResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Power = (c as Creature)?.Power,
                        Toughness = (c as Creature)?.Toughness
                    }).ToList()
                }).ToList(),
                CurrentPlayer = game.GetCurrentPlayer().Id,
                IsGameOver = game.IsGameOver,
                Winner = game.Winner?.Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/attack")]
        public IActionResult Attack(string gameId, [FromBody] AttackRequest request)
        {
            var game = _gameService.Attack(gameId, request.AttackerId, request.DefenderId, request.AttackingCreatureIds);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count,
                    Battlefield = p.Battlefield.Select(c => new CardResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Power = (c as Creature)?.Power,
                        Toughness = (c as Creature)?.Toughness
                    }).ToList()
                }).ToList(),
                CurrentPlayer = game.GetCurrentPlayer().Id,
                IsGameOver = game.IsGameOver,
                Winner = game.Winner?.Id
            };
            return Ok(response);
        }

        [HttpPost("{gameId}/end-turn")]
        public IActionResult EndTurn(string gameId)
        {
            var game = _gameService.EndTurn(gameId);
            var response = new GameStateResponse
            {
                GameId = game.Id,
                Players = game.Players.Select(p => new PlayerResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Life = p.Life,
                    HandCount = p.Hand.Count,
                    Hand = p.Hand.Select(c => new CardResponse(c)).ToList(),
                    DeckCount = p.Deck.Cards.Count,
                    Battlefield = p.Battlefield.Select(c => new CardResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Power = (c as Creature)?.Power,
                        Toughness = (c as Creature)?.Toughness
                    }).ToList()
                }).ToList(),
                CurrentPlayer = game.GetCurrentPlayer().Id,
                IsGameOver = game.IsGameOver,
                Winner = game.Winner?.Id
            };
            return Ok(response);
        }
    }
}