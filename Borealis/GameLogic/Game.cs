using Borealis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis.GameLogic
{
    public class Game
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentPlayer { get; set; }
        public Player NonCurrentPlayer { get; set; }

        public Game(string _player1, string _player2)
        {
            Player1 = new Player(_player1);
            Player2 = new Player(_player2);
            CurrentPlayer = Player1;
            NonCurrentPlayer = Player2;
        }

        public void PlayGame()
        {
            // Initialize game state
            Player1.DrawOpeningHand();
            Player2.DrawOpeningHand();

            while (true)
            {
                // Player 1's turn
                TakeTurn(Player1, Player2);

                // Check if game has ended
                if (Player2.Life <= 0 || Player2.Library.Count == 0)
                {
                    break;
                }

                // Swap current and non-current players
                SwapPlayers();

                // Player 2's turn
                TakeTurn(Player2, Player1);

                // Check if game has ended
                if (Player1.Life <= 0 || Player1.Library.Count == 0)
                {
                    break;
                }

                SwapPlayers();
            }
        }

        private void TakeTurn(Player currentPlayer, Player nonCurrentPlayer)
        {
            // Draw a card from the library
            currentPlayer.DrawCard();

            // Play lands (not implemented yet)
            // ...

            // Summon creatures from hand
            SummonCreaturesFromHand(currentPlayer);

            // Combat phase
            foreach (var attacker in currentPlayer.Battlefield.Where(c => !c.IsLand))
            {
                // Deal damage to the non-current player
                nonCurrentPlayer.Life -= attacker.Power;
            }
        }

        private void SummonCreaturesFromHand(Player player)
        {
            var creaturesToSummon = player.Hand.Where(c => !c.IsLand && c.ManaCost <= player.AvailableMana).ToList();
            foreach (var creature in creaturesToSummon)
            {
                player.Hand.Remove(creature);
                player.Battlefield.Add(creature);
            }
        }

        private void SwapPlayers()
        {
            CurrentPlayer = NonCurrentPlayer;
            NonCurrentPlayer = Player1 == CurrentPlayer ? Player2 : Player1;
        }
    }
}
