using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aurora.DTO
{

    [JsonDerivedType(typeof(CreatureDTO))]
    [JsonDerivedType(typeof(LandDTO))]
    public class CardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual string Type { get; set; } = "Card";
    }
}
