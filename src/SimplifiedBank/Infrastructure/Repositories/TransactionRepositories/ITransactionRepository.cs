using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Repositories.TransactionRepositories
{
    public interface ITransactionRepository
    {
        Task CreateTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetLatestTransaction(int id);
    }
}