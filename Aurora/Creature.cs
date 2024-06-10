using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public class Creature : Card
    {
        public IEnumerable<Mana> ManaCost { get; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsBlocked { get; set; }
        public Creature BlockedBy { get; set; }

        public void DealDamage(Creature target)
        {
            target.Toughness -= Power;
        }

        public Creature(string name, IEnumerable<Mana> manaCost, int power, int toughness)
            : base(name)
        {
            ManaCost = manaCost;
            Power = power;
            Toughness = toughness;
        }
        
    }
}
