namespace Aurora.Server.Models
{
    public class CreatureCastDTO
    {
        public string PlayerId { get; set; }
        public string CreatureId { get; set; }
        public List<string> ManaCost { get; set; }
    }
}
