using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameSetupTests
    {
        
        [Fact]
        public void Game_ShouldSetUpPlayersWithShuffledDecks()
        {
            // Arrange

            // Act
            var game = new Game(Helper.GetDeck());

            // Assert
            game.Players.Count.Should().Be(2);
            foreach (var player in game.Players)
            {
                player.Deck.Cards.Count.Should().Be(53);
                player.Hand.Count.Should().Be(7);
                player.Battlefield.Should().BeEmpty();
            }
        }
    }
}