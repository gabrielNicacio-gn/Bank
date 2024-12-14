using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Models;

namespace Bank.Bank.Infrastructure.Interfaces;

public interface ITransactionRepository
{
    Task<bool> UpdateBalanceAccount(Guid senderAccountId, Guid receiverAccountId, decimal amount);
    Task CreateTransaction(Transaction newTransaction);
    Task<IEnumerable<Transaction>> GetLatestTransactions(Guid userIdCurrent);
}