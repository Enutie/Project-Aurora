namespace Borealis
{
    public class Game
    {
        public bool IsRunning { get; private set; }

        public void StartGame()
        {
            IsRunning = true;
        }

        public void EndGame()
        {
            IsRunning = false;
        }

    }

    
}