using Xunit;
using FluentAssertions;
using System.Numerics;

namespace Aurora.Tests
{
    public class PlayerTests
    {
        private IEnumerable<(LandType Type, int Count)> GetLandCounts()
        {
            var landCounts = new[]
            {
                new { Type = LandType.Plains, Count = 10 },
                new { Type = LandType.Island, Count = 10 },
                new { Type = LandType.Swamp, Count = 10 },
                new { Type = LandType.Mountain, Count = 10 },
                new { Type = LandType.Forest, Count = 10 }
            }.Select(lc => (lc.Type, lc.Count));
            return landCounts;
        }
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
            var landCounts = new[]
            {
        new { Type = LandType.Plains, Count = 10 },
        new { Type = LandType.Island, Count = 10 },
        new { Type = LandType.Swamp, Count = 10 },
        new { Type = LandType.Mountain, Count = 10 },
        new { Type = LandType.Forest, Count = 10 }
    };
            var deck = new Deck(GetLandCounts());
            deck.Shuffle();
            var player = new Player { Deck = deck };

            // Act
            player.DrawCard();

            // Assert
            player.Hand.Count.Should().Be(1);
            player.Deck.Cards.Count.Should().Be(49);
        }
    }
}