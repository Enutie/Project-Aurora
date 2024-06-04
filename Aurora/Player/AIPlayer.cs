using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class AIPlayer : Player
    {
        public AIPlayer(string name, List<Card> library) : base(name, library)
        {
        }

        public override void PassTurn()
        {
            var land = Hand.FirstOrDefault();
            Play(land);

        }
    }
}
