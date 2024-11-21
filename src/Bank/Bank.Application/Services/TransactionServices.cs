
using System.Security.Claims;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Bank.Application.Services
{
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
        public async Task<DefaultResponse<TrancsactionResponse>> CreateTransaction(CreateNewTransactionDto transactionDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var accountSender = await _accountServices.GetAccount();
                var accountReceiver = await _context.Accounts
                    .Where(a => a.NumberAccount == transactionDto.NumberAccountReceiver)
                    .FirstOrDefaultAsync() ?? throw new Exception();
                var newTransaction = new Transaction()
                {
                    IdSender = accountSender.Data!.IdAccount,
                    IdReceiver = accountReceiver.AccountId,
                    Value = transactionDto.Value
                };
                _context.Transactions.Add(newTransaction);
                var countSender = await _context.Accounts
                    .Where(a => a.AccountId == accountSender.Data.IdAccount)
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
                    var data = new TrancsactionResponse()
                    {
                        IdTransaction = newTransaction.Id,
                        IdSender = accountSender.Data.IdAccount,
                        IdReceiver = accountReceiver.AccountId,
                        Value = transactionDto.Value,
                        TimeStamp = DateTime.UtcNow
                    };
                    return new DefaultResponse<TrancsactionResponse>(data, "Transaction Created");
                }
                transaction.Rollback();
                return new DefaultResponse<TrancsactionResponse>("Fail to create transaction");  
            }
            
        }
    }
}