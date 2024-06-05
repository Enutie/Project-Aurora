using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class LandPlayRulesTests
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
        public void Player_ShouldNotBeAllowedToPlayMoreThanOneLandPerTurn()
        {
            // Arrange
            var game = new Game(GetLandCounts());
            var currentPlayer = game.GetCurrentPlayer();
            currentPlayer.Hand.Add(new Land(LandType.Plains));
            currentPlayer.Hand.Add(new Land(LandType.Island));

            // Act
            game.PlayLand(currentPlayer, new Land(LandType.Plains));
            var canPlayAnotherLand = game.CanPlayLand(currentPlayer);

            // Assert
            canPlayAnotherLand.Should().BeFalse();
        }

    }
}
