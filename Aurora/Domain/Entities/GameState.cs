using System;
using System.Collections.Generic;
using Aurora.Application.DTO;
using Aurora.Domain.Enums;

namespace Aurora.Domain.Entities
{
    public class GameState
    {
        public List<Player> Players { get; }
        public int CurrentPlayerIndex { get; set; } = 0;
        public string Id { get; } = Guid.NewGuid().ToString();
        public bool IsGameOver { get; set; }
        public PlayerDTO Winner { get; set; }
        public Phase CurrentPhase { get; set; }
        public Dictionary<string, int> LandsPlayedThisTurn { get; } = new Dictionary<string, int>();
        public bool HasAttackedThisTurn { get; set; }

        public GameState(List<Player> players)
        {
            Players = players;
            foreach (var player in Players)
            {
                LandsPlayedThisTurn[player.Id] = 0;
            }
        }

        public Player GetCurrentPlayer() => Players[CurrentPlayerIndex];
        public string GetCurrentPlayerId() => GetCurrentPlayer().Id;
        public Player GetPlayerById(string id) => Players.Find(p => p.Id == id);
    }
}