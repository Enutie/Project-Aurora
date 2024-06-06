using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class ManaSystemTests
    {
        [Fact]
        public void Land_ShouldProduceMana()
        {
            // Arrange
            var land = new Land(LandType.Forest);
            var player = new Player();

            // Act
            player.PlayLand(land);

            // Assert
            player.ManaPool._mana.Keys.ToList().Should().Contain(Mana.Green);
        }

        [Fact]
        public void Player_ShouldSpendManaFromPool()
        {
            // Arrange
            var player = new Player();
            player.ManaPool.Add(Mana.Green);
            player.ManaPool.Add(Mana.Green);
            var manaCost = new[] { Mana.Green, Mana.Green };

            // Act
            player.ManaPool.Spend(manaCost);

            // Assert
            player.ManaPool._mana.Should().BeEmpty();
        }

        [Fact]
        public void Player_ShouldNotSpendManaWhenInsufficientFunds()
        {
            // Arrange
            var player = new Player();
            player.ManaPool.Add(Mana.Green);
            var manaCost = new[] { Mana.Green, Mana.Green };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => player.ManaPool.Spend(manaCost));
        }

        // Add more tests for the mana system as needed
    }
}