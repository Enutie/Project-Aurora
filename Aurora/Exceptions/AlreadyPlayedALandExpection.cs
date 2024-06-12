using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Exceptions
{
    public class AlreadyPlayedALandExpection : Exception
    {
        public AlreadyPlayedALandExpection(string message) : base(message)
        {
            
        }

        public AlreadyPlayedALandExpection(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
