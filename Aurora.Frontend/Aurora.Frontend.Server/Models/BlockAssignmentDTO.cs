namespace Aurora.Server.Models
{
    public class BlockAssignmentDTO
    {
        public string DefendingPlayerId { get; set; }
        public Dictionary<string, string> BlockerAssignments { get; set; }
    }
}
