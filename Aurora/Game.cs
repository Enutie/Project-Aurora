using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Game
    {
        public List<Player> Players { get; } = new List<Player>();
        private int currentPlayerIndex = 0;
        public string Id { get; set; }
        private Dictionary<Player, int> _landsPlayedThisTurn = new Dictionary<Player, int>();

        public Game(IEnumerable<(LandType Type, int Count)> landCounts)
        {
            Players = new List<Player>();

            var deck1 = new Deck(landCounts);
            deck1.Shuffle();
            var player1 = new Player { Deck = deck1 };
            DrawStartingHand(player1);
            Players.Add(player1);

            var deck2 = new Deck(landCounts);
            deck2.Shuffle();
            var player2 = new Player { Deck = deck2 };
            DrawStartingHand(player2);
            Players.Add(player2);
        }

        private void DrawStartingHand(Player player)
        {
            for (int i = 0; i < 7; i++)
            {
                player.DrawCard();
            }
        }

        public Player GetCurrentPlayer()
        {
            return Players[currentPlayerIndex];
        }

        public void SwitchTurn()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
            _landsPlayedThisTurn[GetCurrentPlayer()] = 0;
        }

        public bool CanPlayLand(Player player)
        {
            if (!_landsPlayedThisTurn.ContainsKey(player))
            {
                _landsPlayedThisTurn[player] = 0;
            }
            return _landsPlayedThisTurn[player] == 0;
        }

        public void PlayLand(Player player, Land land)
        {
            if (GetCurrentPlayer() == player && CanPlayLand(player))
            {
                player.Hand.Remove(land);
                player.Battlefield.Add(land);
                _landsPlayedThisTurn[player]++;
                SwitchTurn();
            }
            else
            {
                throw new InvalidOperationException("Player cannot play a land at this time.");
            }
        }

        public void TakeAITurn()
        {
            Player aiPlayer = Players[1];
            if (GetCurrentPlayer() == aiPlayer)
            {

                aiPlayer.DrawCard();

                if (aiPlayer.Hand.OfType<Land>().Any())
                {
                    Land landToPlay = aiPlayer.Hand.OfType<Land>().FirstOrDefault();

                    if (landToPlay != null && CanPlayLand(aiPlayer))
                    {
                        PlayLand(aiPlayer, landToPlay);
                        return;
                    }
                }

                SwitchTurn();
            }
            else
            {
                throw new InvalidOperationException($"Its not the {aiPlayer.Name}'s Turn");
            }
        }
    }
}
