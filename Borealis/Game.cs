using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis
{
    public class Game
    {
        public IList<Player> Players { get; }

        public Game(Player player1, Player player2)
        {
            Players = new List<Player> { player1, player2 };
        }

        public void Start()
        {
            foreach (Player player in Players)
            {
                DrawInitialHand(player);
            }
        }

        private void DrawInitialHand(Player player)
        {
            int cardsToDrawCount = Math.Min(player.Library.Count(), 7);
            for (int i = 0; i < cardsToDrawCount; i++)
            {
                player.DrawCard();
            }
        }

        public void PerformTurn(Player player)
        {
            player.DrawCard();
            // Add logic for playing cards, attacking, etc.
        }

        public Player CheckWinCondition()
        {
            Player winner = null;

            foreach (Player player in Players)
            {
                if (player.Life <= 0)
                {
                    winner = Players.First(p => p != player);
                    break;
                }
            }

            return winner;
        }
    }
}
