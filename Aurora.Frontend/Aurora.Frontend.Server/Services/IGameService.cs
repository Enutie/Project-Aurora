namespace Aurora.Server.Services
{
    public interface IGameService
    {
        Game CreateGame(string playerName);
        Game GetGame(string gameId);
        Game PlayLand(string gameId, string playerId, int landIndex);
        Game CastCreature(string gameId, string playerId, int creatureIndex);
        Game Attack(string gameId, string attackerId, string defenderId, List<string> attackingCreatureIds);
        Game EndTurn(string gameId);
    }
}
