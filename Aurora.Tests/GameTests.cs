﻿using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameTests
    {
        private IEnumerable<(LandType Type, int Count)> GetLandCounts()
        {
            var landCounts = new[]
            {
                new { Type = LandType.Plains, Count = 10 },
                new { Type = LandType.Island, Count = 10 },
                new { Type = LandType.Swamp, Count = 10 },
                new { Type = LandType.Mountain, Count = 10 },
                new { Type = LandType.Forest, Count = 10 }
            }.Select(lc => (lc.Type, lc.Count));
            return landCounts;
        }
        [Fact]
        public void Game_ShouldStartWithTwoPlayers()
        {
            // Arrange
            var game = new Game(GetLandCounts());

            // Act
            var playerCount = game.Players.Count;

            // Assert
            playerCount.Should().Be(2);
        }

        [Fact]
        public void Game_ShouldAllowPlayerToPlayLand()
        {
            // Arrange
            var game = new Game(GetLandCounts());
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