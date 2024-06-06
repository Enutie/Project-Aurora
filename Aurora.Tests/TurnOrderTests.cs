using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    
    public class TurnOrderTests
    {
        
        [Fact]
        public void Game_ShouldStartWithPlayerOnesTurn()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());

            // Act
            var currentPlayer = game.GetCurrentPlayer();

            // Assert
            currentPlayer.Should().Be(game.Players[0]);
        }

        [Fact]
        public void Game_ShouldSwitchTurnsAfterPlayerAction()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var currentPlayer = game.GetCurrentPlayer();

            // Act
            game.PlayLand(currentPlayer, new Land(LandType.Plains));
            var newCurrentPlayer = game.GetCurrentPlayer();

            // Assert
            newCurrentPlayer.Should().NotBe(currentPlayer);
        }
    }
}