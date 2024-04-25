using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs.Response
{
    public record GetAccountData(int Id, string FullName, decimal Balance);
}