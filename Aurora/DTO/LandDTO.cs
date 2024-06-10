using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.DTO
{
    public class LandDTO : CardDTO
    {
        public LandType Type { get; set; }
        public bool IsTapped { get; set; }
    }
}
