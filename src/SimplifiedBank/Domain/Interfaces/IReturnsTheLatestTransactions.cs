using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Application.DTOs.Response;

namespace SimplifiedBank.Domain.Interfaces
{
    public interface IReturnTheLatestTransactions
    {
        IEnumerable<GetLatestTransactions> GetList(int id);
    }
}