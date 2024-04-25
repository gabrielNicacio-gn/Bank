using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Interfaces.Exceptions
{
    public class InsufficienteBalanceException : Exception
    {
        public InsufficienteBalanceException() { }

        public InsufficienteBalanceException(string message) : base(message) { }
    }
}