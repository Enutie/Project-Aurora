using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.DTO
{
    public class PlayerDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<CardDTO> Hand { get; set; }
        public List<CardDTO> Battlefield { get; set; }
        public int Life { get; set; }
    }
}
