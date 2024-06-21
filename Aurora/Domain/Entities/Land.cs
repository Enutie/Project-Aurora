using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Domain.Enums;

namespace Aurora.Domain.Entities
{

    public class Land : Card
    {
        public LandType Type { get; }
        public Mana ProducedMana => Type switch
        {
            LandType.Plains => Mana.White,
            LandType.Island => Mana.Blue,
            LandType.Swamp => Mana.Black,
            LandType.Mountain => Mana.Red,
            LandType.Forest => Mana.Green,
            _ => throw new ArgumentOutOfRangeException(nameof(Type))
        };

        public Land(LandType type) : base(type.ToString())
        {
            Type = type;
        }
    }
}
