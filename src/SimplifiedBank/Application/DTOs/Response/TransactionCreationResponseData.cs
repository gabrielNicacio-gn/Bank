using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs.Response
{
    public record TransactionCreationResponseData(int IdSender, int IdReceiver);
}