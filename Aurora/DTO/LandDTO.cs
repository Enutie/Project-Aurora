using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.DTO
{
    public class LandDTO : CardDTO
    {
        public string LandType { get; set; }
        public override string Type { get; set; } = "Land";
        public bool IsTapped { get; set; }
    }
}
