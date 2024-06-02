using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Repositories.TransactionRepositories
{
    public interface ITransactionRepository
    {
        void CreateTransaction(Transaction transaction);
        IEnumerable<Transaction> GetLatestTransaction(int id);
    }
}