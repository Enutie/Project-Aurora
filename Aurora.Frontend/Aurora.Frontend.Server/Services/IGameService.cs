namespace Aurora.Server.Services
{
    public interface IGameService
    {
        Game CreateGame(string playerName);
        Game GetGame(string gameId);
        Game PlayLand(string gameId, string playerId, int landIndex);
        Game AIOpponentPlay(string gameId);
    }
}
