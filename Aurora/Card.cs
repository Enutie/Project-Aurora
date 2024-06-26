﻿namespace Aurora
{
    public class Card
    {
        public string Name { get; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsTapped { get; set; } = false;

        public Card(string name)
        {
            Name = name;
        }
    }

    public enum LandType
    {
        Plains,
        Island,
        Swamp,
        Mountain,
        Forest
    }

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
