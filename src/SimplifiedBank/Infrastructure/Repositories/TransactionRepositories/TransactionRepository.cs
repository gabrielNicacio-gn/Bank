using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Extensions;
using SimplifiedBank.Infrastructure.Data;

namespace SimplifiedBank.Infrastructure.Repositories.TransactionRepositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankContext _bankContext;
    public TransactionRepository(BankContext bankContext)
    {
        _bankContext = bankContext;
    }
    public void CreateTransaction(Transaction transaction)
    {
        using (var begin = _bankContext.Database.BeginTransaction())
        {
            _bankContext.Transactions.AddAsync(transaction);
            _bankContext.Accounts
            .Where(ac => ac.Id == transaction.IdSender)
            .ExecuteUpdate(ac => ac.SetProperty(b => b.Balance, b => b.Balance - transaction.Value));
            _bankContext.Accounts
            .Where(ac => ac.Id == transaction.IdReceiver)
            .ExecuteUpdate(ac => ac.SetProperty(b => b.Balance, b => b.Balance + transaction.Value));
            _bankContext.SaveChanges();
            begin.Commit();
        }
    }

    public IEnumerable<Transaction> GetLatestTransaction(int idAccount)
    {
        var listAccount = _bankContext.Transactions
        .Where(tr => tr.IdSender == idAccount || tr.IdReceiver == idAccount)
        .OrderByDescending(tr => tr.HourOfTransaction)
        .AsNoTracking()
        .ToList();
        return listAccount.AsEnumerable();
    }
}
