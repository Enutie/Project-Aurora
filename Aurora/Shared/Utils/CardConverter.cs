using Aurora.Application.DTO;
using Aurora.Domain.Entities;
using Aurora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Shared.Utils
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

        public CreatureDTO ConvertToCreatureDTO(Creature creature)
        {
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
        }

        public LandDTO ConvertToLandDTO(Land land)
        {
            return new LandDTO
            {
                Id = land.Id,
                Name = land.Name,
                LandType = land.Type.ToString(),
                IsTapped = land.IsTapped
            };
        }

        public Creature ConvertToCreature(CreatureDTO creatureDTO)
        {
            return new Creature(creatureDTO.Name)
            {
                Id = creatureDTO.Id,
                IsTapped = creatureDTO.IsTapped,
                ManaCost = creatureDTO.ManaCost.Select(m => (Mana)Enum.Parse(typeof(Mana), m)),
                Power = creatureDTO.Power,
                Toughness = creatureDTO.Toughness,
                IsAttacking = creatureDTO.IsAttacking,
                IsBlocked = creatureDTO.IsBlocked,
            };
        }

        public Land ConvertToLand(LandDTO land)
        {
            var typeName = land.Type;
            Land returnLand;
            if (Enum.TryParse(typeName, true, out LandType type))
            {
                returnLand = new Land(type)
                {
                    Id = land.Id,
                    IsTapped = land.IsTapped
                };
            }
            else
            {
                returnLand = new Land(LandType.Forest)
                {
                    Id = land.Id,
                    IsTapped = land.IsTapped
                };
            }
            return returnLand;
        }

        public PlayerDTO ConvertToPlayerDTO(Player player)
        {
            return new PlayerDTO
            {
                Id = player.Id,
                Name = player.Name,
                Hand = player.Hand.Select(c => ConvertToCardDTO(c)).ToList(),
                Battlefield = player.Battlefield.Select(c => ConvertToCardDTO(c)).ToList(),
                Graveyard = player.Graveyard.Select(c => ConvertToCardDTO(c)).ToList(),
                Life = player.Life
            };
        }
    }
}
