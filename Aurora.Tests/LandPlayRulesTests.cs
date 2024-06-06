using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class LandPlayRulesTests
    {
        
        [Fact]
        public void Player_ShouldNotBeAllowedToPlayMoreThanOneLandPerTurn()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
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
