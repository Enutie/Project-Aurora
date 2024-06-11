using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public enum Mana
    {
        White, Blue, Black, Red, Green, Colorless
    }


    public class ManaPool
    {
        public Dictionary<Mana, int> _mana = new Dictionary<Mana, int>();
        public List<Land> LandsUsed { get; } = new List<Land>();


        public void Add(Mana mana, Land land)
        {
            if (_mana.ContainsKey(mana))
            {
                _mana[mana]++;
            }
            else
            {
                _mana[mana] = 1;
            }
            LandsUsed.Add(land);
        }

        public bool CanAfford(IEnumerable<Mana> cost)
        {
            var remainingCost = cost.GroupBy(m => m).ToDictionary(g => g.Key, g => g.Count());

            // Calculate the total required colorless mana
            int colorlessCost = remainingCost.ContainsKey(Mana.Colorless) ? remainingCost[Mana.Colorless] : 0;
            remainingCost.Remove(Mana.Colorless);

            foreach (var (mana, count) in _mana)
            {
                if (remainingCost.ContainsKey(mana))
                {
                    int costCount = remainingCost[mana];
                    if (count >= costCount)
                    {
                        remainingCost.Remove(mana);
                    }
                    else
                    {
                        remainingCost[mana] -= count;
                    }
                }

                // If there is any remaining count after paying specific mana, use it for colorless mana
                if (colorlessCost > 0)
                {
                    if (count > 0)
                    {
                        colorlessCost -= count;
                        if (colorlessCost < 0)
                        {
                            colorlessCost = 0;
                        }
                    }
                }
            }

            return !remainingCost.Any() && colorlessCost == 0;
        }


        public void Spend(IEnumerable<Mana> cost)
        {
            if (!CanAfford(cost))
            {
                throw new InvalidOperationException("Insufficient mana to spend.");
            }

            foreach (var mana in cost)
            {
                _mana[mana]--;
                if (_mana[mana] == 0)
                {
                    _mana.Remove(mana);
                }
            }
        }

        public void Clear()
        {
            _mana.Clear();
            LandsUsed.Clear();
        }
    }
}
