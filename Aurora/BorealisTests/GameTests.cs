using Borealis;
using FluentAssertions;

namespace BorealisTests
{
    public class GameTests
    {
        [Fact]
        public void CanStartGame()
        {
            var game = new Game();
            game.StartGame();
            Assert.NotNull(game);
            Assert.True(game.IsRunning);

        }

        [Fact]
        public void CanStopGame()
        {
            var game = new Game();
            game.StartGame();
            game.EndGame();
            game.Should().NotBeNull();
            game.IsRunning.Should().Be(false);
        }

        [Fact]
        public void GameStartsWithTwoPlayers()
        {
            var game = new Game();
            game.StartGame();
            int numOfPlayers = game.Players.Count;
            numOfPlayers.Should().Be(2);
        }
    }
}