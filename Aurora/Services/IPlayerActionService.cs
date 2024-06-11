using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
	public interface IPlayerActionService
	{
		GameDTO PlayLand(string gameId, string playerId, LandDTO landDTO);
		GameDTO CastCreature(string gameId, string playerId, CreatureDTO creatureDTO);
		GameDTO Attack(string gameId, string attackingPlayerId, List<string> attackingCreatureIds);
		GameDTO AssignBlockers(string gameId, string defendingPlayerId, Dictionary<string, string> blockerAssignments);
	}
}
