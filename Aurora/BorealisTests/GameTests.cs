using Borealis;

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
            Assert.NotNull(game);
            Assert.False(game.IsRunning);
        }
    }
}