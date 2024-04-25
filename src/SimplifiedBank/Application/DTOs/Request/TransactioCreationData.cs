using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs
{
    public record TransactioCreationData(int IdReceiver, int IdSender, decimal Value);
}