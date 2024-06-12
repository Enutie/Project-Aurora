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
            game.StartMainPhase1();
            game.PlayLand(player, land);

            // Assert
            player.Battlefield.Should().ContainSingle(c => c == land);
            player.Hand.Count.Should().Be(7);
        }

        [Fact]
        public void Game_ShouldSetUpPlayersWithShuffledDecks()
        {
            // Arrange

            // Act
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });

            // Assert
            game.Players.Count.Should().Be(2);
            foreach (var player in game.Players)
            {
                player.Deck.Cards.Count.Should().Be(53);
                player.Hand.Count.Should().Be(7);
                player.Battlefield.Should().BeEmpty();
            }
        }

        [Fact]
        public void Game_ShouldStartWithPlayerOnesTurn()
        {
            // Arrange
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });

            // Act
            var currentPlayer = game.GetCurrentPlayer();

            // Assert
            currentPlayer.Should().Be(game.Players[0]);
        }
    }
}