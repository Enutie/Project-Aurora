using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis
{
    public class Card
    {
        public string Name { get; }
        public int ManaCost { get; }
        public int Power { get; }
        public int Toughness { get; }

        public Card(string name, int manaCost, int power, int toughness)
        {
            Name = name;
            ManaCost = manaCost;
            Power = power;
            Toughness = toughness;
        }
    }
}
