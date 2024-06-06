using Xunit;
using FluentAssertions;
using System.Linq;

namespace Aurora.Tests
{
    public class DeckTests
    {

        [Fact]
        public void Deck_ShouldBeShuffled()
        {
            // Arrange
            var originalDeck = Helper.GetDeck();
            var shuffledDeck = Helper.GetDeck();

            // Act
            shuffledDeck.Shuffle();

            // Assert
            shuffledDeck.Cards.Should().NotBeEquivalentTo(originalDeck.Cards, options => options.WithStrictOrdering());
        }
    }
}