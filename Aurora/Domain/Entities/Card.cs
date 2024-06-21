namespace Aurora.Domain.Entities
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
}
