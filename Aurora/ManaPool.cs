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
            int colorlessCost = remainingCost.ContainsKey(Mana.Colorless) ? remainingCost[Mana.Colorless] : 0;

            if (remainingCost.ContainsKey(Mana.Colorless))
            {
                remainingCost.Remove(Mana.Colorless);
            }

            var availableMana = LandsUsed.Where(l => !l.IsTapped).GroupBy(l => l.ProducedMana).ToDictionary(g => g.Key, g => g.Count());

            foreach (var (mana, count) in availableMana)
            {
                if (remainingCost.ContainsKey(mana))
                {
                    int usedMana = Math.Min(count, remainingCost[mana]);
                    remainingCost[mana] -= usedMana;
                    colorlessCost -= count - usedMana;

                    if (remainingCost[mana] <= 0)
                    {
                        remainingCost.Remove(mana);
                    }
                }
                else
                {
                    colorlessCost -= count;
                }
            }

            return !remainingCost.Any() && colorlessCost <= 0;
        }


        public void Spend(IEnumerable<Mana> cost)
        {
            if (!CanAfford(cost))
            {
                throw new InvalidOperationException("Insufficient mana to spend.");
            }

            var remainingCost = cost.GroupBy(m => m).ToDictionary(g => g.Key, g => g.Count());

            foreach (var mana in cost)
            {
                if (mana == Mana.Colorless)
                    continue;

                var landToTap = LandsUsed.FirstOrDefault(l => l.ProducedMana == mana && !l.IsTapped);
                if (landToTap != null)
                {
                    landToTap.IsTapped = true;
                    remainingCost[mana]--;
                    if (remainingCost[mana] == 0)
                    {
                        remainingCost.Remove(mana);
                    }
                }
            }

            // Spend colorless mana
            int colorlessCost = remainingCost.ContainsKey(Mana.Colorless) ? remainingCost[Mana.Colorless] : 0;
            while (colorlessCost > 0)
            {
                var landToTap = LandsUsed.FirstOrDefault(l => !l.IsTapped);
                if (landToTap != null)
                {
                    landToTap.IsTapped = true;
                    colorlessCost--;
                }
                else
                {
                    throw new InvalidOperationException("Insufficient mana to spend.");
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
