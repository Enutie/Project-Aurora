using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class ManaSystemTests
    {
        [Fact]
        public void Land_ShouldProduceManaAndBeTapped()
        {
            // Arrange
            var land = new Land(LandType.Forest);
            var player = new Player();

            // Act
            player.PlayLand(land);

            // Assert
            player.ManaPool._mana.Keys.Should().Contain(Mana.Green);
            player.ManaPool.LandsUsed.Should().Contain(land);
            land.IsTapped.Should().BeTrue();
        }

        [Fact]
        public void Player_ShouldSpendManaFromPool()
        {
            // Arrange
            var player = new Player();
            var land1 = new Land(LandType.Forest);
            var land2 = new Land(LandType.Forest);
            player.PlayLand(land1);
            player.PlayLand(land2);
            var manaCost = new[] { Mana.Green, Mana.Green };

            // Act
            player.ManaPool.Spend(manaCost);

            // Assert
            player.ManaPool._mana.Should().BeEmpty();
            player.ManaPool.LandsUsed.Should().BeEmpty();
        }

        [Fact]
        public void Player_ShouldNotSpendManaWhenInsufficientFunds()
        {
            // Arrange
            var player = new Player();
            var land = new Land(LandType.Forest);
            player.PlayLand(land);
            var manaCost = new[] { Mana.Green, Mana.Green };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => player.ManaPool.Spend(manaCost));
        }

        [Fact]
        public void Game_ShouldUntapLandsAtBeginningOfTurn()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.GetCurrentPlayer();
            var land = new Land(LandType.Forest);
            player.PlayLand(land);

            // Act
            game.SwitchTurn();
            game.SwitchTurn();

            // Assert
            land.IsTapped.Should().BeFalse();
        }

        [Fact]
        public void Player_ShouldNotCastCreatureWithInsufficientMana()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.GetCurrentPlayer();
            var creature = new Creature("Test Creature", new[] { Mana.Green, Mana.Green }, 2, 2);
            player.Hand.Add(creature);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => game.CastCreature(player, creature));
        }

        [Fact]
        public void Player_ShouldCastCreatureWithSufficientMana()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.GetCurrentPlayer();
            var land = new Land(LandType.Forest);
            player.Hand.Add(land);
            player.PlayLand(land);
            var creature = new Creature("Test Creature", new[] { Mana.Green }, 1, 1);
            player.Hand.Add(creature);

            // Act
            game.CastCreature(player, creature);

            // Assert
            player.Battlefield.Should().Contain(creature);
            player.Hand.Should().NotContain(creature);
            land.IsTapped.Should().BeTrue();
        }

        // Add more tests for the mana system as needed
    }
}