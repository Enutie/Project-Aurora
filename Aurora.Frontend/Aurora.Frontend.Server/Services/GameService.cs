namespace Aurora.Server.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>();

        public Game CreateGame(string playerName)
        {
            var landCounts = new[]
            {
            (LandType.Plains, 10),
            (LandType.Island, 10),
            (LandType.Swamp, 10),
            (LandType.Mountain, 10),
            (LandType.Forest, 10)
        };
            var game = new Game(landCounts);
            game.Players[0].Name = playerName;
            game.Players[1].Name = "AI";
            var playerOneId = Guid.NewGuid().ToString();
            game.Players[0].Id = playerOneId;
            var playerTwoId = Guid.NewGuid().ToString();
            game.Players[1].Id = playerTwoId;
            var gameId = Guid.NewGuid().ToString();
            game.Id = gameId;
            _games[gameId] = game;
            return game;
        }

        public Game GetGame(string gameId)
        {
            if (_games.TryGetValue(gameId, out var game))
            {
                return game;
            }
            throw new ArgumentException("Game not found.", nameof(gameId));
        }

        public Game PlayLand(string gameId, string playerId, int landIndex)
        {
            var game = GetGame(gameId);
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                throw new ArgumentException("Player not found.", nameof(playerId));
            }
            if (landIndex < 0 || landIndex >= player.Hand.Count)
            {
                throw new ArgumentException("Invalid land index.", nameof(landIndex));
            }
            var land = player.Hand[landIndex] as Land;
            if (land == null)
            {
                throw new ArgumentException("Selected card is not a land.", nameof(landIndex));
            }
            game.PlayLand(player, land);
            return game;
        }

        public Game AIOpponentPlay(string gameId)
        {
            var game = GetGame(gameId);
            game.TakeAITurn();
            return game;
        }
    }
}
