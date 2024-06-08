namespace Aurora.Server.Requests
{
    public class CreateGameRequest
    {
        public string PlayerName { get; set; }
    }

    public class CreateGameResponse
    {
        public string GameId { get; set; }
        public List<PlayerResponse> Players { get; set; }
    }

    public class PlayLandRequest
    {
        public string PlayerId { get; set; }
        public int LandIndex { get; set; }
    }

    public class CastCreatureRequest
    {
        public string PlayerId { get; set; }
        public int CreatureIndex { get; set; }
    }

    public class AttackRequest
    {
        public string AttackerId { get; set; }
        public string DefenderId { get; set; }
        public List<string> AttackingCreatureIds { get; set; }
    }

    public class PlayerResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Life { get; set; }
        public int HandCount { get; set; }
        public List<CardResponse> Hand {  get; set; }
        public int DeckCount { get; set; }
        public List<CardResponse> Battlefield { get; set; }
    }

    public class CardResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Power { get; set; }
        public int? Toughness { get; set; }

        public CardResponse()
        {
            
        }

        public CardResponse(Card card)
        {
            Id = card.Id;
            Name = card.Name;
            if(card is Creature creature)
            {
                Power = creature.Power;
                Toughness = creature.Toughness;
                Type = "Creature";
            }
            else
            {
                Type = "Land";
            }
                
        }
    }

    public class GameStateResponse
    {
        public string GameId { get; set; }
        public List<PlayerResponse> Players { get; set; }
        public string CurrentPlayer { get; set; }
        public bool IsGameOver { get; set; }
        public string Winner { get; set; }
    }

}
