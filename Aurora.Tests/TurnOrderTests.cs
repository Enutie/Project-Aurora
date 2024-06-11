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
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });

            // Act
            var currentPlayer = game.GetCurrentPlayer();

            // Assert
            currentPlayer.Should().Be(game.Players[0]);
        }

    }
}