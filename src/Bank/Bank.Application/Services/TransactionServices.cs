
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Bank.Application.Services;

    public class TransactionServices : ITransactionServices
    {
        private readonly BankContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountServices _accountServices;
        public TransactionServices(BankContext context,IHttpContextAccessor httpContextAccessor,IAccountServices accountServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _accountServices = accountServices;
        }
        public async Task<TransactionResponse> CreateTransaction(CreateNewTransactionDto transactionDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var accountSender = await _accountServices.GetAccount();
                if (transactionDto.Value > accountSender.Balance)
                    throw new InsufficientBalanceException("Insufficient balance");
                
                var accountReceiver = await _context.Accounts
                    .Where(a => a.NumberAccount == transactionDto.NumberAccountReceiver)
                    .FirstOrDefaultAsync() ?? throw new AccountNotFoundException("Account not found");
                var newTransaction = new Transaction()
                {
                    IdSender = accountSender.IdAccount,
                    IdReceiver = accountReceiver.AccountId,
                    Value = transactionDto.Value
                };
                _context.Transactions.Add(newTransaction);
                var updatedBalance = await UpdateBalanceAccount
                    (accountSender.IdAccount,accountReceiver.AccountId ,transactionDto.Value);
                if (updatedBalance)
                {
                    transaction.Commit();
                    await _context.SaveChangesAsync();
                    return new TransactionResponse()
                    { 
                        IdTransaction = newTransaction.Id,
                        IdSender = accountSender.IdAccount,
                        IdReceiver = accountReceiver.AccountId,
                        Value = transactionDto.Value,
                        TimeStamp = DateTime.UtcNow
                    };
                }
                transaction.Rollback();
                throw new TransactionBetweenAccountsFailsException("Error in transaction");
            }
        }

        public async Task<IEnumerable<TransactionResponse>> GetLatestTransactions()
        {
            var accountSender = await _accountServices.GetAccount();
            var allLatestTransactions = await _context.Transactions
                .Where(a => a.IdSender == accountSender.IdAccount || a.IdReceiver == accountSender.IdAccount)
                .OrderByDescending(a => a.HourOfTransaction)
                .Select(a=> new TransactionResponse()
                    {IdReceiver = a.IdReceiver,IdSender = a.IdSender,IdTransaction = a.Id,Value=a.Value,TimeStamp = a.HourOfTransaction})
                .ToListAsync();
            return allLatestTransactions.AsEnumerable();
        }

        private async Task<bool> UpdateBalanceAccount(Guid senderAccountId, Guid receiverAccountId, decimal amount)
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
            return countSender > 0 && countReceiver > 0;
        }
    }
