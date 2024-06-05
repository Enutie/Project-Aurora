using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class AIOpponentTests
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
        public void AIOpponent_ShouldPlayLandIfHandIsNotEmpty()
        {
            // Arrange
            var game = new Game(GetLandCounts());
            game.SwitchTurn();
            var aiPlayer = game.Players[1];
            var initialHandCount = aiPlayer.Hand.Count;

            // Act
            game.TakeAITurn();

            // Assert
            aiPlayer.Battlefield.Should().HaveCount(1);
            aiPlayer.Hand.Should().HaveCount(initialHandCount);
            aiPlayer.Battlefield.Should().Contain(aiPlayer.Battlefield.OfType<Land>().First());
        }

        [Fact]
        public void AIOpponent_ShouldDrawAndPlayLandIfHandIsEmpty()
        {
            // Arrange
            var game = new Game(GetLandCounts());
            game.SwitchTurn();
            var aiPlayer = game.Players[1];
            aiPlayer.Hand.Clear();
            aiPlayer.Battlefield.Clear();

            // Act
            game.TakeAITurn();

            // Assert
            aiPlayer.Battlefield.Should().HaveCount(1);
            aiPlayer.Hand.Should().BeEmpty();
        }
    }
}