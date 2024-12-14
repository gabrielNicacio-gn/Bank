using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Application.DTOs.Response;

namespace Bank.Bank.Domain.Interfaces
{
    public interface ITransactionServices
    {
        Task<TransactionResponse> CreateNewTransaction(CreateNewTransactionDto transactionDto, Guid userIdCurrent);
        Task<IEnumerable<TransactionResponse>> GetLatestTransactions(Guid userIdCurrent);
    }
}