using Aurora.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Application.Services
{
    public interface IGameManager
    {
        GameDTO CreateGame(string playerName);
        GameDTO StartGame(List<PlayerDTO> playerDTOs);
        GameDTO GetGameState(string gameId);
        GameDTO AdvanceToNextPhase(string gameId);

        GameDTO SwitchTurn(string gameId);
    }
}
