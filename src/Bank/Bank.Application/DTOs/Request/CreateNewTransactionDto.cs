using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Bank.Application.DTOs.Request
{
    public class CreateNewTransactionDto
    {
        public decimal Value{ get; set; }
        public int NumberAccountReceiver { get; set; }
    }
}