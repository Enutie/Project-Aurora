using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Aurora.Tests
{
    public class WinConditionTests
    {
        
        [Fact]
        public void Game_ShouldEndWhenPlayerReaches0Life()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.Players[0];
            player.Life = 1;

            // Act
            player.TakeDamage(1);

            // Assert
            game.CheckWinConditions();
            game.IsGameOver.Should().BeTrue();
            game.Winner.Should().Be(game.Players[1]);
        }

        [Fact]
        public void Game_ShouldEndWhenPlayerHasEmptyDeck()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.Players[0];
            player.Deck.Cards.Clear();

            // Act
            game.DrawCard(player);

            // Assert
            game.IsGameOver.Should().BeTrue();
            game.Winner.Should().Be(game.Players[1]);
        }
    }
}
