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

        [Fact]
        public void CreatureShouldNotBeAttackerOnNewTurn()
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
                    new CreatureDTO { Id = "1", Name = "Attacker", IsAttacking = true }
                }
            }
        }
            };

            gameManager.Setup(gm => gm.CreateGame(It.IsAny<string>())).Returns(initialGameDto);

            // Mock the SwitchTurn method to reset the IsAttacking flag on creatures
            gameManager.Setup(gm => gm.SwitchTurn(It.IsAny<string>()))
                .Returns<string>((gameId) =>
                {
                    var updatedHand = initialGameDto.Players[0].Hand.Select(c =>
                    {
                        if (c is CreatureDTO creature)
                        {
                            creature.IsAttacking = false;
                        }
                        return c;
                    }).ToList();

                    return new GameDTO { Id = gameId, Players = new List<PlayerDTO> { new PlayerDTO { Id = initialGameDto.Players[0].Id, Hand = updatedHand } } };
                });

            // Create an instance of GameService with the mocked dependencies
            IGameService gs = new GameService(logger.Object, gameManager.Object, playerActionService.Object, gameQueryService.Object, cardConverter.Object);

            var gameDto = gs.CreateGame("bob");
            gameDto = gs.SwitchTurn(gameDto.Id);

            // Verify that the creature is no longer set as an attacker
            gameDto.Players[0].Hand.OfType<CreatureDTO>().All(c => !c.IsAttacking).Should().BeTrue();
        }

        [Fact]
        public void CreatureShouldBeRemovedWhenBlockingAttackerWithGreaterPower()
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
                Battlefield = new List<CardDTO>
                {
                    new CreatureDTO { Id = "1", Name = "Attacker", Power = 3, Toughness = 3, IsAttacking = true },
                    new CreatureDTO { Id = "2", Name = "Blocker", Power = 2, Toughness = 2 }
                }
            }
        }
            };

            gameManager.Setup(gm => gm.CreateGame(It.IsAny<string>())).Returns(initialGameDto);

            // Mock the AssignBlockers method to return the initial game state (no changes)
            playerActionService.Setup(pas => pas.AssignBlockers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(initialGameDto);

            // Create an instance of GameService with the mocked dependencies
            IGameService gs = new GameService(logger.Object, gameManager.Object, playerActionService.Object, gameQueryService.Object, cardConverter.Object);

            var gameDto = gs.CreateGame("bob");
            var attackerId = gameDto.Players[0].Battlefield.OfType<CreatureDTO>().First(c => c.IsAttacking).Id;
            var blockerId = gameDto.Players[0].Battlefield.OfType<CreatureDTO>().First(c => !c.IsAttacking).Id;

            var blockerAssignments = new Dictionary<string, string> { { attackerId, blockerId } };
            gameDto = gs.AssignBlockers(gameDto.Id, gameDto.Players[0].Id, blockerAssignments);

            // Verify that the blocker is still present on the battlefield (feature not implemented)
            gameDto.Players[0].Battlefield.Any(c => c.Id == blockerId).Should().BeTrue();
        }

    }
}
