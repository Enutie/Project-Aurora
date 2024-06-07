namespace Aurora.Server.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>();
        private Deck CreateDeck()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 24; i++)
            {
                deck.Add(new Land(LandType.Plains)
                {
                    Id = Guid.NewGuid().ToString(),
                });
            }
            for (int i = 0; i < 36; i++)
            {
                deck.Add(new Creature("Devoted Hero", new[] { Mana.White }, 1, 2)
                {
                    Id = Guid.NewGuid().ToString(),
                });
            }
            return new Deck(deck);
        }

        public Game CreateGame(string playerName)
        {
            
            var game = new Game(CreateDeck());
            game.Players[0].Name = playerName;
            game.Players[1].Name = "AI";
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
            var player = game.Players.First(p => p.Id == playerId);
            var land = player.Hand[landIndex] as Land;
            game.PlayLand(player, land);
            return game;
        }

        public Game CastCreature(string gameId, string playerId, int creatureIndex)
        {
            var game = GetGame(gameId);
            var player = game.Players.First(p => p.Id == playerId);
            var creature = player.Hand[creatureIndex] as Creature;
            game.CastCreature(player, creature);
            return game;
        }

        public Game Attack(string gameId, string attackerId, string defenderId, List<string> attackingCreatureIds)
        {
            var game = GetGame(gameId);
            var attacker = game.Players.First(p => p.Id == attackerId);
            var defender = game.Players.First(p => p.Id == defenderId);
            var attackingCreatures = attackingCreatureIds.Select(id => attacker.Battlefield.First(c => c.Id == id) as Creature).ToList();
            game.Attack(attacker, defender, attackingCreatures);
            return game;
        }

        public Game EndTurn(string gameId)
        {
            var game = GetGame(gameId);
            game.SwitchTurn();
            game.TakeAITurn();
            return game;
        }
    }
}
