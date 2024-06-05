using Xunit;
using FluentAssertions;
using System.Linq;

namespace Aurora.Tests
{
    public class DeckTests
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
        public void Deck_ShouldContainCorrectNumberOfLands()
        {

            // Act
            var deck = new Deck(GetLandCounts());

            // Assert
            deck.Cards.Count().Should().Be(50);
            foreach (var landCount in GetLandCounts())
            {
                deck.Cards.OfType<Land>().Count(c => c.Type == landCount.Type).Should().Be(landCount.Count);
            }
        }

        [Fact]
        public void Deck_ShouldBeShuffled()
        {
            // Arrange
            var originalDeck = new Deck(GetLandCounts());
            var shuffledDeck = new Deck(GetLandCounts());

            // Act
            shuffledDeck.Shuffle();

            // Assert
            shuffledDeck.Cards.Should().NotBeEquivalentTo(originalDeck.Cards, options => options.WithStrictOrdering());
        }
    }
}