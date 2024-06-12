using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Aurora.DTO
{
    public class GameDTO
    {
        public string Id { get; set; }
        public List<PlayerDTO> Players { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public bool IsGameOver { get; set; }
        public PlayerDTO Winner { get; set; }
        public string CurrentPhase { get; set; }
    }
}
