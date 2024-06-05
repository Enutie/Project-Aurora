using FluentAssertions;

namespace Aurora.Tests
{
    public class CardTests
    {
        [Fact]
        public void Card_ShouldHaveValidName()
        {
            var cardName = "Plains";

            var card = new Card(cardName);

            card.Name.Should().Be(cardName);
        }

        [Fact]
        public void Land_ShouldHaveValidType()
        {
            var landType = LandType.Plains;

            var land = new Land(landType);

            land.Type.Should().Be(landType);
        }
    }
}
