using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
	public interface IGameManager
	{
		GameDTO CreateGame(string playerName);
		GameDTO StartGame(List<PlayerDTO> playerDTOs);
		GameDTO GetGameState(string gameId);
		GameDTO SwitchTurn(string gameId);
	}
}
