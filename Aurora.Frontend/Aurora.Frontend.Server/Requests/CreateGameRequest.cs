namespace Aurora.Server.Requests
{
    public class CreateGameRequest
    {
        public string PlayerName { get; set; }
    }

    public class CreateGameResponse
    {
        public string GameId { get; set; }
        public IEnumerable<PlayerResponse> Players { get; set; }
        public string CurrentPlayer { get; set; }
    }

    public class GameStateResponse
    {
        public string GameId { get; set; }
        public IEnumerable<PlayerResponse> Players { get; set; }
        public string CurrentPlayer { get; set; }
    }

    public class PlayLandRequest
    {
        public string PlayerId { get; set; }
        public int LandIndex { get; set; }
    }

    public class PlayerResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int HandCount { get; set; }
        public int BattlefieldCount { get; set; }
    }
}
