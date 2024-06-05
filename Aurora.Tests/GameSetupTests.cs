using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameSetupTests
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
        public void Game_ShouldSetUpPlayersWithShuffledDecks()
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

            // Act
            var game = new Game(GetLandCounts());

            // Assert
            game.Players.Count.Should().Be(2);
            foreach (var player in game.Players)
            {
                player.Deck.Cards.Count.Should().Be(43);
                player.Hand.Count.Should().Be(7);
                player.Battlefield.Should().BeEmpty();
            }
        }
    }
}