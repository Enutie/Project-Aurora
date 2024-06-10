using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Borealis.GameLogic;
using Borealis.Models;
using FluentAssertions;

namespace Borealis.Tests.GameTests
{
    public class GameTests
    {
        [Fact]
        public void Constructor_ShouldInitializeGameWithDefaultValues()
        {
            // Arrange
            var player1Name = "Player 1";
            var player2Name = "Player 2";

            // Act
            var game = new Game(player1Name, player2Name);

            // Assert
            game.Player1.Name.Should().Be(player1Name);
            game.Player2.Name.Should().Be(player2Name);
            game.CurrentPlayer.Should().Be(game.Player1);
            game.NonCurrentPlayer.Should().Be(game.Player2);
        }

        [Fact]
        public void PlayGame_ShouldEndGameWhenPlayerReachesZeroLife()
        {
            // Arrange
            var player1Name = "Player 1";
            var player2Name = "Player 2";
            var game = new Game(player1Name, player2Name);

            // Create some cards for the players
            var landCard = new Card("Forest", 0, 0, 0, true);
            var creature1 = new Card("Grizzly Bears", 2, 2, 2, false);
            var creature2 = new Card("Hill Giant", 3, 3, 3, false);

            // Add cards to player libraries
            game.Player1.Library.AddRange(new[] { landCard, landCard, creature1 });
            game.Player2.Library.AddRange(new[] { landCard, landCard, landCard, creature2 });

            // Act
            game.PlayGame();

            // Assert
            game.Player1.Life.Should().Be(0);
            game.Player2.Life.Should().Be(17);
        }
    }
}
