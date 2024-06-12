using Xunit;
using FluentAssertions;
using System.Numerics;

namespace Aurora.Tests
{
    public class PlayerTests
    {
        
        [Fact]
        public void Player_ShouldStartWithEmptyHand()
        {
            // Arrange
            var player = new Player();

            // Act
            var handCount = player.Hand.Count;

            // Assert
            handCount.Should().Be(0);
        }
        [Fact]
        public void Player_ShouldStartWith20Life()
        {
            // Arrange
            var player = new Player();

            // Assert
            player.Life.Should().Be(20);
        }

        [Fact]
        public void Player_ShouldLoseLifeWhenDamaged()
        {
            // Arrange
            var player = new Player();
            var damage = 5;

            // Act
            player.TakeDamage(damage);

            // Assert
            player.Life.Should().Be(15);
        }

    }
}