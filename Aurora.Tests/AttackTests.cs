using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class AttackTests
    {
        [Fact]
        public void Attack_ShouldDealDamageToDefendingPlayer_WhenAttackerIsUnblocked()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var attacker = game.Players[0];
            var defender = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            attacker.Battlefield.Add(attackingCreature);

            // Act
            game.Attack(attacker, defender, new List<Creature> { attackingCreature });

            // Assert
            defender.Life.Should().Be(17);
        }

        [Fact]
        public void Attack_ShouldDealDamageToBothCreatures_WhenAttackerIsBlocked()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var attacker = game.Players[0];
            var defender = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            var blockingCreature = new Creature("Blocker", new[] { Mana.White }, 2, 4);
            attacker.Battlefield.Add(attackingCreature);
            defender.Battlefield.Add(blockingCreature);

            // Act
            game.Attack(attacker, defender, new List<Creature> { attackingCreature });

            // Assert
            attackingCreature.Toughness.Should().Be(1);
            blockingCreature.Toughness.Should().Be(1);
        }

        [Fact]
        public void Attack_ShouldClearAttackingAndBlockingStatus_AfterCombatResolution()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var attacker = game.Players[0];
            var defender = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            var blockingCreature = new Creature("Blocker", new[] { Mana.White }, 2, 4);
            attacker.Battlefield.Add(attackingCreature);
            defender.Battlefield.Add(blockingCreature);

            // Act
            game.Attack(attacker, defender, new List<Creature> { attackingCreature });

            // Assert
            attackingCreature.IsAttacking.Should().BeFalse();
            attackingCreature.IsBlocked.Should().BeFalse();
            attackingCreature.BlockedBy.Should().BeNull();
        }

        [Fact]
        public void Attack_ShouldThrowException_WhenPlayerAttacksMultipleTimesInSameTurn()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var attacker = game.Players[0];
            var defender = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            attacker.Battlefield.Add(attackingCreature);

            // Act & Assert
            game.Attack(attacker, defender, new List<Creature> { attackingCreature });
            Assert.Throws<InvalidOperationException>(() => game.Attack(attacker, defender, new List<Creature> { attackingCreature }));
        }

        [Fact]
        public void SwitchTurn_ShouldResetHasAttackedFlag()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player1 = game.Players[0];
            var player2 = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            player1.Battlefield.Add(attackingCreature);

            // Act
            game.Attack(player1, player2, new List<Creature> { attackingCreature });
            game.SwitchTurn();

            // Assert
            var exception = Record.Exception(() => game.Attack(player2, player1, new List<Creature>()));
            exception.Should().BeNull();
        }

        [Fact]
        public void Attack_ShouldNotAllowAttackingWithOpponentsCreatures()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player1 = game.Players[0];
            var player2 = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);
            player2.Battlefield.Add(attackingCreature);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => game.Attack(player1, player2, new List<Creature> { attackingCreature }));
        }

        [Fact]
        public void Attack_ShouldNotAllowAttackingWithCreaturesNotOnBattlefield()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var attacker = game.Players[0];
            var defender = game.Players[1];
            var attackingCreature = new Creature("Attacker", new[] { Mana.Green }, 3, 3);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => game.Attack(attacker, defender, new List<Creature> { attackingCreature }));
        }
    }
}