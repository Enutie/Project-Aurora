using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            game.DeclareAttackers(attackingPlayer, new List<Creature> { attackingCreature });
            game.AssignBlockers(defendingPlayer, new Dictionary<Creature, Creature>
    {
        { attackingCreature, blockingCreature }
    });

            // Assert
            defendingPlayer.Battlefield.Should().NotContain(blockingCreature);
            defendingPlayer.Graveyard.Should().Contain(blockingCreature);
        }
    }
}
