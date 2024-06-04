using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public Game() 
        {
            Players = new List<Player>
            {
                new Player("PlayerOne", BasicLandDeck()),
                new AIPlayer("PlayerAI", BasicLandDeck())
            };
            StartGame();
        }

        public void StartGame()
        {
            foreach (Player p in Players)
            {
                p.DrawStartingHand();
            }
        }

        private List<Card> BasicLandDeck()
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 60; i++)
            {
                deck.Add(new Card());
            }
            return deck;
        }
    }
}
