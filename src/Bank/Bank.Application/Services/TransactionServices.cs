
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
        public async Task<TrancsactionResponse> CreateTransaction(CreateNewTransactionDto transactionDto)
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
                var countSender = await _context.Accounts
                    .Where(a => a.AccountId == accountSender.IdAccount)
                    .ExecuteUpdateAsync(a =>
                        a.SetProperty(account => account.Balance,
                            account => account.Balance - transactionDto.Value));
                var countReceiver = await _context.Accounts
                    .Where(a => a.AccountId == accountReceiver.AccountId)
                    .ExecuteUpdateAsync(a =>
                        a.SetProperty(account => account.Balance,
                            account => account.Balance + transactionDto.Value));
                if (countSender > 0 && countReceiver > 0)
                {
                    transaction.Commit();
                    await _context.SaveChangesAsync();
                    return new TrancsactionResponse()
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

        public Task<IEnumerable<TrancsactionResponse>> GetLatestTransactions()
        {
            throw new NotImplementedException();
        }
    }
