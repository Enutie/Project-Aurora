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

        // Add more test cases as needed
    }
}