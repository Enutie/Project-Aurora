using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class AIOpponentTests
    {
        
        [Fact]
        public void AIOpponent_ShouldPlayLandIfHandIsNotEmpty()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            game.SwitchTurn();
            var aiPlayer = game.Players[1];
            var initialHandCount = aiPlayer.Hand.Count;

            // Act
            game.TakeAITurn();

            // Assert
            aiPlayer.Battlefield.Should().HaveCount(1);
            aiPlayer.Hand.Should().HaveCount(initialHandCount-1);
            aiPlayer.Battlefield.Should().Contain(aiPlayer.Battlefield.OfType<Land>().First());
        }
    }
}