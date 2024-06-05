namespace Aurora
{
    public class Card
    {
        public string Name { get; }

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

        public Land(LandType type) : base(type.ToString())
        {
            Type = type;
        }
    }
}
