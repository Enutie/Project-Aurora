using Xunit;
using FluentAssertions;

namespace Aurora.Tests
{
    public class CreatureTests
    {
        
        [Fact]
        public void Creature_ShouldHaveValidProperties()
        {
            // Arrange
            var creatureName = "Grizzly Bears";
            var manaCost = new[] { Mana.Green, Mana.Green };
            var power = 2;
            var toughness = 2;

            // Act
            var creature = new Creature(creatureName, manaCost, power, toughness);

            // Assert
            creature.Name.Should().Be(creatureName);
            creature.ManaCost.Should().BeEquivalentTo(manaCost);
            creature.Power.Should().Be(power);
            creature.Toughness.Should().Be(toughness);
        }

        [Fact]
        public void Creature_ShouldBeCastFromHand()
        {
            // Arrange
            var game = new Game(Helper.GetDeck());
            var player = game.Players[0];
            var creature = new Creature("Devoted Hero", new[] { Mana.White }, 1, 2);
            var land = new Land(LandType.Plains);
            player.Hand.Add(land);
            player.Hand.Add(creature);

            // Act
            player.PlayLand(land);
            game.CastCreature(player, creature);

            // Assert
            player.Hand.Should().NotContain(creature);
            player.Battlefield.Should().Contain(creature);
        }

        // Add more tests for creatures as needed
    }
}