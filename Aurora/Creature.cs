using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Creature : Card
    {
        public Creature(string name) : base(name)
        {
        }

        public Creature(string name, IEnumerable<Mana> manaCost,
            int power, int toughness): base(name)
        {
            ManaCost = manaCost;
            Power = power;
            Toughness = toughness;
        }

        public IEnumerable<Mana> ManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public bool IsAttacking { get; set; } = false;
        public bool IsBlocked { get; set; } = false ;
        public Creature BlockedBy { get; set; }

        public void DealDamage(Creature target)
        {
            target.Toughness -= Power;
        }

    }
}
