using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
	public interface IGameQueryService
	{
		PlayerDTO GetPlayerInfo(string gameId, string playerId);
		LandDTO GetLandById(string landId);
		CreatureDTO GetCreatureById(string creatureId);
	}
}
