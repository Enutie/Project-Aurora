using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class ManaSystemTests
    {
        [Fact]
        public void Land_ShouldNotBeTappedWhenPlayed()
        {
            // Arrange
            var land = new Land(LandType.Forest);
            var player = new Player();
            player.Hand.Add(land);

            // Act
            player.PlayLand(land);

            // Assert
            land.IsTapped.Should().BeFalse();
            player.Battlefield.Should().Contain(land);
        }

        [Fact]
        public void Player_ShouldTapLandsWhenCastingCreature()
        {
            // Arrange
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });
            var player = game.GetCurrentPlayer();
            var land = new Land(LandType.Forest);
            player.Hand.Add(land);
            player.PlayLand(land);
            var creature = new Creature("Test Creature", new[] { Mana.Green }, 1, 1);
            player.Hand.Add(creature);

            // Act
            game.CastCreature(player, creature);

            // Assert
            land.IsTapped.Should().BeTrue();
            player.Battlefield.Should().Contain(creature);
            player.Hand.Should().NotContain(creature);
        }

        [Fact]
        public void Player_ShouldNotSpendManaWhenInsufficientFunds()
        {
            // Arrange
            var player = new Player();
            var land = new Land(LandType.Forest);
            player.Hand.Add(land);
            player.PlayLand(land);
            var manaCost = new[] { Mana.Green, Mana.Green };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => player.ManaPool.Spend(manaCost));
        }

        [Fact]
        public void Game_ShouldUntapLandsAtBeginningOfTurn()
        {
            // Arrange
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });
            var player = game.GetCurrentPlayer();
            var land = new Land(LandType.Forest);
            player.Hand.Add(land);
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
            var game = new Game(new List<Player>() { new Player("Bob"), new Player("AI") });
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
            var game = new Game(new List<Player>()
            {
                new Player("Bob"),
                new Player("AI")
            });
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

        [Fact]
        public void AnyManaShouldBeAbleToPayForColorless()
        {
            var game = new Game(new List<Player>() {
                new Player("Bob"),
                new Player("AI")
            });
            var land = new Land(LandType.Forest);
            game.Players[0].Hand.Add(land);
            game.Players[0].PlayLand(land);
            game.SwitchTurn();
            var land2 = new Land(LandType.Forest);
            game.Players[0].Hand.Add(land2);
            game.PlayLand(game.Players[0], land2);
            var creature = new Creature("T", new[] { Mana.Green, Mana.Colorless }, 2, 2);
            game.Players[0].Hand.Add(creature);
            game.CastCreature(game.Players[0], creature);
            game.Players[0].Battlefield.Should().HaveCount(3);
        }

        // Add more tests for the mana system as needed
    }
}