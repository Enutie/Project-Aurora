namespace BorealisTests
{
    public class PlayerTests
    {
        [Fact]
        public void PlayerAlwaysHaveADeck()
        {
            var player = new Player();
            player.Deck.Should().NotBeEmpty();
        }

    }
}
