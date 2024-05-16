using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Interfaces.Exceptions
{
    public class SecretKeyInvalidException : Exception
    {
        public SecretKeyInvalidException() { }

        public SecretKeyInvalidException(string message) : base(message) { }
    }
}