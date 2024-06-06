using FluentAssertions;

namespace Aurora.Tests
{
    public class PlayerLifeTests
    {
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
