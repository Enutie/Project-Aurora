using Microsoft.AspNetCore.Mvc;
using Aurora.Services;
using Aurora.DTO;
using Aurora.Server.Models;
using Aurora.Exceptions;

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

        [HttpPost("start")]
        public IActionResult StartGame([FromBody] List<PlayerDTO> players)
        {
            var gameDto = _gameService.StartGame(players);
            return Ok(gameDto);
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGameState(string gameId)
        {
            try
            {
                var gameDto = _gameService.GetGameState(gameId);
                return Ok(gameDto);
            }
            catch (GameNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the game state.");
            }

        }

        [HttpPost("create-game")]
        public IActionResult CreateGame([FromBody] string playerName)
        {
            var gameDto = _gameService.CreateGame(playerName);
            return Ok(gameDto);
        }
        
        [HttpPost("{gameId}/play-land")]
        public IActionResult PlayLand(string gameId, [FromBody] LandPlayDTO landPlayDto)
        {
            // Retrieve the LandDTO based on the provided LandId
            var landDto = _gameService.GetLandById(landPlayDto.LandId);

            if (landDto == null)
            {
                return BadRequest("Invalid land ID");
            }

            var gameDto = _gameService.PlayLand(gameId, landPlayDto.PlayerId, landDto);
            return Ok(gameDto);
        }

        [HttpPost("{gameId}/cast-creature")]
        public IActionResult CastCreature(string gameId, [FromBody] CreatureCastDTO creatureCastDto)
        {
            var creatureDto = _gameService.GetCreatureById(creatureCastDto.CreatureId);

            if (creatureDto == null)
            {
                return BadRequest("Invalid creature ID");
            }

            var gameDto = _gameService.CastCreature(gameId, creatureCastDto.PlayerId, creatureDto);
            return Ok(gameDto);
        }

        [HttpPost("{gameId}/attack")]
        public IActionResult Attack(string gameId, [FromBody] AttackDTO attackDto)
        {
            var gameDto = _gameService.Attack(gameId, attackDto.AttackingPlayerId, attackDto.AttackingCreatureIds);
            return Ok(gameDto);
        }

        [HttpPost("{gameId}/assign-blockers")]
        public IActionResult AssignBlockers(string gameId, [FromBody] BlockAssignmentDTO blockAssignmentDto)
        {
            var gameDto = _gameService.AssignBlockers(gameId, blockAssignmentDto.DefendingPlayerId, blockAssignmentDto.BlockerAssignments);
            return Ok(gameDto);
        }

        [HttpPost("{gameId}/advance-phase")]
        public IActionResult AdvanceToNextPhase(string gameId)
        {
            var gameDto = _gameService.AdvanceToNextPhase(gameId);
            return Ok(gameDto);
        }

    }
}