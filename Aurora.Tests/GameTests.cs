using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameTests
    {
        
        [Fact]
        public void Game_ShouldStartWithTwoPlayers()
        {
            // Arrange
            var game = new Game(new List<Player>()
            {
                new Player("Bob"),
                new Player("AI")
            });

            // Act
            var playerCount = game.Players.Count;

            // Assert
            playerCount.Should().Be(2);
        }

        [Fact]
        public void Game_ShouldAllowPlayerToPlayLand()
        {
            // Arrange
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });
            var player = game.Players[0];
            var land = new Land(LandType.Plains);
            player.Hand.Add(land);

            // Act
            game.PlayLand(player, land);

            // Assert
            player.Battlefield.Should().ContainSingle(c => c == land);
            player.Hand.Count.Should().Be(7);
        }

    }
}