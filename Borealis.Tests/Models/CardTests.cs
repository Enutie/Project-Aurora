using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Borealis.Models;
using FluentAssertions;

namespace Borealis.Tests.Models
{
    public class CardTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCardWithDefaultValues()
        {
            // Arrange
            var expectedName = "Grizzly Bears";
            var expectedManaCost = 2;
            var expectedPower = 2;
            var expectedToughness = 2;
            var expectedIsLand = false;

            // Act
            var card = new Card(expectedName, expectedManaCost, expectedPower, expectedToughness, expectedIsLand);

            // Assert
            card.Name.Should().Be(expectedName);
            card.ManaCost.Should().Be(expectedManaCost);
            card.Power.Should().Be(expectedPower);
            card.Toughness.Should().Be(expectedToughness);
            card.IsLand.Should().Be(expectedIsLand);
        }
    }
}
