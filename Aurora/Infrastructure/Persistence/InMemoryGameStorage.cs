using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Domain.Entities;
using Aurora.Infrastructure.Interfaces;

namespace Aurora.Infrastructure.Persistence
{
    public class InMemoryGameStorage : IGameStorage
    {
        private readonly Dictionary<string, Game> _games = new Dictionary<string, Game>();
        public void AddGame(Game game)
        {
            _games[game.GetGameState().Id] = game;
        }

        public IEnumerable<Game> GetAllGames()
        {
            return _games.Values;
        }

        public Game GetGame(string Id)
        {
            return _games[Id];
        }

        public void RemoveGame(string Id)
        {
            _games.Remove(Id);
        }

        public bool TryGetGame(string Id, out Game game)
        {
            return _games.TryGetValue(Id, out game);
        }
    }
}
