using Aurora.Application.DTO;
using Aurora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Shared.Utils
{
    public interface ICardConverter
    {
        CardDTO ConvertToCardDTO(Card card);
        PlayerDTO ConvertToPlayerDTO(Player player);
        Land ConvertToLand(LandDTO land);
        LandDTO ConvertToLandDTO(Land land);
        CreatureDTO ConvertToCreatureDTO(Creature creature);
        Creature ConvertToCreature(CreatureDTO creature);
    }
}
