
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;
using Bank.Bank.Application.Exceptions;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bank.Bank.Application.Services;

    public class TransactionServices : ITransactionServices
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        public TransactionServices(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }
        public async Task<TransactionResponse> CreateNewTransaction(CreateNewTransactionDto transactionDto, Guid userIdCurrent)
        {
                var accountSender = await _accountRepository.GetAccountById(userIdCurrent);
                if (transactionDto.Value > accountSender!.Balance)
                    throw new InsufficientBalanceException("Insufficient balance");
                
                var accountReceiver = await _accountRepository.GetAccountByNumberAccount(transactionDto.NumberAccountReceiver)
                    ?? throw new AccountNotFoundException("Account not found");
                var newTransaction = new Transaction()
                {
                    IdSender = accountSender.AccountId,
                    IdReceiver = accountReceiver.AccountId,
                    Value = transactionDto.Value
                };
                await _transactionRepository.CreateTransaction(newTransaction);
                var updatedBalance = await _transactionRepository.UpdateBalanceAccount(accountSender.AccountId
                    , accountReceiver.AccountId, transactionDto.Value);
                if (updatedBalance)
                {
                    return new TransactionResponse()
                    { 
                        IdTransaction = newTransaction.Id,
                        IdSender = accountSender.AccountId,
                        IdReceiver = accountReceiver.AccountId,
                        Value = transactionDto.Value,
                        TimeStamp = DateTime.UtcNow
                    };
                }
                throw new TransactionBetweenAccountsFailsException("Error in transaction");
        }

        public async Task<IEnumerable<TransactionResponse>> GetLatestTransactions(Guid userIdCurrent)
        {
            var allLatestTransactions = await _transactionRepository.GetLatestTransactions(userIdCurrent);
            var listTransactionsResponse = allLatestTransactions
                .Select(a => new TransactionResponse() {
                IdReceiver = a.IdReceiver, IdSender = a.IdSender, IdTransaction = a.Id, Value = a.Value,
                TimeStamp = a.HourOfTransaction });
            return listTransactionsResponse;
        }
    }
