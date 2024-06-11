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

    }
}