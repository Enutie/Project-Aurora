using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Borealis.Tests
{
    public class CardTests
    {
        [Fact]
        public void Card_HasCorrectProperties()
        {
            // Arrange
            string name = "Grizzly Bears";
            int manaCost = 2;
            int power = 2;
            int toughness = 2;

            // Act
            Card card = new Card(name, manaCost, power, toughness);

            // Assert
            card.Name.Should().Be(name);
            card.ManaCost.Should().Be(manaCost);
            card.Power.Should().Be(power);
            card.Toughness.Should().Be(toughness);
        }
    }
}
