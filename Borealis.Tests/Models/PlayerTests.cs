using Borealis.Models;
using FluentAssertions;

namespace Borealis.Tests
{

    public class PlayerTests
    {

        [Fact]
        public void Constructor_ShouldInitializePlayerWithDefaultValues()
        {
            // Arrange
            var expectedName = "Player 1";
            var expectedLife = 20;

            // Act
            var player = new Player(expectedName);

            // Assert
            player.Name.Should().Be(expectedName);
            player.Life.Should().Be(expectedLife);
            player.Library.Should().BeEmpty();
            player.Hand.Should().BeEmpty();
            player.Battlefield.Should().BeEmpty();
        }
    }
}