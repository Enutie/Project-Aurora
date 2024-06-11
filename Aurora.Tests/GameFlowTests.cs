using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.DTO;
using Aurora.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
namespace Aurora.Tests
{
    public class GameFlowTests
    {
        [Fact]
        public void CardShouldLeaveHandWhenPlayed()
        {
            // Create mocks for the dependencies
            var logger = new Mock<ILogger<GameService>>();
            var gameManager = new Mock<IGameManager>();
            var playerActionService = new Mock<IPlayerActionService>();
            var gameQueryService = new Mock<IGameQueryService>();
            var cardConverter = new Mock<ICardConverter>();

            // Set up the mocks
            var initialGameDto = new GameDTO
            {
                Id = "1",
                Players = new List<PlayerDTO>
        {
            new PlayerDTO
            {
                Id = "1",
                Hand = new List<CardDTO>
                {
                    new LandDTO { Id = "1", LandType = "Forest" },
                    new CreatureDTO { Id = "2", Name = "Creature" }
                }
            }
        }
            };

            gameManager.Setup(gm => gm.CreateGame(It.IsAny<string>())).Returns(initialGameDto);
            playerActionService.Setup(pas => pas.PlayLand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LandDTO>()))
                .Returns<string, string, LandDTO>((gameId, playerId, landDTO) =>
                {
                    var updatedHand = initialGameDto.Players.First(p => p.Id == playerId).Hand.Where(c => c.Id != landDTO.Id).ToList();
                    return new GameDTO { Id = gameId, Players = new List<PlayerDTO> { new PlayerDTO { Id = playerId, Hand = updatedHand } } };
                });

            // Create an instance of GameService with the mocked dependencies
            IGameService gs = new GameService(logger.Object, gameManager.Object, playerActionService.Object, gameQueryService.Object, cardConverter.Object);

            var gamedto = gs.CreateGame("bob");
            LandDTO land = gamedto.Players[0].Hand.OfType<LandDTO>().FirstOrDefault();
            var handsizeForPlay = gamedto.Players[0].Hand.Count;
            gamedto = gs.PlayLand(gamedto.Id, gamedto.Players[0].Id, land);
            var handsizeAfterPlay = gamedto.Players[0].Hand.Count;
            handsizeAfterPlay.Should().BeLessThan(handsizeForPlay);
        }

    }
}
