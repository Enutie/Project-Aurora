using Xunit;
using FluentAssertions;
using System.Numerics;

namespace Aurora.Tests
{
    public class PlayerTests
    {
        
        [Fact]
        public void Player_ShouldStartWithEmptyHand()
        {
            // Arrange
            var player = new Player();

            // Act
            var handCount = player.Hand.Count;

            // Assert
            handCount.Should().Be(0);
        }

        [Fact]
        public void Player_ShouldDrawCardFromDeck()
        {
            // Arrange
            
            var deck = Helper.GetDeck();
            deck.Shuffle();
            var player = new Player { Deck = deck };

            // Act
            player.DrawCard();

            // Assert
            player.Hand.Count.Should().Be(1);
            player.Deck.Cards.Count.Should().Be(59);
        }
    }
}