namespace Aurora.Server.Models
{
    public class AttackDTO
    {
        public string AttackingPlayerId { get; set; }
        public List<string> AttackingCreatureIds { get; set; }
    }
}
