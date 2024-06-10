using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis.Models
{
    public class Card
    {
        public string Name { get; set; }
        public int ManaCost { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public bool IsLand { get; set; }

        public Card(string _name, int _manaCost, int _power, int _thoughness, bool _isLand)
        {
            Name = _name;
            ManaCost = _manaCost;
            Power = _power;
            Toughness = _thoughness;
            IsLand = _isLand;
        }
    }
}
