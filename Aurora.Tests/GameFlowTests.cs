using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.DTO;
using Aurora.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
namespace Aurora.Tests
{
    public class GameFlowTests
    {
        [Fact]
        public void CardShouldLeaveHandWhenPlayed()
        {
            var logger = new Mock<ILogger<GameService>>();
            IGameService gs = new GameService(logger.Object);
            var gamedto = gs.CreateGame("bob");
            LandDTO land = (LandDTO)gamedto.Players[0].Hand.FirstOrDefault(c => c.Type == "Land");
            var handsizeForPLay = gamedto.Players[0].Hand.Count;
            gamedto = gs.PlayLand(gamedto.Id, gamedto.Players[0].Id, land);
            var handsizeAfterPlay = gamedto.Players[0].Hand.Count;
            handsizeAfterPlay.Should().BeLessThan(handsizeForPLay);

        }

    }
}
