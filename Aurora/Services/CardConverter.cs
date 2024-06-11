using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
    public class CardConverter : ICardConverter
    {
        public CardDTO ConvertToCardDTO(Card card)
        {
            switch (card)
            {
                case Creature creature:
                    return new CreatureDTO
                    {
                        Id = creature.Id,
                        Name = creature.Name,
                        Power = creature.Power,
                        Toughness = creature.Toughness,
                        ManaCost = creature.ManaCost.Select(m => m.ToString()).ToList(),
                        IsTapped = creature.IsTapped,
                        IsAttacking = creature.IsAttacking,
                        IsBlocked = creature.IsBlocked,
                        BlockedBy = creature.BlockedBy != null ? ConvertToCardDTO(creature.BlockedBy) as CreatureDTO : null
                    };

                case Land land:
                    return new LandDTO
                    {
                        Id = land.Id,
                        Name = land.Name,
                        LandType = land.Type.ToString(),
                        IsTapped = land.IsTapped
                    };

                default:
                    return new CardDTO
                    {
                        Id = card.Id,
                        Name = card.Name
                    };
            }
        }
    }
}
