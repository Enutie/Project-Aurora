using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora
{
    public static class DeckFactory
    {
        public static Deck CreateDeck(LandType landType)
        {
            List<Card> cards = new List<Card>();
            for(int i = 0; i < 60; i++)
            {
                cards.Add(new Land(landType));
            }

            return new Deck(cards);
        }

        public static Deck MonoGreenVanilla()
        {
            List<Card> cards = new List<Card>();
            for(int i = 0; i < 24; i++)
            {
                cards.Add(new Land(LandType.Forest));
            }
            for(int i = 0;i < 4; i++)
            {
                cards.Add(new Creature("Woodland Druid", new[] { Mana.Green }, 1, 2));
                cards.Add(new Creature("Bear Cub", new[] { Mana.Green, Mana.Colorless }, 2, 2));
                cards.Add(new Creature("Cylian Elf", new[] { Mana.Green, Mana.Colorless }, 2, 2));
                cards.Add(new Creature("Alpine Grizzly", new[] {Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless}, 4, 2));
                cards.Add(new Creature("Centaur Courser", new[] { Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 3, 3));
                cards.Add(new Creature("Axebane Beast", new[] { Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 3, 4));
            }
            for(int i = 0; i < 2; i++)
            {
                cards.Add(new Creature("Broodhunter Wurm", new[] { Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 4, 3));
                cards.Add(new Creature("Feral Krushok", new[] { Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 5, 4));
                cards.Add(new Creature("Alpha Tyrranax", new[] { Mana.Green, Mana.Green, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless, Mana.Colorless }, 6, 5));
            }
            return new Deck(cards);
        }
    }
}
