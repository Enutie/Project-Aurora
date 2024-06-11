using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
    public interface IGameStorage
    {
        void AddGame(Game game);
        Game GetGame(string Id);
        bool TryGetGame(string Id, out Game game);
        void RemoveGame(string Id);
        IEnumerable<Game> GetAllGames();
    }
}
