using Xunit;
using FluentAssertions;
using Aurora.Exceptions;

namespace Aurora.Tests
{
    public class ManaCostTests
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
            game.AdvanceToNextPhase();
            game.PlayLand(player,land);
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
            game.AdvanceToNextPhase();
            Assert.Throws<InvalidMoveException>(() => game.CastCreature(player, creature));
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
            game.AdvanceToNextPhase();
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
            // Turn one
            var land = new Land(LandType.Forest);
            game.Players[0].Hand.Add(land);
            game.StartMainPhase1();
            game.Players[0].PlayLand(land);
            if (game.SwitchTurn())
            {
                game.AssignBlockers(game.Players[0], []);
                game.SwitchTurn();
            }


            // Turn two
            var land2 = new Land(LandType.Forest);
            game.Players[0].Hand.Add(land2);
            game.StartMainPhase1();
            game.PlayLand(game.Players[0], land2);
            var creature = new Creature("T", new[] { Mana.Green, Mana.Colorless }, 2, 2);
            game.Players[0].Hand.Add(creature);
            game.CastCreature(game.Players[0], creature);
            game.Players[0].Battlefield.Should().HaveCount(3);
        }

        [Fact]
        public void Player_ShouldPlayTwoCreaturesWithDifferentManaCosts()
        {
            // Arrange
            var game = new Game(new List<Player>()
    {
        new Player("Bob"),
        new Player("AI")
    });
            var player = game.GetCurrentPlayer();

            for (int i = 0; i < 4; i++)
            {
                var land = new Land(LandType.Forest);
                player.Hand.Add(land);
                game.AdvanceToNextPhase();
                game.PlayLand(player, land);
                if (game.SwitchTurn())
                {
                    game.AssignBlockers(game.Players[0], []);
                    game.SwitchTurn();
                }
            }

            var creature1 = new Creature("Creature 1", new[] { Mana.Green, Mana.Colorless }, 1, 1);
            var creature2 = new Creature("Creature 2", new[] { Mana.Green, Mana.Green }, 2, 2);
            player.Hand.Add(creature1);
            player.Hand.Add(creature2);

            // Act
            game.AdvanceToNextPhase();
            game.CastCreature(player, creature1);
            game.CastCreature(player, creature2);

            // Assert
            player.Battlefield.Should().Contain(creature1);
            player.Battlefield.Should().Contain(creature2);
            player.Hand.Should().NotContain(creature1);
            player.Hand.Should().NotContain(creature2);
            player.Battlefield.OfType<Land>().Count(l => l.IsTapped).Should().Be(4);
        }

        [Fact]
        public void Player_CannotPlayCreatureWithInsufficientMana()
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
            game.AdvanceToNextPhase();
            game.PlayLand(player, land);

            var creature = new Creature("Expensive Creature", new[] { Mana.Green, Mana.Green, Mana.Colorless }, 3, 3);
            player.Hand.Add(creature);

            // Act & Assert
            Assert.Throws<InvalidMoveException>(() => game.CastCreature(player, creature));
            player.Battlefield.Should().NotContain(creature);
            player.Hand.Should().Contain(creature);
        }

        [Fact]
        public void Player_CannotPlayCreatureNotInHand()
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
            game.AdvanceToNextPhase();
            game.PlayLand(player, land);

            var creature = new Creature("Phantom Creature", new[] { Mana.Green }, 1, 1);

            // Act & Assert
            Assert.Throws<InvalidMoveException>(() => game.CastCreature(player, creature));
            player.Battlefield.Should().NotContain(creature);
            player.Hand.Should().NotContain(creature);
        }

        [Fact]
        public void Player_CannotCastCreatureWithWrongMana()
        {
            // Arrange
            var game = new Game(new List<Player>()
    {
        new Player("Bob"),
        new Player("AI")
    });
            var player = game.GetCurrentPlayer();

            for (int i = 0; i < 2; i++)
            {
                var land = new Land(LandType.Forest);
                player.Hand.Add(land);
                game.AdvanceToNextPhase();
                game.PlayLand(player, land);
                if (game.SwitchTurn())
                {
                    game.AssignBlockers(game.Players[0], []);
                    game.SwitchTurn();
                }
            }

            var creature = new Creature("Mixed Mana Creature", new[] { Mana.Green, Mana.Red }, 2, 2);
            player.Hand.Add(creature);

            // Act & Assert
            game.AdvanceToNextPhase();
            Assert.Throws<InvalidMoveException>(() => game.CastCreature(player, creature));
            player.Battlefield.Should().NotContain(creature);
            player.Hand.Should().Contain(creature);
            player.Battlefield.OfType<Land>().Count(l => l.IsTapped).Should().Be(0);
        }

        [Fact]
        public void Player_CanCastCreatureWithSpecificAndColorlessMana()
        {
            // Arrange
            var game = new Game(new List<Player>()
    {
        new Player("Bob"),
        new Player("AI")
    });
            var player = game.GetCurrentPlayer();

            for (int i = 0; i < 4; i++)
            {
                var land = new Land(LandType.Forest);
                player.Hand.Add(land);
                game.AdvanceToNextPhase();
                game.PlayLand(player, land);
                if (game.SwitchTurn())
                {
                    game.AssignBlockers(game.Players[0], []);
                    game.SwitchTurn();
                }
            }

            var creature = new Creature("Specific and Colorless Mana Creature", new[] { Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 3, 3);
            player.Hand.Add(creature);

            // Act
            game.AdvanceToNextPhase();
            game.CastCreature(player, creature);

            // Assert
            player.Battlefield.Should().Contain(creature);
            player.Hand.Should().NotContain(creature);
            player.Battlefield.OfType<Land>().Count(l => l.IsTapped).Should().Be(4);
        }

        [Fact]
        public void Player_CanCastCreatureWithColorlessMana()
        {
            // Arrange
            var game = new Game(new List<Player>()
    {
        new Player("Bob"),
        new Player("AI")
    });
            var player = game.GetCurrentPlayer();

            for (int i = 0; i < 4; i++)
            {
                var land = new Land(LandType.Forest);
                game.StartMainPhase1();
                player.Hand.Add(land);
                game.PlayLand(player, land);
                if (game.SwitchTurn())
                {
                    game.AssignBlockers(game.Players[0], []);
                    game.SwitchTurn();
                }
            }

            var creature = new Creature("Colorless Mana Creature", new[] { Mana.Colorless, Mana.Colorless }, 3, 3);
            player.Hand.Add(creature);

            // Act
            game.AdvanceToNextPhase();
            game.CastCreature(player, creature);

            // Assert
            player.Battlefield.Should().Contain(creature);
            player.Hand.Should().NotContain(creature);
            player.Battlefield.OfType<Land>().Count(l => l.IsTapped).Should().Be(2);
        }

        [Fact]
        public void Player_CanCastCreatureWithColorlessManaEvenThoughTheyDidntPlayALandThatTurn()
        {
            // Arrange
            var game = new Game(new List<Player>()
    {
        new Player("Bob"),
        new Player("AI")
    });
            var player = game.GetCurrentPlayer();

            for (int i = 0; i < 2; i++)
            {
                var land = new Land(LandType.Forest);
                player.Hand.Add(land);
                game.AdvanceToNextPhase();
                game.PlayLand(player, land);
                if (game.SwitchTurn())
                {
                    game.AssignBlockers(game.Players[0], []);
                    game.SwitchTurn();
                }
            }
            game.SwitchTurn();

            var creature = new Creature("Colorless Mana Creature", new[] { Mana.Colorless, Mana.Colorless }, 3, 3);
            player.Hand.Add(creature);

            // Act
            game.AdvanceToNextPhase();
            game.CastCreature(player, creature);

            // Assert
            player.Battlefield.Should().Contain(creature);
            player.Hand.Should().NotContain(creature);
            player.Battlefield.OfType<Land>().Count(l => l.IsTapped).Should().Be(2);
        }
    }
}