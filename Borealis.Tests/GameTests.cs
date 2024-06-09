using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Borealis.Tests
{
    public class GameTests
    {
        [Fact]
        public void Game_CanCreateNewGameWithTwoPlayers()
        {
            // Arrange
            IEnumerable<Card> library1 = new List<Card>();
            IEnumerable<Card> library2 = new List<Card>();
            Player player1 = new Player(20, library1);
            Player player2 = new Player(20, library2);

            // Act
            Game game = new Game(player1, player2);

            // Assert
            game.Players.Should().HaveCount(2);
            game.Players.Should().Contain(player1);
            game.Players.Should().Contain(player2);
        }

        [Fact]
        public void Game_CanStartAndSetupInitialState()
        {
            // Arrange
            Card card1 = new Card("Grizzly Bears", 2, 2, 2);
            Card card2 = new Card("Silvercoat Lion", 3, 2, 2);
            IEnumerable<Card> library1 = new List<Card> { card1 };
            IEnumerable<Card> library2 = new List<Card> { card2 };
            Player player1 = new Player(20, library1);
            Player player2 = new Player(20, library2);
            Game game = new Game(player1, player2);

            // Act
            game.Start();

            // Assert
            player1.Hand.Should().HaveCount(1);
            player2.Hand.Should().HaveCount(1);
        }

        [Fact]
        public void Game_CanPerformTurn()
        {
            // Arrange
            Card card1 = new Card("Grizzly Bears", 2, 2, 2);
            Card card2 = new Card("Silvercoat Lion", 3, 2, 2);
            Card card3 = new Card("Eager Construct", 4, 3, 3);
            Card card4 = new Card("Ornithopter", 0, 0, 2);
            IEnumerable<Card> library1 = new List<Card> { card1, card2, card3, card4 };
            IEnumerable<Card> library2 = new List<Card> { card4 };
            Player player1 = new Player(20, library1);
            Player player2 = new Player(20, library2);
            Game game = new Game(player1, player2);
            game.Start();

            // Act
            game.PerformTurn(player1);

            // Assert
            player1.Hand.Should().HaveCount(4); // Player1 initially drew 3 cards, then drew 1 more during the turn
            player1.Library.Should().BeEmpty(); // Player1's library is now empty
            player2.Hand.Should().HaveCount(1); // Player2's hand remains unchanged
        }

        [Fact]
        public void Game_CanCheckWinCondition_PlayerWithZeroLife()
        {
            // Arrange
            Card card1 = new Card("Grizzly Bears", 2, 2, 2);
            Card card2 = new Card("Silvercoat Lion", 3, 2, 2);
            IEnumerable<Card> library1 = new List<Card> { card1 };
            IEnumerable<Card> library2 = new List<Card> { card2 };
            Player player1 = new Player(20, library1);
            Player player2 = new Player(0, library2); // Player2 starts with 0 life

            Game game = new Game(player1, player2);

            // Act
            Player winner = game.CheckWinCondition();

            // Assert
            winner.Should().Be(player1);
        }
    }
}
