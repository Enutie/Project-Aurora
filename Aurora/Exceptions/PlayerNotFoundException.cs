﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException(string message) : base(message) { }
    }
}
