using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Exceptions;
using FluentAssertions;

namespace Aurora.Tests
{
    public class CombatTests
    {
        [Fact]
        public void BlockingCreature_WithLowerToughness_GetsRemovedFromBattlefield()
        {
            // Arrange
            var attackingPlayer = new Player("Attacking Player");
            var defendingPlayer = new Player("Defending Player");
            var game = new Game(new List<Player> { attackingPlayer, defendingPlayer });

            var attackingCreature = new Creature("Attacking Creature", new List<Mana>(), 3, 3);
            var blockingCreature = new Creature("Blocking Creature", new List<Mana>(), 2, 2);

            attackingPlayer.Battlefield.Add(attackingCreature);
            defendingPlayer.Battlefield.Add(blockingCreature);

            // Act
            game.StartCombatPhase();
            game.DeclareAttackers(attackingPlayer, new List<Creature> { attackingCreature });
            game.AssignBlockers(defendingPlayer, new Dictionary<Creature, Creature>
        {
            { attackingCreature, blockingCreature }
        });

            // Assert
            defendingPlayer.Battlefield.Should().NotContain(blockingCreature);
        }

        [Fact]
        public void BlockingCreature_WithLowerToughness_MovedToGraveyard()
        {
            // Arrange
            var attackingPlayer = new Player("Attacking Player");
            var defendingPlayer = new Player("Defending Player");
            var game = new Game(new List<Player> { attackingPlayer, defendingPlayer });

            var attackingCreature = new Creature("Attacking Creature", new List<Mana>(), 3, 3);
            var blockingCreature = new Creature("Blocking Creature", new List<Mana>(), 2, 2);

            attackingPlayer.Battlefield.Add(attackingCreature);
            defendingPlayer.Battlefield.Add(blockingCreature);

            // Act
            game.StartCombatPhase();
            game.DeclareAttackers(attackingPlayer, new List<Creature> { attackingCreature });
            game.AssignBlockers(defendingPlayer, new Dictionary<Creature, Creature>
    {
        { attackingCreature, blockingCreature }
    });

            // Assert
            defendingPlayer.Battlefield.Should().NotContain(blockingCreature);
            defendingPlayer.Graveyard.Should().Contain(blockingCreature);
        }


        [Fact]
        public void AIDefendingAction_ShouldAssignBlockers()
        {
            // Arrange
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });

            var attackingCreature1 = new Creature("Alice", Enumerable.Empty<Mana>(), 1, 1);
            var attackingCreature2 = new Creature("Bobby", Enumerable.Empty<Mana>(), 2, 2);
            player1.Battlefield.Add(attackingCreature1);
            player1.Battlefield.Add(attackingCreature2);

            var blocker1 = new Creature("Charlie", Enumerable.Empty<Mana>(), 1, 1);
            var blocker2 = new Creature("Daren", Enumerable.Empty<Mana>(), 2, 1);
            player2.Battlefield.Add(blocker1);
            player2.Battlefield.Add(blocker2);

            // Act
            game.StartCombatPhase();
            game.DeclareAttackers(player1, new List<Creature> { attackingCreature1, attackingCreature2 });


            // Assert
            game.CurrentPhase.Should().Be(Phase.Combat);
            player2.Graveyard.Should().Contain(blocker1);
            player2.Graveyard.Should().Contain(blocker2);
            player2.Battlefield.Should().NotContain(blocker1);
            player2.Battlefield.Should().NotContain(blocker2);
            player2.Life.Should().Be(20);
        }

        [Fact]
        public void AIDefendingAction_ShouldThrowInvalidPhaseException_WhenNotInCombatPhase()
        {
            // Arrange
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });
            game.StartMainPhase1();

            // Act & Assert
            Assert.Throws<InvalidPhaseException>(() => game.AssignBlockers(player2, Enumerable.Empty<KeyValuePair<Creature, Creature>>().ToDictionary()));
        }

        [Fact]
        public void AIAttackingAction_ShouldDeclareAttackers()
        {
            // Arrange
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });
            var attackingCreature1 = new Creature("Alice", [], 1, 1);
            var attackingCreature2 = new Creature("Bobby", [], 2, 2);
            player2.Battlefield.Add(attackingCreature1);
            player2.Battlefield.Add(attackingCreature2);
            player2.Hand.Clear();
            // Act
            if (game.SwitchTurn())
            {
                game.AssignBlockers(player1, []);
            }
            player1.Life.Should().Be(17);
        }

        [Fact]
        public void AIAttackingAction_ShouldThrowInvalidPhaseException_WhenNotInCombatPhase()
        {
            // Arrange
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });
            var attackingCreature = new Creature("Alice", [], 1, 1);
            player1.Battlefield.Add(attackingCreature);
            game.StartMainPhase1();
            // Act & Assert
            Assert.Throws<InvalidPhaseException>(() => game.DeclareAttackers(player1, [attackingCreature]));
        }

        [Fact]
        public void AttackingOpponentWhenNoBlockersShouldDamageOpponent()
        {
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });
            var attackingCreature = new Creature("Alice", [], 1, 1);
            player1.Battlefield.Add(attackingCreature);
            game.StartCombatPhase();

            game.DeclareAttackers(player1, [attackingCreature]);
            player2.Life.Should().Be(19);
        }

        [Fact]
        public void AIAttackingWithNoBlockersShouldDamagePlayer()
        {
            var player1 = new Player("Player 1");
            var player2 = new Player("Player 2");
            var game = new Game(new List<Player> { player1, player2 });
            var attackingCreature = new Creature("Alice", [], 1, 1);
            player2.Battlefield.Add(attackingCreature);
            player2.Hand.Clear(); // In case that the ai has a turn one play
            game.SwitchTurn();
            player1.Life.Should().Be(19);
        }
    }
}
