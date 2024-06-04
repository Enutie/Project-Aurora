using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Aurora.Tests
{
    public class GameTests
    {
        [Fact]
        public void PlayerStartWith7CardHandAnd53Deck()
        {
            Game game = new Game();

            game.Players[0].Hand.Should().HaveCount(7);
            game.Players[0].Library.Should().HaveCount(53);
        }

        [Fact]
        public void PlayerCanPlayCard()
        {
            var game = new Game();

            var card = game.Players[0].Hand.First();
            game.Players[0].Play(card);
            game.Players[0].Hand.Should().HaveCount(6);
            game.Players[0].Battlefield.Should().HaveCount(1);
        }

        [Fact]
        public void AIWIllPlayCard()
        {
            var game = new Game();

            game.Players[0].PassTurn();
            game.Players[1].PassTurn();
            game.Players[1].Hand.Should().HaveCount(6);
            game.Players[1].Battlefield.Should().HaveCount(1);
        }

    }
}
