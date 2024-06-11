using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
    public interface IGameService
    {
        GameDTO CreateGame(string playerName);
        GameDTO StartGame(List<PlayerDTO> playerDTOs);
        GameDTO PlayLand(string gameId, string playerId, LandDTO landDTO);
        GameDTO CastCreature(string gameId, string playerId, CreatureDTO creatureDTO);
        GameDTO Attack(string gameId, string attackingPlayerId, List<string> attackingCreatureIds);
        GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments);
        GameDTO SwitchTurn(string gameId);
        GameDTO GetGameState(string gameId);
        PlayerDTO GetPlayerInfo(string gameId, string playerId);
        LandDTO GetLandById(string landId);
        CreatureDTO GetCreatureById(string creatureId);
    }
}
