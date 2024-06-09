using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Borealis.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_HasCorrectInitialState()
        {
            // Arrange
            int initialLife = 20;
            IEnumerable<Card> library = new List<Card>(); // Empty library for now

            // Act
            Player player = new Player(initialLife, library);

            // Assert
            player.Life.Should().Be(initialLife);
            player.Library.Should().BeEquivalentTo(library);
            player.Hand.Should().BeEmpty();
            player.Battlefield.Should().BeEmpty();
        }

        [Fact]
        public void Player_CanDrawCardFromLibrary()
        {
            // Arrange
            Card card1 = new Card("Grizzly Bears", 2, 2, 2);
            Card card2 = new Card("Silvercoat Lion", 3, 2, 2);
            IEnumerable<Card> library = new List<Card> { card1, card2 };
            Player player = new Player(20, library);

            // Act
            player.DrawCard();

            // Assert
            player.Hand.Should().HaveCount(1);
            player.Hand.Should().Contain(card1);
            player.Library.Should().HaveCount(1);
            player.Library.Should().Contain(card2);
        }

        [Fact]
        public void Player_CanPlayCardFromHandToBattlefield()
        {
            // Arrange
            Card card = new Card("Grizzly Bears", 2, 2, 2);
            IEnumerable<Card> library = new List<Card> { card };
            Player player = new Player(20, library);
            player.DrawCard();

            // Act
            player.PlayCard(card);

            // Assert
            player.Hand.Should().BeEmpty();
            player.Battlefield.Should().HaveCount(1);
            player.Battlefield.Should().Contain(card);
        }
    }
}
