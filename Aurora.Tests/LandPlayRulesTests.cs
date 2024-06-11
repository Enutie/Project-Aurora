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
            var game = new Game(new List<Player>()
            {
                new Player("Bob"),
                new Player("AI")
            });
            var currentPlayer = game.GetCurrentPlayer();
            var land = new Land(LandType.Plains);
            var land2 = new Land(LandType.Island);
            currentPlayer.Hand.Add(land);
            currentPlayer.Hand.Add(land2);

            // Act
            game.PlayLand(currentPlayer, land);
            var canPlayAnotherLand = game.CanPlayLand(currentPlayer);

            // Assert
            canPlayAnotherLand.Should().BeFalse();
        }

    }
}
