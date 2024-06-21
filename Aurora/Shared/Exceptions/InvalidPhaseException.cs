using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Shared.Exceptions
{
    public class InvalidPhaseException : Exception
    {
        public InvalidPhaseException(string message) : base(message)
        {

        }

        public InvalidPhaseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
