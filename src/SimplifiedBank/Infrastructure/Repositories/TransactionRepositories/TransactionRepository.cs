using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Infrastructure.Data;

namespace SimplifiedBank.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext _bankContext;
        public TransactionRepository(BankContext bankContext)
        {
            _bankContext = bankContext;
        }
        public async Task CreateTransaction(Transaction transaction)
        {
            using (var begin = _bankContext.Database.BeginTransaction())
            {
                await _bankContext.Transactions.AddAsync(transaction);
                await _bankContext.Accounts
                .Where(ac => ac.Id == transaction.IdSender)
                .ExecuteUpdateAsync(ac => ac.SetProperty(b => b.Balance, b => b.Balance - transaction.Value));
                await _bankContext.Accounts
                .Where(ac => ac.Id == transaction.IdReceiver)
                .ExecuteUpdateAsync(ac => ac.SetProperty(b => b.Balance, b => b.Balance + transaction.Value));
                _bankContext.SaveChanges();
                begin.Commit();
            }
        }
    }
}