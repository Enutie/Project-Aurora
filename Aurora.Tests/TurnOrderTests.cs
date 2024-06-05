using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class TurnOrderTests
    {
        private IEnumerable<(LandType Type, int Count)> GetLandCounts()
        {
            var landCounts = new[]
            {
                new { Type = LandType.Plains, Count = 10 },
                new { Type = LandType.Island, Count = 10 },
                new { Type = LandType.Swamp, Count = 10 },
                new { Type = LandType.Mountain, Count = 10 },
                new { Type = LandType.Forest, Count = 10 }
            }.Select(lc => (lc.Type, lc.Count));
            return landCounts;
        }
        [Fact]
        public void Game_ShouldStartWithPlayerOnesTurn()
        {
            // Arrange
            var game = new Game(GetLandCounts());

            // Act
            var currentPlayer = game.GetCurrentPlayer();

            // Assert
            currentPlayer.Should().Be(game.Players[0]);
        }

        [Fact]
        public void Game_ShouldSwitchTurnsAfterPlayerAction()
        {
            // Arrange
            var game = new Game(GetLandCounts());
            var currentPlayer = game.GetCurrentPlayer();

            // Act
            game.PlayLand(currentPlayer, new Land(LandType.Plains));
            var newCurrentPlayer = game.GetCurrentPlayer();

            // Assert
            newCurrentPlayer.Should().NotBe(currentPlayer);
        }
    }
}