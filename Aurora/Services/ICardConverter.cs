﻿using Aurora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Services
{
    public interface ICardConverter
    {
        CardDTO ConvertToCardDTO(Card card);
    }
}
