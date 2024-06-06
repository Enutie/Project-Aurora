﻿using System;
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

        public void Add(Mana mana)
        {
            if (_mana.ContainsKey(mana))
            {
                _mana[mana]++;
            }
            else
            {
                _mana[mana] = 1;
            }
        }

        public bool CanAfford(IEnumerable<Mana> cost)
        {
            var remainingCost = cost.GroupBy(m => m).ToDictionary(g => g.Key, g => g.Count());
            foreach (var (mana, count) in _mana)
            {
                if (remainingCost.ContainsKey(mana))
                {
                    remainingCost[mana] -= count;
                    if (remainingCost[mana] <= 0)
                    {
                        remainingCost.Remove(mana);
                    }
                }
            }
            return !remainingCost.Any();
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
    }
}