using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Bank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankContext _context;

    public TransactionRepository(BankContext context)
    {
        _context = context;
    }
    public async Task<bool> UpdateBalanceAccount(Guid senderAccountId, Guid receiverAccountId, decimal amount)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var countSender = await _context.Accounts
                .Where(a => a.AccountId == senderAccountId)
                .ExecuteUpdateAsync(a =>
                    a.SetProperty(account => account.Balance,
                        account => account.Balance - amount));
            var countReceiver = await _context.Accounts
                .Where(a => a.AccountId == receiverAccountId)
                .ExecuteUpdateAsync(a =>
                    a.SetProperty(account => account.Balance,
                        account => account.Balance + amount));
            if (countSender > 0 && countReceiver > 0)
            {
                transaction.Commit();
                return true;
            }
            transaction.Rollback();
            return false;
        }
    }

    public async Task CreateTransaction(Transaction newTransaction)
    {
        _context.Transactions.Add(newTransaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetLatestTransactions(Guid userIdCurrent)
    {
        var allLatestTransactions = await _context.Transactions
            .Where(a => a.IdSender == userIdCurrent || a.IdReceiver == userIdCurrent)
            .OrderByDescending(a => a.HourOfTransaction)
            .ToListAsync();
        return allLatestTransactions.AsEnumerable();
    }
}