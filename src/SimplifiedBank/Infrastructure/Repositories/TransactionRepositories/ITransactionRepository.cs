using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task CreateTransaction(Transaction transaction);
    }
}