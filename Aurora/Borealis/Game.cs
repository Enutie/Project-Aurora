namespace Borealis
{
    public class Game
    {
        public bool IsRunning { get; private set; }
        public List<int> Players { get; private set; }

        public void StartGame()
        {
            IsRunning = true;
            Players = new List<int>();
            Players.Add(1);
            Players.Add(2);
        }

        public void EndGame()
        {
            IsRunning = false;
        }

    }

    
}