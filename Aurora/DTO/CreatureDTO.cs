using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.DTO
{
    public class CreatureDTO : CardDTO
    {
        public List<Mana> ManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsBlocked { get; set; }
        public CreatureDTO BlockedBy { get; set; }
    }
}
