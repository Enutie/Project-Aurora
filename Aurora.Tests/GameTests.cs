using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameTests
    {
        private Deck GetDeck
            ()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 24; i++)
            {
                deck.Add(new Land(LandType.Plains));
            }
            for (int i = 0; i < 36; i++)
            {
                deck.Add(new Creature("Devoted Hero", new[] { Mana.White }, 1, 2));
            }
            return new Deck(deck);
        }
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